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

    int index;              // 위치 인덱스.
    bool isReverse;         // 방향.

    void Start()
    {
        // 초기값 대입.
        transform = base.transform;

        // 모든 자식들을 검색하면서 위치 정보를 받아온다.
        positions = new Vector3[transform.childCount];
        for(int i = 0; i<transform.childCount; i++)
        {
            positions[i] = transform.GetChild(i).position;
            Destroy(transform.GetChild(i).gameObject);
        }

        // 최초 위치는 0번째 위치.
        transform.position = positions[0];
        StartCoroutine(Movement());
    }

    IEnumerator Movement()
    {
        WaitForSeconds wait = new WaitForSeconds(waitTime);

        while (true)
        {
            // 목적지까지 moveSpeed의 속도로 이동.
            Vector3 destination = positions[index];
            transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);

            // 목적지와의 거리가 0이라면 (도착)
            if (Vector3.Distance(transform.position, destination) <= 0)
            {
                yield return wait;                              // 대기.
                index += (isReverse ? -1 : 1);                  // 인덱스 증가.
                if(index >= positions.Length || index < 0)      // 범위를 벗어남.
                {
                    isReverse = !isReverse;                     // 방향 반대로.
                    index += (isReverse ? -1 : 1);              // 인덱스 1 증가.
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
