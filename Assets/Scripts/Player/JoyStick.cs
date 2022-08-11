using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class JoyStick : TouchState
{
    [SerializeField] RectTransform stick;                   // 스틱.
    [SerializeField] UnityEvent<float, float> OnInput;      // 스틱을 움직였을 때 호출되는 이벤트 함수.

    private float radius; // 조이스틱의 반지름 길이.

    private void Start()
    {
        // RectTransform.sizeDelta : Vector2
        // = x가 너비, y가 높이.
        radius = rootRect.sizeDelta.x / 2f * rootRect.lossyScale.x;     // 조이스틱의 너비 / 2 * 비율.
    }

    // 시작 상태 : 터치가 시작되었다.
    protected override void OnBeginTouch()
    {
       // 특정 값의 초기 값을 넣을 수도 있다.

    }

    // 지속 상태 : 터치 지속 중. 나의 터치 위치에 따라 stick을 움직인다.
    protected override void OnStayTouch()
    {
        // 터치의 시작 위치가 유효하지 않았기 때문에 리턴.
        if (!isBeginInsideRootRect)
            return;

        #region 스틱 움직임

        Vector3 stickPosition = touchPosition;                                  // 스틱의 위치.
        Vector3 originPosition = rootRect.position;                             // 원점이자 stick의 원래 위치.
        float distance = Vector3.Distance(originPosition, touchPosition);       // 원점과 해당 포지션의 거리.

        // 만약 둘 사이의 거리가 반지름보다 크다면
        if (distance > radius)
        {
            Vector3 direction = (touchPosition - originPosition).normalized;    // 원점으로부터 포지션까지의 방향.
            stickPosition = originPosition + direction * radius;                // 원점 위치에서 해당 방향으로 반지름 길이의 위치 값.
        }

        // 실제 stick의 포지션 조정.
        stick.position = stickPosition;

        // 스틱을 움직인 비율(radio)
        // 원점으로부터 스틱이 움직인 거리 / 스틱이 움직일 수 있는 전체 거리.
        float x = (stickPosition.x - originPosition.x) / radius;
        float y = (stickPosition.y - originPosition.y) / radius;

        // 이베트 함수 호출.
        OnInput?.Invoke(x, y);

        #endregion
    }

    // 종료 상태 : 터치를 종료했기 때문에 값을 원래대로 되돌린다.
    protected override void OnEndedTouch()
    {
        // 터치의 시작 위치가 유효하지 않았기 때문에 리턴.
        if (!isBeginInsideRootRect)
            return;

        // 터치가 끝났기 때문에 stick을 원래 위치로 변경.
        stick.position = rootRect.position;
        OnInput.Invoke(0f, 0f);
    }
}
