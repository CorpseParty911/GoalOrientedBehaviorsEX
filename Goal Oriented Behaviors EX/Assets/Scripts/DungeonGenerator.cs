using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public GameObject roomPrefab;
    public GameObject player;
    public float roomChance;
    public float numRooms;

    bool[,] map = new bool[10,10];
    Vector3 start = new Vector3(0, 1.5f, 0);

    // Start is called before the first frame update
    void Start()
    {
        ResetMap();
        Generate();
        player.transform.position = start;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Generate();
            player.transform.position = start;
        }
    }

    void Generate()
    {
        ResetMap();
        while (BuildMap() < numRooms);

        for (int i = 0; i < 10; ++i)
        {
            for (int j = 0; j < 10; ++j)
            {
                if (map[i, j])
                {
                    GameObject newRoom = Instantiate(roomPrefab, new Vector3((j - 5) * 13, 0, (i - 5) * 21), new Quaternion());
                    openDoors(newRoom, i , j);
                }
            }
        }
    }

    int BuildMap()
    {
        int placedRooms = 1;
        map[5, 5] = true;
        Stack<Position> roomList = new Stack<Position>();
        roomList.Push(new Position(5, 5));
        Position currentRoom;

        while (placedRooms < numRooms)
        {
            if (roomList.Count > 0)
                currentRoom = roomList.Pop();
            else
                break;
            if (Random.value < roomChance && currentRoom.y < 9)
                if (!hasNeighbors(currentRoom.x, currentRoom.y + 1))
                {
                    map[currentRoom.x, currentRoom.y + 1] = true;
                    roomList.Push(new Position(currentRoom.x, currentRoom.y + 1));
                    ++placedRooms;
                }
            if (Random.value < roomChance && currentRoom.x > 0)
                if (!hasNeighbors(currentRoom.x - 1, currentRoom.y))
                {
                    map[currentRoom.x - 1, currentRoom.y] = true;
                    roomList.Push(new Position(currentRoom.x - 1, currentRoom.y));
                    ++placedRooms;
                }
            if (Random.value < roomChance && currentRoom.y > 0)
                if (!hasNeighbors(currentRoom.x, currentRoom.y - 1))
                {
                    map[currentRoom.x, currentRoom.y - 1] = true;
                    roomList.Push(new Position(currentRoom.x, currentRoom.y - 1));
                    ++placedRooms;
                }
            if (Random.value < roomChance && currentRoom.x < 9)
                if (!hasNeighbors(currentRoom.x + 1, currentRoom.y))
                {
                    map[currentRoom.x + 1, currentRoom.y] = true;
                    roomList.Push(new Position(currentRoom.x + 1, currentRoom.y));
                    ++placedRooms;
                }
        }
        return placedRooms;
    }

    void openDoors(GameObject room, int i, int j)
    {
        foreach (MeshRenderer mesh in room.GetComponentsInChildren<MeshRenderer>())
            if (mesh.gameObject.name == "Door1" && j < 9 && map[i, j + 1])
                Destroy(mesh.gameObject);
            else if (mesh.gameObject.name == "Door2" && i > 0 && map[i - 1, j])
                Destroy(mesh.gameObject);
            else if (mesh.gameObject.name == "Door3" && j > 0 && map[i, j - 1])
                Destroy(mesh.gameObject);
            else if (mesh.gameObject.name == "Door4" && i < 9 && map[i + 1, j])
                Destroy(mesh.gameObject);
    }

    bool hasNeighbors(int i, int j)
    {
        int neighborCount = 0;
        if (i > 0 && map[i - 1, j])
            ++neighborCount;
        if (i < 9 && map[i + 1, j])
            ++neighborCount;
        if (j > 0 && map[i, j - 1])
            ++neighborCount;
        if (j < 9 && map[i, j + 1])
            ++neighborCount;
        return neighborCount > 1;
    }

    private void ResetMap()
    {
        for (int i = 0; i < 10; ++i)
        {
            for (int j = 0; j < 10; ++j)
            {
                if (map[i, j])
                {
                    map[i, j] = false;
                }
            }
        }

        GameObject[] objects = GameObject.FindGameObjectsWithTag("Room");

        for (int i = 0; i < objects.Length; ++i)
        {
            Destroy(objects[i]);
        }
    }
}

class Position
{
    public int x;
    public int y;

    public Position(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}