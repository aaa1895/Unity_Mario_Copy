using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PowerUp : MonoBehaviour
{
    SpriteRenderer sprite_render;
    public Sprite mushroom;
    public LayerMask layer;
    //public Sprite Fire;
    Animator ani;
    Rigidbody2D rb;
    CircleCollider2D circleCollider;

    bool onGround = false;
    public float speed=1;
    public float jump_force = 5;
    float up;
    // Start is called before the first frame update
    void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        sprite_render = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        up = rb.transform.position.y+1f;
        sprite_render.sprite = mushroom;
        if(MarioContoller.PowerUp == 0)
        {
            ani.enabled= false;
            sprite_render.sprite = mushroom;
        }
        else
        {
            ani.enabled= true;
            //
            //sprite_render.sprite = Fire;
        }
        move();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.transform.position.y >= up)
        {
            circleCollider.isTrigger = false;
            if (sprite_render.sprite != mushroom)
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }

    }
    private void FixedUpdate()
    {
        if (circleCollider.isTrigger == false)
        {
            onGround = Physics2D.Linecast(transform.position, transform.position - (transform.up * 0.9f), layer);
            if (onGround)
            {
                rb.velocity = Vector2.zero;
                rb.AddForce(new Vector2(speed, 0), ForceMode2D.Impulse);
            }
            //rb.velocity = new Vector2((speed * Time.fixedDeltaTime)*2, rb.velocity.y);
        }
    }

    void move()
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(0, jump_force*2), ForceMode2D.Impulse);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            if (MarioContoller.PowerUp == 1)
            {
                if (sprite_render.sprite != mushroom)
                {
                    MarioContoller.PowerUp = 2;
                }
            }
            else
            {
                if(MarioContoller.PowerUp== 2) 
                {
                    MarioContoller.PowerUp = 2;
                }
                else
                    MarioContoller.PowerUp++;
            }
                
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Wall" )
        {
            speed = -speed;
        }
    }
}
