using System.IO;
using System.Text;
using UnityEngine;
using Encryption;
using Newtonsoft.Json;

public class DataManagement
{
    #region < Key >
    /*
    [ 암호 생성기 ] 
    https://passwordsgenerator.net/kr/
    * 특수문자 미포함 (구글 클라우드 저장할 때 에러남)
    * 16글자로 생성
    */
    public static readonly string sKey = "vXhhHVKRR967ZxEu";

    #endregion

    #region < Path >

    public static readonly string BundlePathFolder = "BundlePath";

    public static readonly string SheetDataType = "json";
    public static readonly string SheetDataFolder = "SheetDatas";
    
    public static readonly string ResourceDataFolder = "Resource";

    public static readonly string GameObjectCollectionPath = "_PrefabCollection";
    public static readonly string AudioClipCollectionPath = "_AudioClipCollection";
    public static readonly string SpriteCollectionPath = "_SpriteCollection";
    public static readonly string MaterialCollectionPath = "_MaterialCollection";
    public static readonly string GenericCollectionPath = "_GenericCollection";

    #endregion



    public static string GetEncryptJson(string data)
    {
        var encryptedJson = AESCryptor.Encrypt(data, sKey);
        return encryptedJson;
    }
    public static string GetDecryptJson(string data)
    {
        var descryptedJson = AESCryptor.Decrypt(data, sKey);
        return descryptedJson;
    }

    public static bool SaveDataToStreamingAsset_Enc(string folderName, string fileName,  string fileType, object data)
    {
        try
        {
            string pathBuild = $"{Application.streamingAssetsPath}/{folderName}";

            string convertToJson = JsonConvert.SerializeObject(data);

            Debug.Log($"SAVE({fileName}) \n\r{convertToJson}");

            string encryptedJson = GetEncryptJson(convertToJson);

            Debug.Log("SAVE(Encrypted) \n\r" + encryptedJson);


            DirectoryInfo di = new DirectoryInfo(pathBuild);

            if (di.Exists.Equals(false))
            {
                di.Create();
            }

            string fileNameBuild = $"{fileName}.{fileType}";

            FileInfo[] infos = di.GetFiles();

            int j = 1;
            bool findSameName = true;
            while (findSameName.Equals(true))
            {
                findSameName = false;

                for (int i = 0; i < infos.Length; i++)
                {
                    if (infos[i].Name.Equals(fileNameBuild))
                    {
                        fileName = $"{fileName} ({j})";
                        fileNameBuild = $"{fileName}.{fileType}";
                        findSameName = true;
                        break;
                    }
                }

                j++;
            }

            File.WriteAllText($"{pathBuild}/{fileNameBuild}", encryptedJson);

            return true;
        }
        catch
        {
            Debug.LogError("Cannot Save");
            return false;
        }
    }

    public static bool LoadDataFromStreamingAsset_Dec<T>(string folderName, string fileName, string fileType, out T data) where T : class, new()
    {
        BetterStreamingAssets.Initialize();

        string buildPath = $"{folderName}/{fileName}.{fileType}";

        data = null;

        // 복호화 시도
        try
        {
            string encryptedJson = BetterStreamingAssets.ReadAllText(buildPath);
            string decryptedJson = null;

            Debug.Log("LOAD(Encrypted) \n\r" + encryptedJson);

            decryptedJson = GetDecryptJson(encryptedJson);

            Debug.Log("LOAD(Decrypted) \n\r" + decryptedJson);

            // 복호화 성공 시
            if (!string.IsNullOrEmpty(decryptedJson))
            {
                data = JsonConvert.DeserializeObject<T>(decryptedJson);

                Debug.Log($"Data({fileName}) Decrypt Success");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Cannot Decrypt");
            return false;
        }

        return data != null;
    }

    public static bool LoadAssetBundleFromStreamingAsset(string folderName, out UnityEngine.Object[] objectFile)
    {
        BetterStreamingAssets.Initialize();

        string buildPath = $"{ResourceDataFolder}/{folderName}";

        // Init
        objectFile = null;
        string[] filePath = BetterStreamingAssets.GetFiles(buildPath);
        short stringIndex = 0;

        for(short i =0;i<filePath.Length;i++)
        {
            if(filePath[i].Contains(".").Equals(false))
            {
                stringIndex = i;
                break;
            }
        }

        try
        {
            AssetBundle bundle = BetterStreamingAssets.LoadAssetBundle(filePath[stringIndex]);

            UnityEngine.Object[] assets = bundle.LoadAllAssets();

            objectFile = assets;

            return true;
        }
        catch
        {
            Debug.LogError("File is Damaged");
            return false;
        }
    }



    public static string ByteToString(byte[] strByte) 
    { 
        string str = Encoding.Default.GetString(strByte); 
        return str; 
    }
    public static byte[] StringToByte(string str) 
    { 
        byte[] StrByte = Encoding.UTF8.GetBytes(str); 
        return StrByte; 
    }

    public static bool ConvertAssetBundle(UnityEngine.Object[] objectFile, out AudioClip audioFile, out Sprite spriteFile)
    {
        // Init
        audioFile = null;
        spriteFile = null;

        try
        {
            for (int i = 0; i < objectFile.Length; i++)
            {
                switch (objectFile[i].GetType().ToString())
                {
                    case "UnityEngine.AudioClip":
                        audioFile = objectFile[i] as AudioClip;
                        break;
                    case "UnityEngine.Sprite":
                        spriteFile = objectFile[i] as Sprite;
                        break;
                }
            }

            return true;
        }
        catch
        {
            Debug.LogError("Wrong file imported");

            return false;
        }
    }

    public static void SaveDataToLocal_Enc(string dataKey, object data)
    {
        string json = JsonConvert.SerializeObject(data);

        Debug.Log($"SAVE({dataKey}) \n\r{json}");

        string encryptedJson = GetEncryptJson(json);

        Debug.Log("SAVE(Encrypted) \n\r" + encryptedJson);

        PlayerPrefs.SetString(dataKey, encryptedJson);
        PlayerPrefs.Save();
    }

    public static bool LoadDataFromLocal_Des<T>(string dataKey, out T data) where T : class, new()
    {
        string encryptedJson = PlayerPrefs.GetString(dataKey);
        string decryptedJson = null;
        data = null;

        Debug.Log("LOAD(Encrypted) \n\r" + encryptedJson);

        // 복호화 시도
        try
        {
            decryptedJson = GetDecryptJson(encryptedJson);

            Debug.Log("LOAD(Decrypted) \n\r" + decryptedJson);
        }
        catch (System.Exception e)
        {
            decryptedJson = null;
            Debug.Log("LOAD(Decrypted) Error \n\r");
        }

        // 복호화 성공 시
        if (!string.IsNullOrEmpty(decryptedJson))
        {
            data = JsonConvert.DeserializeObject<T>(decryptedJson);


            Debug.Log($"Data({dataKey}) Decrypt Success");
        }

        return data != null;
    }
}
