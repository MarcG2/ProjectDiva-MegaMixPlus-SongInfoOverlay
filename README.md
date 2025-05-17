# ProjectDiva-MegaMixPlus-Song-Info-Overlay

This mod for Hatsune Miku: Project DIVA MegaMix+ generates a custom text overlay based on the song selected. When you start a song, it shows some information about it pulled from the pv_db.txt file. My primary motivation for the project was I wanted to easily know which mod pack a selected song if from. But it can also display almost any arbitrary information stored in the pv_db.txt file.

There are two parts to this mod: 1) The overlay function/configurator which I developed. And 2) the actual "mod" part which just allows another app to check the currently played. [The repo for it can be found here.](https://github.com/Jay184/marcgii-song-data)  (Thank you to Jay for providing it!) 

The overlay functionality is achieved using [GameOverlay.Net](https://github.com/michel-pi/GameOverlay.Net).  I chose it largely because the project supposedly offered an easy way to get an overlay working on full-screen exclusive apps.  Unfortunately, I was unable to get  that mode working.  Since it would be limited to Windows 10 usage anyway, I won't continue working on that feature. 

###### Automatic overlay triggered upon song selection

<img title="Sample Image" src="Overlay Sample 1.png" alt="" width="1309">

###### Configuration Window

<img title="Configuration Window" src="OVL configurator.png" alt="">

## Notes on usage

- The overlay only works in borderless window mode.  

- When the game is launched, the configuration window by default is minimized to the system tray.  Don't close the window!  It effectively turns off the mod and requires a game restart. 

- Save Settings must be pressed for config changes to take effect.

- Ctrl+1 manually spawns the overlay.  Ctrl+2 spawns the secondary overlay config and takes a screenshot.  The intended use case is providing the song name and mod on a screenshot. 

- The suggested offset information only applies to Extreme difficulty.  And is based on the information [here](https://docs.google.com/spreadsheets/d/1lUPXtailDKVATC-jfrQM9n-U0OFzBC6frz4fc81rDdM/edit?gid=0#gid=0).

## Requirements

- Windows 10+ only

- .NET Framework 4.8.  (Is included with standard Windows install) 

- [DivaModLoader](https://github.com/blueskythlikesclouds/DivaModLoader)

## Installation

- This mod is installed like any other MegaMix+ mod.   Refer to [this guide](https://gamebanana.com/tuts/15379) for general directions on mod installation.

- Download the [latest release](https://github.com/MarcG2/ProjectDiva-MegaMixPlus-SongInfoOverlay/releases/) and extract the zip to your mod folder.

