using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPlatform : MonoBehaviour
{
    // �����̴� ��ü�� ������ �浹�ϸ� �ش� ��ü�� ���� �ڽ����� �����
    // ���� ������ �� �ְ� �Ѵ�.
    private void OnCollisionEnter(Collision collision)
    {
        collision.transform.SetParent(transform);
    }
    private void OnCollisionExit(Collision collision)
    {
        collision.transform.SetParent(null);
    }
}
