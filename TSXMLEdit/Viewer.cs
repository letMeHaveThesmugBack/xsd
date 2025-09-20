using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TSXMLEdit.Properties;
using TSXMLLib;
using TSXMLLib.WFControls;

namespace TSXMLEdit
{
    public partial class Viewer : System.Windows.Forms.Form
    {
        private enum FileStatus
        {
            NoFile,
            NoNDJ,
            Locked,
            Unlocked,
            UnsavedChanges
        }

        public Viewer()
        {
            InitializeComponent();

            progressBarTimer.Tick += (s, e) =>
            {
                ProgressBar.Value = ProgressBar.Minimum + ((ProgressBar.Value + ProgressBar.Step) % (ProgressBar.Maximum - ProgressBar.Minimum));
            };

            Task.Run(() => PerformQueuedTasksAsync(cancellationTokenSource.Token));
        }

        private readonly Dictionary<TabPage, Document> documents = [];
        private readonly Dictionary<Document, TabPage> tabPages = [];
        private readonly Dictionary<Document, FileStatus> fileStatuses = [];

        private Document? currentDocument = null;

        private readonly System.Windows.Forms.Timer progressBarTimer = new()
        {
            Interval = 100
        };
        private readonly CancellationTokenSource cancellationTokenSource = new();
        private readonly Queue<Task> tasks = new();

        internal void OnValueChanged(object? sender, EventArgs e)
        {
            if (currentDocument is Document doc) fileStatuses[doc] = FileStatus.UnsavedChanges;
            SetFileStatusSymbol(FileStatus.UnsavedChanges);
        }

        private void SetFileStatusSymbol(FileStatus status) => EditStateStatusLabel.Image = status switch
        {
            FileStatus.NoNDJ => Resources.w98_file_question,
            FileStatus.Locked => Resources.w98_file_padlock,
            FileStatus.Unlocked => Resources.w98_message_file,
            FileStatus.UnsavedChanges => Resources.w98_registration,
            _ => Resources.w98_file_question // TODO: set these values when a new file is loaded, when a file is saved, and when a file is locked/unlocked
        };

        private async Task PerformQueuedTasksAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (tasks.Count > 0)
                {
                    Task task = tasks.Dequeue();
                    ProgressBar.ToolTipText = task.ToString(); // TODO: implement task status tooltips
                    progressBarTimer.Start();

                    await task;
                }

