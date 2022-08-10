using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class JoyStick : TouchState
{
    [SerializeField] RectTransform stick;                   // ��ƽ.
    [SerializeField] UnityEvent<float, float> OnInput;      // ��ƽ�� �������� �� ȣ��Ǵ� �̺�Ʈ �Լ�.

    private float radius; // ���̽�ƽ�� ������ ����.

    private void Start()
    {
        // RectTransform.sizeDelta : Vector2
        // = x�� �ʺ�, y�� ����.
        radius = rootRect.sizeDelta.x / 2f * rootRect.lossyScale.x;     // ���̽�ƽ�� �ʺ� / 2 * ����.
    }

    // ���� ���� : ��ġ�� ���۵Ǿ���.
    protected override void OnBeginTouch()
    {
       // Ư�� ���� �ʱ� ���� ���� ���� �ִ�.

    }

    // ���� ���� : ��ġ ���� ��. ���� ��ġ ��ġ�� ���� stick�� �����δ�.
    protected override void OnStayTouch()
    {
        // ��ġ�� ���� ��ġ�� ��ȿ���� �ʾұ� ������ ����.
        if (!isBeginInsideRootRect)
            return;

        #region ��ƽ ������

        Vector3 stickPosition = touchPosition;                                  // ��ƽ�� ��ġ.
        Vector3 originPosition = rootRect.position;                             // �������� stick�� ���� ��ġ.
        float distance = Vector3.Distance(originPosition, touchPosition);       // ������ �ش� �������� �Ÿ�.

        // ���� �� ������ �Ÿ��� ���������� ũ�ٸ�
        if (distance > radius)
        {
            Vector3 direction = (touchPosition - originPosition).normalized;    // �������κ��� �����Ǳ����� ����.
            stickPosition = originPosition + direction * radius;                // ���� ��ġ���� �ش� �������� ������ ������ ��ġ ��.
        }

        // ���� stick�� ������ ����.
        stick.position = stickPosition;

        // ��ƽ�� ������ ����(radio)
        // �������κ��� ��ƽ�� ������ �Ÿ� / ��ƽ�� ������ �� �ִ� ��ü �Ÿ�.
        float x = (stickPosition.x - originPosition.x) / radius;
        float y = (stickPosition.y - originPosition.y) / radius;

        // �̺�Ʈ �Լ� ȣ��.
        OnInput?.Invoke(x, y);

        #endregion
    }

    // ���� ���� : ��ġ�� �����߱� ������ ���� ������� �ǵ�����.
    protected override void OnEndedTouch()
    {
        // ��ġ�� ���� ��ġ�� ��ȿ���� �ʾұ� ������ ����.
        if (!isBeginInsideRootRect)
            return;

        // ��ġ�� ������ ������ stick�� ���� ��ġ�� ����.
        stick.position = rootRect.position;
        OnInput.Invoke(0f, 0f);
    }
}
