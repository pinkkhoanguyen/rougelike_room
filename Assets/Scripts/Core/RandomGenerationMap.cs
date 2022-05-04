using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGenerationMap : MonoBehaviour
{
    /// Thuat toan duoc chia ra lam cac qua trinh sau day:
    /// - 1. Qua trinh tao main path, day la giai doan toan ra duong di chinh cho map
    /// . DUong di chinh o day co nghia la tu phong bat dau (started room), qua mot loat cac phong trung gian
    /// de den duoc diem dic, chinh la phong Boss (Boss room).
    /// - 2. Qua trinh tao cac nhanh phu. Sau khi tao ra nhanh chinh, tu mot phong bat ky (ngoai tru phong boss) ta tao ra cac duong
    /// di khac. Moi lan tao mot phong can kiem tra xem voi khong gian hien tai co the dat duoc phong do vao vi tri ay hay khong
    /// 
    [Header("Config Level")]
    [SerializeField] private int numberOfRoom;
    [SerializeField] private Room[] templates;
    [Header("Config Debug")]
    [SerializeField] Material materialLine;

    public static RandomGenerationMap instance;
    public Material MaterialLine { get => materialLine; }
    public GeneratorMainPath GenMainPath { get => genMainPath;  }

    private ConfigLevel configLevel;
    private GeneratorMainPath genMainPath;
    private GeneratorOtherPath genOtherPath;
    private void Awake()
    {
        if (instance == null) instance = this;
        configLevel = new ConfigLevel(numberOfRoom, templates);
        genMainPath = gameObject.AddComponent<GeneratorMainPath_ThamLam>();
        genOtherPath = gameObject.AddComponent<GeneratorOtherPath_V1>();
    }

    private void Start()
    {
        StartCoroutine(generate());
    }

    IEnumerator generate() {
        yield return GenMainPath.generate(configLevel);
        yield return new WaitForSeconds(2f);
        yield return genOtherPath.generate(configLevel, configLevel.Rooms);
        Debug.Log("STARTED");
        configLevel.FinishRooms.ForEach(e => {
            e.Status = STATUS_ROOM.IS_STARTED;
        });
    }
}

[SerializeField]
public class ConfigLevel
{
    [SerializeField] private int numberOfRoom;
    [SerializeField] private Room[] listRoomTemplate;
    [SerializeField] private List<Room> rooms = new List<Room>();
    [SerializeField] private List<Room> finishRooms = new List<Room>();
    public ConfigLevel(int numberOfRoom, Room[] listRoomTemplate)
    {
        this.numberOfRoom = numberOfRoom;
        this.listRoomTemplate = listRoomTemplate;
    }

    public int NumberOfRoom { get => numberOfRoom; set => numberOfRoom = value; }
    public Room[] ListRoomTemplate { get => listRoomTemplate; set => listRoomTemplate = value; }
    public List<Room> Rooms { get => rooms; set => rooms = value; }
    public List<Room> FinishRooms { get => finishRooms; set => finishRooms = value; }
}

public interface GeneratorMainPath
{
    IEnumerator generate(ConfigLevel config);
    public void createLobyRoom(Room room);
}

public interface GeneratorOtherPath
{
    IEnumerator generate(ConfigLevel config, List<Room> mainRooms);
}