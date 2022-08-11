using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// abstract class
// = 내부에 미구현 함수가 존재하는 클래스.
//   해당 클래스만으로는 객체 선언이 불가능하다.
public abstract class TouchState : MonoBehaviour
{
    protected enum STATE
    {
        Ready,      // 터치를 대기하는 중.
        Begin,      // 터치를 시작했다.
        Stay,       // 계속 누르는 중이다.
        Ended,      // 터치를 종료했다.
    }

    // 조이스틱의 상태.

    [SerializeField] protected Camera uiCam;                // 해당 Canvas를 비추는 camera.
    [SerializeField] protected RectTransform rootRect;      // 기준 Rect.

    protected bool isBeginInsideRootRect
    {
        get;                // 값을 참조할 때는 protected.
        private set;        // 값을 대입할 때는 private.
    }                 // 터치가 rootRect안에서 시작되었는가?
    protected bool isBeginInsideUI
    {
        get;
        private set;
    }                       // 터치가 어떠한 UI 위에서 시작되었는가?
    protected STATE state
    {
        get;                // 값을 참조할 때는 protected.
        private set;        // 값을 대입할 때는 private.
    }                                // 터치의 현재 상태.
    protected Vector3 touchPosition
    {
        get
        {
            // 마우스 위치는 '스크린 좌표'인데 실제 UI는 '월드 좌표'에 있기 때문이다.
            // 실제 마우스의 z는 0인데 rootRect의 z는 n이기 때문에 동일하게 맞춘다.
            if (Input.GetMouseButton(0))
            {
                Vector3 position = uiCam.ScreenToWorldPoint(Input.mousePosition);
                if(rootRect != null)
                    position.z = rootRect.position.z;

                return position;
            }
            else
            {
                // 터치를 하고 있지 않을때는 위치를 zero로 맞춘다.
                return Vector3.zero;
            }
        }
    }                      // 현재 터치 위치.

    void Update()
    {
        // 상태 머신 (state machine) : 특정 상태에 따라 처리를 하는 패턴.
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
        if (Input.GetMouseButtonDown(0))    // 마우스 입력이 들어오면.
        {
            // 터치가 시작된 위치가 rootRect 내부에 있을 경우 bool 변수를 true.
            if (rootRect != null)
                isBeginInsideRootRect = RectTransformUtility.RectangleContainsScreenPoint(rootRect, touchPosition);
            else
                isBeginInsideRootRect = false;

            // 터치가 시작된 위치가 특정 UI 위에서인가?
            isBeginInsideUI = EventSystem.current.IsPointerOverGameObject();

            // 터치를 시작하면 상태를 Begin으로 변경.
            state = STATE.Begin;
        }
    }
    private void BeginTouch()
    {
        OnBeginTouch();                     // 첫 시작을 실행한 후
        state = STATE.Stay;                 // 상태를 누르고 있는 중으로 변경.
    }
    private void StayTouch()
    {
        if (Input.GetMouseButton(0))        // 계속 누르고 있지 않다면.
            OnStayTouch();                  // 누르는 중이라면 Stay함수 호출.
        else
            state = STATE.Ended;            // 상태를 Ended로 변경.
    }
    private void EndedTouch()
    {
        OnEndedTouch();                     // 종료 함수 호출.

        isBeginInsideRootRect = false;
        isBeginInsideUI = false;

        state = STATE.Ready;                // 상태를 Ready로 변경.
    }

    // abstract 함수
    // = 실제 구현부가 없는 함수, abstract클래스에서만 사용할 수 있다.
    //   자신을 상속하는 클래스에 대해 함수의 구현을 강제할 수 있다.
    protected abstract void OnBeginTouch();
    protected abstract void OnStayTouch();
    protected abstract void OnEndedTouch();
}
