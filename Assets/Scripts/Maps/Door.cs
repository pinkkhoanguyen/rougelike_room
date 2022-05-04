using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
     
    [SerializeField] private STATUS_DOOR status = STATUS_DOOR.IS_HIDEN;
    private Room room;
    [SerializeField]  private int direction;
    private LineRenderer line;
    private bool isDrawing = false;
    private BoxCollider2D box;
    private void Awake()
    {
        box = transform.GetChild(1).GetComponent<BoxCollider2D>();
    }
    public int Direction { 
        get => direction; 
        set {
            if (value != 1 && value != 2 && value != 4 && value != 8)
                throw new System.Exception("Direction invalid");
            direction = value;
        }
    }

    public STATUS_DOOR Status { 
        get => status;
        set {
            if (value == STATUS_DOOR.IS_HIDEN) hide();
            if (value == STATUS_DOOR.IS_OPENED) open();
            if (value == STATUS_DOOR.IS_BLOCKED) block();
            status = value;
        } }

    public Room Room { get => room; set => room = value; }
    public bool IsDrawing { get => isDrawing; set => isDrawing = value; }
    public BoxCollider2D Box { get => box; }

    public void hide() {
        //this.gameObject.SetActive(false);
        GetComponent<SpriteRenderer>().color = Color.white;

    }

    public void block() {
        //GetComponent<SpriteRenderer>().color = new Color32(227, 227, 227, 255);
    }

    public void open() {
        GetComponent<SpriteRenderer>().color = Color.green;
    }

    public Vector2 getPoint() {
        return transform.GetChild(0).transform.position;
    }
    public void drawLine() {
        if (Room == null) return;
        line.SetPosition(0, this.transform.position);
        line.SetPosition(1, this.Room.pickDoor(RoomUtils.OP_DIR(Direction)).transform.position);

    }
    public void startLine() {
        line = this.gameObject.AddComponent<LineRenderer>();
        line.startColor = Color.black;
        line.endColor = Color.black;
        line.materials = new Material[] { RandomGenerationMap.instance.MaterialLine };
        line.startWidth = 0.5f;
        line.endWidth = 0.5f;
        isDrawing = true;
    }
    private void Update()
    {
        if(isDrawing)
        drawLine();
    }
}


public class DOOR_DIRECTION
{
    static public int LEFT = 1;
    static public int DOWN = 2;
    static public int RIGHT = 4;
    static public int TOP = 8;


    static public int[] BASIC_DIRECTION = { TOP, RIGHT, DOWN, LEFT };

}

public enum STATUS_DOOR { 
    IS_HIDEN,
    IS_BLOCKED,
    IS_OPENED
}

