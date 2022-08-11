using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MovePlatform : BasicPlatform
{
    [SerializeField] float moveSpeed;
    [SerializeField] float waitTime;

    Vector3[] positions;
    new Transform transform;

    int index;              // ��ġ �ε���.
    bool isReverse;         // ����.

    void Start()
    {
        // �ʱⰪ ����.
        transform = base.transform;

        // ��� �ڽĵ��� �˻��ϸ鼭 ��ġ ������ �޾ƿ´�.
        positions = new Vector3[transform.childCount];
        for(int i = 0; i<transform.childCount; i++)
        {
            positions[i] = transform.GetChild(i).position;
            Destroy(transform.GetChild(i).gameObject);
        }

        // ���� ��ġ�� 0��° ��ġ.
        transform.position = positions[0];
        StartCoroutine(Movement());
    }

    IEnumerator Movement()
    {
        WaitForSeconds wait = new WaitForSeconds(waitTime);

        while (true)
        {
            // ���������� moveSpeed�� �ӵ��� �̵�.
            Vector3 destination = positions[index];
            transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);

            // ���������� �Ÿ��� 0�̶�� (����)
            if (Vector3.Distance(transform.position, destination) <= 0)
            {
                yield return wait;                              // ���.
                index += (isReverse ? -1 : 1);                  // �ε��� ����.
                if(index >= positions.Length || index < 0)      // ������ ���.
                {
                    isReverse = !isReverse;                     // ���� �ݴ��.
                    index += (isReverse ? -1 : 1);              // �ε��� 1 ����.
                }
            }

            yield return null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
            return;

        Gizmos.color = Color.green;
        foreach (Vector3 position in positions)
            Gizmos.DrawSphere(position, 0.1f);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, positions[index]);
    }
}
