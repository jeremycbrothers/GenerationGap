using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Room;

public class bgElementGenerator
{
    private Room.Grid<Cell> room;
    private Cell currCell;
    private Vector3 roomPosition;

    /* Booleans used to decide whether a kind of bg object can be generated at a specific spot */
    private bool canGenSingle = false;
    private bool canGenDouble = false;
    private bool canGenBush = false;

    public enum SingleCell : int { bgGrass1 = 44, bgGrass2 = 45, mushroom = 136};
    public enum Bush : int { bushBase = 132, bushTopLeft = 128, bushTopMid = 129, bushTopRight = 130, bushBottomLeft = 131, bushBottomRight = 133 };
    public enum DoubleCell : int { RockLeft = 134, RockRight = 135};

    public bgElementGenerator(Room.Grid<Cell> room, Vector3 roomPosition)
    {
        this.room = room;
        this.roomPosition = roomPosition;
        currCell = room.getCellAtCoord(0, 0);
    }

    /**
        Loops through room, checking each tile to see if it is a ground tile and whether the one above it
        is empty. If both of these conditions are met, placeElement() is called.
     **/

    public void genElements()
    {
        for (int x = 0; x < room.getCellsRows(); x++)
        {
            for (int y = 0; y < room.getCellsCols(); y++)
            {
                checkTile(x, y); //check what elements can be generated at a tile
                genTiles(x, y); //choose from valid elements to be generated
                //reset bool values
                canGenSingle = false;
                canGenDouble = false;
                canGenBush = false;
            }
        }
    }

    /**
     * Checks what size bg elements can be generated at a given tile
     * @param x - coordinate for row in grid
     * @param y - coordinate for column in grid
     */
    private void checkTile(int x, int y)
    {
        currCell = room.getCellAtCoord(x, y);
        canGenSingle = checkSingle(x, y);
        if (canGenSingle) canGenDouble = checkDouble(x, y);
        if (canGenDouble) canGenBush = checkBush(x, y);
    }

    /** 
     * Check to see if single-tile bg object can be placed
     * @param x - coordinate for row in grid
     * @param y - coordinate for column in grid
     */
    private bool checkSingle(int x, int y)
    {
        if (currCell.getHasGroundTile()) //current cell is ground
        {
            Cell nextCell = room.getCellAtCoord(x, y + 1);
            if (nextCell.getCellType() == CellType.Empty || nextCell.getCellType() == CellType.Enemy) //cell above current cell is empty
            {
                return true; //A single cell bg element can be generated here
            }
        }
        return false;
    }

    /**
     * Check to see if double-tile bg object can be placed
     * @param x - coordinate for row in grid
     * @param y - coordinate for column in grid
     */
    private bool checkDouble(int x, int y)
    {
        if(x + 1 < room.getCellsRows()) //next row is not out of bounds
        {
            if(room.getCellAtCoord(x + 1, y).getHasGroundTile() && (room.getCellAtCoord(x + 1, y + 1).getCellType() == CellType.Empty || room.getCellAtCoord(x + 1, y + 1).getCellType() == CellType.Enemy))
            {
                return true;
            }
        }
        return false;
    }

    /**
     * Check to see if bush can be placed
     * @param x - coordinate for row in grid
     * @param y - coordinate for column in grid
     */
    private bool checkBush(int x, int y)
    {
        if (x - 1 >= 0 && x + 1 < room.getCellsRows() && //check whether any cells to be used are out of range
            y + 2 < room.getCellsCols())
        {
            if (room.getCellAtCoord(x-1, y).getHasGroundTile() && room.getCellAtCoord(x + 1, y).getHasGroundTile()) //enough ground space
            {
                if((room.getCellAtCoord(x - 1, y + 1).getCellType() == CellType.Empty || room.getCellAtCoord(x - 1, y + 1).getCellType() == CellType.Enemy) && //enough empty space above ground
                    (room.getCellAtCoord(x, y + 1).getCellType() == CellType.Empty || room.getCellAtCoord(x - 1, y + 1).getCellType() == CellType.Enemy) &&
                    (room.getCellAtCoord(x + 1, y + 1).getCellType() == CellType.Empty || room.getCellAtCoord(x - 1, y + 1).getCellType() == CellType.Enemy) &&
                    (room.getCellAtCoord(x - 1, y + 2).getCellType() == CellType.Empty || room.getCellAtCoord(x - 1, y + 1).getCellType() == CellType.Enemy) &&
                    (room.getCellAtCoord(x, y + 2).getCellType() == CellType.Empty || room.getCellAtCoord(x - 1, y + 1).getCellType() == CellType.Enemy) &&
                    (room.getCellAtCoord(x + 1, y + 2).getCellType() == CellType.Empty || room.getCellAtCoord(x - 1, y + 1).getCellType() == CellType.Enemy))
                {
                    return true;
                }
            }
        }
        return false;
    }

