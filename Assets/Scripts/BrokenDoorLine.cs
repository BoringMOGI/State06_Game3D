using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenDoorLine : MonoBehaviour
{
    [Tooltip("�� �ڽ� ���� �߿��� �ּ� ��� Ȯ�������� �μ� �� �ִ°�?")]
    [SerializeField] int minBrokenCount;        // �ּ� ����.
    [SerializeField] float brokenPersent;       // Ȯ��.

    const float BREAK_FORCE = 50f;

    List<BrokenDoor> doorList;

    void Start()
    {
        BrokenDoor[] allDoors = transform.GetComponentsInChildren<BrokenDoor>();
        doorList = new List<BrokenDoor>(allDoors);

        int index = 0;
        while (doorList.Count > 0)
        {
            SetRandomDoor(index < minBrokenCount ? 100f : brokenPersent);
            index += 1;
        }
    }

    private void SetRandomDoor(float persent)
    {
        int randomIndex = Random.Range(0, doorList.Count);
        BrokenDoor door = doorList[randomIndex];
        doorList.RemoveAt(randomIndex);

        if (Random.value * 100f < persent)
            door.SetBrokenDoor(BREAK_FORCE);
    }

}
