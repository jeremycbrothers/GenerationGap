using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// The room namespace defines a room. In this case a "room" is made up two main components:
///     1. 2D grid of cells.
///     2. An individual cell.
///</summary>
namespace Room
{
    public enum  CellType : int { Empty, Template_Ground, Random_Ground, Spawn_Point, End_Point, Default = -1, Background = -2, Enemy=7, Null=8 };
    public enum Sprites : int { Grass = 0, Dirt = 3 };

    ///<summary>
    /// The cell class is the object in which we will manage data about individual grid cells.
    /// A Cell can have multiple sprites in it however, there is member that keeps track of
    /// ground sprites specifically. 
    ///</summary>
    public class Cell 
    {
        private bool hasGroundTile;
        private CellType type;
        private GameObject tile;
        private float cellSize;
        private Rigidbody2D cellRB;
        
        public Cell(string cellId, float cellSize)
        {
            this.cellSize = cellSize;
            type = CellType.Default;
            hasGroundTile = false;
            tile = new GameObject();
            tile.name = "Cell_" + cellId;
            tile.AddComponent<SpriteRenderer>();
            tile.AddComponent<Rigidbody2D>();
            cellRB = tile.GetComponent<Rigidbody2D>();
            cellRB.bodyType = RigidbodyType2D.Static;
            cellRB.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }

        /**
            createFinishLine configures a particular cell to indicate the end of a level
            @param sprite (Sprite) - the sprite used to indicate to the player where the end of the level is
            @param cellSize (float) - the size of the cell
        **/
        public void createFinishLine(Sprite sprite)
        {
            placeTile(tile.transform.position, sprite);
            tile.AddComponent<BoxCollider2D>();
            tile.GetComponent<BoxCollider2D>().isTrigger = true;
            this.getGameObject().tag = "FinishLine";
            tile.gameObject.layer = 0; // Default layer
        }

        /**
            placeGroundTile places only ground tiles in the center of current cell. 
            Once a ground tile has been placed, the cell's hasGroundTile is set to true.
            This can be used by other components to determine if a certian object can be placed "on the ground".
            @param position (Vector3) - The position in the cell where the sprite will be placed.
                                        The center of the sprite will be placed at this position.
            @param spriteToPlace (Sprite) - A sprite from a sliced sprite sheet.
            @param cellSize (float) - used to find the center of the cell.
        **/
        public void placeGroundTile(Vector3 position, Sprite spriteToPlace)
        {
            placeTile(position, spriteToPlace);
            tile.gameObject.layer = 8; // Ground layer
            tile.AddComponent<BoxCollider2D>();
            BoxCollider2D boxCollider = tile.GetComponent<BoxCollider2D>();
            boxCollider.size = new Vector2(cellSize, cellSize);
            boxCollider.usedByComposite = true;
            hasGroundTile = true;
        }

        /**
          placeBGTile places only bg tiles in the center of current cell.
            @param position (Vector3) - The position in the cell where the sprite will be placed.
                                        The center of the sprite will be placed at this position.
            @param spriteToPlace (Sprite) - A sprite from a sliced sprite sheet.
            @param cellSize (float) - used to find the center of the cell. 
        **/
        public void placeBGTile(Vector3 position, Sprite spriteToPlace)
        {
            placeTile(position, spriteToPlace);
            type = CellType.Background;
            tile.gameObject.layer = 0;
            tile.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("Background");
        }

        /**
            placeTile is a helper method for placeGroundTile and placeBGTile
        **/
        public void placeTile(Vector3 position, Sprite spriteToPlace)
        {
            setSprite(spriteToPlace);
            //tile.gameObject.transform.position = position; 
        }

        /**
            Set the sprite used in a cell
            @param sprite (Sprite) - A sprite from a sliced sprite sheet.
        **/
        public void setSprite(Sprite sprite)
        {
            tile.GetComponent<SpriteRenderer>().sprite = sprite;
        }

        /** Returns the type of a cell **/
        public CellType getCellType()
        {
            return type;
        }

        /** Sets type of a cell **/
        public void setCellType(CellType cellType)
        {
            type = cellType;
        }

        /** Returns whether tile contains ground tile **/
        public bool getHasGroundTile()
        {
            return hasGroundTile;
        }

        public GameObject getGameObject()
        {
            return tile;
        }
    };

    ///<summary>
    /// The grid class creates a 2D array of Cells (which could be modified to create a grid of an unspecified type).
    /// It's main responsibility is to "build" a room. Which will then be used to construct a level of random rooms.
    ///</summary>
    public class Grid<Cell> 
    {
        public string tilePallet;
        private Room.Cell[,] cells;
        private TextMesh[,] debugTextArray; // DEBUG CODE
        private int width;
        private int height;
        private float cellSize;
        private Sprite[] sprites;
        private Sprite spriteToPlace;
        private GameObject room;
                
