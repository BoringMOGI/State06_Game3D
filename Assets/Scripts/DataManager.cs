using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

public static class DataManager
{
    [System.Serializable] public struct StageData
    {
        public string stageName;
        public int clearCount;
    }

    [System.Serializable] public struct SaveData
    {
        public string playerName;
        public int playerLevel;
        public int playerExp;
        public StageData[] stageDatas;
    }

    private static string GetPath(string fileName)
    {
        // Application.dataPath : 유니티 프로젝트 경로를 기반.
        // Application.persistentDataPath : AppData 경로를 기반. (모바일은 이쪽을 사용)
        return string.Format("{0}/{1}.txt", Application.persistentDataPath, fileName);
    }
    public static void SaveJson(string fileName, object obj)
    {
        // using : 끄거나 닫아야하는 객체들을 자동으로 관리해준다.
        using (FileStream stream = new FileStream(GetPath(fileName), FileMode.OpenOrCreate))
        using (StreamWriter writer = new StreamWriter(stream))
        {
            writer.Write(JsonUtility.ToJson(obj));
            Debug.Log("데이터 저장 완료");
        }
    }
    public static string LoadJson(string fileName)
    {
        using (FileStream stream = new FileStream(GetPath(fileName), FileMode.OpenOrCreate))
        using (StreamReader reader = new StreamReader(stream))
        {
            string read = reader.ReadToEnd();
            return read;
        }
    }
}

 