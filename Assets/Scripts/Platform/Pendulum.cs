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
        

        // ���� ������ ������ ���ض�.
        // transform.rotation = Quaternion.Euler(0, 0, rot);

        // ���� �������� Ư�� ������ �󸶸�ŭ ���ƶ�.
        // transform.Rotate(Vector3.forward * angle);
    }
}
