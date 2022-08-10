using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] Rigidbody bulletPrefab;    // �߻�ü.
    [SerializeField] Transform muzzle;          // �߻� ��ġ.
    [SerializeField] float firePower;           // �߻� ��.
    [SerializeField] float fireRate;            // �߻� ����.

    private void Start()
    {
        StartCoroutine(FireCycle());
    }

    IEnumerator FireCycle()
    {
        WaitForSeconds wait = new WaitForSeconds(fireRate);
        while(true)
        {
            Rigidbody bullet = Instantiate(bulletPrefab, muzzle.position, Quaternion.identity);
            bullet.AddForce(muzzle.forward * firePower, ForceMode.Impulse);
            yield return wait;
        }
    }

}
