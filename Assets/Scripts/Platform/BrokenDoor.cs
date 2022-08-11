using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FixedJoint))]
public class BrokenDoor : MonoBehaviour
{
    [Tooltip("�μ����� ���Ŀ� ������� �ð�.")]
    [SerializeField] float destroyTime;
    [SerializeField] ParticleSystem destoryFx;
    FixedJoint joint;

    public void SetBrokenDoor(float breakForce)
    {
        joint = GetComponent<FixedJoint>();
        joint.breakForce = breakForce;

        GetComponent<MeshRenderer>().material.color = Color.red;

        StartCoroutine(CheckBroken());
    }

    IEnumerator CheckBroken()
    {
        while (joint != null)
            yield return null;

        yield return new WaitForSeconds(destroyTime);

        Instantiate(destoryFx, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}
