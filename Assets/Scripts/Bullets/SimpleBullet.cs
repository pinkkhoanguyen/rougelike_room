using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBullet : Bullet
{
    private Rigidbody2D myRigidbody;
    public float speed;
    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    protected override BulletInfo loadInfo()
    {
        return new BulletInfo(speed, 2f, 3f);
    }

    protected override void move(Vector2 target)
    {
        myRigidbody.AddForce(target * this.info.Speed, ForceMode2D.Impulse);
    }
    private void OnTriggerEnter2D(Collider2D other) {
        print(other.name);
        Destroy(gameObject); 
    }
}
