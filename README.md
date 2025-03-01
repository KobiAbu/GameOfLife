# Game of Life – Kobi Abu

## Description
This is an implementation of Conway's Game of Life in Unity. The game consists of two scenes: the Main Menu and the Game Screen.

## Main Menu
The main menu allows users to customize the game using the following options:

1. **Tile Color** – Choose a tile color from pre-determined options.
2. **Background Color** – Choose a background color from pre-determined options.
3. **Grid Population Percentage** – Set the initial percentage of living cells in the grid (implemented using a random value between 0 and 1).
4. **Grid Size** – Choose a grid size between **10x10** and **100x100**.

Additionally, the main menu features a **Play** button to start the game.

## Game Screen
The game screen consists of:
- The **main camera**, displaying the Game of Life simulation.
- A **bottom bar** showing:
  - The number of intervals (generations) that have passed.
  - The current count of living cells.
  - Controls for adjusting the game speed:
    - " + " and " - " buttons (or using the Up/Down arrow keys).
- **Click and drag** to move the view within the grid.
- **Mouse wheel zoom** to zoom in and out.

## Controls
- **Click & Drag** – Move the grid.
- **Mouse Wheel** – Zoom in and out.
- **Up/Down Arrows** – Increase/decrease game speed.
- **"+" / "-" Buttons** – Adjust game speed.

## How to Play
1. Customize the game settings in the main menu.
2. Click "Play" to start the simulation.
3. Observe how the grid evolves based on Conway's Game of Life rules.
4. Adjust speed, zoom, and move around the grid as needed.

## Author
**Kobi Abu**

