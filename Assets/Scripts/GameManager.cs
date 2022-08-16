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

    Coroutine limitTimeCoroutine;

    void Start()
    {
        limitTimeUI.UpdateLimitTime(limitGameTime);         // UI���� ���� �� ����.
        StartCoroutine(CountDown());
    }

    private void StartGame()
    {
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
    }
    private void TimeOut()
    {
        player.OnStopPlayer();
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
            limitTimeUI.UpdateLimitTime(remaining);     // �� ���� UI�� ����.
            if (remaining <= 0.0f)                      // ���� �ð��� ���ٸ�.
            {
                remaining = 0.0f;                       // ���� ���� 0���� ����.
                limitTimeUI.UpdateLimitTime(0f);        // UI����.
                break;                                  // ī���� ����.
            }

            yield return null;
        }

        // Ÿ�Ӿƿ�!!
        TimeOut();
    }

 

}
