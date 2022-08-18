using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMemory : MonoBehaviour, IMemory
{
    public struct TransformInfo
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
    }


    public int GetID()
    {
        int id = GetInstanceID();
        Debug.Log($"{gameObject.name}의 아이디 추출 : " + id);
        return id;
    }

    public string GetMemoryJson()
    {
        TransformInfo info = new TransformInfo()
        {
            position = transform.position,
            rotation = transform.rotation,
            scale = transform.localScale
        };

        return JsonUtility.ToJson(info, true);
    }

    public void ModifyMemory(string json)
    {
        TransformInfo t = JsonUtility.FromJson<TransformInfo>(json);
        transform.position = t.position;
        transform.rotation = t.rotation;
        transform.localScale = t.scale;
    }



   
}
