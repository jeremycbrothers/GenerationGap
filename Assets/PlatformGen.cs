using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGen : MonoBehaviour
{
    [SerializeField] private GameObject start;
    private GameObject[] roomArray;
    [SerializeField] private int numRooms;
    private int roomWidth = 16;
    private GameObject tileMap;
    private GameObject roomStart;
    private GameObject lastRoom;
    private int lastIndex = -1; //index of last chosen room in room array
    private GameObject newRoom; //newly generated room
    private GameObject typeRoom; //Type of room chosen randomly from array of rooms

    private void Awake()
    {
        //initialize array of room types
        roomArray = Resources.LoadAll<GameObject>("room_prefabs");

        tileMap = GameObject.Find("Grid");
        //generate first room at origin
        roomStart = Instantiate(start, new Vector3(0,0,0), Quaternion.identity);
        roomStart.transform.SetParent(tileMap.transform);

        lastRoom = roomStart;

        GenRooms();

    }

    //Loop that generates specified number of rooms in a continuous row
    private void GenRooms()
    {
        while(numRooms != 0)
        {
            typeRoom = PickRoom(); //type of room to be generated
            
            newRoom = Instantiate(typeRoom, lastRoom.transform.position, Quaternion.identity); //new platform generated at position of old one
            newRoom.transform.SetParent(tileMap.transform);

            //move new room 16 units to right of last room
            newRoom.transform.position += new Vector3(roomWidth, 0, 0);

            lastRoom = newRoom;
            numRooms--;
        }
    }

    //Randomly picks a room type from the array of different platform lengths
    private GameObject PickRoom()
    {
        //Debug.Log("Starting PickRoom()...");
        int index = Random.Range(0, roomArray.Length);

        //Debug.Log("Last index: " + lastIndex);
        //Debug.Log("New index: " + index);

        while (index == lastIndex)
        {
            //Debug.Log("Regenerating index...");
            index = Random.Range(0, roomArray.Length);
            //Debug.Log("New index: " + index);
        }

        //Debug.Log("returning!");
        lastIndex = index;
        return roomArray[index];
    }
}
