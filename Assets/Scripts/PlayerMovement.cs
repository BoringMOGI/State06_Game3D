using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour, IForce
{
    [Header("Rotate")]
    [SerializeField] Transform horizontalEye;   // ���� ȸ�� �þ�.
    [SerializeField] Transform verticalEye;     // ���� ȸ�� �þ�.
    [SerializeField] float sencitivityX;        // x�� �þ� �ΰ���.
    [SerializeField] float sencitivityY;        // y�� �þ� �ΰ���.
    [SerializeField] float minRoateX;
    [SerializeField] float maxRotateX;

    [Header("Jump")]
    [SerializeField] float jumpPower;
    [SerializeField] float groundCheckRadius;
    [SerializeField] LayerMask groundMask;

    [Header("Movemenet")]
    [SerializeField] Transform body;        // ��ü ��ġ ����.
    [SerializeField] Animator anim;         // �ִϸ�����.
    [SerializeField] float moveSpeed;       // �̵� �ӵ�.
    [SerializeField] float runSpeed;        // �޸��� �ӵ�.
    [SerializeField] float rotateSpeed;     // ȸ�� �ӵ�.

    [Range(0.0f, 1.0f)]
    [SerializeField] float runRatio;        // �޸��� ���� (���̽�ƽ�� ��ŭ ��￴�� ��)

    new Transform transform;                // Ʈ�������� ĳ��.
    Rigidbody rigid;                        // ������ �ٵ�.
    Quaternion lookAt;                      // �ٶ� ����.    
    
    float rotateX;                          // x�� ȸ�� ��.
    bool isLockControl;                     // Ű �Է� ����. (true�� �÷��̾��� �������� ������ �� ����.)
    bool isLockVelocity;                    // �ӵ� ���� ����. (true�� ���� �ӵ� ���� ������ �� ����.)

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
    }                      // �����̰� �ִ°�?
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
    }                           // �޸��� �ִ°�?

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        transform = base.transform;

        lookAt = transform.rotation;                // �ٶ� ������ �ʱ� ���� ���� �����̴�.
        rotateX = verticalEye.eulerAngles.x;        // ���� x�� ȸ�� ���� �⺻ ���̴�.
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundCheckRadius, groundMask) && rigid.velocity.y <= 0.01f;

        RotateBody();
        // RotateCam();

        // Ű �Է� ������ �ɸ��� �ʾ��� ���.
        if (!isLockControl)
        {
            // Movement();
            // Jump();
        }
        else
        {
            // �ִϸ��̼� �Ķ�����.
            isMovement = false;
            isRun = false;

            // ĳ���� ����.
            if (!isLockVelocity)
                rigid.velocity = new Vector3(0f, rigid.velocity.y, 0f); 
        }

        anim.SetFloat("velocityY", rigid.velocity.y);
    }

    // �÷��̾� ������ ����.
    public void Movement(float x, float z)
    {
        if (isLockControl)
            return;

        // �Է� ���� ���� ���� ���͸� ���� ��, Rigidbody�� MovePositoin�� �̿��Ѵ�.
        // ������ ī�޶��� ���� ��ǥ�� ���� ������ �Ǿ�� �Ѵ�.
        Vector3 direction = (horizontalEye.forward * z + horizontalEye.right * x);
        Vector3 directionNor = direction.normalized;

        // Mathf.abs : T
        // = Ư�� ���� ���� ���� �����Ѵ�.
        isRun = (Mathf.Abs(x) >= runRatio) || (Mathf.Abs(z) >= runRatio);
        isMovement = (directionNor != Vector3.zero);

        // Ű �Է��� ����� �ش� �������� �ٶ󺻴�.
        if (directionNor != Vector3.zero)
        {
            lookAt = Quaternion.LookRotation(directionNor);
        }

        // �̵����� ���Ҷ� ����ȭ���� ���� ���͸� ����ϱ� ������
        // ������ ���� �ٸ� �ӵ��� ������.
        Vector3 movement = direction * (isRun ? runSpeed : moveSpeed);          // �̵���.
        movement.y = rigid.velocity.y;                                          // ���� �ӵ���.

        if(!isLockVelocity)
            rigid.velocity = movement;                                          // �ӵ� ����.
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

    // �÷��̾� ȸ�� ����.
    void RotateBody()
    {
        // direction�������� ���ϴ� ȸ�� ��.
        // ���� ������Ʈ�� �ƴ϶� �� �Ʒ��� �ִ� ��ü ������Ʈ�� ������.
        body.rotation = Quaternion.Lerp(body.rotation, lookAt, rotateSpeed * Time.deltaTime);
    }
    void RotateCam()
    {
        float x = Input.GetAxis("Mouse X") * sencitivityX;     // ���콺�� x�� �̵���.
        float y = Input.GetAxis("Mouse Y") * sencitivityY;     // ���콺�� y�� �̵���.

        // x���� �������� ���� ȸ�� �.
        rotateX = Mathf.Clamp(rotateX - y, minRoateX, maxRotateX);      // ���� ȸ������ �ּ�, �ִ� ������ �ǵ��� ����.
        verticalEye.localRotation = Quaternion.Euler(rotateX, 0, 0);

        // y���� �������� ���� ȸ�� �.
        horizontalEye.Rotate(Vector3.up * x);
    }
    
    // �÷��̾� ������ ����.
    void SwitchControl(int value)
    {
        isLockControl = (value == 1);
    }


    // �������̽� �Լ�.
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
