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
        private string logPath;
        private string recordingsFolder;
        private readonly string ffmpegPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FFmpeg", "ffmpeg.exe");

        public event EventHandler<string> RecorderStatusChanged;

        private void UpdateStatus(string message)
        {
            RecorderStatusChanged?.Invoke(this, message);
        }

        public void StartRecording()
        {
            if (!File.Exists(ffmpegPath))
            {
                MessageBox.Show("FFmpeg not found at: " + ffmpegPath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("FFmpeg not found");
                return;
            }

            if (ffmpegProcess != null && !ffmpegProcess.HasExited)
                return;

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            recordingsFolder = Path.Combine(desktopPath, "Recordings");
            Directory.CreateDirectory(recordingsFolder);

            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            outputPath = Path.Combine(recordingsFolder, $"recording_{timestamp}.mp4");
            logPath = Path.Combine(recordingsFolder, $"ffmpeg_log_{timestamp}.txt");

            ffmpegProcess = new Process();
            ffmpegProcess.StartInfo.FileName = ffmpegPath;
            ffmpegProcess.StartInfo.Arguments = $"-y -f gdigrab -framerate 30 -i desktop -video_size 1280x720 -draw_mouse 1 -c:v libx264 -preset ultrafast -tune zerolatency -crf 23 \"{outputPath}\"";
            ffmpegProcess.StartInfo.RedirectStandardError = true;
            ffmpegProcess.StartInfo.UseShellExecute = false;
            ffmpegProcess.StartInfo.CreateNoWindow = true;

            try
            {
                ffmpegProcess.ErrorDataReceived += (s, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                        File.AppendAllText(logPath, e.Data + Environment.NewLine);
                };

                bool started = ffmpegProcess.Start();
                if (started)
                {
                    ffmpegProcess.BeginErrorReadLine();
                    UpdateStatus("Recording...");
                }
                else
                {
                    UpdateStatus("Failed to start recording.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error starting FFmpeg:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("Error starting recording.");
            }
        }

        public void PauseRecording()
        {
            // FFmpeg does not support pause, so we just update the UI
            UpdateStatus("Paused (Note: FFmpeg doesn't support real pause)");
        }

        public void StopRecording()
        {
            if (ffmpegProcess != null && !ffmpegProcess.HasExited)
            {
                try
                {
                    ffmpegProcess.Kill(true);
                    ffmpegProcess.WaitForExit();
                    ffmpegProcess = null;

                    UpdateStatus("Stopped");

                    MessageBox.Show("Recording saved to: " + outputPath, "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Process.Start("explorer.exe", recordingsFolder);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error stopping FFmpeg:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    UpdateStatus("Error stopping recording.");
                }
            }
        }
    }
}
