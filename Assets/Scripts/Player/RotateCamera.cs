using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : TouchState
{
    [System.Serializable]
    public struct MinMax
    {
        public float min;
        public float max;
    }

    [SerializeField] Vector2 sencitivity;       // 회전 민감도.
    [SerializeField] MinMax limitRotateX;       // x축 회전 제한 각도.

    Vector3 beforeTouch;                        // 이전 터치 위치.
    float rotateX;                              // 수직 회전 각도.

    protected override void OnBeginTouch()
    {
        if (isBeginInsideUI)
            return;

        beforeTouch = touchPosition;        // 이전 위치의 최초 값은 현재 위치다.
    }

    protected override void OnStayTouch()
    {
        if (isBeginInsideUI)
            return;

        // 터치 이동량 * 민감도.
        Vector2 movement = (touchPosition - beforeTouch) * sencitivity;

        // x축을 기준으로 수직(상하) 회전 운동.
        rotateX = Mathf.Clamp(rotateX - (movement.y * Time.deltaTime), limitRotateX.min, limitRotateX.max);
        transform.rotation = Quaternion.Euler(rotateX, transform.eulerAngles.y, 0);

        // y축을 기준으로 수평(좌우) 회전 운동.
        transform.Rotate(Vector3.up * movement.x * Time.deltaTime);

        // 이전 위치 갱신.
        beforeTouch = touchPosition;
    }
    protected override void OnEndedTouch()
    {
        if (isBeginInsideUI)
            return;
    }
}
