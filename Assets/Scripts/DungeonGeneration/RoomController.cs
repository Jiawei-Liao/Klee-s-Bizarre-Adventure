using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomInfo
{
    public string name;
    public int x;
    public int y;
}

public class RoomController : MonoBehaviour
{
    public static RoomController instance;
    string currentWorldName = "Mondstadt";
    RoomInfo currentLoadRoomData;
    Room currentRoom;
    Queue<RoomInfo> loadRoomQueue = new Queue<RoomInfo>();
    public List<Room> loadedRooms = new List<Room>();
    bool isLoadingRoom = false;
    
    void Awake()
    {
        instance = this;
    }
    
    void Start()
    {
        //LoadRoom("Start", 0, 0);
        //LoadRoom("Empty", 1, 0);
        //LoadRoom("Empty", -1, 0);
        //LoadRoom("Empty", 0, 1);
        //LoadRoom("Empty", 0, -1);
    }

    void Update()
    {
        UpdateRoomQueue();
    }

    void UpdateRoomQueue()
    {
        if(isLoadingRoom)
        {
            return;
        }

        if(loadRoomQueue.Count == 0)
        {
            return;
        }

        currentLoadRoomData = loadRoomQueue.Dequeue();
        isLoadingRoom = true;
    
        StartCoroutine(LoadRoomRoutine(currentLoadRoomData));
    }
    
    public void LoadRoom(string name, int x, int y)
    {
        if (DoesRoomExist(x, y))
        {
            return;
        }

        RoomInfo newRoomData = new RoomInfo();
        newRoomData.name = name;
        newRoomData.x = x;
        newRoomData.y = y;
        loadRoomQueue.Enqueue(newRoomData);
    }

    IEnumerator LoadRoomRoutine(RoomInfo info)
    {
        string roomName = currentWorldName + info.name;
        AsyncOperation loadRoom = SceneManager.LoadSceneAsync(roomName, LoadSceneMode.Additive);
        while (loadRoom.isDone == false)
        {
            yield return null;
        }
    }

    public void RegisterRoom(Room room)
    {
        if(!DoesRoomExist(currentLoadRoomData.x, currentLoadRoomData.y))
        {
            room.transform.position = new Vector3(
                currentLoadRoomData.x * room.width,
                currentLoadRoomData.y * room.height,
                0
            );
            
            room.x = currentLoadRoomData.x;
            room.y = currentLoadRoomData.y;
            room.name = currentWorldName + "-" + currentLoadRoomData.name + " " + room.x + ", " + room.y;
            room.transform.parent = transform;

            isLoadingRoom = false;

            if(loadedRooms.Count == 0)
            {
                CameraController.instance.currentRoom = room;
            }
            
            loadedRooms.Add(room);
            room.RemoveUnconnectedDoors();
        }
        else
        {
            Destroy(room.gameObject);
            isLoadingRoom = false;
        }
    }

    public bool DoesRoomExist(int x, int y)
    {
        return loadedRooms.Find(item => item.x == x && item.y == y) != null;
    }

    public Room FindRoom(int x, int y)
    {
        return loadedRooms.Find(item => item.x == x && item.y == y);
    }

    public string GetRandomRoomName()
    {
        string[] possibleRooms = new string[] {
            "Empty"
            };
        return possibleRooms[Random.Range(0, possibleRooms.Length)];
    }

    public void OnPlayerEnterRoom(Room room)
    {
        CameraController.instance.currentRoom = room;
        currentRoom = room;

        UpdateRooms();
    }

    private void UpdateRooms();
    {
        foreach(Room room in loadedRooms)
        {
            if(currentRoom != room)
            {
                EnemyController[] enemies = room.GetComponentsInChildren<EnemyController>();
                if(enemies != null)
                {
                    foreach(EnemyController enemy in enemies)
                    {
                        enemy.notInRoom = true;
                    }
                }
            }
            else
            {
                EnemyController[] enemies = room.GetComponentsInChildren<EnemyController>();
                if(enemies != null)
                {
                    foreach(EnemyController enemy in enemies)
                    {
                        enemy.notInRoom = false;
                    }
                }
            }
        }
    }
}
