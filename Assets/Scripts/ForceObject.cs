using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IForce
{
    void OnContactForce(ForceData forceData);
}
public struct ForceData
{
    public string objectName;
    public float force;
    public float stunTime;
    public Vector3 direction;
}

public class ForceObject : MonoBehaviour
{
    [SerializeField] string objectName;         // 오브젝트의 이름.
    [SerializeField] float stunTime;            // 출돌시 플레이어의 조작을 멈추는 시간.
    [SerializeField] float mass;                // 질량.

    float acceleration;     // 가속도.
    Vector3 beforePos;      // 이전 위치.

    List<ContactPoint> list = new List<ContactPoint>();

    private void OnCollisionEnter(Collision collision)
    {
        // 충돌 대상과 상호작용이 가능한지 인터페이스 검색.
        IForce target = collision.gameObject.GetComponent<IForce>();
        if (target == null)
            return;

        list.Add(collision.contacts[0]);

        // 전달 값 계산, 구조체 생성.
        ForceData forceData = new ForceData()
        {
            objectName = objectName,
            force = mass * acceleration,
            direction = Vector3.up + collision.contacts[0].normal * -1f,
            stunTime = stunTime
        };
        target.OnContactForce(forceData);
    }

    Vector3 dir;

    void LateUpdate()
    {
        // 이전 위치를 갱신하기 전에
        // 현재 위치와 이전 위치의 거리를 가속도로 변환
        // 이후에 현재 위치를 이전위치로 대입한다.
        acceleration = Vector3.Distance(transform.position, beforePos);
        beforePos = transform.position;
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;

        foreach(ContactPoint contact in list)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(contact.point, 0.1f);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(contact.point, contact.point + (Vector3.up + contact.normal * 0.5f * -1f));
        }
    }
}
