using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Monster
{
    public int id;
    public string name;
    public int hp;
    public int power;
    public int defence;
}

public class ItemManager : MonoBehaviour
{
    [SerializeField] SheetManager sheetManager;
    [SerializeField] Item[] items;
    [SerializeField] Monster[] monsters;


    void Start()
    {
        // ��Ʈ �Ŵ������� ������ �ڷ����� �ٿ�ε� �޾ƿʹ޶�� ��û.
        sheetManager.GetDatas<Item>("item", GetSheetData);
        sheetManager.GetDatas<Monster>("monster", (download) => { monsters = download; });
    }

    private void GetSheetData(Item[] downloadDatas)
    {
        Debug.Log($"�ٿ�ε� ���� : {downloadDatas.Length}");
        items = downloadDatas;
    }
   
}
