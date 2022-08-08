using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour, IForce
{
    [Header("Rotate")]
    [SerializeField] Transform horizontalEye;   // 수평 회전 시야.
    [SerializeField] Transform verticalEye;     // 수직 회전 시야.
    [SerializeField] float sencitivityX;        // x축 시야 민감도.
    [SerializeField] float sencitivityY;        // y축 시야 민감도.
    [SerializeField] float minRoateX;
    [SerializeField] float maxRotateX;

    [Header("Jump")]
    [SerializeField] float jumpPower;
    [SerializeField] float groundCheckRadius;
    [SerializeField] LayerMask groundMask;

    [Header("Movemenet")]
    [SerializeField] Transform body;        // 몸체 위치 정보.
    [SerializeField] Animator anim;         // 애니메이터.
    [SerializeField] float moveSpeed;       // 이동 속도.
    [SerializeField] float runSpeed;        // 달리기 속도.
    [SerializeField] float rotateSpeed;     // 회전 속도.

    [Range(0.0f, 1.0f)]
    [SerializeField] float runRatio;        // 달리기 비율 (조이스틱을 얼만큼 기울였을 때)

    new Transform transform;                // 트랜스폼을 캐싱.
    Rigidbody rigid;                        // 리지드 바디.
    Quaternion lookAt;                      // 바라볼 방향.    
    
    float rotateX;                          // x축 회전 값.
    bool isLockControl;                     // 키 입력 제한. (true면 플레이어의 움직임을 제어할 수 없다.)
    bool isLockVelocity;                    // 속도 제어 제한. (true면 직접 속도 값을 제어할 수 없다.)

    bool isGrounded
    {
        get
        {
            return anim.GetBool("isGrounded");
        }
        set
        {
            anim.SetBool("isGrounded", value);
        }
    }                      // 땅 위에 서 있는가?
    bool isMovement
    {
        get
        {
            return anim.GetBool("isMovement");
        }
        set
        {
            anim.SetBool("isMovement", value);
        }
    }                      // 움직이고 있는가?
    bool isRun
    {
        get
        {
            return anim.GetBool("isRun");
        }
        set
        {
            anim.SetBool("isRun", value);
        }
    }                           // 달리고 있는가?

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        transform = base.transform;

        lookAt = transform.rotation;                // 바라볼 방향의 초기 값은 원래 방향이다.
        rotateX = verticalEye.eulerAngles.x;        // 최초 x축 회전 값은 기본 값이다.
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundCheckRadius, groundMask) && rigid.velocity.y <= 0.01f;

        RotateBody();
        // RotateCam();

        // 키 입력 제한이 걸리지 않앗을 경우.
        if (!isLockControl)
        {
            // Movement();
            // Jump();
        }
        else
        {
            // 애니메이션 파라이터.
            isMovement = false;
            isRun = false;

            // 캐릭터 정지.
            if (!isLockVelocity)
                rigid.velocity = new Vector3(0f, rigid.velocity.y, 0f); 
        }

        anim.SetFloat("velocityY", rigid.velocity.y);
    }

    // 플레이어 움직임 제어.
    public void Movement(float x, float z)
    {
        if (isLockControl)
            return;

        // 입력 값에 따른 방향 벡터를 구한 뒤, Rigidbody의 MovePositoin을 이용한다.
        // 정면은 카메라의 로컬 좌표계 기준 정면이 되어야 한다.
        Vector3 direction = (horizontalEye.forward * z + horizontalEye.right * x);
        Vector3 directionNor = direction.normalized;

        // Mathf.abs : T
        // = 특정 수의 절대 값을 리턴한다.
        isRun = (Mathf.Abs(x) >= runRatio) || (Mathf.Abs(z) >= runRatio);
        isMovement = (directionNor != Vector3.zero);

        // 키 입력이 생기면 해당 방향으로 바라본다.
        if (directionNor != Vector3.zero)
        {
            lookAt = Quaternion.LookRotation(directionNor);
        }

        // 이동량을 구할때 정규화되지 않은 벡터를 사용하기 때문에
        // 정도에 따라서 다른 속도를 가진다.
        Vector3 movement = direction * (isRun ? runSpeed : moveSpeed);          // 이동량.
        movement.y = rigid.velocity.y;                                          // 수직 속도량.

        if(!isLockVelocity)
            rigid.velocity = movement;                                          // 속도 대입.
    }
    public void Jump()
    {
        if (isLockControl)
            return;

        // 점프 키를 누르고, 땅에 서 있는 경우.
        if (isGrounded)
        {
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            anim.SetTrigger("onJump");
        }
    }

    // 플레이어 회전 제어.
    void RotateBody()
    {
        // direction방향으로 향하는 회전 값.
        // 기준 오브젝트가 아니라 그 아래에 있는 몸체 오브젝트를 돌린다.
        body.rotation = Quaternion.Lerp(body.rotation, lookAt, rotateSpeed * Time.deltaTime);
    }
    void RotateCam()
    {
        float x = Input.GetAxis("Mouse X") * sencitivityX;     // 마우스의 x축 이동량.
        float y = Input.GetAxis("Mouse Y") * sencitivityY;     // 마우스의 y축 이동량.

        // x축을 기준으로 수직 회전 운동.
        rotateX = Mathf.Clamp(rotateX - y, minRoateX, maxRotateX);      // 수직 회전량이 최소, 최대 범위가 되도록 조정.
        verticalEye.localRotation = Quaternion.Euler(rotateX, 0, 0);

        // y축을 기준으로 수평 회전 운동.
        horizontalEye.Rotate(Vector3.up * x);
    }
    
    // 플레이어 움직임 제한.
    void SwitchControl(int value)
    {
        isLockControl = (value == 1);
    }


    // 인터페이스 함수.
    void IForce.OnContactForce(ForceData data)
    {
        StartCoroutine(Stun(data.stunTime));
        Vector3 dir = data.direction;
        rigid.AddForce(dir * data.force, ForceMode.Impulse);
    }

    IEnumerator Stun(float stunTime)
    {
        isLockControl = true;
        isLockVelocity = true;

        yield return new WaitForSeconds(stunTime);

        isLockControl = false;
        isLockVelocity = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(base.transform.position, groundCheckRadius);
    }
}
