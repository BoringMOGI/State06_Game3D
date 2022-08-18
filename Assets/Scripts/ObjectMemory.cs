using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMemory
{
    int GetID();                        // 고유 ID를 반환.
    string GetMemoryJson();             // 저장해야할 데이터를 json으로 묶어서 리턴.
    void ModifyMemory(string json);     // json을 불러와 자신이 원하는 데이터에 대입.
}
