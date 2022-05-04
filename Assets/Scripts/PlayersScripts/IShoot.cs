using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public abstract class IShoot : MonoBehaviour
    {
        protected Spirit spirit;
        protected Transform PointSpiritInPlayer;
        protected ShootingMassage massage = new ShootingMassage();

        public ShootingMassage Massage { get => massage;}

        protected void runAwake()
        {
        }
        public void setAttributes(Player player)
        {
            this.spirit = player.Spirit;
            PointSpiritInPlayer = player.transform.GetChild(1).transform;
        }
        public abstract void onBeginShoot(ShootingMassage massage);
        public abstract void onEndShoot(ShootingMassage massage);

    }

    public class ShootingMassage
    {
        public string agr1;
        public string arg2;

        public ShootingMassage(string agr1, string arg2)
        {
            this.agr1 = agr1;
            this.arg2 = arg2;
        }
        public ShootingMassage() { }
    }
}
