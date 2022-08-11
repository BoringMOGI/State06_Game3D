using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowTrap : MonoBehaviour
{
    [SerializeField] Rigidbody prefab;      // 무엇을?
    [SerializeField] Transform muzzle;      // 어디서?
    [SerializeField] float power;           // 얼만큼 힘으로?
    [SerializeField] float rate;            // 재발사 간격.

    bool isReady = true;

    public void OnFire()
    {
        if (!isReady)
            return;

        isReady = false;

        // prefab을 생성한 후 muzzle위치에 두고 발사하라.
        Rigidbody target = Instantiate(prefab, muzzle.position, muzzle.rotation);
        target.AddForce(muzzle.forward * power, ForceMode.Impulse);
        StartCoroutine(OnRate());
    }
    IEnumerator OnRate()
    {
        // rate만큼 기다려라.
        yield return new WaitForSeconds(rate);
        isReady = true;
    }
}
