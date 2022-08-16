using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Data/GameData")]
public class GameData : ScriptableObject
{
    public StageInfo.Stage currentStage;    // 현재 스테이지.
    public int deadCount;                   // 죽은 횟수.
    public float limitTime;                 // 제한 시간.
    public float remainingTime;             // 남은 시간.

    public void Clear()
    {
        deadCount = 0;
        limitTime = 0f;
        remainingTime = 0f;
    }
}
