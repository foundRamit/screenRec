using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace ScreenRecorderApp
{
    public class Recorder
    {
        private Process ffmpegProcess;
        private string outputPath;
        private readonly string ffmpegPath = Path.Combine("FFmpeg", "ffmpeg.exe");
        private string recordingsFolder;

        public void StartRecording()
        {
            if (ffmpegProcess != null && !ffmpegProcess.HasExited) return;

            // Setup Desktop\Recordings folder
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            recordingsFolder = Path.Combine(desktopPath, "Recordings");
            Directory.CreateDirectory(recordingsFolder);

            // Timestamp-based file name
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            outputPath = Path.Combine(recordingsFolder, $"recording_{timestamp}.mp4");

            // Launch ffmpeg with video + audio (change audio device if needed)
            ffmpegProcess = new Process();
            ffmpegProcess.StartInfo.FileName = ffmpegPath;
            ffmpegProcess.StartInfo.Arguments = $"-y -f gdigrab -framerate 30 -i desktop -f dshow -i audio=\"virtual-audio-capturer\" -c:v libx264 -preset ultrafast -c:a aac -b:a 128k \"{outputPath}\"";
            ffmpegProcess.StartInfo.UseShellExecute = false;
            ffmpegProcess.StartInfo.CreateNoWindow = true;
            ffmpegProcess.Start();
        }

        public void PauseRecording()
        {
            // FFmpeg doesn't support real pause from CLI — implement chunked recording if needed
        }

        public void StopRecording()
        {
            if (ffmpegProcess != null && !ffmpegProcess.HasExited)
            {
                ffmpegProcess.Kill(true);
                ffmpegProcess.WaitForExit();
                ffmpegProcess = null;

                MessageBox.Show("Recording saved to Desktop\\Recordings", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Process.Start("explorer.exe", recordingsFolder);
            }
        }
    }
}
// testing