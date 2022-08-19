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
        async.allowSceneActivation = false;     // �ε� �Ϸ�� �ڵ� Ȱ��ȭ.

        while(!async.isDone)
        {
            // �ڵ�Ȱ��ȭ�� ���� ������ �ִ� 90%���� �ö󰣴�.
            if(async.progress >= 0.9f)
            {
                // 90%���� �ε尡 �Ϸ�Ǿ����� Ȱ��ȭ�� �Ҵ�.
                async.allowSceneActivation = true;                
            }

            progressImage.fillAmount = async.progress / 0.9f;
            progressText.text = (async.progress * 100f).ToString();

            yield return null;
        }

        Debug.Log("�Ϸ�!!");
    }
}
