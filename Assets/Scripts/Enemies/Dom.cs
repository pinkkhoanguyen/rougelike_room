using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dom : MonoBehaviour
{
    [SerializeField] float speed = 2f;
    [SerializeField] EnemyBullet bulletTemplate;
    [SerializeField] float sleep;
    [SerializeField] float distanceWithPlayer;
    [SerializeField] float defaultTimeForShooting;
    private float timeForShooting;
    private Transform player;
    private float countSleep;
    private Vector2 target = Vector2.zero;
    private int state = 0;
    private Animator myAni;
    private bool isShooting;
    public int State
    {
        get => state; set
        {
            if (state == 0)
                myAni.SetBool("isMoving", false);
            else
                myAni.SetBool("isMoving", true);
            state = value;
        }
    }

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        myAni = GetComponent<Animator>();
        countSleep = sleep;
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, target) <= 0.1f)
        {
            State = 0;
            target = Vector2.zero;
        }
        if (State == 0)
        {
            indle();
        }
        else if (State == 1)
        {
            randomMMoving();
        }


        // shootting
        if (Vector2.Distance(transform.position, player.position) <= distanceWithPlayer)
        {
            isShooting = true;
        }
        else isShooting = false;

        if (isShooting) shoot();
        else timeForShooting = 0;
    }

    private void shoot()
    {
        timeForShooting -= Time.deltaTime;
        if (timeForShooting <= 0) {
            EnemyBullet bullet = Instantiate(bulletTemplate, transform.position, Quaternion.identity);
            Vector2 dir = player.position - transform.position;
            dir.Normalize();
            bullet.setDirection(dir);
            timeForShooting = defaultTimeForShooting;
        }
    }

    private void indle()
    {
        if (target == Vector2.zero)
        {
            target = transform.position;
            target.x += UnityEngine.Random.Range(-1.3f, 1.3f);
            target.y += UnityEngine.Random.Range(-1.3f, 1.3f);
        }
        countSleep -= Time.deltaTime;
        if (countSleep <= 0)
        {
            State = 1;
            countSleep = sleep;
        }

    }

    void randomMMoving()
    {
        Vector2 dir = target - (Vector2)transform.position;
        transform.Translate(dir * speed * Time.deltaTime);
    }
}
