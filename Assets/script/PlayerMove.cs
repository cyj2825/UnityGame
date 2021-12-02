using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Scene간 이동이나 재시작, 불러오기를 하려면 UnityEngine.SceneManagement를 추가해주어야 함

public class PlayerMove : MonoBehaviour
{
    private int coin;
    private bool isClear = false;
    public Text coinText;
    private Animator animator;    // 애니메이션 컴포넌트를 불러오는 것
    private Vector2 pos;    // 캐릭터 움직임을 위해 키보드 입력을 받을 때 입력된 float형을 저장하기 위한 변수.
    // vector2는 유니티에서 지원하는 변수형. 말 그대로 2방향의 벡터(x, y)를 저장하는 자료형이다.

    public float movePower = 0.5f;    // 유니티에서 float형 변수 뒤에는 f를 붙인다.(안 붙여도 되긴함)
    public float jumpPower = 80f;

    Rigidbody2D rigid;    // rigidbody는 전에 추가했던 컴포넌트인데, 물리엔진을 사용하기 위한 컴포넌트.
    bool isJumping = false;    // 점프중인지 판단을 위한 bool형 변수. 설정하지 않으면 공중에서도 계속 점프 가능해짐.
    // Start is called before the first frame update
    void Start()    // 처음 시작할 때 1번 실행됨
    {
        animator = gameObject.GetComponent<Animator>();   // 이 스크립트를 적용한 게임오브젝트의 <> 안 컴포넌트들이 이 스크립트에서 다룰 수 있도록 함
        rigid = gameObject.GetComponent<Rigidbody2D>();
        coin = 0;
        coinText.text = "Coin:" + coin;
    }

    // Update is called once per frame
    void Update()   // 매 프레임마다 계속 반복실행됨.
    {
        Move();
        Jump();
        if(isClear)
        {
            if (Input.GetKey(KeyCode.A))
                SceneManager.LoadScene("SampleScene");
        }
    }
    private void Move()
    {
        Vector3 moveVelocity = Vector3.zero;   // x, y, z를 나타낼 수 있는 자료형. Vector3.zero로 (0, 0, 0)으로 초기화 가능
        if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))    // Input.GetKey는 해당 키 입력을 받는 함수. KeyCode.~~을 하면 해당 키 입력을 받는다는 의미
        {
            pos.x = Input.GetAxisRaw("Horizontal");    // -1~1 사이의 값을 오른쪽, 왼쪽 키를 누름에 따라 반환. 왼쪽 = -1, 오른쪽 = 1
            // 애니메이션 방향에서 x가 있었던 것 기억하시나요? 그 x를 설정하기 위함 + 방향에 따른 이동을 구현하기 위함
            if(pos.x < 0)   // 왼쪽키라면?
            {
                moveVelocity = Vector3.left;  // 왼쪽 할당
            }
            else if (pos.x > 0)   // 오른쪽 키
            {
                moveVelocity = Vector3.right;
            }
            pos.y = 0;    // 여기서는 y축(위 아래)로 이동이 없으니 0으로 고정!

            animator.SetBool("idle", false);     // 애니메이터에서 있던 idle 변수 기억하시나요? 그걸 false로 바꿔준다는 의미
            // 그렇게 되면 walk의 애니메이션이 재생될 것이다.
            transform.position += moveVelocity * movePower * Time.deltaTime;
            // Time.deltaTime이란? : 컴퓨터 성능에 따라 60프레임, 30프레임 등 천차만별. 이게 없을 시 프레임이 높은 화면은 캐릭터를 더 많이 이동시켜버림.
            // 따라서 60프레임, 30프레임 화면 둘 다에서 이동한 거리가 같을 수 있도록 보정해주는 것
        }
        else
        {
            animator.SetBool("idle", true); // 움직이지 않는다면 가만히 있는 애니메이션 재생.
        }
        animator.SetFloat("x", pos.x); // 마지막으로 애니메이션의 방향 설정
    }
    public void Jump()
    {
        if(Input.GetButtonDown("Jump") && !isJumping)   // 버튼입력을 한 번 받은걸로 인식, 눌렀을 때 점프중이라면 실행이 되지 않음! 중복입력 방지 위함.
        {
            isJumping = true;  // 점프하고 있을테니까 true로 변경
            animator.SetBool("jump", true);
            rigid.velocity = Vector2.zero;
            Vector2 jumpVelocity = new Vector2(0, jumpPower);
            rigid.AddForce(jumpVelocity, ForceMode2D.Impulse);   // ForceMode2D는 힘이 가해지는 방법. impluse는 한번에 충격을 주는 것이고, Force는 자동차 엑셀을 밟는 것처럼 서서히!

        }
    }
    // 점프 후 땅에 착지했는지 확인하기 위함!
    private void OnCollisionEnter2D(Collision2D collision)    //collider 컴포넌트가 붙은 것끼리 충돌 시 실행되는 함수.
    {
        isJumping = false;
        animator.SetBool("jump", false);
        if (collision.gameObject.name == "C_Monster")
        {
            Debug.Log("GameOver");
            SceneManager.LoadScene("SampleScene");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)  // OnTriggerEnter2D는 Collider2D에서 isTrigger가 체크된 물체와 부딪힐 시 발생하는 함수
    {
        if(collision.gameObject.name == "Coin" && collision.isActiveAndEnabled)
        {
            GameObject.FindWithTag("Coin").SetActive(false);
            Debug.Log("Coin");
            coin++;
            coinText.text = "Coin:" + coin;
        }
        else if(collision.gameObject.name == "Goal")
        {
            Debug.Log("Game Clear!");
            GameObject.Find("Canvas").transform.GetChild(1).gameObject.SetActive(true);
            isClear = true;
            movePower = 0;
            jumpPower = 0;
        }
        else // 추가 위의 두 경우가 아닌 trigger에 enter 했을 경우
        {
            return;
        }
    }
}
