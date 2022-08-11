using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowTrap : MonoBehaviour
{
    [SerializeField] Rigidbody prefab;      // ������?
    [SerializeField] Transform muzzle;      // ���?
    [SerializeField] float power;           // ��ŭ ������?
    [SerializeField] float rate;            // ��߻� ����.

    bool isReady = true;

    public void OnFire()
    {
        if (!isReady)
            return;

        isReady = false;

        // prefab�� ������ �� muzzle��ġ�� �ΰ� �߻��϶�.
        Rigidbody target = Instantiate(prefab, muzzle.position, muzzle.rotation);
        target.AddForce(muzzle.forward * power, ForceMode.Impulse);
        StartCoroutine(OnRate());
    }
    IEnumerator OnRate()
    {
        // rate��ŭ ��ٷ���.
        yield return new WaitForSeconds(rate);
        isReady = true;
    }
}
