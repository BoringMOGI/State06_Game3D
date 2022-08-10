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

    [SerializeField] Vector2 sencitivity;       // ȸ�� �ΰ���.
    [SerializeField] MinMax limitRotateX;       // x�� ȸ�� ���� ����.

    Vector3 beforeTouch;                        // ���� ��ġ ��ġ.
    float rotateX;                              // ���� ȸ�� ����.

    protected override void OnBeginTouch()
    {
        if (isBeginInsideUI)
            return;

        beforeTouch = touchPosition;        // ���� ��ġ�� ���� ���� ���� ��ġ��.
    }

    protected override void OnStayTouch()
    {
        if (isBeginInsideUI)
            return;

        // ��ġ �̵��� * �ΰ���.
        Vector2 movement = (touchPosition - beforeTouch) * sencitivity;

        // x���� �������� ����(����) ȸ�� �.
        rotateX = Mathf.Clamp(rotateX - (movement.y * Time.deltaTime), limitRotateX.min, limitRotateX.max);
        transform.rotation = Quaternion.Euler(rotateX, transform.eulerAngles.y, 0);

        // y���� �������� ����(�¿�) ȸ�� �.
        transform.Rotate(Vector3.up * movement.x * Time.deltaTime);

        // ���� ��ġ ����.
        beforeTouch = touchPosition;
    }
    protected override void OnEndedTouch()
    {
        if (isBeginInsideUI)
            return;
    }
}
