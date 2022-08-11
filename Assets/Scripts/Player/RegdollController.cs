using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegdollController : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject regdoll;
    [SerializeField] Rigidbody regdollChest;

    void Start()
    {
        Vector3 vec1 = new Vector3(10, 10, 0);
        Vector3 vec2 = new Vector3(20, 30, 0);
        Vector3 dir = vec2 - vec1;

        Debug.Log(Vector3.Distance(vec1, vec2));
        Debug.Log(Vector3.Magnitude(dir));
        Debug.Log(dir);
        Debug.Log(dir.normalized);

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            // 현재 플레이어의 모든 위치 정보를 regdoll에 복사한다.
            CopyCharactorTransformToRegdoll(player.transform, regdoll.transform);

            player.SetActive(false);
            regdoll.SetActive(true);
            regdollChest.AddForce((-Vector3.forward + Vector3.up) * 200f, ForceMode.Impulse);
        }

    }

    void CopyCharactorTransformToRegdoll(Transform origin, Transform target)
    {
        Transform[] originChilds = origin.GetComponentsInChildren<Transform>();
        Transform[] targetChilds = target.GetComponentsInChildren<Transform>();

        for(int i = 0; i<originChilds.Length; i++)
        {
            targetChilds[i].localPosition = originChilds[i].localPosition;
            targetChilds[i].localRotation = originChilds[i].localRotation;
        }
    }
}
