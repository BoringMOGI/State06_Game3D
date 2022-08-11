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
    [SerializeField] float stayPlatformTime;        // ����(��Ƽ��) �ð�.
    [SerializeField] float brokePlatformTime;       // �η����� �ð�.
    [SerializeField] int level;

    [Header("effect")]
    [SerializeField] ParticleSystem brokeFxPrefab;  // �μ����� ����Ʈ.
    [SerializeField] float vibratePower;            // ���� ����.
    [SerializeField] float vibrateRate;             // ���� �ֱ�.

    // 1. �浹�� �Ͼ�� level�� �ø���.
    // 2. �ִ� ������ �Ǹ� ���� �ð� �� �������.(�μ�����.)
    // 3. �浹 �� ��� ���� �ð��� ��� �����ϰ� ������ ���� level�� �ø���.

    bool isMaxLevel => (level >= brokenColor.MaxLevel);

    float standTime;     // ��� ���ִ� �ð�.

    
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
        // �������� ������ Ʈ������ ����.
        Transform mesh = meshRenderer.transform;

        // �� ���� ������ ����ϴ� flip����.
        Vector3 flip = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));

        while(true)
        {
            // �� ���� ���� ���⸦ ���.
            Vector3 power = new Vector3(Random.Range(0f, vibratePower), Random.Range(0f, vibratePower), Random.Range(0f, vibratePower));

            // ������ �߰�.
            power.x *= flip.x;
            power.y *= flip.y;
            power.z *= flip.z;

            // ���� ������ �̵�.
            mesh.localPosition = power;

            // �� ���� ���� ����.
            flip *= -1f;

            // n�ʸ�ŭ ���.
            yield return new WaitForSeconds(vibrateRate);
        }
    }
    IEnumerator Broke()
    {
        // �ش� �ð���ŭ ��ٸ���.
        // �̶� �ִϸ��̼��� ����Ѵ�.
        Coroutine vibrateCoroutine = StartCoroutine(Vibrate());         // ���� �߻�.
        yield return new WaitForSeconds(brokePlatformTime);             // ���� �ð� ���.
        StopCoroutine(vibrateCoroutine);                                // ���� ����.

        // ��ƼŬ �߻�.
        // �ִϸ��̼� ����.
        Instantiate(brokeFxPrefab, transform.position, Quaternion.identity);    // ��ƼŬ ����.
        platformCollider.enabled = false;                                       // �÷��� �ݶ��̴� ��Ȱ��ȭ.

        // �ð� ���� ���İ� ����.
        float time = 0.2f;
        Color color = Color.white;
        color.a = time;

        // n�� ���� ���İ� ����.
        while((time -= Time.deltaTime) > 0.0f)
        {
            color.a = time;
            meshRenderer.material.color = color;

            yield return null;
        }

        // ������Ʈ ����.
        color.a = 0f;
        Destroy(gameObject);
    }

}