                else
                {
                    ProgressBar.ToolTipText = string.Empty;
                    progressBarTimer.Stop();
                    ProgressBar.Value = ProgressBar.Minimum;
                }
            }
        }

        private async Task AddDocument(Task<Document> creationTask)
        {
            Document doc = await creationTask;

            TabPage page = new()
            {
                Text = doc.Title
            };

            if (doc.Content is ContainerControl content)
            {
                documents.Add(page, doc);
                tabPages.Add(doc, page);

                content.Dock = DockStyle.Fill;
                page.Controls.Add(content);
                MainTabControl.TabPages.Add(page);

                fileStatuses.Add(doc, doc.NDJFile is not null ? FileStatus.Locked : FileStatus.NoNDJ);
                MainTabControl_Selected(this, new(page, MainTabControl.TabPages.IndexOf(page), TabControlAction.Selecting));
            }

            else
            {
                page.Dispose();
            }
        }

        private void RemoveDocument(Document document)
        {
            TabPage associatedTabPage = tabPages[document];
            documents.Remove(associatedTabPage);
            tabPages.Remove(document);
            fileStatuses.Remove(document);

            MainTabControl.TabPages.Remove(associatedTabPage);
        }

        private async Task ReplaceDocument(Document original, Task<Document> creationTask)
        {
            RemoveDocument(original);
            await AddDocument(creationTask);
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                IEnumerable<FileInfo> files = from filename
                                              in OpenFileDialog.FileNames
                                              select new FileInfo(filename);

                foreach (FileInfo file in files)
                {
                    switch (file.Extension)
                    {
                        case ".tsxml":
                            tasks.Enqueue(AddDocument(Document.CreateAsync(file, null)));
                            break;
                        case ".tsndj":
                            tasks.Enqueue(AddDocument(Document.CreateAsync(null, file)));
                            break;
                        default:
                            // TODO: warning that an invalid filetype was selected
                            break;
                    }
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            cancellationTokenSource.Cancel();
            base.OnFormClosing(e);
        }

        private void Viewer_ResizeBegin(object sender, EventArgs e)
        {
            SuspendLayout();
        }

        private void Viewer_ResizeEnd(object sender, EventArgs e)
        {
            ResumeLayout(true);
        }

        private void MainTabControl_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPage is TabPage tabPage)
            {
                currentDocument = documents[e.TabPage];
                SetFileStatusSymbol(fileStatuses[currentDocument]);

                if (currentDocument is Document doc && doc.ChangeReporters is ReadOnlyCollection<IReportsChanges> changeReporters)
                {
                    foreach (IReportsChanges changes in changeReporters)
                    {
                        changes.UnsubscribeChangeHandler(OnValueChanged);
                    }
                }
            }

            if (currentDocument is Document newDoc && newDoc.ChangeReporters is ReadOnlyCollection<IReportsChanges> newChangeReporters)
            {
                foreach (IReportsChanges changeReporter in newChangeReporters)
                {
                    changeReporter.SubscribeChangeHandler(OnValueChanged);
                }
            }
        }

        private void LockUnlockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentDocument is Document doc)
            {
                switch (fileStatuses[doc])
                {
                    case FileStatus.NoNDJ:
                        if (MessageBox.Show("There is no NDJ file associated with the currently selected document. What would you like to do?",
                        Resources.ProgramName, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            // TODO: show a window with options to create one, open local, or open remote, or cancel (instead of this dialog box)
                        }
                        break;
                    case FileStatus.Locked:
                        if (doc.ChangeReporters is not null)
                        {
                            foreach (IReportsChanges changeReporter in doc.ChangeReporters)
                            {
                                changeReporter.ToggleEditing(true);
                            }

                            fileStatuses[doc] = FileStatus.Unlocked;
                            SetFileStatusSymbol(FileStatus.Unlocked); // TODO: this should be handled by one operation
                        }

                        break;
                    case FileStatus.Unlocked: // TODO: this should be a function that does this (same code as directly above)
                        if (doc.ChangeReporters is not null)
                        {
                            foreach (IReportsChanges changeReporter in doc.ChangeReporters)
                            {
                                changeReporter.ToggleEditing(false);
                            }

                            fileStatuses[doc] = FileStatus.Locked;
                            SetFileStatusSymbol(FileStatus.Locked); // TODO: this should be handled by one operation
                        }

                        break;
                    case FileStatus.UnsavedChanges: // TODO: this doesn't work
                        if (MessageBox.Show("There are unsaved changes, would you like to save them?", Resources.ProgramName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            SaveToolStripMenuItem_Click(this, new());
                            goto case FileStatus.Unlocked;
                        }

                        break;
                }
            }

            else
            {
                MessageBox.Show("There is no document currently loaded.", Resources.ProgramName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentDocument is Document doc
                && (fileStatuses[doc] == FileStatus.Unlocked
                    || fileStatuses[doc] == FileStatus.UnsavedChanges)
                && doc.NDJFile is TSNDJFile ndj
                && doc.Model is Viewmodel model)
            {
                if (ndj.URI is null)
                {
                    File.WriteAllText(ndj.File.FullName, model.Serialize()); // TODO: should be async
                    fileStatuses[doc] = FileStatus.Unlocked;
                    SetFileStatusSymbol(FileStatus.Unlocked);
                    return;
                }

                else
                {
                    // TODO: File may be remote, save will only be made to the temp directory. Show a warning dialog asking if you'd like to save as, or cancel.
                }
            }

            System.Media.SystemSounds.Hand.Play();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool anyUnsavedChanges = (from al in fileStatuses.Values
                                      where al == FileStatus.UnsavedChanges
                                      select al).Any();

            if (!anyUnsavedChanges || MessageBox.Show("There are unsaved changes. Are you sure you want to exit?", Resources.ProgramName, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Environment.Exit(0);
            }
        }

        private void PrintPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: implement
        }

        private void PrintToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: implement
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentDocument is Document doc && doc.Model is Viewmodel model)
            {
                if (doc.NDJFile is TSNDJFile ndj)
                {
                    // TODO: save as a new ndj
                }

                else if (MessageBox.Show("The current document does not have any NDJ file associated with it. Would you like to create a new blank NDJ file for it?",
                    Resources.ProgramName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // TODO: all things referencing the name of the program should use the program name from the resources instead of hard coding
                {
                    if (SaveNDJFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllText(SaveNDJFileDialog.FileName, model.Serialize());
                        tasks.Enqueue(ReplaceDocument(doc, Document.AttachNDJAsync(doc, new(SaveNDJFileDialog.FileName))));

                        return;
                    }
                }
            }

            System.Media.SystemSounds.Hand.Play();
        }

        private void OpenRemoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: implement
        }

        private void UndoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SafeInvokeOnActiveControl<TextBoxBase>(t => { t.Undo(); t.ClearUndo(); }, (t) => t.CanUndo);
        }

        private void RedoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: implement (no redo for textboxbase)
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SafeInvokeOnActiveControl<TextBoxBase>(t => t.Cut(), (t) => t.SelectionLength > 0);
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SafeInvokeOnActiveControl<TextBoxBase>(t => t.Copy(), (t) => t.SelectionLength > 0);
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SafeInvokeOnActiveControl<TextBoxBase>(t => t.Paste(), (t) => Clipboard.GetDataObject()?.GetDataPresent(DataFormats.Text) is true);
        }

        private void SelectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SafeInvokeOnActiveControl<TextBoxBase>(t => t.SelectAll());
        }

        private void AttachNDJToolStripMenuItem_Click(object sender, EventArgs e) // TODO: deserialization does not appear to work. investigate asap
        {
            if (currentDocument is Document doc && doc.NDJFile is null)
            {
                if (OpenTSNDJFileDialog.ShowDialog() == DialogResult.OK)
                {
                    tasks.Enqueue(ReplaceDocument(doc, Document.AttachNDJAsync(doc, new(OpenTSNDJFileDialog.FileName)))); // TODO: (general) not verbose enough. not clear what specific errors are happening when something doesn't work, it's just silent. consider adding logging or error messages
                    return;
                }

                else return;
            }

            System.Media.SystemSounds.Hand.Play();
        }

        private void SafeInvokeOnActiveControl<T>(Action<T> action, params Func<T, bool>[] additionalConditions) where T : Control // TODO: this should be generally useful and contained in some library
        {
            if (ActiveControl is T control && additionalConditions.All(f => f is not null && f(control))) action(control);
        }
    }
}
