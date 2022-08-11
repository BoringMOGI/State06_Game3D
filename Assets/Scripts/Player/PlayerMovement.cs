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
    [SerializeField] Transform body;        // ��ü ��ġ ����.
    [SerializeField] Transform camPivot;    // ī�޶� �߽�.
    [SerializeField] Animator anim;         // �ִϸ�����.
    [SerializeField] float moveSpeed;       // �̵� �ӵ�.
    [SerializeField] float runSpeed;        // �޸��� �ӵ�.
    [SerializeField] float rotateSpeed;     // ȸ�� �ӵ�.

    [Range(0.0f, 1.0f)]
    [SerializeField] float runRatio;        // �޸��� ���� (���̽�ƽ�� ��ŭ ��￴�� ��)

    new Transform transform;                // Ʈ�������� ĳ��.
    Rigidbody rigid;                        // ������ �ٵ�.
    Quaternion lookAt;                      // �ٶ� ����.    
    
    bool isLockControl;                     // Ű �Է� ����. (true�� �÷��̾��� �������� ������ �� ����.)
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
    }                      // �� ���� �� �ִ°�?

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        transform = base.transform;
        lookAt = transform.rotation;                // �ٶ� ������ �ʱ� ���� ���� �����̴�.
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
        // ���� üũ.
        isGrounded = Physics.CheckSphere(transform.position, groundCheckRadius, groundMask);

        // �� �ӵ��� �Է� �ӵ� + ���� �ӵ�.
        Vector3 velocity = inputForce + continuousForce;
        if (velocity != Vector3.zero)
        {
            velocity.y = rigid.velocity.y;
            rigid.velocity = velocity;
        }

        // ���� ���� ���� ���������� �����Ѵ�.
        continuousForce = Vector3.MoveTowards(continuousForce, Vector3.zero, Time.fixedDeltaTime * 3f);
    }

    Vector3 inputForce;         // �Է¿� ���� ��.
    Vector3 continuousForce;    // ��� ��ü�� ������ ���� �������� ��.

    // �������� ������ �ִ� �������� ���̴�.
    public void Movement(float x, float z)
    {
        // Ű �Է��� ���� �Է� ���� zero�� �ȴ�.
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
            anim.SetBool("isRun", input.magnitude >= 0.7f);     // �Է� ���� �Ÿ��� 0.7�̻��� ���� �޸��ٰ� �Ǵ�.
            anim.SetBool("isMovement", input != Vector2.zero);  // �Է� ���� zero�� �ƴ� ���� �̵��Ѵٰ� �Ǵ�.
        }

        // �Է� ���� ���� ���� ���͸� ���Ѵ�. ������ ī�޶��� ���� ��ǥ�� ���� ������ �Ǿ�� �Ѵ�.
        // ���� ȸ���� ���� ���� ���Ϳ� y���� ������� �� �ֱ� ������ �����Ѵ�.
        Vector3 forward = camPivot.forward;
        forward.y = 0f;

        Vector3 direction = (forward * z + camPivot.right * x);

        // Ű �Է��� ����� �ش� �������� �ٶ󺻴�, �̵��Ѵ�.
        if (direction.normalized != Vector3.zero)

            lookAt = Quaternion.LookRotation(direction.normalized);
        
        // �Է¿� ���� ��.
        inputForce = direction * moveSpeed;
    }
    public void Jump()
    {
        if (isLockControl)
            return;

        // ���� Ű�� ������, ���� �� �ִ� ���.
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

    // �÷��̾� ȸ�� ����.
    void RotateBody()
    {
        // direction�������� ���ϴ� ȸ�� ��.
        // ���� ������Ʈ�� �ƴ϶� �� �Ʒ��� �ִ� ��ü ������Ʈ�� ������.
        body.rotation = Quaternion.Lerp(body.rotation, lookAt, rotateSpeed * Time.deltaTime);
    }
    
    // �÷��̾� ������ ����.
    void SwitchControl(int value)
    {
        isLockControl = (value == 1);
    }

    #endregion

    #region �������̽� �Լ�.

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
