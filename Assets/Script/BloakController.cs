using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloakController : MonoBehaviour
{
    public GameObject Item;
    public Sprite empty;
    public GameObject partical;
    Animator ani;
    Rigidbody2D rigidbody2;
    
    
    
    Vector2 trans;

    bool touch;
    SpriteRenderer sprite ;
    public bool haveItem = false;
    public int item_count = 0;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2 = GetComponent<Rigidbody2D>();
        ani= GetComponent<Animator>();
        sprite= GetComponent<SpriteRenderer>();
        rigidbody2.bodyType = RigidbodyType2D.Static;
        trans = rigidbody2.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //if (rigidbody2.transform.position.y > trans.y)
        //{ 
        //    rigidbody2.velocity = Vector2.zero;
        //    rigidbody2.MovePosition(trans);
        //    //rigidbody2.AddForce(new Vector2(0, -10), ForceMode2D.Impulse);
        //}
    }   

    //void back()
    //{
    //    if (rigidbody2.transform.position.y == trans.y)
    //       rigidbody2.constraints = RigidbodyConstraints2D.FreezeAll;
    
    //}
    void spawnItem()
    {
        item_count--;
        Instantiate(Item, new Vector3(trans.x, trans.y, 0), Quaternion.identity);
        if (item_count == 0)
        {
            BoxCollider2D box = GetComponent<BoxCollider2D>();
            CapsuleCollider2D cap = GetComponent<CapsuleCollider2D>();
            ani.enabled = false;
            box.enabled = true;
            cap.enabled = false;
            sprite.enabled = true;
            sprite.sprite = empty;
        }

    }
    private void FixedUpdate()
    {
        if(transform.position.y <trans.y&&touch)
        {
            transform.position = trans;
            rigidbody2.velocity = Vector2.zero;
            rigidbody2.bodyType = RigidbodyType2D.Static;
            if(sprite.sprite != empty) 
            {
                touch = false;
            }
            
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && touch == false)
        {
            if(sprite.enabled == true)
            {
                touch = true;
                //rigidbody2.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                rigidbody2.bodyType = RigidbodyType2D.Dynamic;
                rigidbody2.velocity = Vector2.zero;
                rigidbody2.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
                if (haveItem == true)
                {
                    if (item_count != 0)
                    {
                        //SpriteRenderer sprite = GetComponent<SpriteRenderer>();
                        //sprite.sprite = empty;
                        spawnItem();
                        //Invoke("back", 0.1f);
                    }
                }
                else
                {
                    if (MarioContoller.PowerUp != 0)
                    {
                        Destroy(gameObject);
                        GameObject obj = Instantiate(partical);
                        obj.transform.position = trans;
                        obj.transform.position = new Vector3(trans.x, trans.y, -1);
                        Destroy(obj, 0.5f);
                    }

                }
            }
            else
            {
                Vector2 playerVelocity = collision.gameObject.GetComponent<Rigidbody2D>().velocity;
                if (playerVelocity.y > 0)
                    if (rigidbody2.transform.position.x + 0.1f >= collision.gameObject.transform.position.x | rigidbody2.transform.position.x - 0.1f <= collision.gameObject.transform.position.x)
                        spawnItem();
            }
            
        }
    }
}