    /**
     * Places background tiles based on how much space is available
     * @param x - coordinate for row in grid
     * @param y - coordinate for column in grid
     */
    private void genTiles(int x, int y)
    {
      if (canGenSingle && canGenDouble && canGenBush) //a single-tile object or bush can bve generated here
        {
            int pick = Random.Range(0, 3);
            if (pick == 0) //33.3% chance of generating bush
            {
                genBush(x, y);
            } else if (pick == 1)//33.3% chance of generating double object
            {
                genDouble(x, y);
            } else if (pick == 2) //33.3% chance of generating single object
            {
                genSingle(x, y);
            }
        } else if (canGenSingle && canGenDouble) //only a single-tile object can be generated here
        {
            int pick = Random.Range(0, 2);
            if (pick == 0)
            {
                genDouble(x, y);
            } else
            {
                genSingle(x, y);
            }
        } else if (canGenSingle)
        {
            genSingle(x, y);
        }
    }

    /**
     * Generates single-tile background objects
     * Mushrooms have a 10% chance of spawning. Everything else is 90%
     * @param x - coordinate for row in grid
     * @param y - coordinate for column in grid
     */
    private void genSingle(int x, int y)
    {
        if (chanceSpawn(5))
        {
            int index;

            if (chanceSpawn(10))
            {
                index = (int)SingleCell.mushroom;
            } else
            {
                index = (int)Random.Range((float)SingleCell.bgGrass1, (float)SingleCell.bgGrass2 + 1);
            }
            Sprite spriteToPlace = room.getSpriteAtIndex(index);
            Cell cellToPlace = room.getCellAtCoord(x, y + 1);
            Vector3 position = roomPosition + room.GetWorldPosition(x, y + 1) + new Vector3(room.getCellSize(), room.getCellSize());

            cellToPlace.placeBGTile(position, spriteToPlace);
        }
    }

    /**
     * Generates double-tile background objects.
     * Currently only generates a 2 tile rock
     * @param x - coordinate for row in grid
     * @param y - coordinate for column in grid
     */
    private void genDouble(int x, int y) //can only gen rock right now
    {
        if (chanceSpawn(10))
        {
            Cell cellToPlace = room.getCellAtCoord(x, y + 1);
            Sprite spriteToPlace = room.getSpriteAtIndex((int)DoubleCell.RockLeft);
            Vector3 position = roomPosition + room.GetWorldPosition(x, y + 1) + new Vector3(room.getCellSize(), room.getCellSize());

            cellToPlace.placeBGTile(position, spriteToPlace);

            cellToPlace = room.getCellAtCoord(x + 1, y + 1);
            spriteToPlace = room.getSpriteAtIndex((int)DoubleCell.RockRight);
            position = roomPosition + room.GetWorldPosition(x + 1, y + 1) + new Vector3(room.getCellSize(), room.getCellSize());

            cellToPlace.placeBGTile(position, spriteToPlace);
        }
        
    }

    /**
     * Generates a bush out of 6 tiles.
     * @param x - coordinate for row in grid
     * @param y - coordinate for column in grid
     */
    private void genBush(int x, int y)
    {
        if (chanceSpawn(3))
        {
            Sprite spriteToPlace;
            for (int row = x - 1; row <= x + 1; row++)
            {
                for (int col = y + 1; col <= y + 2; col++)
                {
                    //decide which sprite to use depending on current location
                    if (row == x - 1 && col == y + 1) spriteToPlace = room.getSpriteAtIndex((int)Bush.bushBottomLeft);
                    else if (row == x && col == y + 1) spriteToPlace = room.getSpriteAtIndex((int)Bush.bushBase);
                    else if (row == x + 1 && col == y + 1) spriteToPlace = room.getSpriteAtIndex((int)Bush.bushBottomRight);
                    else if (row == x - 1 && col == y + 2) spriteToPlace = room.getSpriteAtIndex((int)Bush.bushTopLeft);
                    else if (row == x && col == y + 2) spriteToPlace = room.getSpriteAtIndex((int)Bush.bushTopMid);
                    else spriteToPlace = room.getSpriteAtIndex((int)Bush.bushTopRight);

                    Cell cellToPlace = room.getCellAtCoord(row, col);
                    Vector3 position = roomPosition + room.GetWorldPosition(row, col) + new Vector3(room.getCellSize(), room.getCellSize());

                    cellToPlace.placeBGTile(position, spriteToPlace);
                }
            }
        }   
    }

    /**
     * Has a 1 out of given max chance of returning true, used to determine
     * likelihood of certain bg objects spawning
     */
    private bool chanceSpawn(int max)
    {
        int randomInt = Random.Range(1, max); //chance of placement 1 out of max
        if (randomInt == 1) return true;
        else return false;
    }

}
