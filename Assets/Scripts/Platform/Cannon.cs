using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] Rigidbody bulletPrefab;    // 발사체.
    [SerializeField] Transform muzzle;          // 발사 위치.
    [SerializeField] float firePower;           // 발사 힘.
    [SerializeField] float fireRate;            // 발사 간격.

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
