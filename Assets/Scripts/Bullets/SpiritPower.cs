using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public class SpiritPower : Bullet
    {
        [SerializeField] float speedRotate = 200f;
        [SerializeField] float distanceTarget = 2f;
        private Rigidbody2D myRigidbody;
        private IShoot managerShooting;
        private bool isLeft;
        private void Awake()
        {
            myRigidbody = GetComponent<Rigidbody2D>();
        }
        public override void startMove()
        {
            managerShooting.onBeginShoot(new ShootingMassage(isLeft ? "1" : "0", ""));
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            this.isMoving = true;
        }

        private void FixedUpdate()
        {
            if (Input.GetMouseButtonDown(0))
            {
                startMove();
            }
            if (isMoving)
            {
                moving();
            }
        }
        private void moving()
        {
            Vector3 moveDir = transform.right;
            Vector2 direction = target - myRigidbody.position;
            direction.Normalize();

            float angle = Vector3.Cross(moveDir, direction).z;
            myRigidbody.angularVelocity = speedRotate * angle * (isLeft ? 1 : -1);
            myRigidbody.velocity = moveDir * speed * (isLeft ? 1 : -1);



            if (Vector2.Distance(target, this.transform.position) <= distanceTarget)
            {
                myRigidbody.angularVelocity = Quaternion.LookRotation(transform.right, direction).z;
            }
            if (Vector2.Distance(target, this.transform.position) <= 0.1f) Destroy(gameObject);
        }
        public override void config(Player player, IShoot shooting)
        {
            base.config(player, shooting);
            if (shooting.Massage.agr1 == "0") isLeft = true;
            else isLeft = false;
            managerShooting = shooting;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Spirit") ||
                collision.CompareTag("Player") ||
                 collision.CompareTag("MyBullet")
             ) return;
            Destroy(gameObject);
        }

    }
}
