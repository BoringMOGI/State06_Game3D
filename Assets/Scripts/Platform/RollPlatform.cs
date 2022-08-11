using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class RollPlatform : MonoBehaviour
{
    [SerializeField] float rollSpeed;

    MeshRenderer meshRenderer;      // �޽� ������.
    float offsetY;                  // ���͸����� y�� offset.

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        RollTexture();
    }

    List<IContinuousForce> forceList = new List<IContinuousForce>();

    private void OnCollisionEnter(Collision collision)
    {
        IContinuousForce target = collision.gameObject.GetComponent<IContinuousForce>();
        if (target != null)
            forceList.Add(target);
    }
    private void OnCollisionExit(Collision collision)
    {
        IContinuousForce target = collision.gameObject.GetComponent<IContinuousForce>();
        if (target != null)
            forceList.Remove(target);
    }
    private void OnCollisionStay(Collision collision)
    {
        // �������̽��� ������, ���� �浹 ���� ��� ��ü���� �������� �� ����.
        foreach(IContinuousForce force in forceList)
        {
            force.AddContinuousForce(transform.forward, rollSpeed);
        }
    }


    private void RollTexture()
    {
        // offset�� y�� �ӵ���ŭ ������Ų��.
        // ���� 1.0�� �Ѿ������ 1.0��ŭ ����.
        if ((offsetY += rollSpeed * 0.1f * Time.deltaTime) > 1.0f)
            offsetY -= 1.0f;

        Material material = meshRenderer.material;
        material.SetTextureOffset("_MainTex", new Vector2(0.0f, offsetY));
    }
}
