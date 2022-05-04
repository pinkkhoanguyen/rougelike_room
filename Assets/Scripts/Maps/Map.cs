using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    private List<Room> listRoom = new List<Room>();



    public void addRoom(Room room) {
        listRoom.Add(room);  
    }

    public Room getRoomByIndex(int index) {
        return listRoom[index];
    }

    public List<Room> getListRoom() {
        return listRoom;
    }
}
