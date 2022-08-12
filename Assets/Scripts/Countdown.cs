using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Countdown : MonoBehaviour
{
    [SerializeField] Animation anim;
    [SerializeField] Text countText;

    bool isCounting;

    public delegate void Callback();

    private void Start()
    {
        countText.text = string.Empty;
    }

    public void OnStartCount(int totalTime, Callback callback)
    {
        // 중복 체크.
        if (isCounting)
            return;

        // 코루틴 실행.
        isCounting = true;
        StartCoroutine(OnCountDown(totalTime, callback));
    }
    public void OnShowString(string str)
    {
        countText.text = str;
        anim.Play("Show");
    }

    IEnumerator OnCountDown(int totalTime, Callback callback)
    {
        // totalTime만큼 반복.
        while(totalTime > 0)
        {
            countText.text = totalTime.ToString();
            totalTime -= 1;

            anim.Play();
            while (anim.isPlaying)
                yield return null;
        }

        // 다끝났으면 콜백 호출.
        isCounting = false;
        callback?.Invoke();
    }

}
