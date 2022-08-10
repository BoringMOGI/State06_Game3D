using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPlatform : MonoBehaviour
{
    [SerializeField] float forcePower;

    private void OnCollisionEnter(Collision collision)
    {   
        // �浹ü�� ������ٵ� null�� �ƴ� ���
        // AddForce�� �� ���� �������� forcePower��ŭ Impulse���·� �ش�.
        collision.rigidbody?.AddForce(transform.up * forcePower, ForceMode.Impulse);
    }
}
