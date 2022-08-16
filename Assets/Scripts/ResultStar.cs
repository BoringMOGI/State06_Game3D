using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultStar : MonoBehaviour
{
    [SerializeField] Image[] starImages;        // 별 이미지는 3개로 고정.


    private void Start()
    {
        foreach (var image in starImages)
            image.gameObject.SetActive(false);
    }

    System.Action callback;

    public void Show(GameData data, System.Action callback)
    {
        this.callback = callback;
        int getStar = 0;

        // 1. 게임 클리어 시.
        if (data.remainingTime > 0f)
            getStar += 1;

        // 2. 죽은 횟수 5회 이하.
        if (data.deadCount <= 5)
            getStar += 1;

        // 3. 남은 시간이 50% 이상.
        if (data.remainingTime / data.limitTime >= 0.5f)
            getStar += 1;

        StartCoroutine(ShowStar(getStar));
    }

    IEnumerator ShowStar(int getStar)
    {
        for (int i = 0; i < starImages.Length; i++)
        {
            if(i < getStar)
                starImages[i].gameObject.SetActive(true);

            yield return new WaitForSeconds(0.3f);
        }

        callback?.Invoke();
    }
}
