using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class MemoryStage : MonoBehaviour
{
    [System.Serializable] public struct Memory
    {
        public int instanceID;      // 고유 ID.
        public string json;         // 저장 데이터.

        public Memory(int instanceID, string json)
        {
            this.instanceID = instanceID;
            this.json = json;
        }
    }
    [System.Serializable] public struct SavedStage
    {
        public bool isSaved;        // 데이터를 저장했는가?
        public StageInfo stage;     // 스테이지 자체의 데이터.
        public Memory[] data;       // 스테이지 내부 오브젝트들의 저장 데이터.
    }

    private readonly string FILE_NAME = "Temporary Stage";


    private void Start()
    {
        SavedStage stageData = new SavedStage();        // 세이브 데이터 구조체.

        try
        {
            string json = DataManager.LoadJson(FILE_NAME);
            stageData = JsonUtility.FromJson<SavedStage>(json);
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
        }

        // 전에 중간 저장을 했었다.
        if(stageData.isSaved)
        {
            Debug.Log("중간 저장 복원!!");
            List<IMemory> targets = GetAllMemory();  // 되돌릴 오브젝트들.
            Memory[] memoryArray = stageData.data;   // 되돌릴 오브젝트들의 데이터.

            foreach(Memory memory in memoryArray)
            {
                int id = memory.instanceID;                     // 이번 반복문의 메모리 아이디.
                IMemory target = FindTarget(targets, id);       // 오브젝트 리스트 내부에서 ID인 오브젝트 찾기.
                target.ModifyMemory(memory.json);               // 해당 오브젝트에게 저장 데이터 전달.
            }

            // 데이터를 다 사용했으니
            // 플래그 수정 후 다시 파일 저장.
            stageData.isSaved = false;
            DataManager.SaveJson(FILE_NAME, stageData);
        }
    }

    private List<IMemory> GetAllMemory()
    {
        // 씬의 모든 IMemory타입을 찾겠다.
        List<IMemory> memories = new List<IMemory>();

        MonoBehaviour[] allObject = FindObjectsOfType<MonoBehaviour>().Distinct().ToArray();
        foreach (MonoBehaviour obj in allObject)
        {            
            // 특정 한 오브젝트가 여러개의 컴포턴트에서 IMemory를 구현했을 수 있기 때문에.
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

        // 찾은 메모리들의 ID와 Json을 Memory 구조체로 만들어 가져온다.
        Memory[] memoryArray = memories.Select(m => new Memory(m.GetID(), m.GetMemoryJson())).ToArray();
        SavedStage stageData = new SavedStage()
        {
            isSaved = true,
            data = memoryArray
        };

        DataManager.SaveJson(FILE_NAME, stageData);
        Debug.Log("데이터 중간 저장!!");
    }

}
