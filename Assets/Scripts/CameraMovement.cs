using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform pivot;
    [SerializeField] float rotateSpeed;

    void Update()
    {
        float x = Input.GetAxis("Mouse X");     // ���콺�� x�� �̵���.
        float y = Input.GetAxis("Mouse Y");     // ���콺�� y�� �̵���.

        pivot.Rotate(Vector3.up * x * rotateSpeed);

    }
}
