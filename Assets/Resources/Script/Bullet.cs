using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float timeDestroy = 1f;
    private Rigidbody2D rigidbody;
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }
    public void move(Vector2 dir) {
        rigidbody.AddForce(speed * dir, ForceMode2D.Impulse);
    }
    private void Start()
    {
        StartCoroutine(destroy());
    }
    IEnumerator destroy() {
        yield return new WaitForSeconds(timeDestroy);
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        StopCoroutine("destroy");

        Destroy(gameObject);
    }
}
