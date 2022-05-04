using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorOtherPath_V1 : MonoBehaviour, GeneratorOtherPath
{
    private ConfigLevel config;
    private List<Room> rooms;
    private int numberOtherRooms;
    public IEnumerator generate(ConfigLevel config, List<Room> mainRooms)
    {
        this.config = config;
        mainRooms = config.Rooms;
        this.rooms = config.FinishRooms;
        mainRooms.ForEach(e => rooms.Add(e));
        numberOtherRooms = 2 * config.NumberOfRoom / 3;
        removeBossRoom();
        return generateListOtherRoom();

    }

    private void removeBossRoom()
    {
       
        rooms[rooms.Count - 1].Type = RoomType.BOSS_ROOM;
        rooms.RemoveAt(rooms.Count - 1);
    }
    Room pickRoom()
    {
        return config.ListRoomTemplate[Random.Range(0, config.ListRoomTemplate.Length)];
    }
    Room createNewRoom(Room template, Vector2 pos, int index)
    {
        Room room = null;
        room = Instantiate(template, pos, Quaternion.identity);
        room.name = "Room " + index;
        return room;
    }
    private IEnumerator generateListOtherRoom()
    {
        /// B1. Chon mot phong bat ky
        /// B2. Tu phong do, tien hanh chon mot huong bat ky, yeu cau huong do phai hop le 
        /// (chua duoc dung den). Neu khong lap lai tu dau. 
        /// B3. Kiem tra xem o huong do da co phong chua, neu chua thi tiep tuc cac buoc,
        /// con neu da ton tai phong o huong do thi tien hanh gop nhanh, Luc nay ta tien hanh chon
        /// currentRoom nhu B1 va quay lai B1. Neu phong do la Boss room, de nghi lam lai
        /// B4. Pick mot phong trong template
        /// B5. Kiem tra voi khoang khong gian hien tai, ta co the dat duoc phong da chon hay khong.
        /// Neu khong dat duoc thi quay lai B2.
        /// B6. Tien hanh init phong, gang relationship

        //B1
        Room currentRoom = rooms[Random.Range(0, rooms.Count)];
        Room newRoom = null;
        Door currentDoor = null;
        Door newDoor = null;
        int direction = -1;
        int countOtherPath = Random.Range(2, 5);
        for (int i = 0; i < numberOtherRooms; i++)
        {
            // B2
            int[] basicDirections = DOOR_DIRECTION.BASIC_DIRECTION;
            direction = basicDirections[Random.Range(0, basicDirections.Length)];

            // Kiem tra huong nay da duoc dung hay chua
            currentDoor = currentRoom.pickDoor(direction);
            if (currentDoor.Status != STATUS_DOOR.IS_HIDEN) { i--; continue; }
            // B3
            Collider2D colider =
                RandomGenerationMapUtils.
                CheckingSpaceFitToRoom.getColiderOnSpace(currentDoor);

            // co ton tai phong o huong dang xet
            // Tien hanh gop nhanh
            if (colider != null)
            {
                Room room = colider.GetComponent<Room>();
                Door door = room.pickDoor(RoomUtils.OP_DIR(direction));
                if (door.Status == STATUS_DOOR.IS_HIDEN)
                {
                    setRelationship(currentRoom, currentDoor, room, door);
                    rooms.Add(room);
                    currentRoom = rooms[Random.Range(0, rooms.Count)];
                }
                i--;
                continue;
            }

            // B4
            newRoom = pickRoom();
            newRoom = createNewRoom(newRoom, new Vector2(1000, 1000), rooms.Count);
            yield return new WaitForFixedUpdate();

            // B5
            if (!RandomGenerationMapUtils.CheckingSpaceFitToRoom.checking(newRoom, currentDoor))
            {
                Destroy(newRoom.gameObject); i--; continue;
            };

            // B6
            newRoom.transform.position = newRoom.getPostionRoom(currentDoor);
            newDoor = newRoom.pickDoor(RoomUtils.OP_DIR(direction));
            rooms.Add(newRoom);

            setRelationship(currentRoom, currentDoor, newRoom, newDoor);
            currentRoom = newRoom;
            countOtherPath--;
            
        }
        rooms.Add(config.Rooms[config.Rooms.Count - 1]);
    }
    private void setRelationship(Room room1, Door door1, Room room2, Door door2)
    {

        door1.Room = room2;
        door2.Room = room1;

        door1.Status = STATUS_DOOR.IS_BLOCKED;
        door2.Status = STATUS_DOOR.IS_BLOCKED;

        door1.startLine();

    }
}
