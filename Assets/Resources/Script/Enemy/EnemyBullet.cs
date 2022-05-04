using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] float speed;
    private Vector2 direction = Vector2.zero;

    public void setDirection(Vector2 d) {
        direction = d;
    }
    private void Update()
    {
        transform.Translate(speed * direction * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) return;
        Destroy(gameObject);
    }
}
