using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    [SerializeField] LayerMask triggerMask;
    [SerializeField] UnityEvent OnTrigger;

    private void OnTriggerEnter(Collider other)
    {
        // 특정 오브젝트의 레이어가 레이어 마스크에 포함되는가?        
        if (triggerMask.IsContain(other.gameObject.layer))
        {
            // 나와 연결된 이벤트를 호출한다!!
            OnTrigger?.Invoke();        
        }
    }
}

public static class Method
{
    public static bool IsContain(this LayerMask mask, int layer)
    {
        // mask변수는 LayerMask(비트) 형식이고 gameobject.layer는 int이기 때문이다.
        // 비트 배열에서 특정 비트가 포함되어 있는지 찾는 방식이다.
        return (mask & (1 << layer)) != 0;
    }
}
