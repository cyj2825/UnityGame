using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float movePower = 20f;
    Vector3 movement;
    int movementFlag = 0;    // 0: 가만히 1: 왼쪽으로 움직이기 2: 오른쪽으로 움직이기
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("ChangeMovement");
    }

    // Update is called once per frame
    void FixedUpdate()    // 물리엔진용 업데이트 함수
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
    // 시간차를 두고 단락별로 실행할 때 필요한 기능
    IEnumerator ChangeMovement()
    {
        // Random.Range(0, 3)은 0, 1, 2 중에 한 숫자가 랜덤하게 지정되는 메소드
        movementFlag = Random.Range(0, 3);
        yield return new WaitForSeconds(3f);

        // 무한 반복을 위해 함수 안에 함수가 끝날 때 자기 자신을 부르는 것이라고 생각하면 된다.
        StartCoroutine("ChangeMovement");
    }
    private void OnTriggerEnter2D(Collider2D collision) // 2D콜라이더에 들어가는 순간 딱 한번! 실행
    {
        // 땅에서 떨어지지 않도록 만들기 위해서 절벽에 가까워지면 다시 방향을 바꾸도록 처리한 것 
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
