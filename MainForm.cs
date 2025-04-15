using System;
using System.Windows.Forms;

namespace ScreenRecorderApp
{
    public partial class MainForm : Form
    {
        private Recorder recorder;
        private Button btnStart, btnPause, btnStop;

        public MainForm()
        {
            InitializeComponent();
            recorder = new Recorder();
        }

        private void InitializeComponent()
        {
            this.btnStart = new Button();
            this.btnPause = new Button();
            this.btnStop = new Button();

            this.btnStart.Text = "Start";
            this.btnPause.Text = "Pause";
            this.btnStop.Text = "Stop";

            this.btnStart.Location = new System.Drawing.Point(30, 30);
            this.btnPause.Location = new System.Drawing.Point(120, 30);
            this.btnStop.Location = new System.Drawing.Point(210, 30);

            this.btnStart.Click += new EventHandler(this.btnStart_Click);
            this.btnPause.Click += new EventHandler(this.btnPause_Click);
            this.btnStop.Click += new EventHandler(this.btnStop_Click);

            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnPause);
            this.Controls.Add(this.btnStop);

            this.Text = "Screen Recorder";
            this.ClientSize = new System.Drawing.Size(320, 100);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            recorder.StartRecording();
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            recorder.PauseRecording();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            recorder.StopRecording();
        }
    }
}
