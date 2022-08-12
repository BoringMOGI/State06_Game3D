using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 게임의 시작과 끝, 클리어, 실패여부를 관리한다.
    [SerializeField] PlayerMovement player;
    [SerializeField] Countdown countdown;
    [SerializeField] int countTime;

    void Start()
    {
        StartCoroutine(StartProcess());
    }

    IEnumerator StartProcess()
    {
        player.SwitchControl(false);

        yield return new WaitForSeconds(2);

        // 카운트 다운 시작.
        bool isEndCouting = false;
        countdown.OnStartCount(countTime, () => { isEndCouting = true; });
        while (!isEndCouting)
            yield return null;

        Debug.Log("게임매니저 : 카운트 다운 종료!");
        countdown.OnShowString("GO!");
        player.SwitchControl(true);
    }

 

}
