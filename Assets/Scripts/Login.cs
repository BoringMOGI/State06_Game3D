using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Login : MonoBehaviour
{
    public enum TYPE
    {
        Login,
        SignUp,
    }

    private static Login instance;
    public static Login Instance => instance;

    [SerializeField] GameObject[] panels;

    private void Awake()
    {
        instance = this;
    }

    public void OnSwitchPanel(TYPE type)
    {
        foreach (GameObject panel in panels)
            panel.SetActive(false);

        panels[(int)type].SetActive(true);
    }
}
