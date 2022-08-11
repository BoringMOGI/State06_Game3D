using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookRotate : MonoBehaviour
{
    [SerializeField] private Transform playerBody;   // ��.
    [SerializeField] private Transform playerHead;   // �Ӹ�.
    [SerializeField] private float limitUpAngle;     // ���� ���� ���� ��.
    [SerializeField] private float limitDownAngle;   // �Ʒ��� ���� ���� ��.

    [Header("Sensitivity")]
    [Range(1f, 10f)]
    [SerializeField] private float sensitivityX;     // ���� �ΰ���.
    [Range(1f, 10f)]
    [SerializeField] private float sensitivityY;     // ���� �ΰ���.

    float xRotate;      // x�� ȸ�� ��.

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;    // ���콺 ���.
        xRotate = playerHead.localRotation.x;        // ���� ���� ȸ�� �� ����.
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivityX;     // ���콺 ���� ȸ�� (+�ΰ���)
        float mouseY = Input.GetAxis("Mouse Y") * sensitivityY;     // ���콺 ���� ȸ�� (+�ΰ���)

        Vector2 axis = new Vector2(mouseX, mouseY);     // ���콺 ȸ�� ��.
        playerBody.Rotate(Vector2.up * axis.x);         // y���� �������� ���� ȸ��.

        // x�� �������� ���� ȸ��.
        xRotate = Mathf.Clamp(xRotate - axis.y, limitUpAngle, limitDownAngle);      // �ּ�, �ִ� ���� ����.
        playerHead.localRotation = Quaternion.Euler(xRotate, 0f, 0f);               // ���� ȸ��.
    }
}
