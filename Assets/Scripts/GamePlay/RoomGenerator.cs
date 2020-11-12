using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Room;

public class RoomGenerator : MonoBehaviour
{
    public string tilePalletName;
    public int numRooms;
    public GameObject playerPrefab;
    public GameObject cameraPrefab;
    private Room.Grid<Cell> room;
    private Room.Grid<Cell>[] roomArray;
    private List<int[,]> roomTemplates;
    private float roomXShift;
    private int roomWidth;
    private int roomHeight;
    [Tooltip("When adjusting cellSize, you may have to resize your sprite via pixel per unit or other method.")]
    [SerializeField]private float cellSize;
    private int numTemplateRooms;


    //used for enemyGenerator to create enemies, may remove/change later
    public Enemy[] enemyPrefabs;

    // Awake is called when the script instance is being loaded.
    private void Awake()
    {
        roomWidth = 10;
        roomHeight = 8;
        roomArray = new Room.Grid<Cell>[numRooms];
        roomTemplates = new List<int[,]>();
        numTemplateRooms = 16;

        // Create the rooms templates
        for(int i = 0; i < numTemplateRooms; i++)
        {
            buildTemplateRoom(i);
        }

        roomXShift = roomWidth * cellSize;
        int randomIndex = 0;
        int lastIndex = 0;
        Vector3 roomPosition = new Vector3();

        for(int i = 0; i < numRooms; i++)
        {
            room = new Room.Grid<Cell>(roomWidth, roomHeight, cellSize, tilePalletName, i);

            randomIndex = Random.Range(0, roomTemplates.Count);
            while(randomIndex == lastIndex)
            {
               randomIndex = Random.Range(0, roomTemplates.Count); 
            }
            //build a room based on a randomly chosen template.
            room.buildRoom(roomTemplates[randomIndex], roomPosition);
            roomArray[i] = room;

            genEnemies(roomPosition, i, roomArray.Length); //generate enemies on the current room w/ difficulty i out of total rooms
            genBackground(roomPosition); //generate background elements on the current room

            // After building the room shift the position of the next room by the room width multiplied by the cell size.
            roomPosition += new Vector3(roomXShift, 0, 0);
            lastIndex = randomIndex;
        }
        
        Room.Cell finishPoint = roomArray[numRooms-1].GetCellByType(Room.CellType.End_Point);
        if(finishPoint != null)
        {
            Sprite sprite = Resources.Load<Sprite>("sign");
            finishPoint.createFinishLine(sprite);
        }
    }

    // Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
    private void Start()
    {
        // Set player in spawn point
        Room.Cell spawnPoint = roomArray[0].GetCellByType(Room.CellType.Spawn_Point);
        if(spawnPoint != null)
        {
            Vector3 position = spawnPoint.getGameObject().transform.position;
            Instantiate(playerPrefab, position, Quaternion.identity);
            cameraPrefab = Instantiate(cameraPrefab, new Vector3(), Quaternion.identity);
        }
    }

    /**
        Creates instance of bgElementGenerator, and calls its genElements function
        @param position (Vector3) - The position of the room being used by bgGen,
                                    Used by bgGen to calculate cell placement
     **/
    private void genBackground(Vector3 position)
    {
        bgElementGenerator bgGen = new bgElementGenerator(room, position);
        bgGen.genElements();
    }

    /**
    Creates instance of EnemyGenerator, and calls its genEnemies function
    @param position (Vector3) - The position of the room being used by bgGen,
                                Used by bgGen to calculate enemy placement
    @param roomNum Used in calculating enemy difficulty
    @param totalRooms Used in calculating enemy difficulty
    **/
    private void genEnemies(Vector3 position, int roomNum, int totalRooms)
    {
        EnemyGenerator enemyGen = new EnemyGenerator(room, position, roomNum, totalRooms, enemyPrefabs);
        enemyGen.genEnemies();
    }

    // Converts a text file into a 2D array of integers.
    private void buildTemplateRoom(int index)
    {
        string filePath = "";
        int[,] roomTemplate;

        filePath = "Assets/Resources/text_files/RoomTemplate_" + index + ".txt";
        roomTemplate = Utilities.Utilities.build2DArrayFromFile(filePath, roomWidth, roomHeight);
        roomTemplates.Add(roomTemplate);
    }
}
