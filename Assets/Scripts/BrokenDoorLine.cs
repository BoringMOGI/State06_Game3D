using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenDoorLine : MonoBehaviour
{
    [Tooltip("내 자식 문들 중에서 최소 몇개는 확정적으로 부술 수 있는가?")]
    [SerializeField] int minBrokenCount;        // 최소 개수.
    [SerializeField] float brokenPersent;       // 확률.

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
