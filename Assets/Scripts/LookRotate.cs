using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookRotate : MonoBehaviour
{
    [SerializeField] private Transform playerBody;   // 몸.
    [SerializeField] private Transform playerHead;   // 머리.
    [SerializeField] private float limitUpAngle;     // 위쪽 각도 제한 값.
    [SerializeField] private float limitDownAngle;   // 아래쪽 각도 제한 값.

    [Header("Sensitivity")]
    [Range(1f, 10f)]
    [SerializeField] private float sensitivityX;     // 수평 민감도.
    [Range(1f, 10f)]
    [SerializeField] private float sensitivityY;     // 수직 민감도.

    float xRotate;      // x축 회전 값.

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;    // 마우스 잠금.
        xRotate = playerHead.localRotation.x;        // 시작 수평 회전 값 대입.
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivityX;     // 마우스 수평 회전 (+민감도)
        float mouseY = Input.GetAxis("Mouse Y") * sensitivityY;     // 마우스 수직 회전 (+민감도)

        Vector2 axis = new Vector2(mouseX, mouseY);     // 마우스 회전 값.
        playerBody.Rotate(Vector2.up * axis.x);         // y축을 기준으로 수평 회전.

        // x축 기준으로 수직 회전.
        xRotate = Mathf.Clamp(xRotate - axis.y, limitUpAngle, limitDownAngle);      // 최소, 최대 각도 제한.
        playerHead.localRotation = Quaternion.Euler(xRotate, 0f, 0f);               // 수직 회전.
    }
}
