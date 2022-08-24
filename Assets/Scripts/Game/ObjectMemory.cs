using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMemory
{
    int GetID();                        // ���� ID�� ��ȯ.
    string GetMemoryJson();             // �����ؾ��� �����͸� json���� ��� ����.
    void ModifyMemory(string json);     // json�� �ҷ��� �ڽ��� ���ϴ� �����Ϳ� ����.
}
