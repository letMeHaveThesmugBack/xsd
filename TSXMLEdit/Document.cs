using Dev.Thesmug.Tsxml.Xsd;
using System;
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

        private static async Task<Document> CreateAsync(Func<Document, Task> loadXml, Func<Document, Task> loadNdj)
        {
            Document doc = new();

            Task xTask = loadXml(doc);
            Task jTask = loadNdj(doc);

            await xTask;
            await jTask;

            // XML not provided, but NDJ provided and has XML URI
            if (doc.XMLFile is null && doc.NDJFile is not null && doc.NDJFile.AssociatedXMLUri is Uri uri)
            {
                await doc.LoadXML(uri);
            }

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

        public static Task<Document> CreateAsync(FileInfo? tsxmlFile, FileInfo? tsndjFile) =>
            CreateAsync(doc => doc.LoadXML(tsxmlFile), doc => doc.LoadNDJ(tsndjFile));

        public static Task<Document> CreateAsync(FileInfo tsxmlFile, Uri tsndjUri, DirectoryInfo? tsndjDestination = null) =>
            CreateAsync(doc => doc.LoadXML(tsxmlFile), doc => doc.LoadNDJ(tsndjUri, tsndjDestination));

        public static Task<Document> CreateAsync(Uri tsxmlUri, Uri tsndjUri, DirectoryInfo? tsxmlDestination = null, DirectoryInfo? tsndjDestination = null) =>
            CreateAsync(doc => doc.LoadXML(tsxmlUri, tsxmlDestination), doc => doc.LoadNDJ(tsndjUri, tsndjDestination));

        public static Task<Document> CreateAsync(Uri tsxmlUri, FileInfo tsndjFile, DirectoryInfo? tsxmlDestination = null) =>
            CreateAsync(doc => doc.LoadXML(tsxmlUri, tsxmlDestination), doc => doc.LoadNDJ(tsndjFile));

        public static Task<Document> AttachNDJAsync(Document document, FileInfo tsndjFile)
        {
            return document.XMLFile is TSXMLFile xml
                ? CreateAsync(doc => doc.LoadXML(xml.File), doc => doc.LoadNDJ(tsndjFile))
                : Task.FromResult(document);
        }

        private async Task LoadXML(FileInfo? file) => XMLFile = file is not null ? await ITSFileFactory<TSXMLFile>.CreateFromLocalFileAsync(file) : null;
        private async Task LoadXML(Uri uri, DirectoryInfo? destination = null) => XMLFile = await ITSFileFactory<TSXMLFile>.CreateFromRemoteFileAsync(uri, destination);
        private async Task LoadNDJ(FileInfo? file) => NDJFile = file is not null ? await ITSFileFactory<TSNDJFile>.CreateFromLocalFileAsync(file) : null;
        private async Task LoadNDJ(Uri uri, DirectoryInfo? destination = null) => NDJFile = await ITSFileFactory<TSNDJFile>.CreateFromRemoteFileAsync(uri, destination);
    }
}
