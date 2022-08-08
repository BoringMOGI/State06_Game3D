using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// abstract class
// = ���ο� �̱��� �Լ��� �����ϴ� Ŭ����.
//   �ش� Ŭ���������δ� ��ü ������ �Ұ����ϴ�.
public abstract class TouchState : MonoBehaviour
{
    protected enum STATE
    {
        Ready,      // ��ġ�� ����ϴ� ��.
        Begin,      // ��ġ�� �����ߴ�.
        Stay,       // ��� ������ ���̴�.
        Ended,      // ��ġ�� �����ߴ�.
    }

    // ���̽�ƽ�� ����.

    [SerializeField] protected Camera uiCam;                // �ش� Canvas�� ���ߴ� camera.
    [SerializeField] protected RectTransform rootRect;      // ���� Rect.

    protected bool isBeginInsideRootRect
    {
        get;                // ���� ������ ���� protected.
        private set;        // ���� ������ ���� private.
    }                 // ��ġ�� rootRect�ȿ��� ���۵Ǿ��°�?
    protected bool isBeginInsideUI
    {
        get;
        private set;
    }                       // ��ġ�� ��� UI ������ ���۵Ǿ��°�?
    protected STATE state
    {
        get;                // ���� ������ ���� protected.
        private set;        // ���� ������ ���� private.
    }                                // ��ġ�� ���� ����.
    protected Vector3 touchPosition
    {
        get
        {
            // ���콺 ��ġ�� '��ũ�� ��ǥ'�ε� ���� UI�� '���� ��ǥ'�� �ֱ� �����̴�.
            // ���� ���콺�� z�� 0�ε� rootRect�� z�� n�̱� ������ �����ϰ� �����.
            if (Input.GetMouseButton(0))
            {
                Vector3 position = uiCam.ScreenToWorldPoint(Input.mousePosition);
                if(rootRect != null)
                    position.z = rootRect.position.z;

                return position;
            }
            else
            {
                // ��ġ�� �ϰ� ���� �������� ��ġ�� zero�� �����.
                return Vector3.zero;
            }
        }
    }                      // ���� ��ġ ��ġ.

    void Update()
    {
        // ���� �ӽ� (state machine) : Ư�� ���¿� ���� ó���� �ϴ� ����.
        switch (state)
        {
            case STATE.Ready:
                ReadyTouch();
                break;
            case STATE.Begin:
                BeginTouch();
                break;
            case STATE.Stay:
                StayTouch();
                break;
            case STATE.Ended:
                EndedTouch();
                break;
        }
    }

    private void ReadyTouch()
    {
        if (Input.GetMouseButtonDown(0))    // ���콺 �Է��� ������.
        {
            // ��ġ�� ���۵� ��ġ�� rootRect ���ο� ���� ��� bool ������ true.
            if (rootRect != null)
                isBeginInsideRootRect = RectTransformUtility.RectangleContainsScreenPoint(rootRect, touchPosition);
            else
                isBeginInsideRootRect = false;

            // ��ġ�� ���۵� ��ġ�� Ư�� UI �������ΰ�?
            isBeginInsideUI = EventSystem.current.IsPointerOverGameObject();

            // ��ġ�� �����ϸ� ���¸� Begin���� ����.
            state = STATE.Begin;
        }
    }
    private void BeginTouch()
    {
        OnBeginTouch();                     // ù ������ ������ ��
        state = STATE.Stay;                 // ���¸� ������ �ִ� ������ ����.
    }
    private void StayTouch()
    {
        if (Input.GetMouseButton(0))        // ��� ������ ���� �ʴٸ�.
            OnStayTouch();                  // ������ ���̶�� Stay�Լ� ȣ��.
        else
            state = STATE.Ended;            // ���¸� Ended�� ����.
    }
    private void EndedTouch()
    {
        OnEndedTouch();                     // ���� �Լ� ȣ��.

        isBeginInsideRootRect = false;
        isBeginInsideUI = false;

        state = STATE.Ready;                // ���¸� Ready�� ����.
    }

    // abstract �Լ�
    // = ���� �����ΰ� ���� �Լ�, abstractŬ���������� ����� �� �ִ�.
    //   �ڽ��� ����ϴ� Ŭ������ ���� �Լ��� ������ ������ �� �ִ�.
    protected abstract void OnBeginTouch();
    protected abstract void OnStayTouch();
    protected abstract void OnEndedTouch();
}
