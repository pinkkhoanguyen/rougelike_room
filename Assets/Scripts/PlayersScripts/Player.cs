using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public class Player : MonoBehaviour
    {
        [Header("Information")]
        [SerializeField] private string _name;
        [SerializeField] private int hp;
        [SerializeField] private float speed;
        [SerializeField] MovingPlayerClassName movingType;

        private IMoving moving;

        public string Name { get => _name; set => _name = value; }
        public int Hp { get => hp; set => hp = value; }
        public float Speed { get => speed; set => speed = value; }

        private void Awake()
        {
            config();
        }
        #region Config for player
        void config() {

            // config moving
            configMoving();
        }
        void configMoving() {
            if(movingType == MovingPlayerClassName.Moving_PC_V1)
               moving = gameObject.AddComponent<Moving_PC_V1>();

            moving.setAttributes(this);
        }
        #endregion
    }

    public enum MovingPlayerClassName
    {
        Moving_PC_V1
    }
}