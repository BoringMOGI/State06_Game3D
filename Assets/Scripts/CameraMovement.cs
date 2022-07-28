using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform pivot;
    [SerializeField] float rotateSpeed;

    void Update()
    {
        float x = Input.GetAxis("Mouse X");     // 마우스의 x축 이동량.
        float y = Input.GetAxis("Mouse Y");     // 마우스의 y축 이동량.

        pivot.Rotate(Vector3.up * x * rotateSpeed);

    }
}
