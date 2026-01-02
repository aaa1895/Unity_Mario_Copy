using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Star : MonoBehaviour
{
    Rigidbody2D rb;
    CircleCollider2D circleCollider;
    public LayerMask layer;
    bool onGround = false;
    float jump_force = 5f;
    public int speed = 5;
    float up;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        circleCollider= rb.GetComponent<CircleCollider2D>();
        up = rb.transform.position.y + 1f;
        move();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.transform.position.y >= up)
        {
            circleCollider.isTrigger = false;
        }
        
    }
    void move()
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(0, jump_force+0.5f), ForceMode2D.Impulse);
    }
    private void FixedUpdate()
    {
        if(circleCollider.isTrigger == false)
        {
            onGround = Physics2D.Linecast(transform.position, transform.position - (transform.up * 0.9f), layer);
            if (onGround)
            {
                rb.velocity = Vector2.zero;
                rb.AddForce(new Vector2(0, jump_force), ForceMode2D.Impulse);
            }
            rb.velocity = new Vector2(speed * Time.fixedDeltaTime, rb.velocity.y);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            speed = -speed;
        }
    }
}
