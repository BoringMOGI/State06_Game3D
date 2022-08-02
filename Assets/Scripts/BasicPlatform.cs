using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPlatform : MonoBehaviour
{
    // 움직이는 물체에 누군가 충돌하면 해당 물체를 나의 자식으로 만들어
    // 같이 움직일 수 있게 한다.
    private void OnCollisionEnter(Collision collision)
    {
        collision.transform.SetParent(transform);
    }
    private void OnCollisionExit(Collision collision)
    {
        collision.transform.SetParent(null);
    }
}