        public Grid(int width, int height, float cellSize, string tilePallet, int roomId)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            this.tilePallet = tilePallet;
            
            room = new GameObject();
            room.name = "Room_" + roomId;

            sprites = Resources.LoadAll<Sprite>(tilePallet);

            cells = new Room.Cell[width, height]; // Init the array;
            debugTextArray = new TextMesh[width, height]; // DEBUG CODE

            // Add the cell objects to the grid and set its type from the array created above.
            for(int x = 0; x < cells.GetLength(0); x++)
            {
                for(int y = 0; y < cells.GetLength(1); y++)
                {
                    cells[x,y] = new Room.Cell(x+"_"+y, cellSize); // Add cell object to array   
                    cells[x,y].getGameObject().transform.parent = room.gameObject.transform;
                }
            }              
        }

        /** Returns the width of the grid **/
        public int getWidth() { return width; }

        /** Returns the height of the grid **/
        public int getHeight() { return height; }


        /**
            buildRoom loops through the grdi and determines which cells get a sprite.
            Some of these sprite placements are determined by the cells type.
            Based on said type, some cells will have a probability of generating a sprite.
            @param template (int[,]) - A 2D array of integers. Each value corresponds to a
                                       value in the CellType enum.
            @param position (Vector3) - The position in world space of the entire grid.
        **/
        public void buildRoom(int[,] template, Vector3 position)
        {
            Vector3 tilePosition;
            Vector3 cell = new Vector3(cellSize, cellSize);
            int probabilityCheck = 5;
            for(int x = 0; x < cells.GetLength(0); x++)
            {
                for(int y = 0; y < cells.GetLength(1); y++)
                {
                    cells[x,y].setCellType((CellType)template[x, y]);
                    // Calculates the center of tile.
                    tilePosition = (position + GetWorldPosition(x,y) + cell);
                    cells[x,y].getGameObject().transform.position = tilePosition;
                    
                    if(cells[x,y].getCellType() == CellType.Template_Ground)
                    {
                        changeToDirt(x, y - 1);
                        cells[x,y].placeGroundTile(tilePosition, sprites[(int)Sprites.Grass]);
                    }

                    if(cells[x,y].getCellType() == CellType.Random_Ground)
                    {
                        if(UnityEngine.Random.Range(0,10) < probabilityCheck)
                        {
                            changeToDirt(x, y - 1);
                            cells[x,y].placeGroundTile(tilePosition, sprites[(int)Sprites.Grass]);                            
                        }

                        probabilityCheck--;
                        if(probabilityCheck < 1)
                        {
                            probabilityCheck = 5;
                        }
                    }
                }
            }
        }

        /**
            Checks whether tile at coordinate (x,y) needs to be changed from a grass texture to a dirt texture
            @param x (int) - row index in grid
            @param y (int) - col index in grid
        **/
        public void changeToDirt(int x, int y)
        {
            if (y>= 0) //make sure array index is not out of bounds
            {
                if (cells[x, y].getHasGroundTile()) //if the tile beneath the current one is ground, change it to a dirt sprite
                {
                    cells[x, y].setSprite(sprites[(int)Sprites.Dirt]);
                }
            }
        }

        public Room.Cell GetCellByType(CellType type)
        {
            for(int x = 0; x < cells.GetLength(0); x++)
            {
                for(int y = 0; y < cells.GetLength(1); y++)
                {
                    if(cells[x,y].getCellType() == type)
                    {
                        return cells[x,y];
                    }
                }
            }
            return null;
        }

        // Debug function for drawing certian data points of an individual cell
        public void SetDebugTextValue(int x, int y, TextMesh text) { debugTextArray[x,y] = text; }

        // Get a 3D vector that contains the x,y,z world cordinates of a cell.
        public Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, y) * cellSize;
        }

        /** Return the cell at specified coordinate in the cells array **/
        public Room.Cell getCellAtCoord(int x, int y)
        {
               return cells[x, y];
        }

        /** Return the sprite at specific index in sprite array **/
        public Sprite getSpriteAtIndex(int index)
        {
            return sprites[index];
        }

        /** Return number of rows in cells array **/
        public int getCellsRows()
        {
            return cells.GetLength(0);
        }

        /** Return number of cols in cells array **/
        public int getCellsCols()
        {
            return cells.GetLength(1);
        }

        /** Return cellSize **/
        public float getCellSize()
        {
            return cellSize;
        }

        public void setCellType(int x, int y, CellType type)
        {
            cells[x, y].setCellType(type);
        }

        /** Return if a cell of a type is at a coords **/
        public bool hasCellOfType(int x, int y, CellType type)
        {
            Room.Cell cell = getCellAtCoord(x, y);
            return (cell.getCellType() == type);
        }
        /** Return if a cell with ground is at coords **/
        public bool hasGround(int x, int y)
        {
            Room.Cell cell = getCellAtCoord(x, y);
            return (cell.getHasGroundTile());
        }
    };
}
