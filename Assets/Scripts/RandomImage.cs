using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

[RequireComponent(typeof(Image))]
public class RandomImage : MonoBehaviour
{
    [SerializeField] StageInfo stageInfo;
    [SerializeField] float rateTime;

    Image image;
    Sprite[] sprites;

    void Start()
    {
        sprites = stageInfo.stageInfos.Select(info => info.sprite).ToArray();
        image = GetComponent<Image>();
        StartCoroutine(ShowRandomLoop());
    }

    IEnumerator ShowRandomLoop()
    {
        WaitForSeconds wait = new WaitForSeconds(rateTime);
        List<Sprite> spriteList = new List<Sprite>();

        while (true)
        {
            // 리스트의 개수가 없다면 스프라이트 배열을 추가한다.
            if (spriteList.Count <= 0)
                spriteList.AddRange(sprites);

            // 리스트의 요소중에서 랜덤한 요소를 하나 뽑아온다.
            int index = Random.Range(0, spriteList.Count);
            Sprite sprite = spriteList[index];
            spriteList.RemoveAt(index);

            image.sprite = sprite;
            yield return wait;
        }
    }
}
