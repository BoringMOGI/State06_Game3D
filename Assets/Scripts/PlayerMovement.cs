using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Transform body;        // ��ü ��ġ ����.
    [SerializeField] Transform eye;         // �þ� ��ġ ����.

    [SerializeField] float sencitivityX;    // x�� �þ� �ΰ���.
    [SerializeField] float sencitivityY;    // y�� �þ� �ΰ���.

    [SerializeField] float moveSpeed;       // �̵� �ӵ�.
    [SerializeField] float rotateSpeed;     // ȸ�� �ӵ�.

    Rigidbody rigid;
    new Transform transform;
    Quaternion lookAt;                      // �ٶ� ����.

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        transform = base.transform;

        lookAt = transform.rotation;        // �ٶ� ������ �ʱ� ���� ���� �����̴�.
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

        // �Է� ���� ���� ���� ���͸� ���� ��, Rigidbody�� MovePositoin�� �̿��Ѵ�.
        // ������ ī�޶��� ���� ��ǥ�� ���� ������ �Ǿ��ϱ� �����̴�.
        Vector3 direction = (eye.forward * z + eye.right * x).normalized;
        if (direction != Vector3.zero)
        {
            // �̵��� ��ŭ ������ �̵�.
            rigid.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);

            // ���� ���ư� ����� �ٶ� ������ �����ϱ� �����̴�.
            // ���� ���� -> ȸ�� ��.
            lookAt = Quaternion.LookRotation(direction);
        }
    }
    void Rotate()
    {
        // direction�������� ���ϴ� ȸ�� ��.
        // ���� ������Ʈ�� �ƴ϶� �� �Ʒ��� �ִ� ��ü ������Ʈ�� ������.
        body.rotation = Quaternion.Lerp(body.rotation, lookAt, rotateSpeed * Time.deltaTime);
    }

    void RotateCam()
    {
        float x = Input.GetAxis("Mouse X") * sencitivityX;     // ���콺�� x�� �̵���.
        float y = Input.GetAxis("Mouse Y") * sencitivityY;     // ���콺�� y�� �̵���.

        eye.Rotate(Vector3.up * x * rotateSpeed * Time.deltaTime);
    }

}
