using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;

namespace VideoInputViewer
{
    public partial class MainForm : Form
    {
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;
        private VideoCapabilities[] videoCapabilities;
        private int framesReceivedCounter;
        private bool isClosing;
        private string currentMoniker;
        private bool controlsVisible = true;

        public MainForm()
        {
            InitializeComponent();
            SetupForm();
        }

        private void SetupForm()
        {
            TopMost = true;
            FormBorderStyle = FormBorderStyle.Sizable;
            DoubleBuffered = true;
        }

        private void MainForm_Load(object sender, EventArgs e) => InitializeCameraSystem();

        private void InitializeCameraSystem()
        {
            RefreshCameraList();
            timerFPS.Start();
        }

        private void RefreshCameraList()
        {
            cboCameras.BeginUpdate();
            cboCameras.Items.Clear();

            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count == 0)
            {
                MessageBox.Show("No webcams found");
                return;
            }

            foreach (FilterInfo device in videoDevices)
                cboCameras.Items.Add(device.Name);

            cboCameras.EndUpdate();
            cboCameras.SelectedIndex = 0;
        }

        private void StartCamera()
        {
            if (cboCameras.SelectedIndex < 0 || isClosing) return;

            currentMoniker = videoDevices[cboCameras.SelectedIndex].MonikerString;
            InitializeVideoSource(currentMoniker);
            InitializeResolutionControls();
            StartVideoCapture();
        }

        private void InitializeVideoSource(string moniker)
        {
            StopCamera();
            videoSource = new VideoCaptureDevice(moniker);
            videoSource.NewFrame += VideoSource_NewFrame;
        }

        private void InitializeResolutionControls()
        {
            cboResolutions.BeginUpdate();
            cboResolutions.Items.Clear();

            videoCapabilities = videoSource.VideoCapabilities;
            if (videoCapabilities != null)
            {
                foreach (var capability in videoCapabilities)
                    cboResolutions.Items.Add($"{capability.FrameSize.Width}x{capability.FrameSize.Height}");

                cboResolutions.SelectedIndex = videoCapabilities.Length - 1;
            }

            cboResolutions.EndUpdate();
        }

        private void StartVideoCapture()
        {
            if (cboResolutions.SelectedIndex >= 0 && videoCapabilities != null)
                videoSource.VideoResolution = videoCapabilities[cboResolutions.SelectedIndex];

            videoSource.Start();
        }

        private void StopCamera()
        {
            if (videoSource == null) return;

            if (videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
                videoSource.NewFrame -= VideoSource_NewFrame;
            }
            videoSource = null;
        }

        private void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            if (isClosing) return;

            try
            {
                Interlocked.Increment(ref framesReceivedCounter);
                UpdateVideoDisplay((Bitmap)eventArgs.Frame.Clone());
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Error processing frame: {ex.Message}");
            }
        }

        private void UpdateVideoDisplay(Bitmap image)
        {
            if (pictureBox.InvokeRequired)
            {
                pictureBox.BeginInvoke(new Action(() => SafeUpdatePictureBox(image)));
            }
            else
            {
                SafeUpdatePictureBox(image);
            }
        }

        private void SafeUpdatePictureBox(Bitmap image)
        {
            if (isClosing || pictureBox.IsDisposed) return;

            var oldImage = pictureBox.Image;
            pictureBox.Image = image;
            oldImage?.Dispose();
        }

        private void HandleResolutionChange()
        {
            if (cboResolutions.SelectedIndex < 0 || string.IsNullOrEmpty(currentMoniker)) return;

            InitializeVideoSource(currentMoniker);
            StartVideoCapture();
        }

        private void cboCameras_SelectedIndexChanged(object sender, EventArgs e) => StartCamera();

        private void cboResolutions_SelectedIndexChanged(object sender, EventArgs e) => HandleResolutionChange();

        private void btnSnapshot_Click(object sender, EventArgs e)
        {
            if (pictureBox.Image == null) return;

            using var saveDialog = new SaveFileDialog
            {
                Filter = "JPEG Image|*.jpg|PNG Image|*.png",
                Title = "Save Snapshot"
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                var format = Path.GetExtension(saveDialog.FileName).ToLower() == ".png"
                    ? ImageFormat.Png
                    : ImageFormat.Jpeg;

                pictureBox.Image.Save(saveDialog.FileName, format);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e) => InitializeCameraSystem();

        private void timerFPS_Tick(object sender, EventArgs e)
        {
            var fps = Interlocked.Exchange(ref framesReceivedCounter, 0);
            Text = $"Webcam Viewer - {(videoSource?.IsRunning == true ? $"{fps} fps" : "Not running")}";
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            isClosing = true;
            StopCamera();
            timerFPS.Stop();

            if (pictureBox.Image != null)
            {
                pictureBox.Image.Dispose();
                pictureBox.Image = null;
            }
        }

        #region Menu Handlers
        private void menuRefresh_Click(object sender, EventArgs e) => btnRefresh.PerformClick();
        private void menuSnapshot_Click(object sender, EventArgs e) => btnSnapshot.PerformClick();

        private void menuToggleControls_Click(object sender, EventArgs e)
        {
            controlsVisible = !controlsVisible;
            btnRefresh.Visible = controlsVisible;
            btnSnapshot.Visible = controlsVisible;
            menuToggleControls.Text = controlsVisible ? "Hide Controls" : "Show Controls";
        }
        #endregion
    }
}