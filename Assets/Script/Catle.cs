using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catle : MonoBehaviour
{
    public GameObject catle_Flag;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody2D flagrb = catle_Flag.GetComponent<Rigidbody2D>();
        if(flagrb.transform.position.y>= 1.3f)
        {
            Debug.Log("f");
            flagrb.drag = 0;
            flagrb.velocity = Vector2.zero;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            Debug.Log("d");
            catle_Flag.SetActive(true);
            Rigidbody2D flagrb = catle_Flag.GetComponent<Rigidbody2D>();
            flagrb.AddForce(new Vector2(0, 4f), ForceMode2D.Impulse);
        }
    }
}
