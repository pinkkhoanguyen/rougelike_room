using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public abstract class IMoving : MonoBehaviour
    {
        protected float speed;
        protected ChangeAnimation mangerAnimator;
        protected void runAwake() {
            mangerAnimator = GetComponent<ChangeAnimation>();
            if (mangerAnimator == null) mangerAnimator = gameObject.AddComponent<ChangeAnimation>();
        }
        public void setAttributes(Player player)
        {
            speed = player.Speed;
        }
    }
}