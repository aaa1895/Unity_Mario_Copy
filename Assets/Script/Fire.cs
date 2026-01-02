using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    Rigidbody2D rb;
    Animator ani;
    SpriteRenderer sp;
    public LayerMask layer;
    bool onGround = false;
    public float jump_force = 2f;
    public float speed = 5;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
        bool mflip = MarioContoller.flip;
        if(mflip == false)
        {
            sp.flipX= false;
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(speed, 0), ForceMode2D.Impulse);
        }
        else
        {
            sp.flipX= true;
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(-speed, 0), ForceMode2D.Impulse);
        }
        
        Destroy(gameObject, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        onGround = Physics2D.Linecast(transform.position, transform.position - (transform.up * 0.8f), layer);
        if (onGround)
        {
            //rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(0, jump_force), ForceMode2D.Impulse);
        }
        //rb.velocity = new Vector2(speed * Time.deltaTime, rb.velocity.y);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!onGround | collision.gameObject.tag == "Monster")
        {
            rb.velocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static;
            ani.SetBool("Hit", true);
            Destroy(gameObject, 0.2f);
        }
        
    }
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "Wall" | collision.gameObject.tag=="Block")
    //    {
    //        rb.velocity = Vector2.zero;
    //        rb.bodyType = RigidbodyType2D.Static;
    //        ani.SetBool("Hit", true);
    //        Destroy(gameObject, 0.2f);
    //    }
        
    //}
}
