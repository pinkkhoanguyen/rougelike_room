using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Lop nay co nhiem vu dinh nghia khi nao nguoi dung se tien hanh ban sung (Tuy theo logic cua cac phien ban)
/// Tien hanh xu ly su kien do. Buoc cuoi cung la goi method shoot cua Spirit
/// Lop nay khong chu dong dinh nghia method shoot. Nhiem vu cua no chi la dinh nghia khi nao sung duoc ban
/// Trong truong hop moi lan ban can co mot du lieu nao do, ta can phai dinh nghia tai lop nay, va khi ban thi truyen
/// no vao  spirit
/// </summary>
namespace Player {
    public class Shooting_PC_V1 : IShoot
    {
        private float countdown;
        
        private bool isShooting = false;
        private void LateUpdate()
        {
            handleShooting();
            if (isShooting) { 
                rotatePoint();
                countdown -= Time.deltaTime;
                if(countdown <= 0)
                {
                    isShooting = false;
                    spirit.State = Spirit.END_SHOT;
                    countdown = spirit.WaitingShootTime;
                }
            }
        }

        private void handleShooting() {
            if (Input.GetMouseButtonDown(0))
            {
                if (isShooting)
                {
                    spirit.shoot(this);
                }
                else if (!isShooting)
                {
                    this.spirit.State = Spirit.TELEPORT;
                }
                countdown = spirit.WaitingShootTime;
                isShooting = true;
            }
        }

        private void rotatePoint() {
            Vector2 pos = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
            float deg = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;
            PointSpiritInPlayer.transform.rotation = Quaternion.Euler(0, 0, deg);
        }

        public override void onBeginShoot(ShootingMassage massage)
        {
            this.massage = massage;
        }

        public override void onEndShoot(ShootingMassage massage)
        {
            this.massage = massage;
        }
    }
}