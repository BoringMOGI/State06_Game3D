using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MessageHandler : MonoBehaviour
{
    [SerializeField] GameObject target;

    void Receive(AnimationEvent e)
    {
        target.SendMessage(e.stringParameter, e.intParameter);
    }

}
