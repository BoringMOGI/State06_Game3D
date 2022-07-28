using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement3D : MonoBehaviour
{
    private const float GRAVITY = -9.81f;

    [SerializeField] private float moveSpeed;   // 이동 속도.
    [SerializeField] private float jumpHeight;  // 점프 높이.

    [Header("Ground")]
    [SerializeField] Transform groundPivot;     // 지면 체크 중심점.
    [SerializeField] float groundRadius;        // 구의 반지름.
    [SerializeField] LayerMask groundMask;      // 레이의 마스크.

    [Header("Value")]
    [Range(0f, 5f)]
    [SerializeField] float gravityScale;        // 중력 배수.

    private new Transform transform;
    private CharacterController controller;

    private Vector3 velocity;       // 나의 하강 속도.
    private bool isGrounded;        // 땅에 서 있는지?

    private float gravity => GRAVITY * gravityScale;        // 실제 중력 : 중력 상수 * 중력 배수.

    void Start()
    {
        transform = base.transform;
        controller = GetComponent<CharacterController>();   // 나의 캐릭터 컨트롤러 검색.
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

        // direction : 방향. (중요 : 0.0 ~ 1.0 사이값)
        // 정규화 : 벡터의 값을 0.0~1.0 사이 값으로 비율 조정.
        // direction.Normalize();
        Vector3 direction = (transform.forward * z) + (transform.right * x);    // 방향.
        Vector3 movement = direction.normalized * moveSpeed;                    // 이동량.

        // 이동.
        controller.Move(movement * Time.deltaTime);
    }
    void Jump()
    {
        if(isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            // Mathf.Sqrt : 제곱근.
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
    void Gravity()
    {
        // 중력.
        // 공중에서 내려올때 지면에 닿으면 작은 속도로 딱 붙기 위해서.
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;     // 중력 가속도.
        controller.Move(velocity * Time.deltaTime); // 아래쪽으로 이동.
    }

    private void OnDrawGizmos()
    {
        // 녹색의 구 형태 기즈모를 그리겠다.
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(groundPivot.position, groundRadius);
    }
}
