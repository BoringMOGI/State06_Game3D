using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SelectRandomStage : MonoBehaviour
{
    [SerializeField] float showTime;
    [SerializeField] StageInfo stageInfo;
    [SerializeField] ShowSelectUI selectUI;
    [SerializeField] Image blindImage;

    private void Start()
    {
        blindImage.enabled = false;
        StartCoroutine(Show());
    }

    IEnumerator Show()
    {
        yield return new WaitForSeconds(showTime);

        blindImage.enabled = true;
        blindImage.color = new Color(1f, 1f, 1f, 0f);
        float time = 0f;
        float blindTime = 0.5f;

        while((time += Time.deltaTime) < blindTime)
        {
            blindImage.color = new Color(1, 1, 1, time / blindTime);
            yield return null;
        }
        blindImage.color = Color.white;

        yield return new WaitForSeconds(1f);

        blindImage.enabled = false;
        gameObject.SetActive(false);

        StageInfo.Stage stage = stageInfo.stageInfos[Random.Range(0, stageInfo.stageInfos.Length)];
        selectUI.Show(stage);
    }
}
