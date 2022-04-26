using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Bullet bullet;
    [SerializeField] Transform gunCore;
    [SerializeField] GameObject playerCore;
    [SerializeField] Transform pointFireBullet;
    [SerializeField] float distanceForCheckCollistionWall = 0.3f;
    [SerializeField] private Vector2 directionMoving = Vector2.zero;
    private Vector2 directionShooting = Vector2.zero;
    private bool isFacingRight = true;
    private Animator myAni;
    private void Awake()
    {
        myAni = playerCore.GetComponent<Animator>();
    }

    private void Update()
    {
        move();
        ditectShootingDirection();
        shoot();
    }



    void move()
    {
        ditectDirection();
        transform.Translate(speed * directionMoving * Time.deltaTime);
        if (directionMoving != Vector2.zero)
        {
            myAni.SetBool("isMoving", true);
        }
        else myAni.SetBool("isMoving", false);
        //if((directionMoving.x > 0 && !isFacingRight) || (directionMoving.x < 0 && isFacingRight)) slipFace();
    }

    void slipFace()
    {
        Vector2 scale = playerCore.transform.localScale;
        scale.x *= -1;
        playerCore.transform.localScale = scale;
        isFacingRight = !isFacingRight;
    }
    void ditectDirection()
    {

        if (Input.GetKeyDown(KeyCode.A))
        {
            directionMoving.x -= 1;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            directionMoving.x += 1;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            directionMoving.y += 1;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            directionMoving.y -= 1;
        }


        if (Input.GetKeyUp(KeyCode.A))
        {
            directionMoving.x += directionMoving.x == 0 ? 0 : 1;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            directionMoving.x -= directionMoving.x == 0 ? 0 : 1;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            directionMoving.y -= directionMoving.y == 0 ? 0 : 1;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            directionMoving.y += directionMoving.y == 0 ? 0 : 1;
        }
    }

    void ditectShootingDirection()
    {
        Vector3 playerSreen = Camera.main.WorldToScreenPoint(gunCore.position);
        directionShooting = Input.mousePosition - playerSreen;
        directionShooting.Normalize();
        float angle = Mathf.Atan2(directionShooting.y, directionShooting.x) * Mathf.Rad2Deg;
        gunCore.rotation = Quaternion.Euler(0, 0, angle);
        if ((Input.mousePosition.x > playerSreen.x && !isFacingRight) || (Input.mousePosition.x < playerSreen.x && isFacingRight)) slipFace();
    }

    private void shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Bullet myBullet = Instantiate(bullet, pointFireBullet.position, Quaternion.identity);
            myBullet.move(directionShooting);
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    print(directionMoving);
    //    print(collision.transform.position);
    //    print(gameObject.transform.position.x);
    //    if (collision.gameObject.CompareTag("collision"))
    //    {
    //        if (directionMoving.x > 0 && collision.transform.position.x > gameObject.transform.position.x)
    //            directionMoving = Vector2.zero;
    //        else if (directionMoving.x < 0 && collision.gameObject.transform.position.x < gameObject.transform.position.x) {
    //            directionMoving.x = 0;
    //            print("wall");
    //        }

    //        if (directionMoving.y > 0 && collision.gameObject.transform.position.y > gameObject.transform.position.y)
    //            directionMoving.y = 0;
    //        else if (directionMoving.y < 0 && collision.transform.position.y < gameObject.transform.position.y)
    //            directionMoving.y = 0;
    //    }

    //}
    private void FixedUpdate()
    {
        checkCollistionWall();
    }

    void checkCollistionWall()
    {
        Vector2 target = transform.position;
        if (directionMoving.x > 0) target.x += distanceForCheckCollistionWall;
        else if (directionMoving.x < 0) target.x -= distanceForCheckCollistionWall;

        if (directionMoving.y > 0) target.y += distanceForCheckCollistionWall;
        else if (directionMoving.y < 0) target.y -= distanceForCheckCollistionWall;

        Collider2D colider = Physics2D.OverlapCircle(target, 0.1f);
        if (colider != null && colider.CompareTag("collision"))
        {
            if (directionMoving.x != 0) directionMoving.x = 0;
            if (directionMoving.y != 0) directionMoving.y = 0;
        }
    }
}
