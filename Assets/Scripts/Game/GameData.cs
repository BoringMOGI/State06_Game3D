using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Data/GameData")]
public class GameData : ScriptableObject
{
    public StageInfo.Stage currentStage;    // ���� ��������.
    public int deadCount;                   // ���� Ƚ��.
    public float limitTime;                 // ���� �ð�.
    public float remainingTime;             // ���� �ð�.

    public void Clear()
    {
        deadCount = 0;
        limitTime = 0f;
        remainingTime = 0f;
    }
}
