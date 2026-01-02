using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Wall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if(collision.gameObject.tag == "PowerUp" | collision.gameObject.tag == "Star")
        //{
        //    Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        //    rb.velocity = Vector2.zero;
        //    rb.AddForce(new Vector2(-speed, 0),ForceMode2D.Impulse);
        //}
    }
}
