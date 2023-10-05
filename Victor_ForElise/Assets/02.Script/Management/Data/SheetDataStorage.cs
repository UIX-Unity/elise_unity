using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DataManager;

public class SheetDataStorage : StorageBase
{
    private Dictionary<string, Sheet> m_SheetData = new Dictionary<string, Sheet>();
    public Dictionary<string, Sheet>.KeyCollection GetSheetDataKeys() => m_SheetData.Keys;
    public Dictionary<string, Sheet>.ValueCollection GetSheetDataValues() => m_SheetData.Values;


    public bool TryGetSheetData(string id, out Sheet data)
    {
        return m_SheetData.TryGetValue(id, out data);
    }

    public override void LoadData()
    {
        string pathBuild = $"{Application.streamingAssetsPath}/{DataManagement.SheetDataFolder}";
        DirectoryInfo di = new DirectoryInfo(pathBuild);

        if (di.Exists.Equals(false))
        {
            di.Create();
        }

        FileInfo[] infos = di.GetFiles();

        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("##Success Load SheetDataStorage##\n\r");

        for (int i=0;i<infos.Length;i++)
        {
            string[] splitName = infos[i].Name.Split('.');

            if(splitName[splitName.Length-1].Equals("meta"))
            {
                continue;
            }
            else if (splitName.Length > 2 || splitName[1] != DataManagement.SheetDataType)
            {
                Debug.LogError($"Wrong FileType:{infos[i].Name}");
            }
            else
            {
                Sheet data;
                if(DataManagement.LoadDataFromStreamingAsset_Dec
                    (DataManagement.SheetDataFolder, splitName[0], splitName[1], out data))
                {
                    stringBuilder.AppendLine($"DataName:{infos[i].Name}");
                    stringBuilder.AppendLine($"Name:{data.Name}\r");

                    m_SheetData.Add(data.Name.ToLower(), data);
                }
                else
                {
                    Debug.LogError("Cannot Load File");
                }
            }
        }

        if(stringBuilder.Length > 1)
        {
            Debug.Log(stringBuilder.ToString());
        }
    }

    public bool SaveSheetData(string fileName, Sheet sheetData)
    {
        string buildString = $"{fileName.ToLower()}_{DataManagement.SheetDataFolder.ToLower()}";

        if (DataManagement.SaveDataToStreamingAsset_Enc
            (DataManagement.SheetDataFolder, buildString, DataManagement.SheetDataType, sheetData))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void Release()
    {
        m_SheetData.Clear();
    }

    public override string GetLog()
    {
        StringBuilder builder = new StringBuilder();

        builder.AppendLine("\n--SheetDataStorage--");
        foreach (var pair in m_SheetData)
        {
            builder.AppendLine($"\nkey:{pair.Key}\n{pair.Value.GetLog()}");
        }

        return builder.ToString();
    }

}
