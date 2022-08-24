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
            // ����Ʈ�� ������ ���ٸ� ��������Ʈ �迭�� �߰��Ѵ�.
            if (spriteList.Count <= 0)
                spriteList.AddRange(sprites);

            // ����Ʈ�� ����߿��� ������ ��Ҹ� �ϳ� �̾ƿ´�.
            int index = Random.Range(0, spriteList.Count);
            Sprite sprite = spriteList[index];
            spriteList.RemoveAt(index);

            image.sprite = sprite;
            yield return wait;
        }
    }
}
