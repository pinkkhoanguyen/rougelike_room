using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Lop dinh nghia cac hanh dong cua spirit. Dat biet, no con dinh nghia luon phuong thuc shoot cua player
/// . Phuogn thuc nay chu yeu la tao 1 bullet va goi ham move cua bullet do ma thoi
/// </summary>
public class Spirit : MonoBehaviour
{
    [Header("List Speed")]
    [SerializeField] float speedIndle;
    [SerializeField] float speedFlowPlayer;
    [SerializeField] float speedTeleport;

    [Header("Other Information")]
    [SerializeField] float sleepOnIndle = 1f;
    [SerializeField] float waitingShootTime = 4f;
    [SerializeField] GameObject teleport_effect;

    [Header("Bullet config")]
    [SerializeField] Player.Bullet[] bullets;
    [SerializeField] BulletType type;
    private Player.Bullet currentBullet;

    private Vector2 target;
    private Animator animator;

    static public int INDLE = 0;
    static public int INDLE_WAITING = 1;
    static public int FOLLOW_PLAYER = 2;
    static public int TELEPORT = 3;
    static public int WAITING_SHOT = 4;
    static public int END_SHOT = 5;

    public int state = 0;
    private float sleepTime = 1f;
    private bool trailIsHiden = false;
    private TrailRenderer trailRenderer;

    #region properties need config by Player instance
    // properties need config by Player instance
    private Player.Player player;
    private Transform pointSpiritInPlayer;
    #endregion

    // INDLE_WAITING > INDLE > FLOW_PLAYER > TELEPORT > WAITING_SHOOT > END_SHOOT
    public int State
    {
        get => state;
        set
        {

            if (value == INDLE) target = Vector2.zero;
            if (value != FOLLOW_PLAYER && value != WAITING_SHOT)
                animator.SetBool("isMoving", false);
            else animator.SetBool("isMoving", true);
            if(value == TELEPORT) Instantiate(teleport_effect, transform.position, Quaternion.identity);

            if (value != WAITING_SHOT) setHiddenTrail(false);
            if (!(state == WAITING_SHOT) || (state == WAITING_SHOT && value == END_SHOT)) {
                state = value;
            }

        }
    }

    public Transform PointSpiritInPlayer { get => pointSpiritInPlayer; set => pointSpiritInPlayer = value; }
    public float WaitingShootTime { get => waitingShootTime; }

    public void config(Player.Player player, Transform pointSpiritInPlayer)
    {
        this.player = player;
        this.PointSpiritInPlayer = pointSpiritInPlayer;
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
        trailRenderer = GetComponentInChildren<TrailRenderer>();

        // config
        //config bullet
        configBullet();

        if (animator == null) animator = GetComponentInChildren<Animator>();
    }
    private void Update()
    {
        if (State == INDLE_WAITING)
        {
            sleep();
        }
        else if (State == INDLE)
        {
            indleRandomMove();
        }
        else if (State == FOLLOW_PLAYER)
        {
            flowPlayer();
        }
        else if (State == TELEPORT)
        {
            teleport();
        }
        else if (State == WAITING_SHOT)
        {
            this.transform.position = PointSpiritInPlayer.transform.position;
            setHiddenTrail(true);           

        }
        else if (State == END_SHOT) {
            State = INDLE_WAITING;
        }
    }

    #region Sleep
    private void sleep()
    {
        sleepTime -= Time.deltaTime;
        if (sleepTime <= 0)
        {
            State = INDLE;
            sleepTime = sleepOnIndle;
        }

    }
    #endregion

    #region INDLE
    private Vector2 createTarget()
    {
        Vector2 result = transform.position;
        result.x += Random.Range(-0.4f, 0.4f);
        result.y += Random.Range(-0.4f, 0.4f);
        return result;
    }
    private void indleRandomMove()
    {
        if (target == Vector2.zero)
            target = createTarget();
        Vector2 direction = target - (Vector2)transform.position;
        direction.Normalize();
        transform.Translate(speedIndle * direction * Time.deltaTime);
        if (Vector2.Distance(transform.position, target) <= 0.01f)
            State = INDLE_WAITING;
    }
    #endregion

    #region FLLOW PLAYER
    private void flowPlayer()
    {
        Vector2 dir = PointSpiritInPlayer.position - transform.position;
        if (Vector2.SqrMagnitude(dir) >= .7f)
            dir.Normalize();
        transform.Translate(dir * speedFlowPlayer * Time.deltaTime);
        if (Vector2.Distance(transform.position, PointSpiritInPlayer.position) <= 0.01f)
        {
            State = INDLE_WAITING;
        }
    }
    #endregion

    #region Shooting
    private void teleport()
    {
        Vector2 dir = PointSpiritInPlayer.position - transform.position;
        if(Vector2.Distance(PointSpiritInPlayer.position, transform.position) <= 1f)
        dir.Normalize();
        transform.Translate(dir * speedTeleport * Time.deltaTime);
        if (Vector2.Distance(transform.position, PointSpiritInPlayer.position) <= 0.1f)
        {
            transform.position = PointSpiritInPlayer.position;
            State = WAITING_SHOT;
            // transform.SetParent(PointSpiritInPlayer.parent);
        }
        //transform.position = PointSpiritInPlayer.position;
        //State = WAITING_SHOT;
    }
    #endregion
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) return;
        State = INDLE_WAITING;
    }
    public void shoot(Player.IShoot shooting)
    {
        Player.Bullet bulletObj = Instantiate(currentBullet, transform.position, Quaternion.identity);
        bulletObj.config(player, shooting);
        bulletObj.startMove();
    }

    private void configBullet()
    {
        switch (type)
        {
            case BulletType.NORMAL:
                currentBullet = bullets[0];
                break;
            case BulletType.SPIRIT_POWER:
                currentBullet = bullets[1];
                break;
            default: throw new System.Exception("TYPE INVALID");
        }
    }
    private void setHiddenTrail(bool hiden) {
        trailIsHiden = hiden;
        trailRenderer.gameObject.SetActive(!hiden);
    }

    public int getDamage() {
        return currentBullet.Damage;
    }
}


public enum BulletType
{
    NORMAL,
    SPIRIT_POWER
}