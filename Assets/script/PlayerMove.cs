using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Scene�� �̵��̳� �����, �ҷ����⸦ �Ϸ��� UnityEngine.SceneManagement�� �߰����־�� ��

public class PlayerMove : MonoBehaviour
{
    private int coin;
    private bool isClear = false;
    public Text coinText;
    private Animator animator;    // �ִϸ��̼� ������Ʈ�� �ҷ����� ��
    private Vector2 pos;    // ĳ���� �������� ���� Ű���� �Է��� ���� �� �Էµ� float���� �����ϱ� ���� ����.
    // vector2�� ����Ƽ���� �����ϴ� ������. �� �״�� 2������ ����(x, y)�� �����ϴ� �ڷ����̴�.

    public float movePower = 0.5f;    // ����Ƽ���� float�� ���� �ڿ��� f�� ���δ�.(�� �ٿ��� �Ǳ���)
    public float jumpPower = 80f;

    Rigidbody2D rigid;    // rigidbody�� ���� �߰��ߴ� ������Ʈ�ε�, ���������� ����ϱ� ���� ������Ʈ.
    bool isJumping = false;    // ���������� �Ǵ��� ���� bool�� ����. �������� ������ ���߿����� ��� ���� ��������.
    // Start is called before the first frame update
    void Start()    // ó�� ������ �� 1�� �����
    {
        animator = gameObject.GetComponent<Animator>();   // �� ��ũ��Ʈ�� ������ ���ӿ�����Ʈ�� <> �� ������Ʈ���� �� ��ũ��Ʈ���� �ٷ� �� �ֵ��� ��
        rigid = gameObject.GetComponent<Rigidbody2D>();
        coin = 0;
        coinText.text = "Coin:" + coin;
    }

    // Update is called once per frame
    void Update()   // �� �����Ӹ��� ��� �ݺ������.
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
        Vector3 moveVelocity = Vector3.zero;   // x, y, z�� ��Ÿ�� �� �ִ� �ڷ���. Vector3.zero�� (0, 0, 0)���� �ʱ�ȭ ����
        if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))    // Input.GetKey�� �ش� Ű �Է��� �޴� �Լ�. KeyCode.~~�� �ϸ� �ش� Ű �Է��� �޴´ٴ� �ǹ�
        {
            pos.x = Input.GetAxisRaw("Horizontal");    // -1~1 ������ ���� ������, ���� Ű�� ������ ���� ��ȯ. ���� = -1, ������ = 1
            // �ִϸ��̼� ���⿡�� x�� �־��� �� ����Ͻó���? �� x�� �����ϱ� ���� + ���⿡ ���� �̵��� �����ϱ� ����
            if(pos.x < 0)   // ����Ű���?
            {
                moveVelocity = Vector3.left;  // ���� �Ҵ�
            }
            else if (pos.x > 0)   // ������ Ű
            {
                moveVelocity = Vector3.right;
            }
            pos.y = 0;    // ���⼭�� y��(�� �Ʒ�)�� �̵��� ������ 0���� ����!

            animator.SetBool("idle", false);     // �ִϸ����Ϳ��� �ִ� idle ���� ����Ͻó���? �װ� false�� �ٲ��شٴ� �ǹ�
            // �׷��� �Ǹ� walk�� �ִϸ��̼��� ����� ���̴�.
            transform.position += moveVelocity * movePower * Time.deltaTime;
            // Time.deltaTime�̶�? : ��ǻ�� ���ɿ� ���� 60������, 30������ �� õ������. �̰� ���� �� �������� ���� ȭ���� ĳ���͸� �� ���� �̵����ѹ���.
            // ���� 60������, 30������ ȭ�� �� �ٿ��� �̵��� �Ÿ��� ���� �� �ֵ��� �������ִ� ��
        }
        else
        {
            animator.SetBool("idle", true); // �������� �ʴ´ٸ� ������ �ִ� �ִϸ��̼� ���.
        }
        animator.SetFloat("x", pos.x); // ���������� �ִϸ��̼��� ���� ����
    }
    public void Jump()
    {
        if(Input.GetButtonDown("Jump") && !isJumping)   // ��ư�Է��� �� �� �����ɷ� �ν�, ������ �� �������̶�� ������ ���� ����! �ߺ��Է� ���� ����.
        {
            isJumping = true;  // �����ϰ� �����״ϱ� true�� ����
            animator.SetBool("jump", true);
            rigid.velocity = Vector2.zero;
            Vector2 jumpVelocity = new Vector2(0, jumpPower);
            rigid.AddForce(jumpVelocity, ForceMode2D.Impulse);   // ForceMode2D�� ���� �������� ���. impluse�� �ѹ��� ����� �ִ� ���̰�, Force�� �ڵ��� ������ ��� ��ó�� ������!

        }
    }
    // ���� �� ���� �����ߴ��� Ȯ���ϱ� ����!
    private void OnCollisionEnter2D(Collision2D collision)    //collider ������Ʈ�� ���� �ͳ��� �浹 �� ����Ǵ� �Լ�.
    {
        isJumping = false;
        animator.SetBool("jump", false);
        if (collision.gameObject.name == "C_Monster")
        {
            Debug.Log("GameOver");
            SceneManager.LoadScene("SampleScene");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)  // OnTriggerEnter2D�� Collider2D���� isTrigger�� üũ�� ��ü�� �ε��� �� �߻��ϴ� �Լ�
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
        else // �߰� ���� �� ��찡 �ƴ� trigger�� enter ���� ���
        {
            return;
        }
    }
}
