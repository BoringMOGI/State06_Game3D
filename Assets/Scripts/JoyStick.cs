using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class JoyStick : MonoBehaviour
{
    [SerializeField] Camera uiCam;
    [SerializeField] RectTransform stick;
    [SerializeField] UnityEvent<float, float> OnInput;

    Vector3 originPos;      // ���� ��ġ.
    float radius;           // ������.
    private void Start()
    {
        // RectTransform.sizeDelta : Vector2
        // = x�� �ʺ�, y�� ����.
        RectTransform myRect = GetComponent<RectTransform>();
        radius = myRect.sizeDelta.x / 2f * myRect.lossyScale.x;     // ���̽�ƽ�� �ʺ� / 2 * ����.
    }
    void Update()
    {
        originPos = transform.position;                                 // ��ƽ�� ���� ��ġ.

        if (!Input.GetMouseButton(0))
        {
            stick.position = originPos;
            OnInput.Invoke(0f, 0f);
            return;
        }

        Vector3 position = uiCam.ScreenToWorldPoint(Input.mousePosition);   // ���콺 �������� ���� ��ǥ ��ġ.

        float distance = Vector3.Distance(originPos, position);             // ������ �ش� �������� �Ÿ�.

        // ���� �� ������ �Ÿ��� ���������� ũ�ٸ�
        if (distance > radius)
        {
            Vector3 direction = (position - originPos).normalized;          // �������κ��� �����Ǳ����� ����.
            position = originPos + direction * radius;                      // ���� ��ġ���� �ش� �������� ������ ������ ��ġ ��.
        }

        position.z = originPos.z;       // ���� �������� z���� ������ z������ ����.
        stick.position = position;      // ���� stick�� ������ ����.

        Vector3 stickMovement = position - originPos;   // ��ƽ�� ������ �̵���.
        float x = stickMovement.x / radius;             // ��ƽ�� x������ ������ ����.
        float y = stickMovement.y / radius;             // ��ƽ�� y������ ������ ����.

        OnInput?.Invoke(x, y);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(originPos, .3f);
    }
}
