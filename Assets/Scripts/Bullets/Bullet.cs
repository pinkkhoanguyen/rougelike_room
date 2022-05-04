using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    protected BulletInfo info;
    private void Awake()
    {
        info = loadInfo();
    }
    protected abstract BulletInfo loadInfo();
    public void startMove(Vector2 target) {
        if (info == null) info = loadInfo();
        target.Normalize();
        move(target);
    }
    protected abstract void move(Vector2 target); 
    public GameObject Prefab { get => prefab; }
}


public class BulletInfo {
    private float speed;
    private float damge;
    private float timeCount;

    public BulletInfo(float speed, float damge, float timeCount)
    {
        this.speed = speed;
        this.damge = damge;
        this.timeCount = timeCount;
    }

    public float Speed { get => speed; set => speed = value; }
    public float Damge { get => damge; set => damge = value; }
    public float TimeCount { get => timeCount; set => timeCount = value; }
}

namespace Player {

    /// <summary>
    /// Qua trinh ban dan gom 2 qua trinh. Chuan bi ban va ket thuc ban
    /// Do do de Player va ShootingManager co the biet duoc du lieu cua bullet trong hai qua trinh nay
    /// Ta con cho chung lien ket voi nhau. Thong qua Spirit ta can truyen Player va ShootingManager vao bullet do
    /// </summary>
    public abstract class Bullet:MonoBehaviour {
        [Header("Information")]
        [SerializeField] protected float speed;
        [SerializeField] protected int damage;


        protected Player player;
        protected Vector2 target;
        protected bool isMoving = false;

        protected IShoot shootingmanager;
        public int Damage { get => damage; }

        public virtual void config(Player player, IShoot shooting = null) {
            this.player = player;
            shootingmanager = shooting;
        }

        public abstract void startMove();

    }
}