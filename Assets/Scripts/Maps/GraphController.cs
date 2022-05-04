using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Graph
{
    public class GraphController
    {
        List<Room> rooms;
        int numberRooms;
        List<Edge> edges = new List<Edge>();
        int[,] matrix_;
        CheckCircle checker = new CheckCircle();
        int[] begin;
        public GraphController(List<Room> rooms, int numberRooms)
        {
            this.rooms = rooms;
            this.numberRooms = numberRooms;
            begin = new int[numberRooms];
            for (int i = 0; i < numberRooms; i++) begin[i] = i;

        }

        public List<Edge> Edges { get => edges; set => edges = value; }

        public int[,] makeGraph(List<Room> rooms, int numberRooms)
        {
            this.rooms = rooms;
            this.numberRooms = numberRooms;
            matrix_ = new int[numberRooms, numberRooms];
            genEdges();
            return makeMatrix(this.Edges);
        }
        public int[,] makeMatrix(List<Edge> tree)
        {
            int[,] matrix_ = new int[numberRooms, numberRooms];
            tree.ForEach(e =>
            {
                matrix_[e.point1, e.point2] = 1;
                matrix_[e.point2, e.point1] = 1;
            });
            return matrix_;
        }

        #region THUAT TOAN TDK
        public int[,] makeMinimumSpanningTree()
        {
            Edges.Sort((a, b) => a.value.CompareTo(b.value));
            Edges.ForEach(e => Debug.Log(e.toString()));
            List<Edge> minimumSpanningTree = kruskal();
            matrix_ = makeMatrix(minimumSpanningTree);
            return matrix_;
        }

        /// Xay dung cac canh
        private void genEdges()
        {
            for (int i = 0; i < numberRooms; i++)
                for (int j = 0; j < numberRooms; j++)
                {
                    if (i == j) continue;
                    Edges.Add(new Edge(rooms[i].Index, rooms[j].Index, Vector2.Distance(rooms[i].transform.position, rooms[j].transform.position)));
                }
        }

        int findRoot(int point) {
            if (begin[point] == point) return point;
            return findRoot(begin[point]);
        }
        private void union(Edge edge) {
            begin[findRoot(edge.point1)] = begin[findRoot(edge.point2)];
        } 
        private List<Edge> kruskal()
        {
            List<Edge> results = new List<Edge>();
            List<int> nodes = new List<int>();
            int index = 0;
            while (index < Edges.Count)
            {
                if (results.Count == numberRooms-1) break;

                // B1. Chon cac canh co do dai ngan nhat
                Edge edge = Edges[index];

                // B2. Kiem tra canh do co tao thanh 1 chu trinh khong ?
                // Neu hai canh deu da duyet roi thi no tao ra chu trinh
                bool valid = findRoot(edge.point1) != findRoot(edge.point2);

                // Neu tao thanh chu trinh thi chon canh khac nguoc lai thi canh dang xet hop le
                // tien hanh add vao danh sach canh duoc chon. Luu cac dinh cua canh dang xet vao mang
                if (valid)
                {
                    results.Add(edge);
                    if (!nodes.Contains(edge.point1)) nodes.Add(edge.point1);
                    if (!nodes.Contains(edge.point2)) nodes.Add(edge.point2);
                    union(edge);
                    //Debug.Log("Count: "+ results.Count+ " rooms: "+ numberRooms);
                }
                Debug.Log(index);
                index++;
            }

            return results;
        }
        #endregion

        #region THUAT TOAN CAI TIEN
       
        private List<Edge> createMainPath() {
            int initPoint = 0;
            List<Edge> path = new List<Edge>();
            int numberMainRoom = numberRooms / 2;

            int index = initPoint;
            bool [] isVisted = new bool[numberRooms];
            while (numberMainRoom>0) {
                int newPoint = index;
                float distance = 1000f;
                isVisted[index] = true;
                for (int i = 0; i < numberRooms; i++) {
                    if (isVisted[i]) continue;
                    if (i == index) continue;
                    if (distance > Vector2.Distance(rooms[index].transform.position, rooms[i].transform.position)) {
                        newPoint = i;
                        distance = Vector2.Distance(rooms[index].transform.position, rooms[i].transform.position);
                    }
                }
                int j = Edges.FindIndex(e => (e.point1 == index && e.point2 == newPoint) || (e.point2 == index && e.point1 == newPoint));
                path.Add(Edges[j]);
                index = newPoint;
                numberMainRoom--;
            }
            return path;
        }
        public List<Edge> showPath() {
            //List<Edge> path = new List<Edge>();
            //path.ForEach(e => Debug.Log(e.toString()));
            return createMainPath();
        }


        #endregion
    }

    public class Edge
    {
        public int point1;
        public int point2;
        public float value;

        public string toString()
        {
            return "point1: " + point1 + "  ,point2: " + point2 + "  : " + value;
        }
        public Edge(int point1, int point2, float value)
        {
            this.point1 = point1;
            this.point2 = point2;
            this.value = value;
        }
    }

    class CheckCircle
    {

        bool[] isVisited;
        int[,] tableGraph;
        int numberRoom;
        public bool startCheck(int numberRoom, int[,] tableGraph, Edge edge)
        {
            this.isVisited = new bool[numberRoom];
            for (int i = 0; i < numberRoom; i++) isVisited[i] = false;
            this.tableGraph = tableGraph;
            this.numberRoom = numberRoom;
            return check(edge.point1, edge);
        }
        private bool check(int index, Edge edge)
        {
            isVisited[index] = true;
            for (int i = 0; i < numberRoom; i++)
            {
                if (tableGraph[index, i] == 0) continue;
                if (!isVisited[i])
                {
                    if (check(i, edge)) return true;
                }
                else if (i == edge.point1 || i == edge.point2) return true;
            }
            return false;
        }
    }
}