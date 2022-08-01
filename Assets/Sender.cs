using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sender : MonoBehaviour
{
    [SerializeField] GameObject[] objects;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            foreach(GameObject obj in objects)
            {
                obj.SendMessage("Receive", "����� �޾ҳ���?");
            }
        }
    }
}
