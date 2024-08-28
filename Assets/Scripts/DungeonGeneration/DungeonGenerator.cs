using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public DungeonGenerationData dungeonGenerationData;
    private List<Vector2Int> dungeonRooms;
    private HashSet<Vector2Int> roomSet;
    private Dictionary<string, (int x, int y)> specialRooms = new Dictionary<string, (int x, int y)>()
    {
        {"Start", (0, 0)}
    };
    
    private void Start()
    {
        dungeonRooms = DungeonCrawlerController.GenerateDungeon(dungeonGenerationData);
        roomSet = new HashSet<Vector2Int>(dungeonRooms);
        SpawnRooms(dungeonRooms);
    }

    private void SpawnRooms(IEnumerable<Vector2Int> rooms)
    {
        RoomController.instance.LoadRoom("Start", 0, 0);
        foreach(Vector2Int roomLocation in rooms)
        {
            RoomController.instance.LoadRoom(RoomController.instance.GetRandomRoomName(), roomLocation.x, roomLocation.y);
        }
        
        Vector2Int currentRoom = dungeonRooms[dungeonRooms.Count - 1];
        specialRooms["Boss"] = GetUnoccupiedPosition(currentRoom.x, currentRoom.y);

        currentRoom = dungeonRooms[Random.Range(0, dungeonRooms.Count - 1)];
        specialRooms["Treasure"] = GetUnoccupiedPosition(currentRoom.x, currentRoom.y);

        currentRoom = dungeonRooms[Random.Range(0, dungeonRooms.Count - 1)];
        specialRooms["Shop"] = GetUnoccupiedPosition(currentRoom.x, currentRoom.y);

        foreach (KeyValuePair<string, (int x, int y)> specialRoomLocation in specialRooms)
        {
            string roomName = specialRoomLocation.Key;
            (int x, int y) roomPosition = specialRoomLocation.Value;
            Debug.Log(roomName + " " + roomPosition.x + " " + roomPosition.y);
            RoomController.instance.LoadRoom(roomName, roomPosition.x, roomPosition.y);
        }
    }

    private (int x, int y) GetUnoccupiedPosition(int ix, int iy)
    {
        int x = ix;
        int y = iy;
        while (true)
        {
            if (Random.value < 0.5f)
            {
                x += Random.Range(0, 2) == 0 ? -1 : 1;
            }
            else
            {
                y += Random.Range(0, 2) == 0 ? -1 : 1;
            }

            Vector2Int position = new Vector2Int(x, y);
            if (specialRooms.ContainsValue((x, y)))
            {
                x = ix;
                y = iy;
            }
            else if (!roomSet.Contains(position))
            {
                return (x, y);
            }
        }
    }
}
