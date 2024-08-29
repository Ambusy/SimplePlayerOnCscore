namespace Player
{
    partial class Player
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Player));
            this.clearList = new System.Windows.Forms.Button();
            this.incMaps = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.spectrumBox = new System.Windows.Forms.PictureBox();
            this.buttonPlay = new System.Windows.Forms.Button();
            this.columnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.bBrowseD = new System.Windows.Forms.Button();
            this.timeSong = new System.Windows.Forms.Label();
            this.curTimeSong = new System.Windows.Forms.Label();
            this.PlayBar = new System.Windows.Forms.TrackBar();
            this.labelVol = new System.Windows.Forms.Label();
            this.Volume = new System.Windows.Forms.TrackBar();
            this.labelTotalTime = new System.Windows.Forms.Label();
            this.columnMap = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnArtist = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.totalTime = new System.Windows.Forms.Label();
            this.columnTrack = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.playList = new System.Windows.Forms.ListView();
            this.columnDuration = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.bBrowseF = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.spectrumBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PlayBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Volume)).BeginInit();
            this.SuspendLayout();
            // 
            // clearList
            // 
            this.clearList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.clearList.Location = new System.Drawing.Point(1194, 37);
            this.clearList.Margin = new System.Windows.Forms.Padding(4);
            this.clearList.Name = "clearList";
            this.clearList.Size = new System.Drawing.Size(129, 38);
            this.clearList.TabIndex = 32;
            this.clearList.Text = "CL";
            this.clearList.UseVisualStyleBackColor = true;
            this.clearList.Visible = false;
            this.clearList.Click += new System.EventHandler(this.clearList_Click);
            // 
            // incMaps
            // 
            this.incMaps.AutoSize = true;
            this.incMaps.Location = new System.Drawing.Point(435, 47);
            this.incMaps.Margin = new System.Windows.Forms.Padding(4);
            this.incMaps.Name = "incMaps";
            this.incMaps.Size = new System.Drawing.Size(42, 21);
            this.incMaps.TabIndex = 31;
            this.incMaps.Text = "IS";
            this.incMaps.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Red;
            this.label1.ForeColor = System.Drawing.Color.Yellow;
            this.label1.Location = new System.Drawing.Point(4, 451);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(309, 17);
            this.label1.TabIndex = 35;
            this.label1.Text = "Unregistered version, click here to register free.";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(466, 451);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(144, 17);
            this.label2.TabIndex = 36;
            this.label2.Text = "Insert registration key";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // spectrumBox
            // 
            this.spectrumBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.spectrumBox.Location = new System.Drawing.Point(6, 474);
            this.spectrumBox.Margin = new System.Windows.Forms.Padding(4);
            this.spectrumBox.Name = "spectrumBox";
            this.spectrumBox.Size = new System.Drawing.Size(1237, 161);
            this.spectrumBox.TabIndex = 30;
            this.spectrumBox.TabStop = false;
            // 
            // buttonPlay
            // 
            this.buttonPlay.Image = ((System.Drawing.Image)(resources.GetObject("buttonPlay.Image")));
            this.buttonPlay.Location = new System.Drawing.Point(6, 37);
            this.buttonPlay.Margin = new System.Windows.Forms.Padding(4);
            this.buttonPlay.Name = "buttonPlay";
            this.buttonPlay.Size = new System.Drawing.Size(97, 38);
            this.buttonPlay.TabIndex = 23;
            this.buttonPlay.UseVisualStyleBackColor = true;
            this.buttonPlay.Visible = false;
            this.buttonPlay.Click += new System.EventHandler(this.buttonPlay_Click);
            // 
            // columnName
            // 
            this.columnName.Text = "NM";
            this.columnName.Width = 250;
            // 
            // bBrowseD
            // 
            this.bBrowseD.Location = new System.Drawing.Point(298, 37);
            this.bBrowseD.Margin = new System.Windows.Forms.Padding(4);
            this.bBrowseD.Name = "bBrowseD";
            this.bBrowseD.Size = new System.Drawing.Size(129, 38);
            this.bBrowseD.TabIndex = 29;
            this.bBrowseD.Text = "BD";
            this.bBrowseD.UseVisualStyleBackColor = true;
            this.bBrowseD.Click += new System.EventHandler(this.bBrowseD_Click);
            // 
            // timeSong
            // 
            this.timeSong.AutoSize = true;
            this.timeSong.Location = new System.Drawing.Point(734, 411);
            this.timeSong.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.timeSong.Name = "timeSong";
            this.timeSong.Size = new System.Drawing.Size(41, 17);
            this.timeSong.TabIndex = 28;
            this.timeSong.Text = "-:--:--";
            // 
            // curTimeSong
            // 
            this.curTimeSong.AutoSize = true;
            this.curTimeSong.Location = new System.Drawing.Point(2, 411);
            this.curTimeSong.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.curTimeSong.Name = "curTimeSong";
            this.curTimeSong.Size = new System.Drawing.Size(41, 17);
            this.curTimeSong.TabIndex = 27;
            this.curTimeSong.Text = "-:--:--";
            // 
            // PlayBar
            // 
            this.PlayBar.Location = new System.Drawing.Point(59, 411);
            this.PlayBar.Margin = new System.Windows.Forms.Padding(4);
            this.PlayBar.Maximum = 1000;
            this.PlayBar.Name = "PlayBar";
            this.PlayBar.Size = new System.Drawing.Size(693, 56);
            this.PlayBar.SmallChange = 50;
            this.PlayBar.TabIndex = 26;
            this.PlayBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.PlayBar.Scroll += new System.EventHandler(this.PlayBar_Scroll);
            this.PlayBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PlayBar_MouseUp);
            // 
            // labelVol
            // 
            this.labelVol.AutoSize = true;
            this.labelVol.Location = new System.Drawing.Point(1108, 439);
            this.labelVol.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelVol.Name = "labelVol";
            this.labelVol.Size = new System.Drawing.Size(28, 17);
            this.labelVol.TabIndex = 25;
            this.labelVol.Text = "VO";
            // 
            // Volume
            // 
            this.Volume.Location = new System.Drawing.Point(1251, 458);
            this.Volume.Margin = new System.Windows.Forms.Padding(4);
            this.Volume.Maximum = 100;
            this.Volume.Name = "Volume";
            this.Volume.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.Volume.Size = new System.Drawing.Size(56, 206);
            this.Volume.TabIndex = 24;
            this.Volume.TickStyle = System.Windows.Forms.TickStyle.None;
            this.Volume.Scroll += new System.EventHandler(this.Volume_Scroll);
            // 
            // labelTotalTime
            // 
            this.labelTotalTime.AutoSize = true;
            this.labelTotalTime.Location = new System.Drawing.Point(932, 411);
            this.labelTotalTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTotalTime.Name = "labelTotalTime";
            this.labelTotalTime.Size = new System.Drawing.Size(26, 17);
            this.labelTotalTime.TabIndex = 22;
            this.labelTotalTime.Text = "TT";
            // 
            // columnMap
            // 
            this.columnMap.Text = "MP";
            this.columnMap.Width = 250;
            // 
            // columnArtist
            // 
            this.columnArtist.Text = "AR";
            this.columnArtist.Width = 200;
            // 
            // totalTime
            // 
            this.totalTime.AutoSize = true;
            this.totalTime.Location = new System.Drawing.Point(1015, 411);
            this.totalTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.totalTime.Name = "totalTime";
            this.totalTime.Size = new System.Drawing.Size(60, 17);
            this.totalTime.TabIndex = 21;
            this.totalTime.Text = "0:00:00 ";
            // 
            // columnTrack
            // 
            this.columnTrack.Text = "TR";
            this.columnTrack.Width = 200;
            // 
            // timer1
            // 
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // playList
            // 
            this.playList.AllowColumnReorder = true;
            this.playList.AllowDrop = true;
            this.playList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnName,
            this.columnMap,
            this.columnArtist,
            this.columnDuration,
            this.columnTrack});
            this.playList.Location = new System.Drawing.Point(6, 83);
            this.playList.Margin = new System.Windows.Forms.Padding(4);
            this.playList.MultiSelect = false;
            this.playList.Name = "playList";
            this.playList.Size = new System.Drawing.Size(1316, 315);
            this.playList.TabIndex = 20;
            this.playList.UseCompatibleStateImageBehavior = false;
            this.playList.View = System.Windows.Forms.View.Details;
            this.playList.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.playList_ColumnClick);
            this.playList.ItemMouseHover += new System.Windows.Forms.ListViewItemMouseHoverEventHandler(this.playList_ListViewItemMouseHover);
            this.playList.SelectedIndexChanged += new System.EventHandler(this.playList_SelectedIndexChanged);
            this.playList.Click += new System.EventHandler(this.playList_Click);
            this.playList.DragDrop += new System.Windows.Forms.DragEventHandler(this.playList_DragDrop);
            this.playList.DragEnter += new System.Windows.Forms.DragEventHandler(this.playList_DragEnter);
            this.playList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.playList_MouseDown);
            // 
            // columnDuration
            // 
            this.columnDuration.Text = "DU";
            this.columnDuration.Width = 180;
            // 
            // bBrowseF
            // 
            this.bBrowseF.Location = new System.Drawing.Point(148, 37);
            this.bBrowseF.Margin = new System.Windows.Forms.Padding(4);
            this.bBrowseF.Name = "bBrowseF";
            this.bBrowseF.Size = new System.Drawing.Size(129, 38);
            this.bBrowseF.TabIndex = 19;
            this.bBrowseF.Text = "BF";
            this.bBrowseF.UseVisualStyleBackColor = true;
            this.bBrowseF.Click += new System.EventHandler(this.bBrowseF_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1324, 701);
            this.Controls.Add(this.clearList);
            this.Controls.Add(this.incMaps);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.spectrumBox);
            this.Controls.Add(this.buttonPlay);
            this.Controls.Add(this.bBrowseD);
            this.Controls.Add(this.timeSong);
            this.Controls.Add(this.curTimeSong);
            this.Controls.Add(this.PlayBar);
            this.Controls.Add(this.labelVol);
            this.Controls.Add(this.Volume);
            this.Controls.Add(this.labelTotalTime);
            this.Controls.Add(this.totalTime);
            this.Controls.Add(this.playList);
            this.Controls.Add(this.bBrowseF);
            this.Name = "Form1";
            this.Text = "MM player";
            this.Activated += new System.EventHandler(this.Form1_Resize);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.spectrumBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PlayBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Volume)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button clearList;
        private System.Windows.Forms.CheckBox incMaps;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox spectrumBox;
        private System.Windows.Forms.Button buttonPlay;
        private System.Windows.Forms.ColumnHeader columnName;
        private System.Windows.Forms.Button bBrowseD;
        private System.Windows.Forms.Label timeSong;
        private System.Windows.Forms.Label curTimeSong;
        private System.Windows.Forms.TrackBar PlayBar;
        private System.Windows.Forms.Label labelVol;
        private System.Windows.Forms.TrackBar Volume;
        private System.Windows.Forms.Label labelTotalTime;
        private System.Windows.Forms.ColumnHeader columnMap;
        private System.Windows.Forms.ColumnHeader columnArtist;
        private System.Windows.Forms.Label totalTime;
        private System.Windows.Forms.ColumnHeader columnTrack;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ListView playList;
        private System.Windows.Forms.ColumnHeader columnDuration;
        private System.Windows.Forms.Button bBrowseF;
    }
}

