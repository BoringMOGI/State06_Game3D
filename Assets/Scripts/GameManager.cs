using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // ������ ���۰� ��, Ŭ����, ���п��θ� �����Ѵ�.
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

        // ī��Ʈ �ٿ� ����.
        bool isEndCouting = false;
        countdown.OnStartCount(countTime, () => { isEndCouting = true; });
        while (!isEndCouting)
            yield return null;

        Debug.Log("���ӸŴ��� : ī��Ʈ �ٿ� ����!");
        countdown.OnShowString("GO!");
        player.SwitchControl(true);
    }

 

}
