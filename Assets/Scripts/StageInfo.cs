using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stage Info", menuName = "Data/Stage")]
public class StageInfo : ScriptableObject
{
    [System.Serializable]
    public struct Stage
    {
        public string sceneName;
        public string stageName;
        public string stageTip;
        public Sprite sprite;
    }

    public Stage[] stageInfos;
}

