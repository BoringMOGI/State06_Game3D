using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMove : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    void Update()
    {
       /* float x = Input.GetAxis("Horizontal");      // ����(��, ���� ������)
        float z = Input.GetAxis("Vertical");        // ����(��, ���� �Ʒ���)

        Move(x, z);*/
    }

    public void Move(float x, float z)
    {
        Vector3 direction = transform.forward * z + transform.right * x;
        Vector3 movement = direction * moveSpeed * Time.deltaTime;

        transform.position += movement;
    }
}
