using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryTimer : MonoBehaviour
{
    [SerializeField] float delayTime;

    bool isTimer = false;
    public void StartTimer()
    {
        if (isTimer)
            return;

        isTimer = true;
        StartCoroutine(Timer());
    }
    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(delayTime);
        Destroy(gameObject);
    }
}
