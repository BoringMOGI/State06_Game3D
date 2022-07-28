using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] Rigidbody target;
    [SerializeField] Vector3 offset;

    float angleY;
    float angleX;

    // Update is called once per frame
    void Update()
    {
        angleY += Input.GetAxis("Horizontal");

    }
}
