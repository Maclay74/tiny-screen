<img src="https://user-images.githubusercontent.com/5504685/189552767-808c7bd2-f906-40a9-8467-f581d7a7e935.png#gh-dark-mode-only" alt="Tiny Screen" style="width: 100%;">

<img src="https://user-images.githubusercontent.com/5504685/189552692-5fc4cd92-4408-4cf9-8864-d895087edd94.png#gh-light-mode-only" alt="Tiny Screen" style="width: 100%;">

*If you cannot do great things, do small things in a great way* - Napoleon Hill

Tiny Screen is an open-source frontend for handheld devices. It targets Windows systems where using native UI isn't convenient due to small screen size.

## Motivation
Windows can't provide great UX on small screens of handheld devices. 
This project is inspired by AyaSpace - frontend for AyaNeo devices with games library, quick hardware settings and shortcuts for applications.
However AyaSpace is dedicated exclusively for AyaNeo devices and [its overall quality isn't great](https://youtu.be/XbA6S0kdu2o?t=631).

Therefore, having support for wide range of handheld devices, open-source codebase and good quality are main targets for Tiny Screen.

## Development
![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![Godot Engine](https://img.shields.io/badge/GODOT-%23FFFFFF.svg?style=for-the-badge&logo=godot-engine)
![SQLite](https://img.shields.io/badge/sqlite-%2307405e.svg?style=for-the-badge&logo=sqlite&logoColor=white)
![GitHub Actions](https://img.shields.io/badge/github%20actions-%232671E5.svg?style=for-the-badge&logo=githubactions&logoColor=white)
![Figma](https://img.shields.io/badge/figma-%23F24E1E.svg?style=for-the-badge&logo=figma&logoColor=white)
![Windows](https://img.shields.io/badge/Windows-0078D6?style=for-the-badge&logo=windows&logoColor=white)

Currently the project is in active development phase, but not even close to be working. Feature set is shaped, but the implementation is on a very early stage.

The solution consists of the main application (TinyScreen) and plugins. Currently only sources of games (such as Steam or EGS) are supported as plugins.

It's on Godot and C#.

## Features
This is MVP features set

- [ ] Onboarding
  - [x] Welcome
  - [ ] Update
    - [x] Check for updates
    - [x] Download archive
    - [ ] Install and restart
  - [ ] Library
    - [x] Import games from Steam
    - [ ] Import games from EGS
    - [ ] Import games from GOG
  - [ ] Emulation
    - [ ] Import roms from RetroArch
    - [ ] Import games from other emulators?
  - [ ] Device
    - [ ] Check is additional hardware settings available
  - [ ] Widgets
    - [ ] Add widgets to home screen
    - [ ] Add widgets to overlay
    - [ ] Hardware-dependent widgets   
 - [ ] Application
   - [ ] Home
     - [ ] Last played games
     - [ ] Widgets 
   - [ ] Games
     - [ ] Filter games
     - [ ] Add external application
     - [ ] Run games
     - [ ] Game settings (details, presets, folder, hide) 
   - [ ] Settings
     - [ ] Library (update on launch, sources, folders)
     - [ ] Device (RGB lightning, triggers sensitivity, vibration, etc)
     - [ ] Emulation
     - [ ] Application (theme, language, start with Windows) 
 - [ ] Overlay
   - [ ] Implementation 
   - [ ] Widgets
   - [ ] Hardware settings (TDP, brightness, etc)

## Supported hardware
Currently no hardware support.
