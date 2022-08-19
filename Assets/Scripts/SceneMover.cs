using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneMover : MonoBehaviour
{
    private static SceneMover instance;
    public static SceneMover Instance => instance;

    [SerializeField] string loadSceneName;
    [SerializeField] Image blindImage;
    [SerializeField] float fadeTime;
    [SerializeField] bool isStopFadeIn;     // �ڵ� ���̵����� ���� ���ΰ�?

    // ���� ���۽� �ڵ����� ȭ���� ��� �����. (FadeIn)
    private void Awake()
    {
        instance = this;
    }
    IEnumerator Start()
    {
        if (isStopFadeIn)
        {
            blindImage.enabled = false;
        }
        else
        {
            blindImage.enabled = true;

            Color color = new Color(0f, 0f, 0f, 1f);
            blindImage.color = color;
            float time = fadeTime;

            while (time > 0.0f)
            {
                time = Mathf.Clamp(time - Time.deltaTime, 0.0f, fadeTime);
                color.a = time / fadeTime;
                blindImage.color = color;
                yield return null;
            }

            blindImage.enabled = false;
        }
    }

    public void LoadScene(string sceneName)
    {
        LoadManager.sceneName = sceneName;
        StartCoroutine(Load(fadeTime));             // ����ڰ� �ð��� ������.
    }
    public void LoadSceneForce(string sceneName)
    {
        LoadManager.sceneName = sceneName;
        StartCoroutine(Load(0f));             // ����ڰ� �ð��� ������.
    }

    IEnumerator Load(float fadeTime)
    {
        blindImage.enabled = true;

        Color color = new Color(0f, 0f, 0f, 0f);
        blindImage.color = color;
        float time = 0.0f;

        while (time < fadeTime)
        {
            time = Mathf.Clamp(time + Time.deltaTime, 0.0f, fadeTime);
            color.a = time / fadeTime;
            blindImage.color = color;
            yield return null;
        }

        SceneManager.LoadScene(loadSceneName);
    }
}