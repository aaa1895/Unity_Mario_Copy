using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

public class MarioContoller : MonoBehaviour
{
    public float speed;
    public float jump_force;
    float move = 0;
    public LayerMask layer;

    Rigidbody2D rb;
    CapsuleCollider2D cap;
    BoxCollider2D box;
    SpriteRenderer sprite;
    static Animator ani;
    

    public AnimatorController nomal;
    public AnimatorController Big;
    public AnimatorController Fire;
    public AnimatorController Little_Star;
    public AnimatorController Big_Star;
    public GameObject Fire_ball;
    public GameObject flag;

    bool isMoving = false;
    bool goJump = false;
    bool onGround = false;
    bool isDead = false;
    bool isClear = false;
    int state = 0;
    static public bool star = false;
    static public bool flip;
    static public int PowerUp= 0;
    int nowPower = 0;


    Vector2 dirVec;
    Vector2 nextVec;
    // Start is called before the first frame update
    void Start()
    {
        rb= GetComponent<Rigidbody2D>();
        ani= GetComponent<Animator>();
        sprite= GetComponent<SpriteRenderer>();
        cap = GetComponent<CapsuleCollider2D>();
        box= GetComponent<BoxCollider2D>();
        dirVec = Vector2.zero;
        nextVec = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if(isDead == false || isClear == false)
        {
            if (isMoving == false)
            {
                move = Input.GetAxisRaw("Horizontal");
            }
            if (move != 0)
            {
                if (move < 0)
                {
                    flip = true;
                    sprite.flipX = true;
                }

                else
                {
                    flip = false;
                    sprite.flipX = false;
                }

                state = 1;
                ani.SetInteger("State", state);
            }
            else
            {
                state = 0;
                ani.SetInteger("State", state);
            }
            if (!onGround)
            {
                if(isDead== false)
                {
                    state = 2;
                    ani.SetInteger("State", state);
                }
                else
                {
                    state = 5;
                    ani.SetInteger("State", state);
                }
                if(isClear == true)
                {
                    state = 4;
                    ani.SetInteger("State", state);
                }
            }
            else
            {
                if (isClear == true)
                {
                    state = 4;
                    ani.SetInteger("State", state);
                }
                Rigidbody2D flagRB = flag.GetComponent<Rigidbody2D>();
                if(flagRB.transform.position.y <= -2.47f)
                {
                    if(isClear == true)
                    {
                        rb.velocity = Vector2.zero;
                        sprite.flipX = true;
                        rb.transform.position = new Vector2(189.95f, rb.position.y);
                        Debug.Log("erge");
                        Invoke("gotoCatle", 0.5f);
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Z) && onGround)
            {
                Jump();
            }
            if (nowPower != PowerUp & PowerUp >= 0)
            {
                StartCoroutine(sizeChange());
                nowPower = PowerUp;
                
            }
            if (PowerUp < 0 | nowPower < 0)
                StartCoroutine(Dead());
        }
        else
        {
            if(isClear == true)
            {
                state = 4;
                ani.SetInteger("State", state);
            }
        }
    }
    void Jump()
    {
        goJump= true;
    }
    IEnumerator Dead()
    {
        Debug.Log("Dead");
        if (isDead == false)//에러방지
        {
            Debug.Log("DEADPROTOCOL");
            cap.enabled = false; // 충돌 무시
            //isMoving = true;//플레이어 입력 무시
            isDead = true;
            onGround = false;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;//회전 및 옆으로 이동 고정
            little();//죽을때는 무조건 1단계로 고정
            ani.updateMode = AnimatorUpdateMode.UnscaledTime;
            state = 5;//애니메이션 죽는걸로 고정
            ani.SetInteger("State", state);//애니메이션 적용
            
            //yield return new WaitForSeconds(0.1f);
            Time.timeScale = 0; // 시간 정지
            float deadUp = 4;
            float graviteforce = -10f;
            while(gameObject.transform.position.y > -7f)
            {
                //임시 중력값 계산
                deadUp += graviteforce * Time.unscaledDeltaTime;

                //위치 이동
                transform.position += Vector3.up * deadUp * Time.unscaledDeltaTime*4f;

                yield return null;
            }
        }
    }
    void goal()//골인
    {
        isMoving = true;
        isClear= true;
        rb.gravityScale = 0;
        state = 4;
        ani.SetInteger("State", state);
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(0, -speed/100), ForceMode2D.Impulse);
    }
    void gotoCatle()//깃발에서 자동으로 성으로 걸어가도록 설정
    {
        if (onGround == true)
        {
            rb.gravityScale = 10f;
            sprite.flipX = false;
            isClear= false;
            isMoving = true;
            move = 1;
            rb.AddForce(new Vector2(speed * move, 0), ForceMode2D.Impulse);
        }

    }
    void pause()//정지
    {
        Time.timeScale = 0f;
    }
    void restart()//재시작
    {
        Time.timeScale = 1f;
    }
    void little()//처음마리오로 돌아가기
    {
        ani.runtimeAnimatorController = nomal;
        cap.offset = new Vector2(-0.01049516f, 0.01574278f);
        cap.size = new Vector2(0.6011841f, 1.031486f);
    }

