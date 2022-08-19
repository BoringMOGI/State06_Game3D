using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShowSelectUI : MonoBehaviour
{
    [SerializeField] Text titleText;
    [SerializeField] Text tipText;
    [SerializeField] Image stageImage;

    [SerializeField] float showTime;

    public void Show(StageInfo.Stage info)
    {
        gameObject.SetActive(true);

        titleText.text = info.stageName;
        tipText.text = info.stageTip;
        stageImage.sprite = info.sprite;

        StartCoroutine(Ready(info));
    }

    IEnumerator Ready(StageInfo.Stage info)
    {
        yield return new WaitForSeconds(showTime);
        SceneMover.Instance.LoadScene(info.sceneName);
    }
}
