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


        Debug.Log("ȭ�� ȸ�� ��...");
    }
    protected override void OnEndedTouch()
    {


    }
}
