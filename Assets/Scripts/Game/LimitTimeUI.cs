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
        // value값을 올림한 후 문자열로 출력.
        int second = Mathf.CeilToInt(value);             // value값을 int값으로 올림.
        TimeSpan span = TimeSpan.FromSeconds(second);    // 해당 값을 시간 구조체로 생성.
        limitText.text = string.Format("{0}:{1}", span.Minutes, span.Seconds);  // 문자열로 변경.
    }
}
