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
        // Ư�� ������Ʈ�� ���̾ ���̾� ����ũ�� ���ԵǴ°�?        
        if (triggerMask.IsContain(other.gameObject.layer))
        {
            // ���� ����� �̺�Ʈ�� ȣ���Ѵ�!!
            OnTrigger?.Invoke();        
        }
    }
}

public static class Method
{
    public static bool IsContain(this LayerMask mask, int layer)
    {
        // mask������ LayerMask(��Ʈ) �����̰� gameobject.layer�� int�̱� �����̴�.
        // ��Ʈ �迭���� Ư�� ��Ʈ�� ���ԵǾ� �ִ��� ã�� ����̴�.
        return (mask & (1 << layer)) != 0;
    }
}
