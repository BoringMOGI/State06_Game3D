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
    [SerializeField] string objectName;         // ������Ʈ�� �̸�.
    [SerializeField] float stunTime;            // �⵹�� �÷��̾��� ������ ���ߴ� �ð�.
    [SerializeField] float mass;                // ����.

    float acceleration;     // ���ӵ�.
    Vector3 beforePos;      // ���� ��ġ.

    List<ContactPoint> list = new List<ContactPoint>();

    private void OnCollisionEnter(Collision collision)
    {
        // �浹 ���� ��ȣ�ۿ��� �������� �������̽� �˻�.
        IForce target = collision.gameObject.GetComponent<IForce>();
        if (target == null)
            return;

        list.Add(collision.contacts[0]);

        // ���� �� ���, ����ü ����.
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
        // ���� ��ġ�� �����ϱ� ����
        // ���� ��ġ�� ���� ��ġ�� �Ÿ��� ���ӵ��� ��ȯ
        // ���Ŀ� ���� ��ġ�� ������ġ�� �����Ѵ�.
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
