using System;
using System.Drawing; // Point, Color
using System.Globalization; // CultureInfo
using System.Windows.Forms;
using System.IO; // FileInfo, DirectoryInfo
using System.Diagnostics; // Process
using Microsoft.Win32; // Registry
using CSCore;
using CSCore.Codecs; // CodeFactory
using CSCore.SoundOut; // WasapiOut
using CSCore.MediaFoundation; // Decoder
using CSCore.Tags.ID3; // ID3V..
using CSCore.Tags.ID3.Frames; // MultiStringTextFrame
using CSCore.DSP; // FFTsize
using CSCore.Streams; // SingleBlockNotificationStream 
using CSCore.Streams.Effects;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace Player
{
    public partial class Player : Form
    {
        int ActiveFormHeight=100, ActiveFormWidth=100;
        TimeSpan totalTimeC = new TimeSpan(0,0,0);
        int ItemPlaying = 0;
        ISoundOut _soundOut = new WasapiOut();
        IWaveSource _source; // source playing
        IWaveSource _sourceNew; // source waiting to play
        MediaFoundationDecoder mediafoundationDecoder = null;
        LineSpectrum _lineSpectrum;
        int ListviewHoverIndex =0;
        MouseButtons ListviewButton;
        Point ListViewXY;
        int playState = 0; // stopped
        bool EndOfStream = false;
        bool AlmostEndOfStream = false;
        bool newStreamBuild = false;
        string nameFilePlaying;
        bool FirstFileInList = true;
        bool ProgramChangesSelection =false;
        private ListViewColumnSorter lvwColumnSorter;
        public BasicSpectrumProvider spectrumProvider;
        public Player()
        {
            InitializeComponent();
            lvwColumnSorter = new ListViewColumnSorter();
        }

        void Form1_Load(object sender, EventArgs e)
        {
            this.bBrowseF.Text = TranslateLocale("BF"); // translate strings on form
            this.columnName.Text = TranslateLocale("NM");
            this.columnMap.Text = TranslateLocale("MP");
            this.columnArtist.Text = TranslateLocale("AR");
            this.columnTrack.Text = TranslateLocale("TR");
            this.columnDuration.Text = TranslateLocale("DU");
            this.labelTotalTime.Text = TranslateLocale("TT");
            this.labelVol.Text = TranslateLocale("VO");
            this.bBrowseD.Text = TranslateLocale("BD");
            this.incMaps.Text = TranslateLocale("IS");
            this.clearList.Text = TranslateLocale("CL");
            try // get data stored in Registry about VOLUME
            {
                Volume.Value = Convert.ToInt32(GetRegistryKey("Player", "Volume", "100"));
            }
            catch
            {
                Volume.Value = 100;
            }
            try // get data stored in Registry about KEY
            {
                string res = GetRegistryKey("Player", "Key", "");
                if (res == "AB-913-GH")
                {
                    label1.Visible = false;
                    label2.Visible = false;
                }
            }
            catch
            {
                ;
            }
        }
        private void label2_Click(object sender, EventArgs e)
        {
            string res = Microsoft.VisualBasic.Interaction.InputBox("Enter Key", "registration key", "");
            if (res == "AB-913-GH")
            {
                SaveRegistryKey("Player", "Key", res);
                label1.Visible = false;
                label2.Visible = false;
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {
            StreamWriter ofl = new StreamWriter(Application.LocalUserAppDataPath + "\\registerPlayer.html");
            ofl.WriteLine("<!DOCTYPE HTML>" +
                "<html>" +
                "    <head>" +
                "        <meta charset='UTF-8'>" +
                "        <meta http-equiv='refresh' content='1';url='http://digilander.libero.it/ambusy/keyPlayer.html'>" +
                "        <script type='text/javascript'>" +
                "            window.location.href = 'http://digilander.libero.it/ambusy/keyPlayer.html'" +
                "        </script>" +
                "        <title>x</title>" +
                "    </head>" +
                "    <body>" +
                "         Redirecting...." +
                "</html>");
            ofl.Close();
            ofl.Dispose();
            Process myProcess = new Process();
            try
            {
                myProcess.StartInfo.UseShellExecute = true;
                myProcess.StartInfo.FileName = Application.LocalUserAppDataPath + "\\registerPlayer.html";
                myProcess.StartInfo.CreateNoWindow = true;
                myProcess.Start();
            }
            catch  
            {
            }
        }
        void Form1_Resize(object sender, EventArgs e)
        {
                resizeForm(e);
        }
        void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (playState > 0)
            {
                _soundOut.Stop();
            }
            _soundOut.Dispose();
        }
        void resizeForm(EventArgs e)
        {
            ActiveFormWidth = this.ClientSize.Width;
            ActiveFormHeight = this.ClientSize.Height;
            playList.Width = ActiveFormWidth - 60;
            columnName.Width = playList.Width * 30 / 100;
            columnArtist.Width = playList.Width * 28 / 100;
            columnMap.Width = playList.Width * 27 / 100;
            columnDuration.Width = playList.Width * 97 / 1000;
            columnTrack.Width = playList.Width * 5 / 100;
            totalTime.Left = playList.Left + playList.Width - columnDuration.Width;
            labelTotalTime.Left = totalTime.Left - labelTotalTime.Width;
            Volume.Height = ActiveFormHeight - (playList.Top + playList.Height) - 85;
            Volume.Left = playList.Width + 1;
            labelVol.Left = playList.Width + 1;
        }
        private void bBrowseD_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog SelectMaps = new FolderBrowserDialog();
            SelectMaps.RootFolder =  Environment.SpecialFolder.Desktop;
            SelectMaps.SelectedPath = GetRegistryKey("Player", "Dir", String.Empty);
            SelectMaps.ShowNewFolderButton = false;
            if (SelectMaps.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                processEntireMap(SelectMaps.SelectedPath, incMaps.Checked);
            }
        }
        void processEntireMap(string path, bool all)
        {
            string[] files;
            Cursor.Current = Cursors.WaitCursor; 
            if (all)
                files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            else
                files = Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly);
            foreach (string name in files)
            {
                addOneFileToList(name);
            }
            Cursor.Current = Cursors.Default; 
        }
        void bBrowseF_Click(object sender, EventArgs e)
        {
            OpenFileDialog SelectFiles = new OpenFileDialog();
            SelectFiles.Filter = TranslateLocale("FI");
            SelectFiles.Title = TranslateLocale("TI"); 
            SelectFiles.FileName = GetRegistryKey("Player", "File", String.Empty);  
            SelectFiles.CheckFileExists = true;
            SelectFiles.InitialDirectory = GetRegistryKey("Player", "Dir", String.Empty);
            SelectFiles.Multiselect = true;
            if (SelectFiles.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (string name in SelectFiles.FileNames ){
                    addOneFileToList(name);
                }
                buttonPlay.Visible = true;
            }
        }
        private void clearList_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            stopPlay();
            clearList.Visible = false;
            playList.Items.Clear();
            curTimeSong.Text = "0:00:00";
            totalTimeC = new TimeSpan(0, 0, 0);
            timeSong.Text = "";
            totalTime.Text = " ";
            buttonPlay.Visible = false;
            buttonPlay.Image = global::Player.Properties.Resources.play;
        }
        void addOneFileToList(string name)
        {
            try // valid MF file?
            {
                mediafoundationDecoder = new MediaFoundationDecoder(name);
            }
            catch
            {
                return; // no: omit
            }
            FileInfo fi = new FileInfo(name);
            String Title, Artist, Track;
            Title = "";
            Artist ="";
            Track = "";
            ID3v2 tag2 = ID3v2.FromFile(name);
            if (tag2 == null)
            {
                ID3v1 tag = ID3v1.FromFile(name);
                if (tag != null)
                {
                    Title = tag.Title;
                    Artist = tag.Artist + " " + tag.Album;
                    String Comment;
                    Comment = tag.Comment;
                    if (Comment.Length == 30)
                        if (Comment[27] == 0)
                            Track = (Comment[28] * 256 + Comment[29]).ToString();
                }
            }
            else
            {
                Artist = getTag(tag2, "TPE1")+ " " + getTag(tag2, "TALB");
                Track = getTag(tag2, "TRCK");
                Title =  getTag(tag2, "TIT2");

            }
            if (Title == "") Title = fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length);
            if (Track != "")
            {
                while (Track.Length < 3) Track = " " + Track;
            }
            var item = new ListViewItem(Title);
            item.Tag = fi.FullName;
            item.SubItems.Add(fi.DirectoryName);
            item.SubItems.Add(Artist);
            item.ToolTipText = fi.DirectoryName;
            TimeSpan le = mediafoundationDecoder.GetLength();
            totalTimeC += le;
            string totTimeS = totalTimeC.ToString();
            int totTimePP = totTimeS.LastIndexOf('.');
            if (totTimePP > -1)  
                    totalTime.Text = totTimeS.Substring(0, totTimePP);
            else
             totalTime.Text = totTimeS;
            item.SubItems.Add(le.ToString().Substring(0, 8));
            item.SubItems.Add(Track);
            playList.Items.Add(item);
            if (FirstFileInList)
            {
                FirstFileInList = false;
                SaveRegistryKey("Player", "File", fi.FullName);
                SaveRegistryKey("Player", "Dir", fi.DirectoryName);
                timeSong.Text = le.ToString().Substring(0, 8);
                curTimeSong.Text = "0:00:00";
            }
            buttonPlay.Visible = true;
            clearList.Visible = true;
        }
        string getTag(ID3v2 tag, string id) {
            MultiStringTextFrame tf = (MultiStringTextFrame)tag[id];
            if (tf == null)
                return ("");
            else
                return tf.Text;
        }
        void buttonPlay_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
            {
                if (playState == 0) // stop
                {
                    startPlay(1);
                }
                else if (playState == 1) // play 
                {
                    _soundOut.Pause();
                    playState = 2;
                    buttonPlay.Image = global::Player.Properties.Resources.play;
                }
                else if (playState == 2) // pause
                {
                    _soundOut.Resume();
                    playState = 1;
                    buttonPlay.Image = global::Player.Properties.Resources.pause;
                }
            }
        }
        void startPlay(int NextItemToBePlayed)
        {
            if (playState > 0)
                stopPlay();
            ItemPlaying = NextItemToBePlayed;
            ProgramChangesSelection = true;
            playList.Items[ItemPlaying - 1].Selected = true;
            playList.Items[ItemPlaying - 1].ForeColor = Color.Red;
            playList.EnsureVisible(ItemPlaying - 1);
            playList.Items[ItemPlaying - 1].Selected = false;
            Application.DoEvents();
            base.Text = playList.Items[ItemPlaying - 1].Text;
            ProgramChangesSelection = false;
            EndOfStream = false;
            AlmostEndOfStream = false;
            if (newStreamBuild) {
                _source = _sourceNew;
            }
            else {
                _source =createSource(false);
            }
            newStreamBuild = false;
            _soundOut.Initialize(_source);
            _soundOut.Volume = (float)Volume.Value / 100;
            _soundOut.Play();
            playState = 1;
            buttonPlay.Image = global::Player.Properties.Resources.pause;
            TimeSpan le = _source.GetLength();
            timeSong.Text = le.ToString().Substring(0, 8);
            curTimeSong.Text = "0:00:00";
            timer1.Enabled = true;
        }
        IWaveSource createSource(bool next)
        {
            ListViewItem item;
            if (next)
                item = playList.Items[ItemPlaying];
            else
                item = playList.Items[ItemPlaying - 1];
           nameFilePlaying = item.Tag.ToString();
            var _source = CodecFactory.Instance.GetCodec(nameFilePlaying);
            ISampleSource source = _source.ToSampleSource();
            const FftSize fftSize = FftSize.Fft32768;
            //create a spectrum provider which provides fft data based on some input
            spectrumProvider = new BasicSpectrumProvider(source.WaveFormat.Channels,
                source.WaveFormat.SampleRate, fftSize);

            //linespectrum used for rendering some fft data
            //in oder to get some fft data, set the previously created spectrumprovider 
            _lineSpectrum = new LineSpectrum(fftSize, spectrumProvider)
            {
                BarCount = 150,
                BarSpacing = 1
            };           
            //the SingleBlockNotificationStream is used to intercept the played samples
            var notificationSource = new SingleBlockNotificationStream(source,150000);
            notificationSource.SingleBlockRead += (s, a) => spectrumProvider.Add(a.Left, a.Right);
            notificationSource.SingleBlockStreamAlmostFinished += (s, a) => AlmostEndOfStream = true;
            notificationSource.SingleBlockStreamFinished += (s, a) => EndOfStream = true;
            return notificationSource.ToWaveSource(16);
        }
        void stopPlay()
        {
            if (playState > 0)
            {
                ListViewItem item = playList.Items[ItemPlaying - 1];
                item.SubItems[0].ForeColor = Color.Black;
                //item.SubItems[3].Text = DurationCurrentSong;
                _soundOut.Stop();
                playState = 0;
                buttonPlay.Image = global::Player.Properties.Resources.play;
                curTimeSong.Text = "0:00:00";
            }
            SaveRegistryKey("Player", "Volume", Volume.Value.ToString());
        }
        Boolean working = false;
        void timer_Tick(object sender, EventArgs e)
        {
            if (working) return;
            working = true;
            if (EndOfStream)
            {
                EndOfStream = false;
                if (ItemPlaying < playList.Items.Count)
                {
                    startPlay(ItemPlaying + 1);
                }
                else
                {
                    stopPlay();
                    timer1.Enabled = false;
                    PlayBar.Value = 0;
                }
            }
            else {
                TimeSpan p = _soundOut.WaveSource.GetPosition();
                TimeSpan le = _soundOut.WaveSource.GetLength();
                int secsPos = p.Seconds + p.Minutes * 60 + p.Hours * 3600;
                int secsSong = le.Seconds + le.Minutes * 60 + le.Hours * 3600;
                long curSec;
                if (secsSong > 0)
                    curSec = secsPos * 1000 / secsSong;
                else
                    curSec = 1;
                PlayBar.Value = Math.Max(PlayBar.Minimum,Math.Min(PlayBar.Maximum, (int)curSec));
                curTimeSong.Text = p.ToString().Substring(0, 8);
                GenerateLineSpectrum();
                if (AlmostEndOfStream && !newStreamBuild && ItemPlaying < playList.Items.Count)
                {
                    newStreamBuild = true;
                    _sourceNew = createSource(true);
                }
            }
            working = false;
         }
        private void GenerateLineSpectrum()
        {
            Image image = spectrumBox.Image;
            var newImage = _lineSpectrum.CreateSpectrumLine(spectrumBox.Size, Color.Green, Color.Red, Color.Black, true);
            if (newImage != null)
            {
                spectrumBox.Image = newImage;
                if (image != null)
                    image.Dispose();
            }
        }
        float GetVolume()
        {
            return _soundOut.Volume;
        }

        private void Volume_Scroll(object sender, EventArgs e)
        {
            AdjustVolume();
        }
        void AdjustVolume()
        {
            if (playState > 0)
                _soundOut.Volume = (float)Volume.Value / 100;
        }

        string GetRegistryKey(string KeyName, string ValueName, string Defaultval)
        {
            string readValue;
            string GetRegistryKey;
            if (ValidKeyName(KeyName))
            {
                readValue = (string)Registry.GetValue(@"HKEY_CURRENT_USER\Software\AMBusy\" + KeyName, ValueName, null);
                if (!(readValue == null))
                    GetRegistryKey = readValue;
                else
                    GetRegistryKey = Defaultval;
            }
            else
                GetRegistryKey = Defaultval;
            return GetRegistryKey;
        }
        void SaveRegistryKey(string KeyName, string ValueName, string ValueData)
        {
            if (ValidKeyName(KeyName))
            {
                Registry.CurrentUser.CreateSubKey(@"Software\AMBusy\" + KeyName);
                Registry.SetValue(@"HKEY_CURRENT_USER\Software\AMBusy\" + KeyName, ValueName, ValueData);
            }
        }
        bool ValidKeyName(string KeyName)
        {
            // A key name is invalid if it begins or ends with \ or contains \\
            if (!KeyName.StartsWith(@"\"))
                if (!KeyName.EndsWith(@"\"))
                    if (!KeyName.Contains(@"\\"))
                        return true;
            return false;
        }

        private void PlayBar_Scroll(object sender, EventArgs e)
        {
            if (playState > 0)
            {
                TimeSpan le = _soundOut.WaveSource.GetLength();
                int secsSong = le.Seconds + le.Minutes * 60 + le.Hours * 3600;
                int secs = secsSong * PlayBar.Value / 1000;
                _soundOut.WaveSource.SetPosition(new TimeSpan(0, 0, secs));
            }
        }
        private void PlayBar_MouseUp(object sender, MouseEventArgs e)
        {
            if (playState > 0)
            {
                TimeSpan le = _soundOut.WaveSource.GetLength();
                int secsSong = le.Seconds + le.Minutes * 60 + le.Hours * 3600;
                int pos = Math.Max(1,Math.Min(500,(e.X)-10));
                int secs = secsSong * pos / 500;
                _soundOut.WaveSource.SetPosition(new TimeSpan(0, 0, secs));
            }
        }
        void playList_Click(object sender, EventArgs e)
        {
            if (ListviewButton == MouseButtons.Right)
            {
                // show popup
                ContextMenuStrip contextMenuStrip= new ContextMenuStrip();
                ToolStripMenuItem popupPlay = new ToolStripMenuItem(TranslateLocale("PL"), null, new EventHandler(PopupPlay_Click));
                contextMenuStrip.Items.Add(popupPlay);
                ToolStripMenuItem popupDel = new ToolStripMenuItem(TranslateLocale("DL"), null, new EventHandler(PopupDelete_Click));
                contextMenuStrip.Items.Add(popupDel);
                ToolStripMenuItem popupIE = new ToolStripMenuItem(TranslateLocale("IE"), null, new EventHandler(PopupDiscogs_Click));
                contextMenuStrip.Items.Add(popupIE);
                contextMenuStrip.Show(ListViewXY);
                contextMenuStrip.BringToFront();
            }
       }
        void PopupPlay_Click(object sender, EventArgs e)
        {
            newStreamBuild = false;
            _sourceNew = null;
            startPlay(ListviewHoverIndex + 1);
        }
        void PopupDelete_Click(object sender, EventArgs e)
        {
            if (ListviewHoverIndex + 1 == ItemPlaying)
                stopPlay();
            string duration = playList.Items[ListviewHoverIndex].SubItems[3].Text;
            TimeSpan le = new TimeSpan(Convert.ToInt32(duration.Substring(0, 2)), Convert.ToInt32(duration.Substring(3, 2)), Convert.ToInt32(duration.Substring(6, 2)));
            totalTimeC -= le;
            playList.Items.RemoveAt(ListviewHoverIndex);
            if (ListviewHoverIndex + 1 == ItemPlaying)
            {
                newStreamBuild = false;
                _sourceNew = null;
                startPlay(ItemPlaying);
            }
        }
        void PopupDiscogs_Click(object sender, EventArgs e)
        {
            StreamWriter ofl = new StreamWriter(Application.LocalUserAppDataPath + "\\discogs.html");
            string s = "http://www.discogs.com/search/?q=";
            string st = playList.Items[ListviewHoverIndex].SubItems[2].Text;
            if (st.Trim() == "")  st = playList.Items[ListviewHoverIndex].SubItems[0].Text;
            string w = "";
            for (int i = 0; i < st.Length; i++)
            {
                if (Char.IsNumber(st[i]) | Char.IsLetter(st[i]))
                {
                    w += st[i];
                }
                else if (st[i] == '\'')  
                {
                    w += "%27";
                }
                else
                {
                    if (w.Length > 0) s += w + "+";
                    w = "";
                }
            }
            s += w + "&type=all";
            ofl.WriteLine("<!DOCTYPE HTML>" +
                "<html>" +
                "    <head>" +
                "        <meta charset='UTF-8'>" +
                "        <meta http-equiv='refresh' content='1';url='"+s+"'>" +
                "        <script type='text/javascript'>" +
                "            window.location.href = '" + s + "'" +
                "        </script>" +
                "        <title>x</title>" +
                "    </head>" +
                "    <body>" +
                "         Redirecting...." +
                "</html>");
            ofl.Close();
            ofl.Dispose();
            Process myProcess = new Process();
            try
            {
                myProcess.StartInfo.UseShellExecute = true;
                myProcess.StartInfo.FileName = Application.LocalUserAppDataPath + "\\discogs.html";
                myProcess.StartInfo.CreateNoWindow = true;
                myProcess.Start();
            }
            catch  
            {
            }
       }
 
        void playList_MouseDown(object sender, EventArgs e) // which button is pressed on object "playList" and at what (X,Y) will popup be shown?
        {
            MouseEventArgs me = (MouseEventArgs)e;
            ListviewButton = me.Button;
            ListViewXY = new Point(me.X + base.Left, me.Y + base.Top + playList.Top);
        }
        void playList_ListViewItemMouseHover(object sender, EventArgs e) // on which line (item) of object "playList" is the mouse located?
        {
            ListViewItemMouseHoverEventArgs me = (ListViewItemMouseHoverEventArgs)e;
            ListviewHoverIndex = me.Item.Index;
        }
        private void playList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (! ProgramChangesSelection & ListviewButton == MouseButtons.Left){
                if (playList.SelectedIndices.Count > 0)
                {
                    if (playList.SelectedIndices[0] + 1 != ItemPlaying)
                    {
                        newStreamBuild = false;
                        _sourceNew = null;
                        startPlay(playList.SelectedIndices[0] + 1);
                    }
                }

            }
        }
        // allows Drag and Drop of File or Map on the Playlist
        private void playList_DragEnter(object sender,System.Windows.Forms.DragEventArgs e)
        {
             if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
             else
                e.Effect = DragDropEffects.None;
        }
        private void playList_DragDrop(object sender,System.Windows.Forms.DragEventArgs e)
        {
            String[] aa = (String[])e.Data.GetData(DataFormats.FileDrop);
            foreach (String name in aa) {
                FileInfo fi = new FileInfo(name);
                if (fi.Exists) // is it a file?
                    addOneFileToList(name);
                else // must be a map
                {
                    String[] files;  
                    files = Directory.GetFiles(name, "*.*", SearchOption.TopDirectoryOnly);
                    int n = playList.Items.Count;
                    foreach (string file in files)                   
                        addOneFileToList(file);
                    if (n == playList.Items.Count)
                        processEntireMap(name,true);
                }
            }
        }
        string TranslateLocale(string s)
        {
            string myLan = CultureInfo.CurrentCulture.Name.Substring(0, 2).ToUpper();
            //  myLan = "NL";
            switch (myLan)
            {
                 case "NL":
                    switch (s)
                    {
                        case "BF": { s = "Blader files"; break; }
                        case "BD": { s = "Blader mappen"; break; }
                        case "TT": { s = "Totale tijd:"; break; }
                        case "VO": { s = "Volume"; break; }
                        case "IS": { s = "Inclusief submappen.      Als alternatief, sleep files of mappen in onderstaande lijst."; break; }
                        case "NM": { s = "Naam"; break; }
                        case "MP": { s = "Map"; break; }
                        case "AR": { s = "Artist"; break; }
                        case "DU": { s = "Duur"; break; }
                        case "TR": { s = "Track"; break; }
                        case "FI": { s = "Muziek(*.wav;*.mp3;*.wma)|*.wav;*.mp3;*.wma|Alle files (*.*)|*.*"; break; }
                        case "TI": { s = "Kies nummers om te horen"; break; }
                        case "PL": { s = "Spelen"; break; }
                        case "DL": { s = "Uit de lijst weghalen"; break; }
                        case "CL": { s = "Maak de lijst leeg"; break; }
                        case "IE": { s = "Zoek in DiscOGS"; break; }
                        default: break;
                    }
                    break;
                default:
                    switch (s)
                    {
                        case "BF": { s = "Browse files"; break; }
                        case "BD": { s = "Browse maps"; break; }
                        case "TT": { s = "Total time:"; break; }
                        case "VO": { s = "Volume"; break; }
                        case "IS": { s = "Include submaps.      Alternatively, drop files or maps onto the list below."; break; }
                        case "NM": { s = "Name"; break; }
                        case "MP": { s = "Map"; break; }
                        case "AR": { s = "Artist"; break; }
                        case "DU": { s = "Duration"; break; }
                        case "TR": { s = "Track"; break; }
                        case "FI": { s = "Songs(*.wav;*.mp3;*.wma)|*.wav;*.mp3;*.wma|All files (*.*)|*.*"; break; }
                        case "TI": { s = "Choose songs to listen to"; break; }
                        case "PL": { s = "Play"; break; }
                        case "DL": { s = "Remove from the list"; break; }
                        case "CL": { s = "Clear the list"; break; }
                        case "IE": { s = "Find in DiscOGS"; break; }
                        default: break;
                    }
                    break;
            }
            return (s);
        }

        private void playList_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            ListView playList = (ListView)sender;
            System.Collections.IComparer oldColumnSorter = playList.ListViewItemSorter;
            this.playList.ListViewItemSorter = lvwColumnSorter;

            // Determine if clicked column is already the column that is being sorted.
            
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            playList.Sort();
            for (int i = 0; i < playList.Items.Count; i++)
            {
                ListViewItem ite = playList.Items[i];
                if (ite.ForeColor == Color.Red) ItemPlaying = i + 1;
            }
            this.playList.ListViewItemSorter = oldColumnSorter;

        }
    }
    public class BasicSpectrumProvider : FftProvider 
    {
        private readonly int _sampleRate;
        public BasicSpectrumProvider(int channels, int sampleRate, FftSize fftSize)
              : base(channels, fftSize)
        {
            if (sampleRate <= 0)
                throw new ArgumentOutOfRangeException("sampleRate");
            _sampleRate = sampleRate;
        }
        public int GetFftBandIndex(float frequency)
        {
            int fftSize = (int)FftSize;
            double f = _sampleRate / 2.0;
            // ReSharper disable once PossibleLossOfFraction
            return (int)((frequency / f) * (fftSize / 2));
        }
        //public override void Add(float left, float right)
        //{
        //    base.Add(left, right);
        //}
    }
    public class LineSpectrum  
    {
        private int _barCount;
        private double _barSpacing;
        private double _barWidth;
        public FftSize FftSize;
        private int _maxFftIndex;
        private int _maximumFrequency = 20000;
        private int _maximumFrequencyIndex;
        private int _minimumFrequency = 20; //Default spectrum from 20Hz to 20kHz
        private int _minimumFrequencyIndex;
        private int[] _spectrumIndexMax;
        private int[] _spectrumLogScaleIndexMax;
        private int[] _spectrumBucketFrequency;
        public BasicSpectrumProvider _spectrumProvider;

        public LineSpectrum(FftSize fftSize, BasicSpectrumProvider _spectrumProviderp)
        {
            FftSize = fftSize;
            _spectrumProvider = _spectrumProviderp;
        }
        public double BarSpacing
        {
            get { return _barSpacing; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value");
                _barSpacing = value;
                UpdateFrequencyMapping();
            }
        }
        public int BarCount
        {
            get { return _barCount; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("value");
                _barCount = value;
                UpdateFrequencyMapping();
            }
        }
        void UpdateFrequencyMapping()
        {
            _maxFftIndex = (int)FftSize / 2 - 1;
            _maximumFrequencyIndex = Math.Min(_spectrumProvider.GetFftBandIndex(_maximumFrequency) + 1, _maxFftIndex);
            _minimumFrequencyIndex = Math.Min(_spectrumProvider.GetFftBandIndex(_minimumFrequency), _maxFftIndex);

            int actualResolution = BarCount;

            int indexCount = _maximumFrequencyIndex - _minimumFrequencyIndex;
            double linearIndexBucketSize = Math.Round(indexCount / (double)actualResolution, 3);

            _spectrumIndexMax = _spectrumIndexMax.CheckBuffer(actualResolution, true);
            _spectrumLogScaleIndexMax = _spectrumLogScaleIndexMax.CheckBuffer(actualResolution, true);
            _spectrumBucketFrequency = new int[_spectrumLogScaleIndexMax.GetUpperBound(0) + 1];

            double minFrequencyLog = Math.Log(_minimumFrequency); // min freq
            double maxFrequencyLog = Math.Log(_maximumFrequency); // max freq
            double bucketFactorLog = ((maxFrequencyLog - minFrequencyLog) / actualResolution);
            for (int i = 1; i < actualResolution; i++)
            {
                _spectrumIndexMax[i - 1] = _minimumFrequencyIndex + (int)(i * linearIndexBucketSize);
                _spectrumLogScaleIndexMax[i - 1] = (int)(Math.Pow(Math.E, minFrequencyLog + (i - 1) * bucketFactorLog) / _maximumFrequency * indexCount);
                if (i > 1)
                {
                    if (_spectrumLogScaleIndexMax[i - 1] <= _spectrumLogScaleIndexMax[i - 2])
                        _spectrumLogScaleIndexMax[i - 1] = _spectrumLogScaleIndexMax[i - 2] + 1;
                }
                _spectrumBucketFrequency[i - 1] = (int)((double)_spectrumLogScaleIndexMax[i - 1] / (double)indexCount * (double)_maximumFrequency) + _minimumFrequency;
            }
            if (actualResolution > 0)
            {
                _spectrumIndexMax[_spectrumIndexMax.Length - 1] =
                _spectrumLogScaleIndexMax[_spectrumLogScaleIndexMax.Length - 1] = _maximumFrequencyIndex;
            }
        }
        public Bitmap CreateSpectrumLine(Size size, Color color1, Color color2, Color background, bool highQuality)
        {
            var fftBuffer = new float[(int)FftSize];
            _barWidth = Math.Max(((size.Width - (BarSpacing * (BarCount - 1))) / BarCount), 0.00001);
            using (Brush brush = new LinearGradientBrush(new RectangleF(0, 0, (float)_barWidth, size.Height), color2, color1, LinearGradientMode.Vertical))
            {
                if (_spectrumProvider.GetFftData(fftBuffer))
                {
                    using (var pen = new Pen(brush, (float)_barWidth))
                    {
                        var bitmap = new Bitmap(size.Width, size.Height);

                        using (Graphics graphics = Graphics.FromImage(bitmap))
                        {
                            // PrepareGraphics(graphics, highQuality);
                            graphics.SmoothingMode = SmoothingMode.HighSpeed;
                            graphics.CompositingQuality = CompositingQuality.HighSpeed;
                            graphics.PixelOffsetMode = PixelOffsetMode.None;
                            graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
                            graphics.Clear(background);

                            // CreateSpectrumLineInternal(graphics, pen, fftBuffer, size);
                            int height = size.Height;
                            SpectrumPointData[] spectrumPoints = CalculateSpectrumPoints(height, fftBuffer);
                            int freeHeadingColumn = 0;

                            for (int i = 0; i < spectrumPoints.Length; i++)
                            {
                                SpectrumPointData p = spectrumPoints[i];
                                int barIndex = p.SpectrumPointIndex;
                                double xCoord = ((_barWidth * barIndex) + (BarSpacing * barIndex) + 1) + _barWidth / 2;

                                var p1 = new PointF((float)xCoord, height);
                                var p2 = new PointF((float)xCoord, height - (float)p.Value - 1);

                                graphics.DrawLine(pen, p1, p2);

                                int newHeaderColumn = (int)((_barWidth * barIndex) + (BarSpacing * barIndex) + 1);
                                if (newHeaderColumn > freeHeadingColumn)
                                {
                                    Rectangle aRectangle;
                                    int frequency = p.Frequency;
                                    if (frequency > 1000) // round to nice numbers
                                        frequency = (int)Math.Round((decimal)(frequency / 100)) * 100;
                                    else
                                        frequency = (int)Math.Round((decimal)(frequency / 10)) * 10;
                                    String stringToPrint = frequency.ToString();
                                    Font PrintFont = new Font("Courier New", 10);
                                    Size textSize = System.Windows.Forms.TextRenderer.MeasureText(stringToPrint, PrintFont);
                                    aRectangle = new Rectangle(new Point((int)((_barWidth * barIndex) + (BarSpacing * barIndex) + 1), 0), textSize);
                                    graphics.FillRectangle(Brushes.Black, aRectangle);
                                    System.Windows.Forms.TextRenderer.DrawText(graphics, stringToPrint, PrintFont, aRectangle, Color.White);
                                    freeHeadingColumn = newHeaderColumn + textSize.Width + 1;
                                }
                            }
                        }
                        return bitmap;
                    }
                }
            }
            return null;
        }
        SpectrumPointData[] CalculateSpectrumPoints(double maxValue, float[] fftBuffer)
        {
            var dataPoints = new List<SpectrumPointData>();

            double value0 = 0, value = 0;
            double actualMaxValue = maxValue;
            double sumBucketValues = 0;
            double numberBucketValues = 0;
            int spectrumPointIndex = 0;
            int ScaleFactorSqr = 3;
            for (int i = _minimumFrequencyIndex; i <= _maximumFrequencyIndex; i++)
            {
                value0 = ((Math.Sqrt(fftBuffer[i])) * ScaleFactorSqr) * actualMaxValue * Math.Log10(i + 1);
                value = Math.Max(0, Math.Min(value0, maxValue));
                sumBucketValues += value;
                numberBucketValues += 1;
                if (i == _spectrumLogScaleIndexMax[spectrumPointIndex])
                {
                    dataPoints.Add(new SpectrumPointData { SpectrumPointIndex = spectrumPointIndex, Value = sumBucketValues / numberBucketValues, Frequency = _spectrumBucketFrequency[spectrumPointIndex] });
                    spectrumPointIndex++;
                    sumBucketValues = 0;
                    numberBucketValues = 0;
                }
            }

            return dataPoints.ToArray();
        }
         struct SpectrumPointData
        {
            public int SpectrumPointIndex;
            public double Value;
            public int Frequency;
        }
    }
}