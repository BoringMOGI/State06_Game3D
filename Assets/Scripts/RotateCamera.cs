using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : TouchState
{
    protected override void OnBeginTouch()
    {

    }
    protected override void OnStayTouch()
    {
        if (isBeginInsideUI)
            return;


        Debug.Log("화면 회전 중...");
    }
    protected override void OnEndedTouch()
    {


    }
}
