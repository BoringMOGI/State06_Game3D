using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 게임의 시작과 끝, 클리어, 실패여부를 관리한다.
    [SerializeField] PlayerMovement player;         // 플레이어.
    [SerializeField] LimitTimeUI limitTimeUI;       // 제한시간 UI.
    [SerializeField] Countdown countdown;           // 카운트 다운 관리 클래스.
    [SerializeField] int countTime;                 // 게임 시작 전 카운트 다운.
    [SerializeField] float limitGameTime;           // 제한시간.

    Coroutine limitTimeCoroutine;

    void Start()
    {
        limitTimeUI.UpdateLimitTime(limitGameTime);         // UI에게 최초 값 전달.
        StartCoroutine(CountDown());
    }

    private void StartGame()
    {
        countdown.OnShowString("GO!");                      // Go라는 문자 출력.
        player.SwitchControl(true);                         // 플레이어의 제어 동작.
        limitTimeCoroutine = StartCoroutine(LimitTime());   // 제한시간 타이머 동작.
    }
    public void ClearGame()
    {
        StopCoroutine(limitTimeCoroutine);      // 코루틴 멈추기.
        countdown.OnShowString("도착");          // 도착 메시지 출력.

        // 플레이어 가리기, 정지.
        player.OnSwitchModel(false);
        player.OnStopPlayer();

        Debug.Log("게임 클리어");
    }
    private void TimeOut()
    {
        player.OnStopPlayer();
    }

    IEnumerator CountDown()
    {
        player.SwitchControl(false);
        yield return new WaitForSeconds(2);

        // 카운트 다운 시작.
        bool isEndCouting = false;
        countdown.OnStartCount(countTime, () => { isEndCouting = true; });
        while (!isEndCouting)
            yield return null;

        StartGame();
    }
    IEnumerator LimitTime()
    {
        float remaining = limitGameTime;                // 남은 시간에 제한 시간 대입.
        while(true)
        {
            remaining -= Time.deltaTime;                // 매 프레임 프레임 간격(deltaTime)만큼 빼기.
            limitTimeUI.UpdateLimitTime(remaining);     // 뺀 값을 UI에 전달.
            if (remaining <= 0.0f)                      // 남은 시간이 없다면.
            {
                remaining = 0.0f;                       // 오차 없이 0으로 대입.
                limitTimeUI.UpdateLimitTime(0f);        // UI전달.
                break;                                  // 카운팅 종료.
            }

            yield return null;
        }

        // 타임아웃!!
        TimeOut();
    }

 

}
