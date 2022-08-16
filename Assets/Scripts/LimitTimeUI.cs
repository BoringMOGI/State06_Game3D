using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class LimitTimeUI : MonoBehaviour
{
    [SerializeField] Text limitText;

    public void UpdateLimitTime(float value)
    {
        // value���� �ø��� �� ���ڿ��� ���.
        int second = Mathf.CeilToInt(value);             // value���� int������ �ø�.
        TimeSpan span = TimeSpan.FromSeconds(second);    // �ش� ���� �ð� ����ü�� ����.
        limitText.text = string.Format("{0}:{1}", span.Minutes, span.Seconds);  // ���ڿ��� ����.
    }
}
