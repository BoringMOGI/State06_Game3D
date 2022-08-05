using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class JoyStick : MonoBehaviour
{
    [SerializeField] Camera uiCam;
    [SerializeField] RectTransform stick;
    [SerializeField] UnityEvent<float, float> OnInput;

    Vector3 originPos;      // 원래 위치.
    float radius;           // 반지름.
    private void Start()
    {
        // RectTransform.sizeDelta : Vector2
        // = x가 너비, y가 높이.
        RectTransform myRect = GetComponent<RectTransform>();
        radius = myRect.sizeDelta.x / 2f * myRect.lossyScale.x;     // 조이스틱의 너비 / 2 * 비율.
    }
    void Update()
    {
        originPos = transform.position;                                 // 스틱의 원래 위치.

        if (!Input.GetMouseButton(0))
        {
            stick.position = originPos;
            OnInput.Invoke(0f, 0f);
            return;
        }

        Vector3 position = uiCam.ScreenToWorldPoint(Input.mousePosition);   // 마우스 포지션의 월드 좌표 위치.

        float distance = Vector3.Distance(originPos, position);             // 원점과 해당 포지션의 거리.

        // 만약 둘 사이의 거리가 반지름보다 크다면
        if (distance > radius)
        {
            Vector3 direction = (position - originPos).normalized;          // 원점으로부터 포지션까지의 방향.
            position = originPos + direction * radius;                      // 원점 위치에서 해당 방향으로 반지름 길이의 위치 값.
        }

        position.z = originPos.z;       // 최종 포지션의 z값을 원점의 z값으로 변경.
        stick.position = position;      // 실제 stick의 포지션 조정.

        Vector3 stickMovement = position - originPos;   // 스틱이 움직인 이동량.
        float x = stickMovement.x / radius;             // 스틱이 x축으로 움직인 비율.
        float y = stickMovement.y / radius;             // 스틱이 y축으로 움직인 비율.

        OnInput?.Invoke(x, y);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(originPos, .3f);
    }
}
