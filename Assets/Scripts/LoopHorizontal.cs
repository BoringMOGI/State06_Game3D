using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class LoopHorizontal : MonoBehaviour
{
    [SerializeField] float scrollSpeed;
    [SerializeField] StageInfo stageInfo;

    Sprite[] sprites;
    Image[] images;
    Vector3 origin;         // ����.
    Vector3 destination;    // ������.
    int startIndex;


    private void Start()
    {
        // �������� �������� ��������Ʈ�� �迭�� �����ϱ�.
        sprites = stageInfo.stageInfos.Select(info => info.sprite).ToArray();


        images = GetComponentsInChildren<Image>();

        float interval = images[0].rectTransform.position.x - images[1].rectTransform.position.x;

        origin = transform.position;
        destination = origin + new Vector3(interval, 0, 0);

        ChangeImage();
    }

    void ChangeImage()
    {
        if (startIndex >= sprites.Length)
            startIndex = 0;

        int start = startIndex++;


        foreach(Image image in images)
        {
            if (start >= sprites.Length)
                start = 0;

            image.sprite = sprites[start++];
        }
    }


    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, scrollSpeed * Time.deltaTime);
        if(transform.position == destination)
        {
            transform.position = origin;
            ChangeImage();
        }
    }

}
