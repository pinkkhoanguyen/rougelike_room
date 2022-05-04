using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class Room : MonoBehaviour
{
    [SerializeField] List<Door> listDoor = new List<Door>();
    //[SerializeField] int width;
    //[SerializeField] int height;
    int index = 0;
    private RoomType type;
    public STATUS_ROOM status = STATUS_ROOM.IS_INIT;
    [SerializeField] private GameObject walls;
    public RoomType Type
    {
        get => type;
        set => type = value;
    }
    //public int Width { get => width; }
    //public int Height { get => height; }
    public List<Door> ListDoor { get => listDoor; set => listDoor = value; }
    public int Index { get => index; set => index = value; }
    public STATUS_ROOM Status { 
        get => status;
        set {
            if (value == STATUS_ROOM.IS_STARTED) {
                BoxCollider2D box = GetComponent<BoxCollider2D>();
                if (box != null) box.isTrigger = true;
                if (walls != null) walls.SetActive(true);
                ListDoor.ForEach(e => e.Box.gameObject.SetActive(true));
            }
            if (value == STATUS_ROOM.IS_INIT) {
                ListDoor.ForEach(e => e.Box.gameObject.SetActive(false));
            }
            if (value == STATUS_ROOM.IS_CLEAN) {
                ListDoor.ForEach(e => e.gameObject.SetActive(false));
            }
            this.status = value;
        }
    }
    private void Awake()
    {
        status = STATUS_ROOM.IS_INIT;

    }

    public Door addDirection(int dir)
    {
        for (int i = 0; i < ListDoor.Count; i++)
        {
            if (ListDoor[i].Direction == dir && ListDoor[i].Status == STATUS_DOOR.IS_HIDEN)
            {
                ListDoor[i].Status = STATUS_DOOR.IS_BLOCKED;
                return ListDoor[i];
            }
            continue;
        }
        return null;
    }
    public Door pickDoor(int direction)
    {
        Door door = null;
        for (int i = 0; i < ListDoor.Count; i++)
        {
            if (ListDoor[i].Direction == direction) return ListDoor[i];
        }
        return door;
    }
    public Door pickDoor(Door door)
    {
        for (int i = 0; i < ListDoor.Count; i++)
        {
            if (ListDoor[i].Direction == door.Direction) return ListDoor[i];
        }
        return null;
    }
    //[SerializeField] Room testRoom;
    //[SerializeField] Door testDoor;
    //[SerializeField] bool isDrawing = false;

    //private void Start()
    //{
      
    //}
    //void OnDrawGizmos()
    //{
    //    if (!isDrawing) return;

    //    Vector2 posColider = testDoor.getPoint();
    //    if (testDoor.Direction == 1) posColider.x -= 1f;
    //    if (testDoor.Direction == 4) posColider.x += 1f;
    //    if (testDoor.Direction == 8) posColider.y += 1f;
    //    if (testDoor.Direction == 2) posColider.y -= 1f;
    //    Gizmos.DrawSphere(posColider, 0.1f);
    //}
    public Vector2 getPostionRoom(Door currentDoor)
    {
        Vector2 pos = Vector2.zero;
        if (currentDoor.Direction == DOOR_DIRECTION.RIGHT) pos = getPostionRightRoom(currentDoor);
        if (currentDoor.Direction == DOOR_DIRECTION.LEFT) pos = getPostionLeftRoom(currentDoor);
        if (currentDoor.Direction == DOOR_DIRECTION.TOP) pos = getPostionTopRoom(currentDoor);
        if (currentDoor.Direction == DOOR_DIRECTION.DOWN) pos = getPostionDownRoom(currentDoor);


        Vector2 delta = (Vector2)transform.position - this.pickDoor(RoomUtils.OP_DIR(currentDoor.Direction)).getPoint();
        if (currentDoor.Direction == 1)
            pos.y += delta.y;

        if (currentDoor.Direction == 4)
            pos.y += delta.y;
        if (currentDoor.Direction == 8)
            pos.x += delta.x;

        if (currentDoor.Direction == 2)
            pos.x += delta.x;
        return pos;
    }
    private Vector2 getPostionLeftRoom(Door door)
    {
        Door pickedDoor = pickDoor(RoomUtils.OP_DIR(door.Direction));
        float left = pickedDoor.getPoint().x - transform.position.x;
        return new Vector2(door.getPoint().x - left, door.getPoint().y);

    }
    private Vector2 getPostionRightRoom(Door door)
    {
        Door pickedDoor = pickDoor(RoomUtils.OP_DIR(door.Direction));
        float right = transform.position.x - pickedDoor.getPoint().x;
        return new Vector2(door.getPoint().x + right, door.getPoint().y);
    }
    private Vector2 getPostionTopRoom(Door door)
    {
        Door pickedDoor = pickDoor(RoomUtils.OP_DIR(door.Direction));
        float top = transform.position.y - pickedDoor.getPoint().y;
        return new Vector2(door.getPoint().x, door.getPoint().y + top);
    }
    private Vector2 getPostionDownRoom(Door door)
    {
        Door pickedDoor = pickDoor(RoomUtils.OP_DIR(door.Direction));
        float down = pickedDoor.getPoint().y - transform.position.y;
        return new Vector2(door.getPoint().x, door.getPoint().y - down);
    }
    public float getWidth()
    {
        return getBoundCollider().x;
    }

    public float getHeight()
    {
        return getBoundCollider().y;
    }

    public Vector2 getBoundCollider()
    {
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        return new Vector2(box.bounds.size.x, box.bounds.size.y);
    }

   

}


public enum RoomType
{
    NORMOAL,
    CHEST_ROOM,
    BOSS_ROOM
}

public enum STATUS_ROOM { 
   IS_HIDEN,
   IS_PLAYING,
   IS_INIT,
   IS_STARTED,
   IS_CLEAN
}