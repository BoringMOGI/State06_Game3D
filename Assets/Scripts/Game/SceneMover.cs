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
    [SerializeField] bool isStopFadeIn;     // 자동 페이드인을 멈출 것인가?

    // 최초 시작시 자동으로 화면을 밝게 만든다. (FadeIn)
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
        StartCoroutine(Load(fadeTime));             // 사용자가 시간을 지정함.
    }
    public void LoadSceneForce(string sceneName)
    {
        LoadManager.sceneName = sceneName;
        StartCoroutine(Load(0f));             // 사용자가 시간을 지정함.
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