    void grow()//버섯먹고 커지기
    {
        ani.runtimeAnimatorController = Big;
        cap.offset = new Vector2(0f, -0.1f);
        cap.size = new Vector2(0.6162429f, 1.749088f);
    }
    void flower()//꽃먹고 변신
    {
        ani.runtimeAnimatorController = Fire;
        cap.offset = new Vector2(0f, -0.1f);
        cap.size = new Vector2(0.6162429f, 1.749088f);
    }
    void starEnd()
    {
        star = false;
    }
    private IEnumerator sizeChange()
    {
        ani.updateMode = AnimatorUpdateMode.UnscaledTime;
        float elased = 0f;
        float duration = 6f;
        isMoving = true;
        pause();
        while (elased < duration) 
        {
            //Debug.Log(elased);
            if (nowPower == 0 && PowerUp == 1)
            {
                yield return new WaitForSecondsRealtime(0.5f);
                little();
                yield return new WaitForSecondsRealtime(0.1f);
                grow();

            }
            else if (nowPower == 1 && PowerUp == 2)
            {
                yield return new WaitForSecondsRealtime(0.3f);
                grow();
                yield return new WaitForSecondsRealtime(0.5f);
                flower();
            }
            else if (nowPower == 2 && PowerUp == 1)
            {
                
                yield return new WaitForSecondsRealtime(0.1f);
                flower();
                yield return new WaitForSecondsRealtime(0.5f);
                grow();
            }
            else if(nowPower==1 && PowerUp == 0)
            {
                Debug.Log("change");
                yield return new WaitForSecondsRealtime(0.5f);
                grow();
                yield return new WaitForSecondsRealtime(0.7f);
                little();
            }
            elased++;
        }
        yield return new WaitForSecondsRealtime(0.3f);
        restart();
        isMoving= false;
        //ani.updateMode = AnimatorUpdateMode.Normal;
    }
    private void FixedUpdate()
    {
        //점프
        onGround = Physics2D.Linecast(transform.position, transform.position - (transform.up * 1f),layer);
        if(isDead!=true)
        {
            if (onGround && goJump)//땅에 닿고 점프키를 눌렀을때 작동
            {
                rb.velocity = Vector2.zero;
                rb.AddForce(new Vector2(0, jump_force), ForceMode2D.Impulse);
                goJump = false;
            }
            rb.velocity = new Vector2(move * speed * Time.fixedDeltaTime, rb.velocity.y);
        }
    }
    private void LateUpdate()
    {
        //불쏘기
        if (PowerUp == 2 && Input.GetKeyDown(KeyCode.X))
        {
            Instantiate(Fire_ball, transform.position, Quaternion.identity);
            //Fire_ball.GetComponent<Rigidbody2D>().AddForce(new Vector2(speed * Time.deltaTime,0),ForceMode2D.Impulse);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "DeadZone")
        {   
            StartCoroutine(Dead());
        }
        if(collision.gameObject.tag == "Flag")
        {
            goal();
        }
        if(collision.gameObject.tag == "Star")
        {
            star = true;
            Invoke("starEnd", 15f);
            if (nowPower==0)
            {
                ani.runtimeAnimatorController = Little_Star;
                Invoke("little", 15f);
                
            }
            else if(nowPower==1) 
            {
                ani.runtimeAnimatorController = Big_Star;
                Invoke("grow", 15f);
            }
            else
            {
                ani.runtimeAnimatorController = Big_Star;
                Invoke("flower", 15f);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Catle")
        {
            rb.drag = 0f;
            rb.velocity = Vector2.zero;
            sprite.enabled = !sprite.enabled;
        }
    }
}
