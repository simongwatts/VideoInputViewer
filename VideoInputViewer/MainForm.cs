using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using OpenCvSharp;
using OpenCvSharp.Extensions;

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
        private bool buttonsVisible = true;
        private bool comboboxesVisible = true;

        // Blur background fields
        private BackgroundSubtractorMOG2 _backgroundSubtractor;
        private bool _blurBackgroundEnabled = false;

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

            // Initialize background subtractor
            _backgroundSubtractor = BackgroundSubtractorMOG2.Create(
                history: 500,
                varThreshold: 32,
                detectShadows: false
            );
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
                using (Bitmap image = (Bitmap)eventArgs.Frame.Clone())
                {
                    if (_blurBackgroundEnabled)
                    {
                        ProcessBlurBackground(image);
                    }
                    else
                    {
                        UpdateVideoDisplay(image);
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Error processing frame: {ex.Message}");
            }
        }

        private void ProcessBlurBackground(Bitmap bitmap)
        {
            using (var mat = bitmap.ToMat())
            {
                // Detect upper body using Haar Cascade
                var cascade = new CascadeClassifier("haarcascade_upperbody.xml");
                var bodies = cascade.DetectMultiScale(mat, 1.1, 3, HaarDetectionTypes.ScaleImage);

                // Create mask for non-body regions
                using (var mask = new Mat(mat.Size(), MatType.CV_8UC1, Scalar.Black))
                {
                    if (bodies.Length > 0)
                    {
                        // Create rectangle around detected body
                        foreach (var body in bodies)
                        {
                            Cv2.Rectangle(mask, body, Scalar.White, -1);
                        }

                        // Expand mask slightly
                        Cv2.Dilate(mask, mask, null, iterations: 3);

                        // Blur the entire image
                        using (var blurred = new Mat())
                        {
                            Cv2.GaussianBlur(mat, blurred, new OpenCvSharp.Size(55, 55), 0);

                            // Combine original (body) and blurred (background)
                            Mat result = new Mat();
                            mat.CopyTo(result, mask);        // Keep body sharp
                            blurred.CopyTo(result, ~mask);   // Blur background

                            UpdateVideoDisplay(result.ToBitmap());
                        }
                    }
                    else
                    {
                        // Fallback if no body detected
                        UpdateVideoDisplay(bitmap);
                    }
                }
            }
        }

        private void UpdateVideoDisplay(Bitmap image)
        {
            if (pictureBox.InvokeRequired)
            {
                // Clone before cross-thread transfer
                var cloned = (Bitmap)image.Clone();
                pictureBox.BeginInvoke(new Action(() => {
                    using (cloned) // Ensure disposal
                    {
                        SafeUpdatePictureBox(cloned);
                    }
                }));
            }
            else
            {
                SafeUpdatePictureBox(image);
            }
        }
        private void SafeUpdatePictureBox(Bitmap image)
        {
            if (isClosing || pictureBox.IsDisposed) return;

            // Create a non-animated copy to prevent ImageAnimator errors
            using (var temp = new Bitmap(image.Width, image.Height))
            using (var g = Graphics.FromImage(temp))
            {
                g.DrawImage(image, 0, 0, image.Width, image.Height);
                var oldImage = pictureBox.Image;
                pictureBox.Image = (Image)temp.Clone();
                oldImage?.Dispose();
            }
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
        private void menuExit_Click(object sender, EventArgs e) => Application.Exit();

        private void menuToggleButtons_Click(object sender, EventArgs e)
        {
            buttonsVisible = !buttonsVisible;
            btnRefresh.Visible = buttonsVisible;
            btnSnapshot.Visible = buttonsVisible;
            menuToggleButtons.Text = buttonsVisible ? "Hide Buttons" : "Show Buttons";
        }

        private void menuToggleComboboxes_Click(object sender, EventArgs e)
        {
            comboboxesVisible = !comboboxesVisible;
            cboCameras.Visible = comboboxesVisible;
            cboResolutions.Visible = comboboxesVisible;
            menuToggleComboboxes.Text = comboboxesVisible ? "Hide ComboBoxes" : "Show ComboBoxes";

            // Adjust picture box position and size
            pictureBox.Top = comboboxesVisible ? 76 : 28;
            pictureBox.Height = comboboxesVisible ? 374 : 422;
        }

        private void menuToggleBlurBackground_Click(object sender, EventArgs e)
        {
            _blurBackgroundEnabled = !_blurBackgroundEnabled;
            menuToggleBlurBackground.Text = _blurBackgroundEnabled ?
                "Disable Blur Background" : "Enable Blur Background";
        }

        private void menuToggleTopMost_Click(object sender, EventArgs e)
        {
            TopMost = !TopMost;
            menuToggleTopMost.Text = TopMost ? "Disable TopMost" : "Enable TopMost";
        }
        #endregion
    }
}