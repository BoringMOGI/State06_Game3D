using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    [SerializeField] GameData gameData;         // ���� ���� ������.
    [SerializeField] Animator anim;             // ĳ���� �ִϸ�����.
    [SerializeField] ResultStar star;           // �� �̹��� ������.
    [SerializeField] Text deadCountText;        // ���� Ƚ�� �ؽ�Ʈ.
    [SerializeField] Text remainingText;        // ���� �ð� �ؽ�Ʈ.
    [SerializeField] Text resultText;           // ��� �ؽ�Ʈ.

    [SerializeField] Button goNextButton;       // Ŭ����) ���� ������ �ε�.
    [SerializeField] Button goMainButton;       // ����) ���� ������ �ε�.

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
        yield return StartCoroutine(DeadCountAnim());       // ���� Ƚ�� ī����.

        yield return new WaitForSeconds(0.3f);
        yield return StartCoroutine(RemainingAnim());       // ���� �ð� ī����.

        yield return new WaitForSeconds(0.3f);

        bool isEndStar = false;
        star.Show(gameData, () => { isEndStar = true; });   // ȹ���� �� ���.
        while (!isEndStar)
            yield return null;

        yield return new WaitForSeconds(0.4f);
        isClear = gameData.remainingTime > 0.0f;
        resultText.text = isClear ? "CLEAR" : "FAIL";       // ���� ��� �ؽ�Ʈ ���.
        resultText.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.2f);
        anim.SetTrigger(isClear ? "onWin" : "onLose");      // �ִϸ��̼� ���.
        

        yield return new WaitForSeconds(0.8f);

        if(isClear)
            goNextButton.gameObject.SetActive(true);           // Ȯ�� ��ư Ȱ��ȭ.
        else
            goMainButton.gameObject.SetActive(true);
    }

    IEnumerator DeadCountAnim()
    {
        // ���ڰ� �����Ǵ� ���� ������ �ִ� �ִϸ��̼�.
        WaitForSeconds wait = new WaitForSeconds(0.04f);

        // total�� ũ�� �����ɸ��� ������ ���ϴ� ���� add�� ��ü�� ������ ���Ѵ�.
        float total = gameData.deadCount;
        float current = 0;
        float add = total * 0.1f;

        while(current < total)
        {
            current = Mathf.Clamp(current + add, 0, total);
            deadCountText.text = string.Format("{0}ȸ", Mathf.FloorToInt(current));
            yield return wait;
        }
    }
    IEnumerator RemainingAnim()
    {
        // ���ڰ� �����Ǵ� ���� ������ �ִ� �ִϸ��̼�.
        WaitForSeconds wait = new WaitForSeconds(0.04f);

        // total�� ũ�� �����ɸ��� ������ ���ϴ� ���� add�� ��ü�� ������ ���Ѵ�.
        float total = gameData.remainingTime;
        float current = 0;
        float add = total * 0.1f;

        if(current <= 0f)
        {
            remainingText.text = string.Format("{0}/{1}��", current, total);
            yield return new WaitForSeconds(0.4f);
        }

        while (current < total)
        {
            current = Mathf.Clamp(current + add, 0, total);
            remainingText.text = string.Format("{0}/{1}��", current, total);
            yield return wait;
        }

    }

}
