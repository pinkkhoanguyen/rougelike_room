using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    [SerializeField] private float distanceWithPlayer = 4f;
    [SerializeField] float speed;
    [SerializeField] float speedWalking;
    [SerializeField] float sleep = 0.7f;
    private float countSleep;
    private Vector2 target;
    private Transform player;
    private int state = 0;  // 
    private Animator ani;
    public int State
    {
        get => state; 
        set
        {
            if (value == 0) ani.SetBool("isMoving", false);
            else ani.SetBool("isMoving", true);
            state = value;
        }
    }

    private void Awake()
    {
        this.player = GameObject.FindGameObjectWithTag("Player").transform;
        countSleep = sleep;
        ani = GetComponent<Animator>();
    }


    private void Update()
    {
        if (Vector2.Distance(transform.position, target) <= 0.1f)
        {
            State = 0;
            target = Vector2.zero;
        }
        else if (Vector2.Distance(transform.position, player.position) <= 0.1)
        {
            State = 0;
            target = Vector2.zero;
        }
        else if(Vector2.Distance(transform.position, player.position) <= distanceWithPlayer)
        {
            State = 2;
            target = Vector2.zero;

        }

        else if (State == 2 && Vector2.Distance(transform.position, player.position) > distanceWithPlayer)
        {
            State = 0;
            target = Vector2.zero;

        }



        if (State == 2)
        {
            moveToPlayer();
        }
        else if (State == 0)
        {
            indle();
        }
        else if (State == 1)
        {
            randomMMoving();
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

    void moveToPlayer()
    {
        Vector2 dir = player.position - transform.position;
        dir.Normalize();
        transform.Translate(dir * speed * Time.deltaTime);
    }

    void randomMMoving()
    {
        Vector2 dir = target - (Vector2)transform.position;
        transform.Translate(dir * speedWalking * Time.deltaTime);
    }
}
