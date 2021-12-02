using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float movePower = 20f;
    Vector3 movement;
    int movementFlag = 0;    // 0: ������ 1: �������� �����̱� 2: ���������� �����̱�
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("ChangeMovement");
    }

    // Update is called once per frame
    void FixedUpdate()    // ���������� ������Ʈ �Լ�
    {
        Move();
    }
    public void Move()
    {
        Vector3 moveVelocity = Vector3.zero;

        if(movementFlag == 1)
        {
            moveVelocity = Vector3.left;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if(movementFlag == 2)
        {
            moveVelocity = Vector3.right;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        transform.position += moveVelocity * movePower * Time.deltaTime;
    }
    // �ð����� �ΰ� �ܶ����� ������ �� �ʿ��� ���
    IEnumerator ChangeMovement()
    {
        // Random.Range(0, 3)�� 0, 1, 2 �߿� �� ���ڰ� �����ϰ� �����Ǵ� �޼ҵ�
        movementFlag = Random.Range(0, 3);
        yield return new WaitForSeconds(3f);

        // ���� �ݺ��� ���� �Լ� �ȿ� �Լ��� ���� �� �ڱ� �ڽ��� �θ��� ���̶�� �����ϸ� �ȴ�.
        StartCoroutine("ChangeMovement");
    }
    private void OnTriggerEnter2D(Collider2D collision) // 2D�ݶ��̴��� ���� ���� �� �ѹ�! ����
    {
        // ������ �������� �ʵ��� ����� ���ؼ� ������ ��������� �ٽ� ������ �ٲٵ��� ó���� �� 
        if(movementFlag == 1)
        {
            movementFlag = 2;
        }
        else if (movementFlag == 2)
        {
            movementFlag = 1;
        }
    }
}
