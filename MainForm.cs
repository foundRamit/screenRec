using System;
using System.Windows.Forms;

namespace ScreenRecorderApp
{
    public partial class MainForm : Form
    {
        private Recorder recorder;
        private Button btnStart, btnPause, btnStop;
        private Label lblStatus;

        public MainForm()
        {
            InitializeComponent();
            recorder = new Recorder();
            recorder.RecorderStatusChanged += Recorder_RecorderStatusChanged;
        }

        private void InitializeComponent()
        {
            this.btnStart = new Button();
            this.btnPause = new Button();
            this.btnStop = new Button();
            this.lblStatus = new Label();

            this.btnStart.Text = "Start";
            this.btnPause.Text = "Pause";
            this.btnStop.Text = "Stop";

            this.btnStart.Location = new System.Drawing.Point(30, 30);
            this.btnPause.Location = new System.Drawing.Point(120, 30);
            this.btnStop.Location = new System.Drawing.Point(210, 30);

            this.lblStatus.Text = "Status: Idle";
            this.lblStatus.Location = new System.Drawing.Point(30, 70);
            this.lblStatus.AutoSize = true;

            this.btnStart.Click += new EventHandler(this.btnStart_Click);
            this.btnPause.Click += new EventHandler(this.btnPause_Click);
            this.btnStop.Click += new EventHandler(this.btnStop_Click);

            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnPause);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.lblStatus);

            this.Text = "Screen Recorder";
            this.ClientSize = new System.Drawing.Size(340, 120);
        }

        private void Recorder_RecorderStatusChanged(object sender, string status)
        {
            lblStatus.Text = "Status: " + status;
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
