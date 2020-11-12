Room Template READ ME:
- As of now making room templates involves maing a text file that ONLY contains numbers 
  and a comma delimeter between the numbers like so:
  1,2,3,4 etc. (The last number does not have a "," after it)

- Each number will represent the type of cell on a grid.
  The number coresponds to a value defined in the enum called
  CellType. This enum is in Room.cs found here:
  "Assets\Scripts\GamePlay\Room.cs"

- CellType values:
  Empty = 0,
  Template_Ground = 1,
  Random_Ground = 2,
  Spawn_Point = 3,
  End_Point = 4
  Default = -1
  Enemy = 7 (An enemy was spawned in this cell)
  Null = 8 (Used for a cell outside room bounds)
  (If more are added to the enum add them here)

- As of now rooms are 10 units wide and 8 units high.


- In your text file you should layout your room with these corners in mind:
  Bottom Left - - - - - - - - Top Left
  -                           -
  -                           -
  -                           -
  -                           -
  -                           -
  -                           -
  -                           -
  -                           -
  -                           -
  -                           -
  Bottom Right- - - - - - - - Top Right

VERY IMPORTANT:
    Because it seems that arrays are rotated 90 degrees counter clockwise, your room template MUST be laid out
    8 numbers wide and 10 numbers high.

  This is probably dude to the way 2D arrays are laid out in C# (Could be wrong though)

