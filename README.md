# VideoInputViewer

A Windows desktop application for real-time webcam monitoring with always-on-top capability, resolution selection, and snapshot capture.

## Features

- 🎥 Real-time webcam feed display
- 🔝 Always stays on top of other windows
- ↔️ Resizable window with aspect ratio preservation
- 📷 Capture snapshots (JPEG/PNG)
- ⚙️ Select from available resolutions
- 🔄 Multiple camera support
- 📊 FPS counter display
- 🔄 Refresh camera list

## Installation

1. **Prerequisites**:
   - .NET Framework 4.6.2
   - Windows 7 or newer
   - Webcam device

2. **Download**:
   - Grab the latest release from [Releases page](https://github.com/simongwatts/VideoInputViewer/releases)
   - Run `VideoInputViewer.exe`

## Usage

1. Launch the application
2. Select camera from dropdown
3. Choose resolution (if available)
4. Use controls:
   - **Snapshot**: Save current frame
   - **Refresh**: Reload camera list
   - Resize window freely
5. Close using X button

## Build from Source

1. **Requirements**:
   - Visual Studio 2019+
   - .NET Framework 4.6.2 Developer Pack

2. **Steps**:
   ```bash
   git clone https://github.com/simongwatts/VideoInputViewer.git
   cd VideoInputViewer
   Open VideoInputViewer.sln
   Build -> Build Solution