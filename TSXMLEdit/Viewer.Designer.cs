namespace TSXMLEdit
{
    partial class Viewer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Viewer));
            MainMenuStrip = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            openRemoteToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator = new ToolStripSeparator();
            saveToolStripMenuItem = new ToolStripMenuItem();
            saveAsToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator5 = new ToolStripSeparator();
            attachNDJToolStripMenuItem = new ToolStripMenuItem();
            lockUnlockToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            printToolStripMenuItem = new ToolStripMenuItem();
            printPreviewToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            exitToolStripMenuItem = new ToolStripMenuItem();
            editToolStripMenuItem = new ToolStripMenuItem();
            undoToolStripMenuItem = new ToolStripMenuItem();
            redoToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator3 = new ToolStripSeparator();
            cutToolStripMenuItem = new ToolStripMenuItem();
            copyToolStripMenuItem = new ToolStripMenuItem();
            pasteToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator4 = new ToolStripSeparator();
            selectAllToolStripMenuItem = new ToolStripMenuItem();
            toolsToolStripMenuItem = new ToolStripMenuItem();
            optionsToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator7 = new ToolStripSeparator();
            overrideXMLURIToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator6 = new ToolStripSeparator();
            viewOperationsToolStripMenuItem = new ToolStripMenuItem();
            cancelOperationToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            StatusStrip = new StatusStrip();
            XMLStatusLabel = new ToolStripStatusLabel();
            NDJStatusLabel = new ToolStripStatusLabel();
            ProgressBar = new ToolStripProgressBar();
            EditStateStatusLabel = new ToolStripStatusLabel();
            OpenFileDialog = new OpenFileDialog();
            MainTabControl = new TabControl();
            OpenTSNDJFileDialog = new OpenFileDialog();
            SaveNDJFileDialog = new SaveFileDialog();
            MainMenuStrip.SuspendLayout();
            StatusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // MainMenuStrip
            // 
            MainMenuStrip.Font = new Font("Microsoft Sans Serif", 8.25F);
            MainMenuStrip.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, editToolStripMenuItem, toolsToolStripMenuItem, helpToolStripMenuItem });
            MainMenuStrip.Location = new Point(0, 0);
            MainMenuStrip.Name = "MainMenuStrip";
            MainMenuStrip.RenderMode = ToolStripRenderMode.System;
            MainMenuStrip.Size = new Size(496, 24);
            MainMenuStrip.TabIndex = 0;
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem, openRemoteToolStripMenuItem, toolStripSeparator, saveToolStripMenuItem, saveAsToolStripMenuItem, toolStripSeparator5, attachNDJToolStripMenuItem, lockUnlockToolStripMenuItem, toolStripSeparator1, printToolStripMenuItem, printPreviewToolStripMenuItem, toolStripSeparator2, exitToolStripMenuItem });
            fileToolStripMenuItem.Font = new Font("Microsoft Sans Serif", 8.25F);
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(35, 20);
            fileToolStripMenuItem.Text = "&File";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Image = Properties.Resources.w2k_folder_open;
            openToolStripMenuItem.ImageTransparentColor = Color.Magenta;
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.O;
            openToolStripMenuItem.Size = new Size(203, 22);
            openToolStripMenuItem.Text = "&Open";
            openToolStripMenuItem.Click += OpenToolStripMenuItem_Click;
            // 
            // openRemoteToolStripMenuItem
            // 
            openRemoteToolStripMenuItem.Image = Properties.Resources.w2k_network_folders;
            openRemoteToolStripMenuItem.Name = "openRemoteToolStripMenuItem";
            openRemoteToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.O;
            openRemoteToolStripMenuItem.Size = new Size(203, 22);
            openRemoteToolStripMenuItem.Text = "Open Remote";
            openRemoteToolStripMenuItem.Click += OpenRemoteToolStripMenuItem_Click;
            // 
            // toolStripSeparator
            // 
            toolStripSeparator.Name = "toolStripSeparator";
            toolStripSeparator.Size = new Size(200, 6);
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Image = Properties.Resources.w2k_floppy_2;
            saveToolStripMenuItem.ImageTransparentColor = Color.Magenta;
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.S;
            saveToolStripMenuItem.Size = new Size(203, 22);
            saveToolStripMenuItem.Text = "&Save";
            saveToolStripMenuItem.Click += SaveToolStripMenuItem_Click;
            // 
            // saveAsToolStripMenuItem
            // 
            saveAsToolStripMenuItem.Image = Properties.Resources.w2k_unknown_20;
            saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            saveAsToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.S;
            saveAsToolStripMenuItem.Size = new Size(203, 22);
            saveAsToolStripMenuItem.Text = "Save &As";
            saveAsToolStripMenuItem.Click += SaveAsToolStripMenuItem_Click;
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new Size(200, 6);
            // 
            // attachNDJToolStripMenuItem
            // 
            attachNDJToolStripMenuItem.Image = Properties.Resources.w98_java;
            attachNDJToolStripMenuItem.Name = "attachNDJToolStripMenuItem";
            attachNDJToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.F;
            attachNDJToolStripMenuItem.Size = new Size(203, 22);
            attachNDJToolStripMenuItem.Text = "Attach NDJ";
            attachNDJToolStripMenuItem.Click += AttachNDJToolStripMenuItem_Click;
            // 
            // lockUnlockToolStripMenuItem
            // 
            lockUnlockToolStripMenuItem.Image = Properties.Resources.w2k_key_2;
            lockUnlockToolStripMenuItem.Name = "lockUnlockToolStripMenuItem";
            lockUnlockToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.L;
            lockUnlockToolStripMenuItem.Size = new Size(203, 22);
            lockUnlockToolStripMenuItem.Text = "Lock/Unlock";
            lockUnlockToolStripMenuItem.Click += LockUnlockToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(200, 6);
            // 
            // printToolStripMenuItem
            // 
            printToolStripMenuItem.Image = Properties.Resources.w2k_printer;
            printToolStripMenuItem.ImageTransparentColor = Color.Magenta;
            printToolStripMenuItem.Name = "printToolStripMenuItem";
            printToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.P;
            printToolStripMenuItem.Size = new Size(203, 22);
            printToolStripMenuItem.Text = "&Print";
            printToolStripMenuItem.Click += PrintToolStripMenuItem_Click;
            // 
            // printPreviewToolStripMenuItem
            // 
            printPreviewToolStripMenuItem.Image = Properties.Resources.w2k_search_5;
            printPreviewToolStripMenuItem.ImageTransparentColor = Color.Magenta;
            printPreviewToolStripMenuItem.Name = "printPreviewToolStripMenuItem";
            printPreviewToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.P;
            printPreviewToolStripMenuItem.Size = new Size(203, 22);
            printPreviewToolStripMenuItem.Text = "Print Pre&view";
            printPreviewToolStripMenuItem.Click += PrintPreviewToolStripMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(200, 6);
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Font = new Font("Microsoft Sans Serif", 8.25F);
            exitToolStripMenuItem.Image = Properties.Resources.w98_shell_window2;
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.ShortcutKeys = Keys.Alt | Keys.F4;
            exitToolStripMenuItem.Size = new Size(203, 22);
            exitToolStripMenuItem.Text = "E&xit";
            exitToolStripMenuItem.Click += ExitToolStripMenuItem_Click;
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { undoToolStripMenuItem, redoToolStripMenuItem, toolStripSeparator3, cutToolStripMenuItem, copyToolStripMenuItem, pasteToolStripMenuItem, toolStripSeparator4, selectAllToolStripMenuItem });
            editToolStripMenuItem.Font = new Font("Microsoft Sans Serif", 8.25F);
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new Size(37, 20);
            editToolStripMenuItem.Text = "&Edit";
            // 
            // undoToolStripMenuItem
            // 
            undoToolStripMenuItem.Image = Properties.Resources.w98_media_player_stream_conn1_inv;
            undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            undoToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Z;
            undoToolStripMenuItem.Size = new Size(162, 22);
            undoToolStripMenuItem.Text = "&Undo";
            undoToolStripMenuItem.Click += UndoToolStripMenuItem_Click;
            // 
            // redoToolStripMenuItem
            // 
            redoToolStripMenuItem.Image = Properties.Resources.w98_media_player_stream_conn1;
            redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            redoToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.Z;
            redoToolStripMenuItem.Size = new Size(162, 22);
            redoToolStripMenuItem.Text = "&Redo";
            redoToolStripMenuItem.Click += RedoToolStripMenuItem_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(159, 6);
            // 
            // cutToolStripMenuItem
            // 
            cutToolStripMenuItem.Font = new Font("Microsoft Sans Serif", 8.25F);
            cutToolStripMenuItem.Image = Properties.Resources.w2k_unknown;
            cutToolStripMenuItem.ImageTransparentColor = Color.Magenta;
            cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            cutToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.X;
            cutToolStripMenuItem.Size = new Size(162, 22);
            cutToolStripMenuItem.Text = "Cu&t";
            cutToolStripMenuItem.Click += CutToolStripMenuItem_Click;
            // 
            // copyToolStripMenuItem
            // 
            copyToolStripMenuItem.Font = new Font("Microsoft Sans Serif", 8.25F);
            copyToolStripMenuItem.Image = Properties.Resources.w2k_multiple_files;
            copyToolStripMenuItem.ImageTransparentColor = Color.Magenta;
            copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            copyToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.C;
            copyToolStripMenuItem.Size = new Size(162, 22);
            copyToolStripMenuItem.Text = "&Copy";
            copyToolStripMenuItem.Click += CopyToolStripMenuItem_Click;
            // 
            // pasteToolStripMenuItem
            // 
            pasteToolStripMenuItem.Image = Properties.Resources.w2k_write;
            pasteToolStripMenuItem.ImageTransparentColor = Color.Magenta;
            pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            pasteToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.V;
            pasteToolStripMenuItem.Size = new Size(162, 22);
            pasteToolStripMenuItem.Text = "&Paste";
            pasteToolStripMenuItem.Click += PasteToolStripMenuItem_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(159, 6);
            // 
            // selectAllToolStripMenuItem
            // 
            selectAllToolStripMenuItem.Image = Properties.Resources.w2k_default_document;
            selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            selectAllToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.A;
            selectAllToolStripMenuItem.Size = new Size(162, 22);
            selectAllToolStripMenuItem.Text = "Select &All";
            selectAllToolStripMenuItem.Click += SelectAllToolStripMenuItem_Click;
            // 
            // toolsToolStripMenuItem
            // 
            toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { optionsToolStripMenuItem, toolStripSeparator7, overrideXMLURIToolStripMenuItem, toolStripSeparator6, viewOperationsToolStripMenuItem, cancelOperationToolStripMenuItem });
            toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            toolsToolStripMenuItem.Size = new Size(45, 20);
            toolsToolStripMenuItem.Text = "&Tools";
            // 
            // optionsToolStripMenuItem
            // 
            optionsToolStripMenuItem.Image = Properties.Resources.w2k_settings;
            optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            optionsToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+,";
            optionsToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Oemcomma;
            optionsToolStripMenuItem.Size = new Size(223, 22);
            optionsToolStripMenuItem.Text = "&Options";
            optionsToolStripMenuItem.Click += OptionsToolStripMenuItem_Click;
            // 
            // toolStripSeparator7
            // 
            toolStripSeparator7.Name = "toolStripSeparator7";
            toolStripSeparator7.Size = new Size(220, 6);
            // 
            // overrideXMLURIToolStripMenuItem
            // 
            overrideXMLURIToolStripMenuItem.Image = Properties.Resources.w98_xml_gear;
            overrideXMLURIToolStripMenuItem.Name = "overrideXMLURIToolStripMenuItem";
            overrideXMLURIToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.X;
            overrideXMLURIToolStripMenuItem.Size = new Size(223, 22);
            overrideXMLURIToolStripMenuItem.Text = "Override XML URI";
            overrideXMLURIToolStripMenuItem.Click += OverrideXMLURIToolStripMenuItem_Click;
            // 
            // toolStripSeparator6
            // 
            toolStripSeparator6.Name = "toolStripSeparator6";
            toolStripSeparator6.Size = new Size(220, 6);
            // 
            // viewOperationsToolStripMenuItem
            // 
            viewOperationsToolStripMenuItem.Image = Properties.Resources.w98_processor;
            viewOperationsToolStripMenuItem.Name = "viewOperationsToolStripMenuItem";
            viewOperationsToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.T;
            viewOperationsToolStripMenuItem.Size = new Size(223, 22);
            viewOperationsToolStripMenuItem.Text = "View Operations";
            viewOperationsToolStripMenuItem.Click += ViewOperationsToolStripMenuItem_Click;
            // 
            // cancelOperationToolStripMenuItem
            // 
            cancelOperationToolStripMenuItem.Image = Properties.Resources.w2k_stop;
            cancelOperationToolStripMenuItem.Name = "cancelOperationToolStripMenuItem";
            cancelOperationToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.End;
            cancelOperationToolStripMenuItem.Size = new Size(223, 22);
            cancelOperationToolStripMenuItem.Text = "Abort";
            cancelOperationToolStripMenuItem.Click += CancelOperationToolStripMenuItem_Click;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { aboutToolStripMenuItem });
            helpToolStripMenuItem.Font = new Font("Microsoft Sans Serif", 8.25F);
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(41, 20);
            helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Font = new Font("Microsoft Sans Serif", 8.25F);
            aboutToolStripMenuItem.Image = Properties.Resources.w2k_help;
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.F1;
            aboutToolStripMenuItem.Size = new Size(180, 22);
            aboutToolStripMenuItem.Text = "&About...";
            aboutToolStripMenuItem.Click += AboutToolStripMenuItem_Click;
            // 
            // StatusStrip
            // 
            StatusStrip.Font = new Font("Microsoft Sans Serif", 8.25F);
            StatusStrip.GripStyle = ToolStripGripStyle.Visible;
            StatusStrip.Items.AddRange(new ToolStripItem[] { XMLStatusLabel, NDJStatusLabel, ProgressBar, EditStateStatusLabel });
            StatusStrip.Location = new Point(0, 707);
            StatusStrip.Name = "StatusStrip";
            StatusStrip.ShowItemToolTips = true;
            StatusStrip.Size = new Size(496, 22);
            StatusStrip.TabIndex = 1;
            // 
            // XMLStatusLabel
            // 
            XMLStatusLabel.AutoSize = false;
            XMLStatusLabel.Font = new Font("Microsoft Sans Serif", 8.25F);
            XMLStatusLabel.Image = Properties.Resources.w98_xml;
            XMLStatusLabel.ImageAlign = ContentAlignment.MiddleLeft;
            XMLStatusLabel.IsLink = true;
            XMLStatusLabel.LinkBehavior = LinkBehavior.AlwaysUnderline;
            XMLStatusLabel.Name = "XMLStatusLabel";
            XMLStatusLabel.Overflow = ToolStripItemOverflow.Never;
            XMLStatusLabel.Size = new Size(180, 17);
            XMLStatusLabel.Spring = true;
            XMLStatusLabel.Text = "None";
            XMLStatusLabel.TextAlign = ContentAlignment.MiddleLeft;
            XMLStatusLabel.ToolTipText = "Current TSXML File";
            XMLStatusLabel.VisitedLinkColor = Color.Blue;
            XMLStatusLabel.Click += XMLStatusLabel_Click;
            // 
            // NDJStatusLabel
            // 
            NDJStatusLabel.AutoSize = false;
            NDJStatusLabel.Font = new Font("Microsoft Sans Serif", 8.25F);
            NDJStatusLabel.Image = Properties.Resources.w98_java;
            NDJStatusLabel.ImageAlign = ContentAlignment.MiddleLeft;
            NDJStatusLabel.IsLink = true;
            NDJStatusLabel.LinkBehavior = LinkBehavior.AlwaysUnderline;
            NDJStatusLabel.Name = "NDJStatusLabel";
            NDJStatusLabel.Overflow = ToolStripItemOverflow.Never;
            NDJStatusLabel.Size = new Size(180, 17);
            NDJStatusLabel.Spring = true;
            NDJStatusLabel.Text = "None";
            NDJStatusLabel.TextAlign = ContentAlignment.MiddleLeft;
            NDJStatusLabel.ToolTipText = "Current TSNDJ File";
            NDJStatusLabel.VisitedLinkColor = Color.Blue;
            NDJStatusLabel.Click += NDJStatusLabel_Click;
            // 
            // ProgressBar
            // 
            ProgressBar.ForeColor = Color.PaleGreen;
            ProgressBar.Name = "ProgressBar";
            ProgressBar.Size = new Size(100, 16);
            // 
            // EditStateStatusLabel
            // 
            EditStateStatusLabel.BackgroundImage = Properties.Resources.w98_file_question;
            EditStateStatusLabel.DisplayStyle = ToolStripItemDisplayStyle.Image;
            EditStateStatusLabel.Image = Properties.Resources.w98_file_question;
            EditStateStatusLabel.Margin = new Padding(0, 3, 2, 2);
            EditStateStatusLabel.Name = "EditStateStatusLabel";
            EditStateStatusLabel.Size = new Size(16, 17);
            EditStateStatusLabel.ToolTipText = "No file selected";
            // 
            // OpenFileDialog
            // 
            OpenFileDialog.DefaultExt = "tsndj";
            OpenFileDialog.Filter = "TS files|*.tsndj;*.tsxml|All files|*.*";
            OpenFileDialog.Multiselect = true;
            // 
            // MainTabControl
            // 
            MainTabControl.Dock = DockStyle.Fill;
            MainTabControl.Location = new Point(0, 24);
            MainTabControl.Name = "MainTabControl";
            MainTabControl.SelectedIndex = 0;
            MainTabControl.Size = new Size(496, 683);
            MainTabControl.TabIndex = 2;
            MainTabControl.Selected += MainTabControl_Selected;
            // 
            // OpenTSNDJFileDialog
            // 
            OpenTSNDJFileDialog.DefaultExt = "tsndj";
            OpenTSNDJFileDialog.Filter = "TSNDJ files|*.tsndj|All files|*.*";
            // 
            // SaveNDJFileDialog
            // 
            SaveNDJFileDialog.DefaultExt = "tsndj";
            SaveNDJFileDialog.Filter = "TSNDJ Files|*.tsndj|All files|*.*";
            // 
            // Viewer
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(496, 729);
            Controls.Add(MainTabControl);
            Controls.Add(StatusStrip);
            Controls.Add(MainMenuStrip);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(256, 256);
            Name = "Viewer";
            Text = "TSXMLEdit";
            FormClosing += Viewer_FormClosing;
            ResizeBegin += Viewer_ResizeBegin;
            ResizeEnd += Viewer_ResizeEnd;
            MainMenuStrip.ResumeLayout(false);
            MainMenuStrip.PerformLayout();
            StatusStrip.ResumeLayout(false);
            StatusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip MainMenuStrip;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem printToolStripMenuItem;
        private ToolStripMenuItem printPreviewToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem undoToolStripMenuItem;
        private ToolStripMenuItem redoToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem cutToolStripMenuItem;
        private ToolStripMenuItem copyToolStripMenuItem;
        private ToolStripMenuItem pasteToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem selectAllToolStripMenuItem;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem optionsToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private StatusStrip StatusStrip;
        private ToolStripStatusLabel XMLStatusLabel;
        private ToolStripProgressBar ProgressBar;
        private ToolStripStatusLabel NDJStatusLabel;
        private ToolStripMenuItem cancelOperationToolStripMenuItem;
        private ToolStripStatusLabel EditStateStatusLabel;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripMenuItem lockUnlockToolStripMenuItem;
        public ToolTip ToolTip;
        private OpenFileDialog OpenFileDialog;
        private ToolStripMenuItem openRemoteToolStripMenuItem;
        private TabControl MainTabControl;
        private OpenFileDialog OpenTSNDJFileDialog;
        private ToolStripMenuItem attachNDJToolStripMenuItem;
        private SaveFileDialog SaveNDJFileDialog;
        private ToolStripMenuItem viewOperationsToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripMenuItem overrideXMLURIToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator7;
    }
}