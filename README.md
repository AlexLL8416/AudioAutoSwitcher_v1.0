# Audio Auto Switcher

A simple Windows utility that lets you quickly switch between your favorite audio output devices directly from the system tray. Configure two audio profiles, and cycle between them with a single click. The application provides visual feedback by changing the tray icon to match the active device.

## Features

*   **System Tray Integration:** Runs quietly in the background and is accessible from the Windows system tray.
*   **One-Click Switching:** Left-click the tray icon to instantly cycle between your configured primary and secondary audio devices.
*   **Dynamic Icons:** The tray icon updates to visually represent the currently active audio device (e.g., speakers, headphones, Bluetooth).
*   **Easy Configuration:** A simple right-click menu provides access to a configuration window where you can select devices, assign custom names, and choose icons.
*   **Persistent Profiles:** Your device configuration is saved locally, so it's ready to go every time you start your computer.
*   **Installer Included:** Comes with an MSI installer for easy setup and a shortcut to launch the application on startup.

## Getting Started

### Installation

1.  Go to the [**Releases**](https://github.com/AlexLL8416/Audio-Auto-Switcher/releases) page of this repository.
2.  Download the latest `InstallerAudioAutoSwitcher.zip` file.
3.  Run the installer and follow the on-screen instructions. The application will be installed, and a shortcut will be added to your startup folder, allowing it to run automatically when you log in to Windows.

### First-Time Configuration

After installation, the application will start, and you'll see an information icon in your system tray. You need to configure your audio devices before you can switch between them.

1.  **Right-click** the application icon in the system tray and select **Configuration**.
2.  In the "Profile 1 (Primary)" section, select your main audio device from the dropdown list.
3.  Optionally, give it a custom name and select a representative icon (Speakers, Headphones, etc.).
4.  Repeat the process for "Profile 2 (Secondary)" to set up your second audio device.
5.  Click the **SAVE CONFIGURATION** button.

The tray icon will now update to reflect your currently active default audio device.

## How to Use

*   **Switch Devices:** **Left-click** the tray icon to cycle between your configured Profile 1 and Profile 2 devices.
*   **Open Menu:** **Right-click** the tray icon to open the context menu.
    *   **Configuration:** Opens the settings window to change your device profiles.
    *   **Exit:** Closes the application.

### Icon States
*   **Device Icon (Speakers, Headphones, etc.):** The application is running correctly, and the current default audio device matches one of your configured profiles.
*   **Information Icon:** The application has not been configured yet. Right-click to open the configuration menu.
*   **Warning Icon:** Your current default audio device is not one of the two devices you have configured in the application.

## Building from Source

### Prerequisites

*   [Visual Studio](https://visualstudio.microsoft.com/) 2022 or later.
*   [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0).
*   Visual Studio Installer Projects extension (for building the `.msi` installer). You can install it from the Visual Studio Marketplace or via the Extensions manager in Visual Studio.

### Steps

1.  Clone the repository:
    ```sh
    git clone https://github.com/AlexLL8416/Audio-Auto-Switcher.git
    ```
2.  Open the `CShard.sln` solution file in Visual Studio.
3.  Set the `AudioAutoSwitcher` project as the startup project.
4.  Build the solution. You can run the application directly from Visual Studio or find the executable in the `AudioAutoSwitcher\bin\Debug` (or `Release`) folder.
5.  To build the installer, right-click the `InstallerAudioAutoSwitcher` project and select **Build**. The `.msi` file will be generated in the `InstallerAudioAutoSwitcher\Release` folder.
