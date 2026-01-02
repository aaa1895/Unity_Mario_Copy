using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public float speed = 3.0f;

    public string upAnime = "Back";
    public string downAnime = "Front";
    public string leftAnime = "Left";
    public string rightAnime = "Right";
    public string deadAnime = "Dead";
    string nowAnimaition = "";
    string oldAnimaition = "";

    float axisH;
    float axisV;
    //회전 각
    public float angleZ = -90.0f;

    //데미지 처리
    public static int hp = 3;
    public static string gameState;
    bool inDamage = false;


    Rigidbody2D rb;
    bool isMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        oldAnimaition = downAnime;
        gameState = "Playing";

        hp = PlayerPrefs.GetInt("PlayerHP");

    }

    // Update is called once per frame
    void Update()
    {
        
        if (isMoving == false)
        {
            axisH = Input.GetAxisRaw("Horizontal");
            axisV = Input.GetAxisRaw("Vertical");

        }
        Vector2 fromPt = transform.position;
        Vector2 toPt = new Vector2(fromPt.x + axisH, fromPt.y + axisV);
        angleZ = GetAngle(fromPt, toPt);

        //이동 각도에서 방량과 애니메이션 변경
        if(angleZ >= -45 && angleZ <45)
        {
            nowAnimaition = rightAnime;
        }
        else if(angleZ >= 45 && angleZ <=135)
        {
            nowAnimaition = upAnime;
        }
        else if(angleZ >= -135 && angleZ <=-45)
        {
            nowAnimaition = downAnime;
        }
        else
        {
            nowAnimaition  = leftAnime;
        }

        if(nowAnimaition != oldAnimaition) 
        {
            oldAnimaition = nowAnimaition;
            GetComponent<Animator>().Play(nowAnimaition);
        }
    }
    private void FixedUpdate()
    {
        if(gameState == "GameClear")
        {
            rb.velocity = new Vector2(0, 0);
        }
        if (gameState != "Playing")
        {
            return;
        }
        if(inDamage)
        {
            //데미지 받는 중엔 점멸 시키기
            float val = Mathf.Sin(Time.time * 50);

            if(val > 0) 
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
            return;
        }    
        //이동
        rb.velocity = new Vector2(axisH, axisV) * speed;

    }
    float GetAngle(Vector2 p1, Vector2 p2)
    {
        float angle;
        if (axisH != 0 || axisV != 0)
        {
            //이동중이라면 각도 변경
            // p1과 p2 차이 구하기(원점을 0으로 하기 위해)
            float dx =p2.x - p1.x;
            float dy =p2.y - p1.y;
            //아크 탄젠트 함수로 각도(라디안)구하기
            float rad = Mathf.Atan2(dy, dx);
            //라디안 각으로 변환
            angle = rad * Mathf.Rad2Deg;
        }
        else
        {
            //정지중이면 이전 각도를 유지
            angle = angleZ;
        }
        return angle;
    }
    void GameOver()
    {
        //GameObject.FindObjectOfType<SoundManager>().GameOverBgm();
        gameState = "GameOver";
        //게임 오버 연출
        // 충돌 판정 비활성
        GetComponent<CircleCollider2D>().enabled = false;
        //이동 중지
        rb.velocity = new Vector2(0, 0);
        //중력값 이용해서 플레이어를 위로 튀어오르게 하는 연출
        rb.gravityScale = 1f;
        rb.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
        //애니메이션 변경
        GetComponent<Animator>().Play(deadAnime);
        //1초후 캐릭터 제거
        Destroy(gameObject, 1.0f);
    }
    //대미지 받기 끝
    void DamageEnd()
    {
        //데미지 받는 중 아님으로 설정
        inDamage = false;
        //스프라이트 되돌리기
        gameObject.GetComponent<SpriteRenderer>().enabled = true;

    }
    void GetDamager(GameObject enemy)
    {
        if(gameState == "Playing")
        {
            //GameObject.FindObjectOfType<SoundManager>().getDamage();
            hp--;
            // hp저장
            PlayerPrefs.SetInt("PlayerHP", hp);
            if(hp > 0)
            {
                rb.velocity = new Vector2(0, 0);
                // 적 캐릭터의 반대 방향으로 히트 백
                Vector3 toPos = (transform.position - enemy.transform.position).normalized;
                rb.AddForce(new Vector2(toPos.x *4, toPos.y * 4), ForceMode2D.Impulse);
                
                //데미지 받는 중으로 설정
                inDamage = true;
                Invoke("DamageEnd", 0.25f);
            }
            else
            {
                GameOver();
            }
        }
    }
    public void SetAxits(float h, float v)
    {
        axisH = h;
        axisV = v;
        if(axisH == 0 && axisV == 0) 
        {
            isMoving = false;

        }
        else
        {
            isMoving=true;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            GetDamager(collision.gameObject);
        }

    }
}
