# Pathfinding demo

Unity version 2022.3.20f1, build-in components only, 3D Mode, Windows build 64bit.

Within demo, user is able to paint a grid and place obstaclesto find shortes path between them, avoiding the obstacles.
Path is drawn with line renderer on traversable tiles automatically when following conditions are met:
1. Goal and start are placed
2. There is at least one path connecting goal and start (i.e. path is not blocked completely by obstacles)

Each tile (node) can be traversable or non-traversable (obstacle). Tiles are orthogonally connected. Manchatan Heuristic is used to calculate
 distance between tiles. Algorithm used for this task is an A* algorithm with heap otpimization.

## User is able to:
1. Adjust the size of the map by presing "Restart" button at the bottom left corner of the screen and specifying X and Y values.
	OR
   by editing \Assets\Configs\DefaultGridConfig scriptable object in editor inspector.
2. Place obstacles on grid (or remove them) - dragging mouse around grid is supported for easier placement.
3. Choosing start/goal point - dragging mouse around grid is supported to quickly preview a new path.
4. Freely look around a map - with WSAD to move and holding right mouse button to rotate. Shift key can be held for faster movement.
5. Spawn character that will run along the path (or walk if path is short enough)
   
## Additionally:
- there is button to "invert" grid, i.e. changing all traversables tiles to obstacles and vice versa. Useful when user wishes to create a maze.
- there is button to reset camera
