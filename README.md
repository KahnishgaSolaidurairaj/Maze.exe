# Maze.exe: Inside the Computer 

Maze.exe: Inside the Computer is a cooperative multiplayer Unity game where players explore a maze that represents the interior of a computer while answering questions about computer architecture.

## Game Overview
Players begin inside a dark maze with three hearts. Their objective is to reach the end of the maze while answering questions about core computer components including:

- Input Devices
- Memory
- ALU
- CPU
- Output Devices

Incorrect answers cause the player to lose a heart. If all three hearts are lost, the player is teleported back to the beginning of the maze.

At the end of the maze, players encounter a final challenge question that tests their understanding of all previously introduced concepts.

## Gameplay Features
- Cooperative multiplayer gameplay (host and client)
- Question billboards that test computer architecture knowledge
- Heart-based health system
- Collectible heart pickups using collision detection
- Physics-based jump platform that applies upward force
- Maze exploration with environmental lighting and guiding lights
- Final challenge board with celebratory visual effects

## Physics Constructs Used

### Collision
Players can collect heart pickups placed throughout the maze. When the player collides with a heart object, it increases their heart count using Unity's collision detection system.

### Forces
A circular jump platform applies an upward force to the player's rigidbody when stepped on. This allows players to briefly jump high above the maze and preview the layout.

## Lighting Design
The game uses a darker "night mode" aesthetic to create atmosphere inside the maze.

Additional lighting includes:
- Spotlights on the jump platform and final question board
- Wall lights throughout the maze to guide player navigation
- Decorative lighting on the heart icon and final tree area to mark the end of the maze

## Controls
- **WASD** – Move
- **Click** – Answer questions (Client only)

## Technologies
- Unity
- C#
- VS Code

## Running the Game
Download the provided Windows output folder or Mac output folder and run the `.exe` file to launch the game.

## Assets: 
- Maze Modular Puzzle Kit by Poly Etereo - https://assetstore.unity.com/packages/3d/environments/maze-modular-puzzle-kit-302221
- Heart from Simple Gems and Items Ultimate Animated Customizable Pack by BenjaTheMaker - https://assetstore.unity.com/packages/3d/props/simple-gems-and-items-ultimate-animated-customizable-pack-73764
