using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // ������ ���۰� ��, Ŭ����, ���п��θ� �����Ѵ�.
    [SerializeField] PlayerMovement player;         // �÷��̾�.
    [SerializeField] LimitTimeUI limitTimeUI;       // ���ѽð� UI.
    [SerializeField] Countdown countdown;           // ī��Ʈ �ٿ� ���� Ŭ����.
    [SerializeField] int countTime;                 // ���� ���� �� ī��Ʈ �ٿ�.
    [SerializeField] float limitGameTime;           // ���ѽð�.
    [SerializeField] GameData gameData;             // ���� ������.

    Coroutine limitTimeCoroutine;

    void Start()
    {
        limitTimeUI.UpdateLimitTime(limitGameTime);         // UI���� ���� �� ����.
        StartCoroutine(CountDown());
    }

    private void StartGame()
    {
        // ���� ������ �ʱ�ȭ.
        gameData.limitTime = limitGameTime;
        gameData.remainingTime = limitGameTime;
        gameData.deadCount = 0;

        // ���� ���� ����.
        countdown.OnShowString("GO!");                      // Go��� ���� ���.
        player.SwitchControl(true);                         // �÷��̾��� ���� ����.
        limitTimeCoroutine = StartCoroutine(LimitTime());   // ���ѽð� Ÿ�̸� ����.
    }
    public void ClearGame()
    {
        StopCoroutine(limitTimeCoroutine);      // �ڷ�ƾ ���߱�.
        countdown.OnShowString("����");          // ���� �޽��� ���.

        // �÷��̾� ������, ����.
        player.OnSwitchModel(false);
        player.OnStopPlayer();

        Debug.Log("���� Ŭ����");
        StartCoroutine(ShowResult());
    }
    private void TimeOut()
    {
        countdown.OnShowString("TIME OUT");          // �޽��� ���.
        player.OnStopPlayer();

        Debug.Log("�ð� �ʰ�!!");
        StartCoroutine(ShowResult());
    }

    IEnumerator ShowResult()
    {  
        yield return new WaitForSeconds(2f);
        SceneMover.Instance.LoadSceneForce("Result");
    }
    IEnumerator CountDown()
    {
        player.SwitchControl(false);
        yield return new WaitForSeconds(2);

        // ī��Ʈ �ٿ� ����.
        bool isEndCouting = false;
        countdown.OnStartCount(countTime, () => { isEndCouting = true; });
        while (!isEndCouting)
            yield return null;

        StartGame();
    }
    IEnumerator LimitTime()
    {
        float remaining = limitGameTime;                // ���� �ð��� ���� �ð� ����.
        while(true)
        {
            remaining -= Time.deltaTime;                // �� ������ ������ ����(deltaTime)��ŭ ����.
            gameData.remainingTime = remaining;

            limitTimeUI.UpdateLimitTime(remaining);     // �� ���� UI�� ����.
            if (remaining <= 0.0f)                      // ���� �ð��� ���ٸ�.
            {
                remaining = 0.0f;                       // ���� ���� 0���� ����.
                gameData.remainingTime = remaining;

                limitTimeUI.UpdateLimitTime(0f);        // UI����.
                break;                                  // ī���� ����.
            }

            yield return null;
        }

        // Ÿ�Ӿƿ�!!
        TimeOut();
    }

 

}
