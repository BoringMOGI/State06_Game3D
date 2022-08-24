using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    [SerializeField] GameData gameData;         // 게임 진행 데이터.
    [SerializeField] Animator anim;             // 캐릭터 애니메이터.
    [SerializeField] ResultStar star;           // 별 이미지 관리자.
    [SerializeField] Text deadCountText;        // 죽은 횟수 텍스트.
    [SerializeField] Text remainingText;        // 남은 시간 텍스트.
    [SerializeField] Text resultText;           // 결과 텍스트.

    [SerializeField] Button goNextButton;       // 클리어) 다음 씬으로 로드.
    [SerializeField] Button goMainButton;       // 실패) 메인 씬으로 로드.

    bool isClear;

    void Start()
    {
        deadCountText.text = string.Empty;
        remainingText.text = string.Empty;
        resultText.text = string.Empty;

        goNextButton.gameObject.SetActive(false);
        goMainButton.gameObject.SetActive(false);

        resultText.gameObject.SetActive(false);

        StartCoroutine(Reward());
    }

    IEnumerator Reward()
    {
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(DeadCountAnim());       // 죽은 횟수 카운팅.

        yield return new WaitForSeconds(0.3f);
        yield return StartCoroutine(RemainingAnim());       // 남은 시간 카운팅.

        yield return new WaitForSeconds(0.3f);

        bool isEndStar = false;
        star.Show(gameData, () => { isEndStar = true; });   // 획득한 별 출력.
        while (!isEndStar)
            yield return null;

        yield return new WaitForSeconds(0.4f);
        isClear = gameData.remainingTime > 0.0f;
        resultText.text = isClear ? "CLEAR" : "FAIL";       // 게임 결과 텍스트 출력.
        resultText.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.2f);
        anim.SetTrigger(isClear ? "onWin" : "onLose");      // 애니메이션 재생.
        

        yield return new WaitForSeconds(0.8f);

        if(isClear)
            goNextButton.gameObject.SetActive(true);           // 확인 버튼 활성화.
        else
            goMainButton.gameObject.SetActive(true);
    }

    IEnumerator DeadCountAnim()
    {
        // 숫자가 증가되는 듯한 느낌을 주는 애니메이션.
        WaitForSeconds wait = new WaitForSeconds(0.04f);

        // total이 크면 오래걸리기 때문에 더하는 값인 add는 전체의 비율로 더한다.
        float total = gameData.deadCount;
        float current = 0;
        float add = total * 0.1f;

        while(current < total)
        {
            current = Mathf.Clamp(current + add, 0, total);
            deadCountText.text = string.Format("{0}회", Mathf.FloorToInt(current));
            yield return wait;
        }
    }
    IEnumerator RemainingAnim()
    {
        // 숫자가 증가되는 듯한 느낌을 주는 애니메이션.
        WaitForSeconds wait = new WaitForSeconds(0.04f);

        // total이 크면 오래걸리기 때문에 더하는 값인 add는 전체의 비율로 더한다.
        float total = gameData.remainingTime;
        float current = 0;
        float add = total * 0.1f;

        if(current <= 0f)
        {
            remainingText.text = string.Format("{0}/{1}초", current, total);
            yield return new WaitForSeconds(0.4f);
        }

        while (current < total)
        {
            current = Mathf.Clamp(current + add, 0, total);
            remainingText.text = string.Format("{0}/{1}초", current, total);
            yield return wait;
        }

    }

}
