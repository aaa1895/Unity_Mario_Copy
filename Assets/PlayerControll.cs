using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class PlayerControll : MonoBehaviour
{
    public float speed=0.5f;
    Rigidbody2D Rigidbody;
    float move=0;
    public float jump_force = 1f;

    public AudioClip clear;
    public AudioClip over;

    AudioSource Audioplayer;

    public LayerMask groundLayer;
    bool goJump = false;
    bool onGround = false;

    Animator animator;
    int state = 0;

    public Text Score;
    public static float sCore = 0f;

    //게임 상태
    //bool isDead = false;

    bool isMoving = false;
    float axisH;

    //게임 상태
    public static string gameState = "Playing";

    public GameObject inputUi;
    public GameObject ButtonUi;

    // Start is called before the first frame update
    void Start()
    {
        sCore = 0;
        clear = GetComponent < AudioClip>();
        over = GetComponent<AudioClip>();
        Audioplayer = GetComponent<AudioSource>();
        Rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        int spawn = Random.Range(1, 4);
        ButtonUi.SetActive(false);


    }
    //불규칙적으로 반복실행
    //가끔 불규칙적으로 발생되어 제대로 적용이 안되는 경우도 있어음
    // Update is called once per frame
    void Update()
    {
        
        if(isMoving == false)
        {
            move = Input.GetAxisRaw("Horizontal");
        }
        //Raw -1,0,1만 가능
        //안붇으면 소수점 같은게 가능 살짝식 움직이는게 가능
        //한번에 힘을줌
        //if (isDead != false)
            //return;
        //isDead랑 같은 방식
        if (gameState != "Playing")
            return;

        //move = Input.GetAxisRaw("Horizontal");
        //transform.Translate(new Vector2(move, 0) * speed * Time.deltaTime);
        if (move == -1)
        {
            transform.GetComponent<SpriteRenderer>().flipX = true;
            state = 1;
            animator.SetInteger("State", state);

        }
        else if (move == 1)
        {
            transform.GetComponent<SpriteRenderer>().flipX = false;
            state = 1;
            animator.SetInteger("State", state);
        }
        else
        {
            state = 0;
            animator.SetInteger("State", state);
        }
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
        if (onGround == false)
        {
            state = 2;
            animator.SetInteger("State", state);
        }



    }
    public void Jump()
    {
        goJump = true;
        //if (onGround == true)
        //{
        //    if (goJump == false)
        //    {
        //        state = 2;
        //        animator.SetInteger("State", state);
        //        Rigidbody.velocity = Vector2.zero;
        //        Rigidbody.AddForce(new Vector2(0, jump_force));
        //    }
        //}
        //animator.SetInteger("State", 0);
    }
    //규칙적으로 반복행(0.02,1초에 50번 호출
    //물리를 사용한 처리는 여기서 처리하는것을 권장
    private void FixedUpdate()
    {
        if (gameState != "Playing")
            return;
        //Linecast = 보이지 않는 선을 그려줌
        //84~97까지 역활을 해줌
        //true / false값으로 받아짐
        //착지판정
        onGround = Physics2D.Linecast(transform.position, transform.position - (transform.up * 1.0f),
            groundLayer);

        if (onGround && goJump)
        {
            
            Rigidbody.velocity = Vector2.zero;
            Rigidbody.AddForce(new Vector2(0, jump_force));
            
            //Vector2 jumpPw = new Vector2(0, jump_force);
            //Rigidbody.AddForce(jumpPw, ForceMode2D.Impulse);
            goJump = false;
        }
        //if (onGround || move != 0)
        
        Rigidbody.velocity = new Vector2(move * speed/2, Rigidbody.velocity.y);
        
        
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.contacts[0].normal.y>0.7f)
    //    {
    //        onGround = true;
    //        goJump = false;
    //    }
    //}
    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    onGround = false;
    //    goJump = true;
    //}
    public void Die()
    {
        animator.SetTrigger("Over");
        //isDead = true;
        gameState = "Dead";
        Rigidbody.velocity = new Vector2(0,0);
        Audioplayer.PlayOneShot(over);
        inputUi.SetActive(false);

    }
    public void Clear()
    {
        animator.SetTrigger("Clear");
        gameState = "Clear";
        Rigidbody.velocity = new Vector2(0, 0);
        Audioplayer.PlayOneShot(clear);
        inputUi.SetActive(false);
        
        //GetComponent<CapsuleCollider2D>().enabled = false;
        //Rigidbody.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
    }
    public void GetItem(int a)
    {
        if (a == 1)
        {
            sCore += 10f;
            //Score.text = "" + sCore.ToString("F0");
        }
        else if (a == 2)
        {
            sCore += 30f;
            //Score.text = "" + sCore.ToString("F0");
        }
        else if (a == 3)
        {
            sCore += 50f;
            //Score.text = "" + sCore.ToString("F0");
        }
        else if (a == 4)
        {
            sCore += 100f;
            //Score.text = "" + sCore.ToString("F0");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Dead")
        {
            Die();
            GetComponent<CapsuleCollider2D>().enabled = false;
            Rigidbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
            

        }
        if (collision.gameObject.tag == "Clear")
        {
            Clear();
        }
        

    }
    
    public void SetAxis(float h, float v)
    {
        move = h;
        if(move == 0)
        {
            isMoving = false;
            //print(move);
        }
        else
        {
            isMoving = true;
            //print(move);
        }
    }
}
