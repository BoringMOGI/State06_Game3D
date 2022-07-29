using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
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
    [SerializeField] float rotateSpeed;     // ȸ�� �ӵ�.

    new Transform transform;
    Rigidbody rigid;
    Quaternion lookAt;                      // �ٶ� ����.    

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

        lookAt = transform.rotation;                // �ٶ� ������ �ʱ� ���� ���� �����̴�.
        rotateX = verticalEye.eulerAngles.x;        // ���� x�� ȸ�� ���� �⺻ ���̴�.
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundCheckRadius, groundMask);

        RotateBody();
        RotateCam();
        Jump();
    }
    private void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        float x = Input.GetAxisRaw("Horizontal");           // ��,��
        float z = Input.GetAxisRaw("Vertical");             // ��,��
                                                            
        // �Է� ���� ���� ���� ���͸� ���� ��, Rigidbody�� MovePositoin�� �̿��Ѵ�.
        // ������ ī�޶��� ���� ��ǥ�� ���� ������ �Ǿ�� �Ѵ�.
        Vector3 direction = (horizontalEye.forward * z + horizontalEye.right * x).normalized;

        // �̵� ���ο� �޸��� ����.
        isMovement = direction != Vector3.zero;
        isRun = Input.GetKey(KeyCode.LeftShift);

        if (isMovement)
        {
            // �ӵ� : �⺻ �̵� �ӵ� * �޸��� ����.
            float speed = moveSpeed * (isRun ? 1.5f : 1.0f);
            rigid.MovePosition(transform.position + direction * speed * Time.deltaTime);

            // ���� ���ư� ����� �ٶ� ������ �����ϱ� �����̴�.
            // ���� ���� -> ȸ�� ��.
            lookAt = Quaternion.LookRotation(direction);
        }
    }
    void Jump()
    {
        // ���� Ű�� ������, ���� �� �ִ� ���.
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            anim.SetTrigger("onJump");
        }
    }
    void RotateBody()
    {
        // direction�������� ���ϴ� ȸ�� ��.
        // ���� ������Ʈ�� �ƴ϶� �� �Ʒ��� �ִ� ��ü ������Ʈ�� ������.
        body.rotation = Quaternion.Lerp(body.rotation, lookAt, rotateSpeed * Time.deltaTime);
    }
    

    float rotateX; // x�� ȸ�� ��.

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

}
