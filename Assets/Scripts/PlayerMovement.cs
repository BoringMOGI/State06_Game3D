using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
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
    [SerializeField] float rotateSpeed;     // 회전 속도.

    new Transform transform;
    Rigidbody rigid;
    Quaternion lookAt;                      // 바라볼 방향.    

    
    float rotateX;                          // x축 회전 값.
    bool isLockControl;                     // 키 입력 제한.

    float prevVelocityY
    {
        get
        {
            return anim.GetFloat("prevVelocityY");
        }
        set
        {
            anim.SetFloat("prevVelocityY", value);
        }
    }
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
    }
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
    }
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
    }

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        transform = base.transform;

        lookAt = transform.rotation;                // 바라볼 방향의 초기 값은 원래 방향이다.
        rotateX = verticalEye.eulerAngles.x;        // 최초 x축 회전 값은 기본 값이다.
    }

    private void FixedUpdate()
    {
        prevVelocityY = rigid.velocity.y;       // 이전 y축 속도.
    }
    private void Update()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundCheckRadius, groundMask);

        RotateBody();
        RotateCam();

        // 키 입력 제한이 걸리지 않앗을 경우.
        if (!isLockControl)
        {
            Movement();
            Jump();
        }
        else
        {
            // 애니메이션 파라이터.
            isMovement = false;
            isRun = false;

            rigid.velocity = new Vector3(0f, rigid.velocity.y, 0f); // 캐릭터 정지.
        }

        anim.SetFloat("velocityY", rigid.velocity.y);
    }

    void Movement()
    {
        float x = Input.GetAxisRaw("Horizontal");           // 좌,우
        float z = Input.GetAxisRaw("Vertical");             // 상,하

        // 입력 값에 따른 방향 벡터를 구한 뒤, Rigidbody의 MovePositoin을 이용한다.
        // 정면은 카메라의 로컬 좌표계 기준 정면이 되어야 한다.
        Vector3 direction = (horizontalEye.forward * z + horizontalEye.right * x).normalized;
        isMovement = (direction != Vector3.zero);
        isRun = Input.GetKey(KeyCode.LeftShift);

        // 키 입력이 생기면 해당 방향으로 바라본다.
        if(direction != Vector3.zero)
            lookAt = Quaternion.LookRotation(direction);

        Vector3 movement = direction * moveSpeed * (isRun ? 1.5f : 1.0f);       // 이동량.
        movement.y = rigid.velocity.y;                                          // 수직 속도량.

        rigid.velocity = movement;                                              // 속도 대입.
    }
    void Jump()
    {
        // 점프 키를 누르고, 땅에 서 있는 경우.
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            anim.SetTrigger("onJump");
        }
    }
    void RotateBody()
    {
        // direction방향으로 향하는 회전 값.
        // 기준 오브젝트가 아니라 그 아래에 있는 몸체 오브젝트를 돌린다.
        body.rotation = Quaternion.Lerp(body.rotation, lookAt, rotateSpeed * Time.deltaTime);
    }
    
    // 유저의 입력을 제한한다.
    void SwitchControl(int value)
    {
        isLockControl = (value == 1);
        Debug.Log($"Switch Control : {isLockControl}");
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


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(base.transform.position, groundCheckRadius);
    }
}
