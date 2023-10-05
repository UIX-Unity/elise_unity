using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Encryption;

public class AssetBundleEncrypter : EditorWindow
{
    private static readonly string encryptKey = "2kKg9agxU84DgjXk";

    string assetBundlePath = "";
    string encryptPath = "";

    string fileName = "";

    [MenuItem("Encrypter/AssetBundle")]
    static void Init()
    {
        // 생성되어있는 윈도우를 가져온다. 없으면 새로 생성한다. 싱글턴 구조인듯하다.
        AssetBundleEncrypter window = (AssetBundleEncrypter)EditorWindow.GetWindow(typeof(AssetBundleEncrypter));
        window.Show();

        Vector2 size = new Vector2(800f, 160f);

        window.minSize = size;
    }

    void OnEnable()
    {
    }

    void OnGUI()
    {
        EditorGUI.BeginChangeCheck();
        ConvertAssetBundleLayOut();
        EditorGUI.EndChangeCheck();
    }

    public void ConvertAssetBundleLayOut()
    {
        GUILayout.Label("Encrypt Asseet Bundle", EditorStyles.boldLabel);
        GUILayout.Space(10f);

        using (var h = new EditorGUILayout.VerticalScope())
        {
            EditorGUILayout.LabelField($"AssetPath: {assetBundlePath}");
            if (GUILayout.Button("Find File", EditorStyles.miniButtonMid, GUILayout.Width(100)))
            {
                assetBundlePath = EditorUtility.OpenFilePanel("Select File", "D:/Current Project/ForElise/Victor_ForElise/AssetBundles", "");
            }
            GUILayout.Space(10f);
            EditorGUILayout.LabelField($"EncryptPath: {encryptPath}");
            if (GUILayout.Button("Find Folder", EditorStyles.miniButtonMid, GUILayout.Width(100)))
            {
                encryptPath = EditorUtility.OpenFolderPanel("Select Folder", "D:/Current Project/ForElise/Victor_ForElise", "");
            }
            GUILayout.Space(20f);
            fileName = EditorGUILayout.TextField("FileName", fileName);
            if (GUILayout.Button("Do Encrpyt", EditorStyles.miniButtonMid))
            {
                EncryptAssetBundle();
            }
            GUI.enabled = true;
        }
    }

    private void EncryptAssetBundle()
    {
        if(string.IsNullOrEmpty(assetBundlePath) || string.IsNullOrEmpty(encryptPath))
        {
            Debug.LogError("Select File and Folder Path");
            return;
        }


        byte[] readFile = File.ReadAllBytes(assetBundlePath);

        AssetBundle bundle = AssetBundle.LoadFromMemory(readFile);
        string convertString = JsonConvert.SerializeObject(bundle);

        convertString = AESCryptor.Encrypt(convertString, encryptKey);
        
        TextAsset asset = new TextAsset(convertString);

        File.WriteAllBytes($"{encryptPath}/{fileName}.bytes", asset.bytes);
    }
}
