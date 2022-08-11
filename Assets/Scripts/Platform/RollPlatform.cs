using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class RollPlatform : MonoBehaviour
{
    [SerializeField] float rollSpeed;

    MeshRenderer meshRenderer;      // 메쉬 렌더러.
    float offsetY;                  // 매터리얼의 y축 offset.

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
        // 인터페이스를 구현한, 나와 충돌 중인 모든 물체에게 지속적인 힘 전달.
        foreach(IContinuousForce force in forceList)
        {
            force.AddContinuousForce(transform.forward, rollSpeed);
        }
    }


    private void RollTexture()
    {
        // offset의 y를 속도만큼 증가시킨다.
        // 만약 1.0을 넘어버리면 1.0만큼 뺀다.
        if ((offsetY += rollSpeed * 0.1f * Time.deltaTime) > 1.0f)
            offsetY -= 1.0f;

        Material material = meshRenderer.material;
        material.SetTextureOffset("_MainTex", new Vector2(0.0f, offsetY));
    }
}
