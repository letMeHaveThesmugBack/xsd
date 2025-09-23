using Dev.Thesmug.Tsxml.Xsd;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using TSXMLLib;
using XSD = Dev.Thesmug.Tsxml.Xsd;

namespace TSXMLEdit
{
    public class Document
    {
        public string Title { get; private set; } = "UNKNOWN"; // TODO: give the forms names
        public Viewmodel? Model { get; private set; }
        public TSXMLFile? XMLFile { get; private set; }
        public TSNDJFile? NDJFile { get; private set; }
        internal ContainerControl? Content { get; private set; }
        internal ReadOnlyCollection<IReportsChanges>? ChangeReporters { get; private set; }

        public static async Task<Document> CreateAsync(Uri? tsxmlSource, Uri? tsndjSource, CancellationToken cancellationToken)
        {
            Document doc = new()
            {
                XMLFile = tsxmlSource is not null ? await ITSFileFactory<TSXMLFile>.CreateAsync(tsxmlSource, cancellationToken) : null,
                NDJFile = tsndjSource is not null ? await ITSFileFactory<TSNDJFile>.CreateAsync(tsndjSource, cancellationToken) : null
            };

            // XML not provided, but NDJ provided and has XML URI
            if (doc.XMLFile is null && doc.NDJFile is not null && doc.NDJFile.AssociatedXMLUri is Uri uri) await doc.AttachXMLAsync(uri, cancellationToken);

            // XML might now have been provided
            if (doc.XMLFile is not null && doc.XMLFile.Form is XSD.Form form)
            {
                doc.Title = form.AssociatedObject is not null ? $"{form.Name} - Unknown" : form.Name;

                Viewmodel model = doc.Model = new();
                doc.Content = model.Render(form);
                
                if (doc.NDJFile is TSNDJFile jFile)
                {
                    model.SetBindingValues(jFile.Objects);

                    Collection<IReportsChanges> changeReporters = [];

                    foreach (Viewmodel.Binding binding in model.Bindings.Values)
                    {
                        if (cancellationToken.IsCancellationRequested) throw new TaskCanceledException();

                        if (IReportsChanges.Create(binding.WFControl) is IReportsChanges reporter)
                        {
                            changeReporters.Add(reporter);
                        }
                    }

                    doc.ChangeReporters = changeReporters.AsReadOnly();
                }
            }

            return doc;
        }

        public async Task<Document> AttachNDJAsync(Uri source, CancellationToken cancellationToken) => XMLFile is not null ? await CreateAsync(XMLFile.Source, source, cancellationToken) : this;

        public async Task<Document> AttachXMLAsync(Uri source, CancellationToken cancellationToken) => NDJFile is not null ? await CreateAsync(source, NDJFile.Source, cancellationToken) : this;

        public async Task<Document> ReloadAsync(CancellationToken cancellationToken) => XMLFile is not null && NDJFile is not null ? await CreateAsync(XMLFile.Source, NDJFile.Source, cancellationToken) : this;
    }
}
