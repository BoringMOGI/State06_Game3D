using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulum : MonoBehaviour
{
    [SerializeField] float limitAngle;
    [SerializeField] float rotateSpeed;
    [SerializeField] bool isStartRandom;

    new Transform transform;
    float randomValue;

    private void Start()
    {
        transform = base.transform;
        randomValue = isStartRandom ? Random.Range(0, 180f) : 0f;
    }

    private void Update()
    {
        float angle = limitAngle * Mathf.Sin(randomValue + Time.time * rotateSpeed);
        transform.rotation = Quaternion.Euler(Vector3.forward * angle);
        

        // 내가 지정한 각도로 변해라.
        // transform.rotation = Quaternion.Euler(0, 0, rot);

        // 현재 각도에서 특정 축으로 얼마만큼 돌아라.
        // transform.Rotate(Vector3.forward * angle);
    }
}
