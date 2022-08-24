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
        // �ߺ� üũ.
        if (isCounting)
            return;

        // �ڷ�ƾ ����.
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
        // totalTime��ŭ �ݺ�.
        while(totalTime > 0)
        {
            countText.text = totalTime.ToString();
            totalTime -= 1;

            anim.Play();
            while (anim.isPlaying)
                yield return null;
        }

        // �ٳ������� �ݹ� ȣ��.
        isCounting = false;
        callback?.Invoke();
    }

}
