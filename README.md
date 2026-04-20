# PROG2001 Endless Runner Game

A 3D endless runner game developed with Unity as part of the PROG2001 Assessment 2 team project.

## Team Members

| Student | Name | Student ID | Scene |
|---------|------|------------|-------|
| Student 1 | Dongxu Li| 24832300 | Jungle Scene |
| Student 2 | Dexin Kong | 24832287 | River Scene |
| Student 3 | Ziheng Huo | 24832223 | Land Scene |
| Student 4 | Chengwu Liu | 24832423 | Sky Scene |

## Game Overview

This is an endless runner game featuring 4 distinct themed environments:
- 🌴 **Jungle Scene** - Tropical forest environment with dense vegetation
- 🌊 **River Scene** - Water-themed area with flowing obstacles
- 🏔️ **Land Scene** - Ground-based terrain with various barriers
- ☁️ **Sky Scene** - Aerial setting with floating platforms

## Controls

| Action | Key |
|--------|-----|
| Jump | W / ↑ / Space |
| Slide | S / ↓ |
| Move Left | A / ← |
| Move Right | D / → |

## Technical Details

- **Engine**: Unity 2022.3 LTS
- **Language**: C#
- **Platform**: WebGL (Browser)
- **Resolution**: 1280 x 720

## Scene Details

### River Scene (Student 2 - Dexin Kong)

- **Theme**: River/Water environment with flowing mechanics
- **Obstacles**: Moving obstacles, rotating barriers, low-hanging branches
- **Special Features**: 
  - Dynamic obstacle spawning system
  - Lane-based player movement
  - Slide mechanic for passing under low obstacles

## Project Structure

```
Assets/
├── Scripts/
│   ├── Player/         # Player controller and mechanics
│   ├── Obstacles/      # Obstacle behaviors
│   └── Managers/       # Game and UI managers
├── Scenes/
│   ├── MenuScene/      # Main menu and scene selection
│   ├── JungleScene/    # Student 1's scene
│   ├── RiverScene/     # Student 2's scene (Dexin Kong)
│   ├── LandScene/      # Student 3's scene
│   └── SkyScene/       # Student 4's scene
└── Prefabs/            # Reusable game objects
```

## Build Instructions

1. Open the project in Unity 2022.3 or later
2. Go to **File → Build Settings**
3. Select **WebGL** as the target platform
4. Switch platform if needed
5. Ensure Color Space is set to **Gamma** in Player Settings
6. Click **Build** and select an output folder

## Documentation

- `Development_Log_Dexin_Kong.md` - Development process and issues encountered
- `Assignment_3_Dexin_Kong.md` - Assignment 3 documentation

## Play Online

The game can be played on itch.io (link to be added after upload).

## License

Educational project for PROG2001 Assessment 2 - Group 14
