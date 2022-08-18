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
        // Application.dataPath : ����Ƽ ������Ʈ ��θ� ���.
        // Application.persistentDataPath : AppData ��θ� ���. (������� ������ ���)
        return string.Format("{0}/{1}.txt", Application.persistentDataPath, fileName);
    }
    public static void SaveJson(string fileName, object obj)
    {
        // using : ���ų� �ݾƾ��ϴ� ��ü���� �ڵ����� �������ش�.
        using (FileStream stream = new FileStream(GetPath(fileName), FileMode.OpenOrCreate))
        using (StreamWriter writer = new StreamWriter(stream))
        {
            writer.Write(JsonUtility.ToJson(obj));
            Debug.Log("������ ���� �Ϸ�");
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

 