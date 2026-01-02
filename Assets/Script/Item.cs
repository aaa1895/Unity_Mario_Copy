using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public GameObject item;
    public GameObject Power;
    public GameObject flower;
    Rigidbody2D rb;
    float speed = -10f;
    // Start is called before the first frame update
    void Start()
    {
        rb= GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void delete()
    {
        Destroy(gameObject);
    }
    void Update()
    {
        if(item.tag =="Coin")
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
            Invoke("delete", 0.5f);
        }
        else
        {
            if (item.tag == "Star")
            {

            }
            else if (item.tag == "LifeUp")
            {

            }
            else
            {
                
            }
        }
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            delete();
        }
        else
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(-speed,0), ForceMode2D.Impulse);
        }
    }
}
