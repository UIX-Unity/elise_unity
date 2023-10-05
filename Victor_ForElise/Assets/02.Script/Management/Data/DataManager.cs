using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public abstract class StorageBase
{
    public abstract void LoadData();
    public abstract void Release();
    public abstract string GetLog();
}
public class DataManager : BaseSingleton<DataManager>, IBaseSingleton
{
    private SheetDataStorage m_SheetDataStorage = new SheetDataStorage();
    public static SheetDataStorage sheetDataStorage => Instance.m_SheetDataStorage;



    public void OnCreateInstance()
    {
        LoadDatas();

        Debug.Log(GetLog());
    }

    public void OnDestroyInstance()
    {
        ReleaseDatas();
    }
    public void LoadDatas()
    {
        m_SheetDataStorage.LoadData();
    }

    public void ReleaseDatas()
    {
        m_SheetDataStorage.Release();
    }
    public string GetLog()
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("#### Data Manager ####\r");
        builder.AppendLine($"{sheetDataStorage.GetLog()}");
        builder.AppendLine("\n######################");
        return builder.ToString();
    }

}
