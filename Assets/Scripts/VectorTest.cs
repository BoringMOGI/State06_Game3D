using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorTest : MonoBehaviour
{
    [SerializeField] Transform model;

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 dir = Vector3.forward * z + Vector3.right * x;
        transform.rotation = Quaternion.LookRotation(dir);
    }
}
