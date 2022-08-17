using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class OpeningManager : MonoBehaviour
{
    [SerializeField] GameObject skipPanel;
    [SerializeField] VideoPlayer video;
    [SerializeField] string nextSceneName;

    // Start코루틴 이벤트 함수.
    IEnumerator Start()
    {
        // 비디오 length와 time의 소수점 문제 때문에 내림을 한다.
        float totalLength = Mathf.Floor((float)video.length);
        skipPanel.SetActive(false);

        while (video.time < totalLength)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // Gameobject.activeSelf : bool
                //  => 게임 오브젝트가 켜져있느가?
                skipPanel.SetActive(!skipPanel.activeSelf);     // 켜고,끄기.

                // 비디오 멈추고 재생하기.
                if (video.isPlaying)
                    video.Pause();
                else
                    video.Play();
            }
            yield return null;
        }

        MoveNextScene();
    }

    public void OnSkip()
    {
        skipPanel.SetActive(false);
        MoveNextScene();
    }
    public void OnCancel()
    {
        skipPanel.SetActive(false);
        video.Play();
    }
    private void MoveNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
