using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomUtils 
{
    public static int OP_DIR(int dir) {
        if (dir == DOOR_DIRECTION.LEFT) return DOOR_DIRECTION.RIGHT;
        else if (dir == DOOR_DIRECTION.DOWN) return DOOR_DIRECTION.TOP;
        else if (dir == DOOR_DIRECTION.RIGHT) return DOOR_DIRECTION.LEFT;
        else if (dir == DOOR_DIRECTION.TOP) return DOOR_DIRECTION.DOWN;
        throw new System.Exception("Dir invalid");
    }
}
