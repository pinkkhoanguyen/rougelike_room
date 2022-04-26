using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Moving_PC_V1 : IMoving
    {
        private Vector2 direction;
        private Vector2 oldDirection;
        private int horizontalDirection = 0;
        private int verticalDirection = 0;
        private void Awake()
        {
            this.runAwake();
        }
        private void Update()
        {
            ditectDirection();
            if (direction != Vector2.zero)
            {
                this.transform.Translate(direction * this.speed * Time.deltaTime);
            }
        }
        private void changeHorizaltalDirection(int hoz)
        {
            horizontalDirection += hoz;
            changeDirection();
        }
        private void changeVerticalDirection(int ver)
        {
            verticalDirection += ver;
            changeDirection();
        }

        private void changeDirection()
        {
            oldDirection = new Vector2(direction.x, direction.y);
            if (horizontalDirection == 0)
                direction.x = 0;
            if (verticalDirection == 0)
                direction.y = 0;
            if (verticalDirection != 0 && horizontalDirection != 0)
                direction = new Vector2(
                    (horizontalDirection > 0 ? 1 : -1) * Mathf.Sqrt(2)/2,
                    (verticalDirection > 0 ? 1 : -1) * Mathf.Sqrt(2) / 2
                    );
            else if (horizontalDirection != 0)
                direction.x = horizontalDirection > 0 ? 1 : -1;
            else if (verticalDirection != 0)
                direction.y = verticalDirection > 0 ? 1 : -1;
            setAnimation();
        }
        private void setAnimation() {
            if (direction.x != 0 && direction.y != 0)
            {
                if (direction.x > 0 && direction.y > 0) mangerAnimator.play("Walking_RightTop");
                if (direction.x < 0 && direction.y < 0) mangerAnimator.play("Walking_LeftDown");
                if (direction.x > 0 && direction.y < 0) mangerAnimator.play("Walking_RightDown");
                if (direction.x < 0 && direction.y > 0) mangerAnimator.play("Walking_LeftTop");
            }
            else if (direction.x == 0 && direction.y == 0) {
                if (oldDirection.x != 0 && oldDirection.y != 0)
                {
                    if (oldDirection.x > 0 && oldDirection.y > 0) mangerAnimator.play("Indle_RightTop");
                    if (oldDirection.x < 0 && oldDirection.y < 0) mangerAnimator.play("Indle_LeftDown");
                    if (oldDirection.x > 0 && oldDirection.y < 0) mangerAnimator.play("Indle_RightDown");
                    if (oldDirection.x < 0 && oldDirection.y > 0) mangerAnimator.play("Indle_LeftTop");
                }
                else if (oldDirection.x > 0) mangerAnimator.play("Indle_Right");
                else if (oldDirection.x < 0) mangerAnimator.play("Indle_Left");
                else if (oldDirection.y < 0) mangerAnimator.play("Indle_Down");
                else if (oldDirection.y > 0) mangerAnimator.play("Indle_Top");
            }
            else if (direction.x > 0) mangerAnimator.play("Walking_Right");
            else if (direction.x < 0) mangerAnimator.play("Walking_Left");
            else if (direction.y < 0) mangerAnimator.play("Walking_Down");
            else if (direction.y > 0) mangerAnimator.play("Walking_Top");
            
        }

        private void ditectDirection()
        {
            /// KEY DOWN
            if (Input.GetKeyDown(KeyCode.A))
            {
                changeHorizaltalDirection(-1);
            }
            else if (Input.GetKeyUp(KeyCode.A))
            {
                changeHorizaltalDirection(1);
            }


            if (Input.GetKeyDown(KeyCode.D))
            {
                changeHorizaltalDirection(1);
            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                changeHorizaltalDirection(-1);
            }


            if (Input.GetKeyDown(KeyCode.W))
            {
                changeVerticalDirection(1);
            }
            
            else if (Input.GetKeyUp(KeyCode.W))
            {
                changeVerticalDirection(-1);
            }


            if (Input.GetKeyDown(KeyCode.S))
            {
                changeVerticalDirection(-1);
            }
            else if (Input.GetKeyUp(KeyCode.S))
            {
                changeVerticalDirection(1);
            }


            /// KEY UP




        }
    }
}