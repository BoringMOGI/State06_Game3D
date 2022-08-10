using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPlatform : MonoBehaviour
{
    [SerializeField] float forcePower;

    private void OnCollisionEnter(Collision collision)
    {   
        // 충돌체의 리지드바디가 null이 아닐 경우
        // AddForce를 내 기준 위쪽으로 forcePower만큼 Impulse형태로 준다.
        collision.rigidbody?.AddForce(transform.up * forcePower, ForceMode.Impulse);
    }
}
