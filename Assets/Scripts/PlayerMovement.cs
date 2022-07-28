using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Transform body;        // 몸체 위치 정보.
    [SerializeField] Transform eye;         // 시야 위치 정보.

    [SerializeField] float sencitivityX;    // x축 시야 민감도.
    [SerializeField] float sencitivityY;    // y축 시야 민감도.

    [SerializeField] float moveSpeed;       // 이동 속도.
    [SerializeField] float rotateSpeed;     // 회전 속도.

    Rigidbody rigid;
    new Transform transform;
    Quaternion lookAt;                      // 바라볼 방향.

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        transform = base.transform;

        lookAt = transform.rotation;        // 바라볼 방향의 초기 값은 원래 방향이다.
    }

    private void Update()
    {
        Debug.Log(eye.eulerAngles);

        Rotate();
        RotateCam();
    }
    private void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        // 입력 값에 따른 방향 벡터를 구한 뒤, Rigidbody의 MovePositoin을 이용한다.
        // 정면은 카메라의 로컬 좌표계 기준 정면이 되야하기 때문이다.
        Vector3 direction = (eye.forward * z + eye.right * x).normalized;
        if (direction != Vector3.zero)
        {
            // 이동량 만큼 물리적 이동.
            rigid.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);

            // 내가 나아갈 방향과 바라볼 방향은 동일하기 때문이다.
            // 방향 벡터 -> 회전 값.
            lookAt = Quaternion.LookRotation(direction);
        }
    }
    void Rotate()
    {
        // direction방향으로 향하는 회전 값.
        // 기준 오브젝트가 아니라 그 아래에 있는 몸체 오브젝트를 돌린다.
        body.rotation = Quaternion.Lerp(body.rotation, lookAt, rotateSpeed * Time.deltaTime);
    }

    void RotateCam()
    {
        float x = Input.GetAxis("Mouse X") * sencitivityX;     // 마우스의 x축 이동량.
        float y = Input.GetAxis("Mouse Y") * sencitivityY;     // 마우스의 y축 이동량.

        eye.Rotate(Vector3.up * x * rotateSpeed * Time.deltaTime);
    }

}
