using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Enemy
{
    public abstract class Enemy : MonoBehaviour
    {
        [Header("Main information")]
        [SerializeField] protected float speed;
        [SerializeField] protected int hp;
        [SerializeField] protected string _name;
        [SerializeField] protected int level;
        [SerializeField] protected int damage;
        [SerializeField] protected GameObject effectOnDead;

        protected Player.Player player;
        private EnemyHpBar hpBar;
        #region Setter nad getter
        protected int Hp
        {
            get => hp;
            set
            {
                if (value <= 0)
                {
                    OnDead();
                }
                else
                {
                    hp = value;
                    hpBar.setHp(hp);
                }
            }
        }

        #endregion
        private void Awake()
        {
            runAwake();
        }

        protected void runAwake()
        {
            findPlayer();
            hpBar = GetComponent<EnemyHpBar>();
            if (hpBar == null) hpBar = gameObject.AddComponent<EnemyHpBar>();
            hpBar.configMaxHP(hp);
        }

        protected void runStart() { }

        protected void findPlayer()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player.Player>();
        }

        private void configColorEffect(GameObject effect)
        {
            Color myColor = transform.GetChild(0).GetComponent<SpriteRenderer>().color;
            int childs = effect.transform.GetChild(0).childCount;
            for (int i = 0; i < childs; i++) {
                SpriteRenderer render = effect.transform.GetChild(0).GetChild(i).GetComponent<SpriteRenderer>();
                render.color = myColor;
            }
            ParticleSystem particleSystem = effect.GetComponentInChildren<ParticleSystem>();
            var main = particleSystem.main;
            main.startColor = myColor;
        }

        public void getDamaged(int damage)
        {
            int HpClone = Hp;
            HpClone -= damage;
            if (HpClone < 0) HpClone = 0;
            Hp = HpClone;
        }

        protected void OnDead() {
            GameObject effect = Instantiate(effectOnDead,transform.position,Quaternion.identity);
            configColorEffect(effect);
            ShakeCamera.instance.shake();
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("MyBullet")) {
                OnGetDamaged(collision.gameObject);   
            }
        }

        virtual protected void OnGetDamaged(GameObject bullet) {
            int damage = player.attack();
            getDamaged(damage);
            Destroy(bullet);
        }
    }
}