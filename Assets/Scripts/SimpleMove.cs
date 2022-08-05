using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMove : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    void Update()
    {
       /* float x = Input.GetAxis("Horizontal");      // 수평(즉, 왼쪽 오른쪽)
        float z = Input.GetAxis("Vertical");        // 수직(즉, 위쪽 아래쪽)

        Move(x, z);*/
    }

    public void Move(float x, float z)
    {
        Vector3 direction = transform.forward * z + transform.right * x;
        Vector3 movement = direction * moveSpeed * Time.deltaTime;

        transform.position += movement;
    }
}
