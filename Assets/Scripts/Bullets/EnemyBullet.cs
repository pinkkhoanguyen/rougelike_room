using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Enemy
{
    public class EnemyBullet : Bullet
    {
        private Rigidbody2D myBody;
        private Vector2 target;
        private bool isMove = false;
        private void Awake()
        {
            myBody = GetComponent<Rigidbody2D>();
        }

        public void config(BulletInfo info)
        {
            this.info = info;
        }

        protected override BulletInfo loadInfo()
        {
            return null;
        }

        private void Update()
        {
            transform.Translate(target * this.info.Speed * Time.deltaTime);
        }

        protected override void move(Vector2 target)
        {
            this.target = target;
            isMove = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(tag)) return;
            Destroy(gameObject);
        }
    }
}