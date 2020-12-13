using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Audio_Recorder_Forms
{
    public partial class Form1 : Form
    {
        [DllImport("winmm.dll")]
        private static extern long mciSendString(string Command, StringBuilder Retstring, int Return_String, IntPtr Call_back);
        public Timer timer1 = new Timer();
        public string tempFolderPath = "";
        public string soundPath = "";
        public System.Media.SoundPlayer sound = null;
        public bool is_sound_playing = false;
        public int count = 0;

        public Form1()
        {
            InitializeComponent();
            tempFolderPath = "C:\\Mine\\IT\\Audio_Recorder_Forms\\Records\\";
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            timer1.Start();
            mciSendString("open new Type waveaudio alias recsound", null, 0, IntPtr.Zero);
            mciSendString("Record recsound",null, 0, IntPtr.Zero); 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            timer1.Enabled = false;
            count = Directory.GetFiles(tempFolderPath).Count();
            mciSendString("save recsound C:\\Mine\\IT\\Audio_Recorder_Forms\\Records\\rec"+count+".wav", null, 0, IntPtr.Zero);
            mciSendString("close recsound", null, 0, IntPtr.Zero);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!is_sound_playing)
            {
                soundPath = GetNewestFile(new DirectoryInfo(tempFolderPath)).FullName;
                sound = new System.Media.SoundPlayer(soundPath);
                sound.Load();
                sound.Play();
                is_sound_playing = true;
            }
            else
            {
                sound.Stop();
                is_sound_playing = false;
            }
        }

        public static FileInfo GetNewestFile(DirectoryInfo directory)
        {
            return directory.GetFiles()
                .Union(directory.GetDirectories().Select(d => GetNewestFile(d)))
                .OrderByDescending(f => (f == null ? DateTime.MinValue : f.LastWriteTime))
                .FirstOrDefault();
        }
    }
}
