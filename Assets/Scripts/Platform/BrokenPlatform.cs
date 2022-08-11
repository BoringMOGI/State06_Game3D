using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPlatform : MonoBehaviour
{
    [Header("component")]
    [SerializeField] BrokenColor brokenColor;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Collider platformCollider;

    [Header("life cycle")]
    [SerializeField] float stayPlatformTime;        // 유지(버티는) 시간.
    [SerializeField] float brokePlatformTime;       // 부러지는 시간.
    [SerializeField] int level;

    [Header("effect")]
    [SerializeField] ParticleSystem brokeFxPrefab;  // 부서지는 이펙트.
    [SerializeField] float vibratePower;            // 진동 정도.
    [SerializeField] float vibrateRate;             // 진동 주기.

    // 1. 충돌이 일어나면 level을 올린다.
    // 2. 최대 레벨이 되면 일정 시간 후 사라진다.(부서진다.)
    // 3. 충돌 후 계속 일정 시간을 계속 유지하고 있으면 다음 level로 올린다.

    bool isMaxLevel => (level >= brokenColor.MaxLevel);

    float standTime;     // 계속 서있던 시간.

    
    private void Start()
    {
        meshRenderer.material.color = brokenColor.brokenColor[level];
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isMaxLevel)
            return;

        PlatformLevelUp();
    }
    private void OnCollisionStay(Collision collision)
    {
        if (isMaxLevel)
            return;

        standTime += Time.deltaTime;
        if (standTime >= stayPlatformTime)
            PlatformLevelUp();
    }

    void PlatformLevelUp()
    {
        level++;
        standTime = 0.0f;
        meshRenderer.material.color = brokenColor.brokenColor[level];
        if (isMaxLevel)
            StartCoroutine(Broke());
    }

    IEnumerator Vibrate()
    {
        // 포지션을 움직일 트랜스폼 대입.
        Transform mesh = meshRenderer.transform;

        // 각 축의 방향을 담당하는 flip생성.
        Vector3 flip = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));

        while(true)
        {
            // 각 축의 진동 세기를 계산.
            Vector3 power = new Vector3(Random.Range(0f, vibratePower), Random.Range(0f, vibratePower), Random.Range(0f, vibratePower));

            // 방향을 추가.
            power.x *= flip.x;
            power.y *= flip.y;
            power.z *= flip.z;

            // 실제 포지션 이동.
            mesh.localPosition = power;

            // 각 축의 방향 반전.
            flip *= -1f;

            // n초만큼 대기.
            yield return new WaitForSeconds(vibrateRate);
        }
    }
    IEnumerator Broke()
    {
        // 해당 시간만큼 기다린다.
        // 이때 애니메이션을 재생한다.
        Coroutine vibrateCoroutine = StartCoroutine(Vibrate());         // 진동 발생.
        yield return new WaitForSeconds(brokePlatformTime);             // 일정 시간 대기.
        StopCoroutine(vibrateCoroutine);                                // 진동 중지.

        // 파티클 발생.
        // 애니메이션 중지.
        Instantiate(brokeFxPrefab, transform.position, Quaternion.identity);    // 파티클 생성.
        platformCollider.enabled = false;                                       // 플랫폼 콜라이더 비활성화.

        // 시간 값과 알파값 적용.
        float time = 0.2f;
        Color color = Color.white;
        color.a = time;

        // n초 동안 알파값 제거.
        while((time -= Time.deltaTime) > 0.0f)
        {
            color.a = time;
            meshRenderer.material.color = color;

            yield return null;
        }

        // 오브젝트 제거.
        color.a = 0f;
        Destroy(gameObject);
    }

}
