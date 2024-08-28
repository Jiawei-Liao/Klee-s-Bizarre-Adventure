using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int width;
    public int height;
    public int x;
    public int y;

    public Door leftDoor;
    public Door rightDoor;
    public Door topDoor;
    public Door bottomDoor;

    public List<Door> doors = new List<Door>();


    void Start()
    {
        if(RoomController.instance == null)
        {
            Debug.Log("Wrong Scene!");
            return;
        }
        Door[] foundDoors = GetComponentsInChildren<Door>();
        foreach(Door door in foundDoors)
        {
            doors.Add(door);
            switch(door.doorType)
            {
                case Door.DoorType.left:
                    leftDoor = door;
                    break;
                case Door.DoorType.right:
                    rightDoor = door;
                    break;
                case Door.DoorType.top:
                    topDoor = door;
                    break;
                case Door.DoorType.bottom:
                    bottomDoor = door;
                    break;
            }
        }
        
        RoomController.instance.RegisterRoom(this);
    }

    public void RemoveUnconnectedDoors()
    {
        foreach(Door door in doors)
        {
            switch(door.doorType)
            {
                case Door.DoorType.left:
                    if(GetLeft() == null)
                    {
                        door.gameObject.SetActive(false);
                    }
                    break;
                case Door.DoorType.right:
                    if(GetRight() == null)
                    {
                        door.gameObject.SetActive(false);
                    }
                    break;
                case Door.DoorType.top:
                    if(GetTop() == null)
                    {
                        door.gameObject.SetActive(false);
                    }
                    break;
                case Door.DoorType.bottom:
                    if(GetBottom() == null)
                    {
                        door.gameObject.SetActive(false);
                    }
                    break;
            }
        }
    }
    
    public Room GetRight()
    {
        if(RoomController.instance.DoesRoomExist(x + 1, y))
        {
            return RoomController.instance.FindRoom(x + 1, y);
        }
        return null;
    }

    public Room GetLeft()
    {
        if(RoomController.instance.DoesRoomExist(x - 1, y))
        {
            return RoomController.instance.FindRoom(x - 1, y);
        }
        return null;
    }

    public Room GetTop()
    {
        if(RoomController.instance.DoesRoomExist(x, y + 1))
        {
            return RoomController.instance.FindRoom(x, y + 1);
        }
        return null;
    }

    public Room GetBottom()
    {
        if(RoomController.instance.DoesRoomExist(x, y - 1))
        {
            return RoomController.instance.FindRoom(x, y - 1);
        }
        return null;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0));
    }

    public Vector3 GetRoomCenter()
    {
        return new Vector3(x * width, y * height);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            CameraController.instance.currentRoom = this;
        }
    }
}
