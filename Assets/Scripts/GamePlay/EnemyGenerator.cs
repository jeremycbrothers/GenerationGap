using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Room;

public class EnemyGenerator : MonoBehaviour
{
    private Room.Grid<Cell> room;
    private Cell currCell;
    private Vector3 roomPosition;

    private Enemy[] enemyPrefabs;

    private int roomNum; //used to determine difficulty
    private int totalRooms;

    public EnemyGenerator(Room.Grid<Cell> room, Vector3 roomPosition, int roomNum, int totalRooms, Enemy[] enemyPrefabs)
    {
        this.room = room;
        this.roomPosition = roomPosition;
        currCell = room.getCellAtCoord(0, 0);
        this.roomNum = roomNum;
        this.totalRooms = totalRooms;
        this.enemyPrefabs = enemyPrefabs;
    }

    /**
        Loops through room, checking each tile to see if it has enough ground and empty
        If so, placeEnemy() is called.
     **/
    public void genEnemies()
    {

        for (int x = 1; x < room.getCellsRows()-1; x++) //we start at 1 higher and end at 1 lower to not go out of bounds when doing below adjacency checks
        {
            for (int y = 0; y < room.getCellsCols()-1; y++) //similar here (0 higher and 1 lower)
            {

                if (room.hasGround(x, y) && room.hasGround(x+1, y) && room.hasGround(x-1, y)) //check for 3 tiles of ground
                {
                    if (room.hasCellOfType(x, y+1, CellType.Empty) && room.hasCellOfType(x+1, y+1, CellType.Empty) && room.hasCellOfType(x-1, y+1, CellType.Empty)) //check 3 tiles of empty
                    {
                        placeEnemy(x, y);
                    }
                }
            }
        }
    }

    /** Below 2 functions responsible for difficulty **/

    /** Randomly Pick one of the enemies from RoomGenerator's enemy prefab array **/
    private Enemy pickEnemy()
    {
        return enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
    }
    /** Randomly decide whether to place an enemy in an open space **/
    private bool shouldPlaceEnemy()
    {
        float chance = (float)roomNum / (float)totalRooms + 0.3f;
        return Random.Range(0f, 1f) < chance;
    }


    /**
        Attempt to place enemy. Uses above functions to randomize
     **/
    private void placeEnemy(int x, int y)
    {
        if (shouldPlaceEnemy())
        {
            Enemy enemyToPlace = pickEnemy();
            Instantiate(enemyToPlace);
            Vector3 position = roomPosition + room.GetWorldPosition(x, y + 1) + new Vector3(room.getCellSize(), room.getCellSize());
            enemyToPlace.transform.position = position;

            room.setCellType(x, y + 1, CellType.Enemy); //set cell to enemy to block off from other enemies
            room.setCellType(x + 1, y + 1, CellType.Enemy);
            room.setCellType(x - 1, y + 1, CellType.Enemy);
        }
    }
}