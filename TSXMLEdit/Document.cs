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

        internal Uri? XMLFileUri { get; private set; }
        internal Uri? NDJFileUri { get; private set; }

        private static async Task<Document> CreateAsync(Func<Document, Task> loadXml, Func<Document, Task> loadNdj, CancellationToken cancellationToken)
        {
            Document doc = new();

            Task xTask = loadXml(doc);
            Task jTask = loadNdj(doc);

            await xTask;
            await jTask;

            // XML not provided, but NDJ provided and has XML URI
            if (doc.XMLFile is null && doc.NDJFile is not null && doc.NDJFile.AssociatedXMLUri is Uri uri)
            {
                await doc.LoadXML(uri, cancellationToken);
            }

            // XML might now have been provided
            if (doc.XMLFile is TSXMLFile xFile && doc.XMLFile.Form is XSD.Form form)
            {
                doc.Title = form.AssociatedObject is not null ? $"{form.Name} - Unknown" : form.Name;
                doc.XMLFileUri = xFile.URI ?? new(xFile.File.FullName);

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
                    doc.NDJFileUri = jFile.URI ?? new(jFile.File.FullName);
                }
            }

            return doc;
        }

        public static Task<Document> CreateAsync(FileInfo? tsxmlFile, FileInfo? tsndjFile, CancellationToken cancellationToken) =>
            CreateAsync(doc => doc.LoadXML(tsxmlFile, cancellationToken), doc => doc.LoadNDJ(tsndjFile, cancellationToken), cancellationToken);

        public static Task<Document> CreateAsync(FileInfo tsxmlFile, Uri tsndjUri, CancellationToken cancellationToken, DirectoryInfo? tsndjDestination = null) =>
            CreateAsync(doc => doc.LoadXML(tsxmlFile, cancellationToken), doc => doc.LoadNDJ(tsndjUri, cancellationToken, tsndjDestination), cancellationToken);

        public static Task<Document> CreateAsync(Uri tsxmlUri, Uri tsndjUri, CancellationToken cancellationToken, DirectoryInfo? tsxmlDestination = null, DirectoryInfo? tsndjDestination = null) =>
            CreateAsync(doc => doc.LoadXML(tsxmlUri, cancellationToken, tsxmlDestination), doc => doc.LoadNDJ(tsndjUri, cancellationToken, tsndjDestination), cancellationToken);

        public static Task<Document> CreateAsync(Uri tsxmlUri, FileInfo tsndjFile, CancellationToken cancellationToken, DirectoryInfo? tsxmlDestination = null) =>
            CreateAsync(doc => doc.LoadXML(tsxmlUri, cancellationToken, tsxmlDestination), doc => doc.LoadNDJ(tsndjFile, cancellationToken), cancellationToken);

        public static Task<Document> AttachNDJAsync(Document document, FileInfo tsndjFile, CancellationToken cancellationToken)
        {
            return document.XMLFile is TSXMLFile xml
                ? CreateAsync(doc => doc.LoadXML(xml.File, cancellationToken), doc => doc.LoadNDJ(tsndjFile, cancellationToken), cancellationToken)
                : Task.FromResult(document); // TODO (!URGENT!): this might not work correctly because it does it all from files, when it should do it from remote if they are defined that way. make a catch-all that auto switches if necessary UPDATE: it indeed does not work correctly! FIX ASAP
        }

        public static Task<Document> ReloadAsync(Document document, CancellationToken cancellationToken)
        {
            return CreateAsync(doc => doc.LoadXML(document.XMLFile?.File, cancellationToken), doc => doc.LoadNDJ(document.NDJFile?.File, cancellationToken), cancellationToken); // TODO: same as above
        }

        private async Task LoadXML(FileInfo? file, CancellationToken cancellationToken) => XMLFile = file is not null ? await ITSFileFactory<TSXMLFile>.CreateFromLocalFileAsync(file, cancellationToken) : null;
        private async Task LoadXML(Uri uri, CancellationToken cancellationToken, DirectoryInfo? destination = null) => XMLFile = await ITSFileFactory<TSXMLFile>.CreateFromRemoteFileAsync(uri, cancellationToken, destination);
        private async Task LoadNDJ(FileInfo? file, CancellationToken cancellationToken) => NDJFile = file is not null ? await ITSFileFactory<TSNDJFile>.CreateFromLocalFileAsync(file, cancellationToken) : null;
        private async Task LoadNDJ(Uri uri, CancellationToken cancellationToken, DirectoryInfo? destination = null) => NDJFile = await ITSFileFactory<TSNDJFile>.CreateFromRemoteFileAsync(uri, cancellationToken, destination);
    }
}
