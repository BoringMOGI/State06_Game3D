using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    public static string sceneName;
    
    [SerializeField] Image progressImage;
    [SerializeField] Text progressText;

    IEnumerator Start()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;     // 로드 완료시 자동 활성화.

        while(!async.isDone)
        {
            // 자동활성화를 껐기 때문에 최대 90%까지 올라간다.
            if(async.progress >= 0.9f)
            {
                // 90%까지 로드가 완료되었으면 활성화를 켠다.
                async.allowSceneActivation = true;                
            }

            progressImage.fillAmount = async.progress / 0.9f;
            progressText.text = (async.progress * 100f).ToString();

            yield return null;
        }

        Debug.Log("완료!!");
    }
}
