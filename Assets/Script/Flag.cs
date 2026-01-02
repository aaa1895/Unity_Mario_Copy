using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    public GameObject flat_paint;
    public GameObject Player;
    BoxCollider2D box;
    Rigidbody2D rb;

    bool clear = false;

    // Start is called before the first frame update
    void Start()
    {
        box= GetComponent<BoxCollider2D>();
        rb = flat_paint.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
      if(clear == true)
        {
            if (rb.transform.position.y == -3.473913f)
            {
                Debug.Log("qwe");
                Rigidbody2D playerRB = Player.GetComponent<Rigidbody2D>();
                playerRB.MovePosition(new Vector2(189.85f, -2.5f));
            }
        }
    }
    void flag_p()
    {
        rb.AddForce(new Vector2(0, -4f), ForceMode2D.Impulse);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            clear = true;
            flag_p();
            
        }
            
    }
}
