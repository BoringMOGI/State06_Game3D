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
        // 시트 매니저에게 아이템 자료형을 다운로드 받아와달라고 요청.
        sheetManager.GetDatas<Item>("item", GetSheetData);
        sheetManager.GetDatas<Monster>("monster", (download) => { monsters = download; });
    }

    private void GetSheetData(Item[] downloadDatas)
    {
        Debug.Log($"다운로드 성공 : {downloadDatas.Length}");
        items = downloadDatas;
    }
   
}
