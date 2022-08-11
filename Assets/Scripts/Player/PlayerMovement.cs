using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour, IForce, IContinuousForce
{
    [Header("Jump")]
    [SerializeField] float jumpPower;
    [SerializeField] float groundCheckRadius;
    [SerializeField] LayerMask groundMask;

    [Header("Movemenet")]
    [SerializeField] Transform body;        // 몸체 위치 정보.
    [SerializeField] Transform camPivot;    // 카메라 중심.
    [SerializeField] Animator anim;         // 애니메이터.
    [SerializeField] float moveSpeed;       // 이동 속도.
    [SerializeField] float runSpeed;        // 달리기 속도.
    [SerializeField] float rotateSpeed;     // 회전 속도.

    [Range(0.0f, 1.0f)]
    [SerializeField] float runRatio;        // 달리기 비율 (조이스틱을 얼만큼 기울였을 때)

    new Transform transform;                // 트랜스폼을 캐싱.
    Rigidbody rigid;                        // 리지드 바디.
    Quaternion lookAt;                      // 바라볼 방향.    
    
    bool isLockControl;                     // 키 입력 제한. (true면 플레이어의 움직임을 제어할 수 없다.)
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

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        transform = base.transform;
        lookAt = transform.rotation;                // 바라볼 방향의 초기 값은 원래 방향이다.
    }

    private void Update()
    {
        RotateBody();
        anim.SetFloat("velocityY", rigid.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        if (Input.GetKeyDown(KeyCode.P))
            UnityEngine.SceneManagement.SceneManager.LoadScene("SelectStage");
    }

    private void FixedUpdate()
    {
        // 지면 체크.
        isGrounded = Physics.CheckSphere(transform.position, groundCheckRadius, groundMask);

        // 내 속도는 입력 속도 + 지속 속도.
        Vector3 velocity = inputForce + continuousForce;
        if (velocity != Vector3.zero)
        {
            velocity.y = rigid.velocity.y;
            rigid.velocity = velocity;
        }

        // 지속 적인 힘은 지속적으로 감소한다.
        continuousForce = Vector3.MoveTowards(continuousForce, Vector3.zero, Time.fixedDeltaTime * 3f);
    }

    Vector3 inputForce;         // 입력에 의한 힘.
    Vector3 continuousForce;    // 어떠한 물체가 나에게 가한 지속적인 힘.

    // 누군가가 나에게 주는 지속적인 힘이다.
    public void Movement(float x, float z)
    {
        // 키 입력이 잠기면 입력 힘은 zero가 된다.
        if (isLockControl)
        {
            inputForce = Vector3.zero;
            anim.SetBool("isRun", false);
            anim.SetBool("isMovement", false);
            return;
        }
        else
        {
            Vector2 input = new Vector2(x, z);  
            anim.SetBool("isRun", input.magnitude >= 0.7f);     // 입력 값의 거리가 0.7이상일 경우는 달린다고 판단.
            anim.SetBool("isMovement", input != Vector2.zero);  // 입력 값이 zero가 아닐 경우는 이동한다고 판단.
        }

        // 입력 값에 따른 방향 벡터를 구한다. 정면은 카메라의 로컬 좌표계 기준 정면이 되어야 한다.
        // 축의 회전에 따라 정면 벡터에 y값이 들어있을 수 있기 때문에 제거한다.
        Vector3 forward = camPivot.forward;
        forward.y = 0f;

        Vector3 direction = (forward * z + camPivot.right * x);

        // 키 입력이 생기면 해당 방향으로 바라본다, 이동한다.
        if (direction.normalized != Vector3.zero)

            lookAt = Quaternion.LookRotation(direction.normalized);
        
        // 입력에 따른 힘.
        inputForce = direction * moveSpeed;
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

    bool isLockArea;
    IEnumerator OnLockArea()
    {
        float beforeMoveSpeed = moveSpeed;
        moveSpeed = 0;

        while (isLockArea)
            yield return null;

        moveSpeed = beforeMoveSpeed;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag.Equals("LockControlArea"))
        {
            isLockArea = true;
            StartCoroutine(OnLockArea());
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag.Equals("LockControlArea"))
        {
            isLockArea = false;
        }
    }

    #region private

    // 플레이어 회전 제어.
    void RotateBody()
    {
        // direction방향으로 향하는 회전 값.
        // 기준 오브젝트가 아니라 그 아래에 있는 몸체 오브젝트를 돌린다.
        body.rotation = Quaternion.Lerp(body.rotation, lookAt, rotateSpeed * Time.deltaTime);
    }
    
    // 플레이어 움직임 제한.
    void SwitchControl(int value)
    {
        isLockControl = (value == 1);
    }

    #endregion

    #region 인터페이스 함수.

    void IForce.OnContactForce(ForceData data)
    {
        StartCoroutine(Stun(data.stunTime));
        Vector3 dir = data.direction;
        rigid.AddForce(dir * data.force, ForceMode.Impulse);
    }
    void IContinuousForce.AddContinuousForce(Vector3 direction, float power)
    {
        continuousForce = direction * power;
    }

    #endregion

    IEnumerator Stun(float stunTime)
    {
        isLockControl = true;
        yield return new WaitForSeconds(stunTime);
        isLockControl = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(base.transform.position, groundCheckRadius);
    }
}
