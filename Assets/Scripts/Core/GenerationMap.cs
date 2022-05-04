using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationMap : MonoBehaviour
{
    [SerializeField] private Room[] roomTemplate;
    [SerializeField] int numberOfRoom = 6;
    [SerializeField] GameObject point;
    Graph.GraphController graphController;
    
    private ShowStep showStep ;
    private List<Room> rooms = new List<Room>();
    private Transform[] points;
    // Tao path mac dinh tu diem khoi dau den room boss

    Room pickRandomRoom() { 
       return roomTemplate[Random.Range(0, roomTemplate.Length)];
    }
    void initStartRoom()
    {
        Room room = Instantiate(roomTemplate[0], Vector2.zero, Quaternion.identity);
        room.GetComponent<Rigidbody2D>().isKinematic = true;
        room.name = "Room 0";
        rooms.Add(room);
    }
    void placedRoom() {
        Room newRoom = null;
        for (int i = 0; i < numberOfRoom-1; i++)
        {
            // random phong
            newRoom = pickRandomRoom();
            // random vi tri phong xung quanh startedRoom;
            Vector2 pos = rooms[0].transform.position;
            pos.x += Random.Range(-1f, 1f);
            pos.y += Random.Range(-1f, 1f);
            newRoom = Instantiate(newRoom, pos, Quaternion.identity);
            newRoom.name = "Room " + (i + 1);
            newRoom.Index = i + 1;
            rooms.Add(newRoom);
        }
    }
    void createGraph() {
        points = new Transform[numberOfRoom];
        showStep = new ShowStep(numberOfRoom);
        for (int i = 0; i < numberOfRoom; i++) {
            showStep.PointObj[i] = Instantiate(point, rooms[i].transform.position, Quaternion.identity);
            showStep.PointObj[i].SetActive(true);
            rooms[i].gameObject.SetActive(false);
            points[i] = rooms[i].transform;
        }
        graphController = new Graph.GraphController(rooms, numberOfRoom);
        int [,] matrix = graphController.makeGraph(rooms,numberOfRoom);
        showStep.ListLine = showStep.drawGraph(matrix, points);
    }

    void createMinimumSpanningTree() {
        int[,] matrix = graphController.makeMinimumSpanningTree();
        showStep.ListLine.ForEach(e => Destroy(e.gameObject));
        showStep.drawGraph(matrix, points);
    }
    List<int> createMainPath() {
        List<Graph.Edge> edges =  graphController.showPath();
        int[,] matrix = graphController.makeMatrix(edges);
        showStep.ListLine.ForEach(e => Destroy(e.gameObject));
        showStep.drawGraph(matrix, points);

        List<int> listInt = new List<int>();
        for (int i = 0; i < edges.Count; i++) {
            listInt.Add(edges[i].point1);
            listInt.Add(edges[i].point2);
            Vector2 dir = points[edges[i].point2].position - points[edges[i].point1].position;
            createRelationshipRooms(edges[i].point1, edges[i].point2, Mathf.Atan2(dir.y, dir.x)*Mathf.Rad2Deg);
        }
        return listInt;
    }

    void createRelationshipRooms(int i, int j, float angle) {
        int direction = -1;
        Vector2 distance = rooms[i].transform.position - rooms[j].transform.position;
        if (rooms[i].transform.position.x > rooms[j].transform.position.x) direction = 1;
        else direction = 4;

        if (rooms[i].transform.position.y > rooms[j].transform.position.y) 
            if (Mathf.Abs(distance.x) < Mathf.Abs(distance.y))  direction = 2;
        else if (Mathf.Abs(distance.x) < Mathf.Abs(distance.y)) direction = 8;

        Door currentDoor = rooms[i].pickDoor(direction);
        Door newDoor = rooms[j].pickDoor(RoomUtils.OP_DIR(direction));

        currentDoor.Status = STATUS_DOOR.IS_BLOCKED;
        newDoor.Status = STATUS_DOOR.IS_BLOCKED;

        currentDoor.Room = rooms[j];
        newDoor.Room = rooms[i];

        Debug.Log(direction);
    }
    
    // Tao cac path phu
    [SerializeField] float timeStep = 2f;
    //public IEnumerator generateMap()
    //{
    //    initStartRoom();
    //    placedRoom();
    //    yield return new WaitForSeconds(timeStep);
    //    createGraph();
    //    yield return new WaitForSeconds(timeStep);
    //    createMinimumSpanningTree();

    //}


    //private void genOtherPath() {
    //    List<Room> otherRooms = new List<Room>();
    //    for (int i = 0; i < numberOfRoom / 2; i++) {
    //        Room currentRoom = rooms[Random.Range(0, rooms.Count)];
    //        // random huong cua phong do
    //        int basicDirect = DOOR_DIRECTION.BASIC_DIRECTION[Random.Range(0, DOOR_DIRECTION.BASIC_DIRECTION.Length)];
    //    }
    //}
    public IEnumerator generateMap()
    {
        initStartRoom();
        placedRoom();
        yield return new WaitForSeconds(timeStep);
        createGraph();
        yield return new WaitForSeconds(timeStep);
        List<int> path = createMainPath();
        yield return new WaitForSeconds(timeStep);
        for (int i = 0; i < numberOfRoom; i++) {
            if (path.Contains(i)) {
                rooms[i].gameObject.SetActive(true);
                continue; 
            }
            Destroy(showStep.PointObj[i]);
            Destroy(rooms[i].gameObject);
        }
    }
    
    private void Start()
    {
        StartCoroutine(generateMap());
    }

}

class ShowStep {
    private List<LineRenderer> listLine = new List<LineRenderer>();
    private GameObject[] pointObj;
    private int numberOfRoom;

    
    public ShowStep(int numberOfRoom)
    {
        this.numberOfRoom = numberOfRoom;
        pointObj = new GameObject[numberOfRoom];
    }

    public List<LineRenderer> ListLine { get => listLine; set => listLine = value; }
    public GameObject[] PointObj { get => pointObj; set => pointObj = value; }

    public List<LineRenderer> drawGraph(int[,] matrix_, Transform[] points)
    {
        int[,] matrix = matrix_;
        List<LineRenderer> listLine = new List<LineRenderer>();
        for (int i = 0; i < numberOfRoom; i++)
        {
            for (int j = 0; j < numberOfRoom; j++)
            {
                if (matrix[i, j] == 1)
                {
                    GameObject game = new GameObject();
                    LineRenderer line = game.AddComponent<LineRenderer>();
                    line.startColor = Color.red;

                    line.endColor = Color.red;
                    line.startWidth = 0.02f;
                    line.endWidth = 0.02f;
                    line.SetPosition(0, points[i].transform.position);
                    line.SetPosition(1, points[j].transform.position);
                    listLine.Add(line);
                }
            }
        }
        return listLine;
    }

    
}
class checkColiisionRoom { 

}