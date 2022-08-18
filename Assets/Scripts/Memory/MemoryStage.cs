using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class MemoryStage : MonoBehaviour
{
    [System.Serializable] public struct Memory
    {
        public int instanceID;      // ���� ID.
        public string json;         // ���� ������.

        public Memory(int instanceID, string json)
        {
            this.instanceID = instanceID;
            this.json = json;
        }
    }
    [System.Serializable] public struct SavedStage
    {
        public bool isSaved;        // �����͸� �����ߴ°�?
        public StageInfo stage;     // �������� ��ü�� ������.
        public Memory[] data;       // �������� ���� ������Ʈ���� ���� ������.
    }

    private readonly string FILE_NAME = "Temporary Stage";


    private void Start()
    {
        SavedStage stageData = new SavedStage();        // ���̺� ������ ����ü.

        try
        {
            string json = DataManager.LoadJson(FILE_NAME);
            stageData = JsonUtility.FromJson<SavedStage>(json);
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
        }

        // ���� �߰� ������ �߾���.
        if(stageData.isSaved)
        {
            Debug.Log("�߰� ���� ����!!");
            List<IMemory> targets = GetAllMemory();  // �ǵ��� ������Ʈ��.
            Memory[] memoryArray = stageData.data;   // �ǵ��� ������Ʈ���� ������.

            foreach(Memory memory in memoryArray)
            {
                int id = memory.instanceID;                     // �̹� �ݺ����� �޸� ���̵�.
                IMemory target = FindTarget(targets, id);       // ������Ʈ ����Ʈ ���ο��� ID�� ������Ʈ ã��.
                target.ModifyMemory(memory.json);               // �ش� ������Ʈ���� ���� ������ ����.
            }

            // �����͸� �� ���������
            // �÷��� ���� �� �ٽ� ���� ����.
            stageData.isSaved = false;
            DataManager.SaveJson(FILE_NAME, stageData);
        }
    }

    private List<IMemory> GetAllMemory()
    {
        // ���� ��� IMemoryŸ���� ã�ڴ�.
        List<IMemory> memories = new List<IMemory>();

        MonoBehaviour[] allObject = FindObjectsOfType<MonoBehaviour>().Distinct().ToArray();
        foreach (MonoBehaviour obj in allObject)
        {            
            // Ư�� �� ������Ʈ�� �������� ������Ʈ���� IMemory�� �������� �� �ֱ� ������.
            IMemory[] targets = obj.GetComponents<IMemory>();
            foreach(IMemory memory in targets)
            {
                memories.Add(memory);
            }
            
        }

        return memories;
    }
    private IMemory FindTarget(List<IMemory> list, int id)
    {
        return list.Where(t => t.GetID() == id).ToArray()[0];
    }

    private void OnApplicationQuit()
    {
        List<IMemory> memories = GetAllMemory();

        // ã�� �޸𸮵��� ID�� Json�� Memory ����ü�� ����� �����´�.
        Memory[] memoryArray = memories.Select(m => new Memory(m.GetID(), m.GetMemoryJson())).ToArray();
        SavedStage stageData = new SavedStage()
        {
            isSaved = true,
            data = memoryArray
        };

        DataManager.SaveJson(FILE_NAME, stageData);
        Debug.Log("������ �߰� ����!!");
    }

}
