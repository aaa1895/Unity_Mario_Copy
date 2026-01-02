using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Monster : MonoBehaviour
{
    SpriteRenderer sprite_render;
    public LayerMask layer;
    public Transform playertrans;
    Animator ani;
    Rigidbody2D rb;
    BoxCollider2D box;
    CapsuleCollider2D cap;
    

    bool onGround = false;
    bool ismove = false;
    bool isDead = false;
    bool shellMove = false;
    bool isHit = false;

    float posi;
    public float speed = 1;
    public float jump_force = 5;
    public bool goomba = false;
    Vector2 dirVec;
    Vector2 nextVec;
    // Start is called before the first frame update
    void Start()
    {
        box = GetComponent<BoxCollider2D>();
        cap = GetComponent<CapsuleCollider2D>();
        sprite_render = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        dirVec = Vector2.zero;
        nextVec = Vector2.zero;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(ismove != true)
        {
            if(transform.position.x-playertrans.position.x > 20)
            {
                ismove= true;
            }
        }
        if (speed < 0)
        {
            sprite_render.flipX = false;
        }
            
        else
        {
            sprite_render.flipX = true;
        }
            
        if (ismove == true)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
        if(shellMove == true)
        {
            rb.velocity = new Vector2(speed*7, rb.velocity.y);   
        }
    }
    void stop()
    {
        shellMove= false;
        ismove = false;
        CircleCollider2D circle;
        circle = GetComponent<CircleCollider2D>();
        box.enabled = false;
        //cap.enabled = false;
        circle.enabled = true;
        ani.StopPlayback();
        gameObject.tag = "Shell";
    }
    private void FixedUpdate()
    {
        onGround = Physics2D.Linecast(transform.position, transform.position - (transform.up * 0.9f), layer);
    }
    void hit()
    {
        box.isTrigger = true;
        sprite_render.flipY = true;
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(speed / 2, 15), ForceMode2D.Impulse);
        Destroy(gameObject, 2f);
    }
    IEnumerator hitBack()
    {
        yield return new WaitForSeconds(2f);
        isHit = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isHit == false)
        {
            isHit = true;
            if (gameObject.tag == "Monster")
            {
                Debug.Log(gameObject.tag);
                if (collision.gameObject.tag == "Player")
                {
                    if (isDead != true && MarioContoller.star == false)
                    {
                        MarioContoller.PowerUp -= 1;
                    }
                    else if (isDead != true && MarioContoller.star == true)
                        hit();
                }
                if (collision.gameObject.tag == "Fire" | collision.gameObject.tag == "Shell")
                {
                    hit();
                }
                else if (collision.gameObject.tag == "DeadZone")
                    Destroy(gameObject);
            }
            else
            {
                Debug.Log(gameObject.tag);
                if (collision.gameObject.tag == "Player")
                {
                    Vector2 playerVelocity = collision.gameObject.GetComponent<Rigidbody2D>().transform.position;
                    if (ismove == false)
                    {
                        shellMove = true;
                    }
                    if (shellMove == true)
                    {
                        if (playerVelocity.x + 0.1f >= rb.velocity.x |
                            playerVelocity.x - 0.1f <= rb.velocity.x)
                        {
                            MarioContoller.PowerUp -= 1;
                        }

                    }
                }
            }
            StartCoroutine(hitBack());
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            speed = -speed;
        }
        if(collision.gameObject.tag == "Player")
        {
            if(isDead!=true)
            {
                rb.velocity = Vector2.zero;
                isDead = true;
                ismove = false;
                Rigidbody2D marioRB = collision.gameObject.GetComponent<Rigidbody2D>();
                marioRB.velocity = Vector2.zero;
                marioRB.AddForce(new Vector2(0, 20f), ForceMode2D.Impulse);
                ani.SetBool("Hit", true);
                if (goomba == true)
                    Destroy(gameObject, 0.2f);
                else
                    stop();
            }
            else
            {
                rb.velocity = Vector2.zero;
                Rigidbody2D marioRB = collision.gameObject.GetComponent<Rigidbody2D>();
                marioRB.velocity = Vector2.zero;
                marioRB.AddForce(new Vector2(0, 20f), ForceMode2D.Impulse);
                if (gameObject.tag == "Shell")
                    shellMove = true;
                else
                    stop();
            }
        }
        
    }
    
}
