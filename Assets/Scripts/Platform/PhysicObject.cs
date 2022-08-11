using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicObject : MonoBehaviour, IContinuousForce
{
    Rigidbody rigid;
    Vector3 continuousForce;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {   
        if(continuousForce != Vector3.zero)
        {
            rigid.MovePosition(rigid.position + continuousForce * Time.fixedDeltaTime);
        }

        continuousForce = Vector3.MoveTowards(continuousForce, Vector3.zero, Time.fixedDeltaTime * 3f);
    }

    public void AddContinuousForce(Vector3 direction, float power)
    {
        continuousForce = direction * power;
    }
}
