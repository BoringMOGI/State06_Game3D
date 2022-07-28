using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement3D : MonoBehaviour
{
    private const float GRAVITY = -9.81f;

    [SerializeField] private float moveSpeed;   // �̵� �ӵ�.
    [SerializeField] private float jumpHeight;  // ���� ����.

    [Header("Ground")]
    [SerializeField] Transform groundPivot;     // ���� üũ �߽���.
    [SerializeField] float groundRadius;        // ���� ������.
    [SerializeField] LayerMask groundMask;      // ������ ����ũ.

    [Header("Value")]
    [Range(0f, 5f)]
    [SerializeField] float gravityScale;        // �߷� ���.

    private new Transform transform;
    private CharacterController controller;

    private Vector3 velocity;       // ���� �ϰ� �ӵ�.
    private bool isGrounded;        // ���� �� �ִ���?

    private float gravity => GRAVITY * gravityScale;        // ���� �߷� : �߷� ��� * �߷� ���.

    void Start()
    {
        transform = base.transform;
        controller = GetComponent<CharacterController>();   // ���� ĳ���� ��Ʈ�ѷ� �˻�.
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundPivot.position, groundRadius, groundMask);

        Movement();
        Jump();
        Gravity();
    }

    void Movement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        // direction : ����. (�߿� : 0.0 ~ 1.0 ���̰�)
        // ����ȭ : ������ ���� 0.0~1.0 ���� ������ ���� ����.
        // direction.Normalize();
        Vector3 direction = (transform.forward * z) + (transform.right * x);    // ����.
        Vector3 movement = direction.normalized * moveSpeed;                    // �̵���.

        // �̵�.
        controller.Move(movement * Time.deltaTime);
    }
    void Jump()
    {
        if(isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            // Mathf.Sqrt : ������.
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
    void Gravity()
    {
        // �߷�.
        // ���߿��� �����ö� ���鿡 ������ ���� �ӵ��� �� �ٱ� ���ؼ�.
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;     // �߷� ���ӵ�.
        controller.Move(velocity * Time.deltaTime); // �Ʒ������� �̵�.
    }

    private void OnDrawGizmos()
    {
        // ����� �� ���� ����� �׸��ڴ�.
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(groundPivot.position, groundRadius);
    }
}
