using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorMainPath_ThamLam : MonoBehaviour, GeneratorMainPath
{
    List<Room> rooms = new List<Room>();
    private ConfigLevel config;

    private void Start()
    {

    }
    public IEnumerator generate(ConfigLevel config)
    {
        this.config = config;
        rooms = config.Rooms;
        initStartedRoom();
        yield return createMainPath();
    }
    Room createNewRoom(Room template, Vector2 pos, string name)
    {
        Room room = null;
        room = Instantiate(template, pos, Quaternion.identity);
        room.name = name;
        return room;
    }
    Room initStartedRoom()
    {
        Room room = createNewRoom(config.ListRoomTemplate[0], Vector2.zero, "Room 0");

        rooms.Add(room);
        return room;
    }

    Room pickRoom()
    {
        return config.ListRoomTemplate[Random.Range(0, config.ListRoomTemplate.Length)];
    }
    IEnumerator createMainPath()
    {
        Room currentRoom = rooms[0];
        Room newRoom = null;
        int direction = -1;
        //int oldDirection = -1;
        Door currentDoor = null;
        Door newDoor = null;
        for (int i = 0; i < config.NumberOfRoom / 3; i++)
        {
            int count = 20;


            /// 1. Chon huong cho phong dang xet
            /// 2. Kiem tra huong do co ton tai hay chua neu chua thi tiep tuc buoc tiep theo
            /// con huong nay da ton tai thi tiep tuc tim huong khac
            /// 3. Pick 1 phong trong danh sach templete (co the theo cac quy tac nhat dinh)
            /// 4. Kiem tra voi khong gian hien co, ta co the dat phong nay vao duoc khong
            /// Neu khong dat duoc thi tien hanh pick phong khac.
            /// 5. Moi dieu kien thoan man thi tien hanh Init phong do. Sau do, cai dat cac relationship (loi di) voi can 
            /// phong cu

            // B1
            int[] basicDirections = DOOR_DIRECTION.BASIC_DIRECTION;
            //oldDirection = direction;
            direction = basicDirections[Random.Range(0, basicDirections.Length)];

            // B2
            if (currentRoom.pickDoor(direction).Status != STATUS_DOOR.IS_HIDEN) { i--; continue; };

            // B3
            newRoom = pickRoom();
            currentDoor = currentRoom.pickDoor(direction);
            newRoom = createNewRoom(newRoom, new Vector2(1000, 1000), "Room " + (i + 1));
            yield return new WaitForFixedUpdate();
            // B4
            bool fit = RandomGenerationMapUtils.CheckingSpaceFitToRoom.checking(newRoom, currentDoor);
            //print(fit + "  - " + newRoom.name);
            if (!fit) { Destroy(newRoom.gameObject); i--; continue; };

            // B5
            newRoom.Index = currentRoom.Index + 1;
            newRoom.transform.position = newRoom.getPostionRoom(currentDoor);
            newDoor = newRoom.pickDoor(RoomUtils.OP_DIR(direction));
            rooms.Add(newRoom);


            currentDoor.Room = newRoom;
            newDoor.Room = currentRoom;

            currentDoor.Status = STATUS_DOOR.IS_BLOCKED;
            newDoor.Status = STATUS_DOOR.IS_BLOCKED;

            currentDoor.startLine();

            currentRoom = newRoom;
        }

    }

    public void createLobyRoom(Room room) {
        Room lobyRoom = createNewRoom(config.ListRoomTemplate[0], new Vector2(1000, 1000),room.name);
        rooms.ForEach(e =>
        {
            e.ListDoor.ForEach(d =>
            {
                if (d.Room == room) d.Room = lobyRoom;
            });
        });
        Vector2 pos = room.transform.position;
        Destroy(room.gameObject);
        lobyRoom.transform.position = pos;
        rooms.Remove(room);
    }
}



namespace RandomGenerationMapUtils
{
    public class CheckingSpaceFitToRoom
    {
        static public bool checking(Room room, Door door)
        {
            Vector2 posColider = door.getPoint();
            posColider = room.getPostionRoom(door);
            Collider2D colider = Physics2D.OverlapBox(posColider, new Vector2(room.getWidth() + .1f, room.getHeight() + .1f), 0f);
            if (colider != null)
            {
                //Debug.Log("W: " + room.getWidth() + ", H:" + room.getHeight()+ " POS "+posColider);

                Room getRoom = colider.GetComponent<Room>();
                return getRoom.Index == room.Index;
            }
            //Debug.Log("checking");
            return true;           
        }

        static public Collider2D getColiderOnSpace(Door door) {
            Vector2 posColider = door.getPoint();
            if (door.Direction == 1) posColider.x -= 0.5f;
            if (door.Direction == 4) posColider.x += 0.5f;
            if (door.Direction == 8) posColider.y += 0.5f;
            if (door.Direction == 2) posColider.y -= 0.5f;
            return Physics2D.OverlapCircle(posColider, 0.1f);
        }
    }
}
