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

    // Start�ڷ�ƾ �̺�Ʈ �Լ�.
    IEnumerator Start()
    {
        // ���� length�� time�� �Ҽ��� ���� ������ ������ �Ѵ�.
        float totalLength = Mathf.Floor((float)video.length);
        skipPanel.SetActive(false);

        while (video.time < totalLength)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // Gameobject.activeSelf : bool
                //  => ���� ������Ʈ�� �����ִ���?
                skipPanel.SetActive(!skipPanel.activeSelf);     // �Ѱ�,����.

                // ���� ���߰� ����ϱ�.
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
