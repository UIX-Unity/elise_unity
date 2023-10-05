using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


public class ResourceManager : BaseSingleton<ResourceManager>, IBaseSingleton
{
    #region < Path >

    public static readonly string GameObjectCollectionPath = "_PrefabCollection";
    public static readonly string AudioClipCollectionPath = "_AudioClipCollection";
    public static readonly string SpriteCollectionPath = "_SpriteCollection";
    public static readonly string MaterialCollectionPath = "_MaterialCollection";
    public static readonly string GenericCollectionPath = "_GenericCollection";

    #endregion

    private AudioResourceStorage m_AudioResourceStorage = new AudioResourceStorage();
    public static AudioResourceStorage audioResourceStorage => Instance.m_AudioResourceStorage;

    private SpriteResourceStorage m_SpriteResourceStorage = new SpriteResourceStorage();
    public static SpriteResourceStorage spriteResourceStorage => Instance.m_SpriteResourceStorage;

    private MaterialResourceStorage m_MaterialResourceStorage = new MaterialResourceStorage();
    public static MaterialResourceStorage materialResourceStorage => Instance.m_MaterialResourceStorage;

    public void OnCreateInstance()
    {

    }

    public void OnDestroyInstance()
    {
        Release();
    }

    public void LoadDatas(ESceneLocate locate)
    {

    }

    public void UnloadDatas()
    {

    }

    public string GetLog()
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("#### Resource Manager ####\r");
        builder.AppendLine($"{audioResourceStorage.GetLog()}");
        builder.AppendLine($"{spriteResourceStorage.GetLog()}");
        builder.AppendLine($"{materialResourceStorage.GetLog()}");
        builder.AppendLine("\n######################");
        return builder.ToString();
    }

    public static void Release()
    {
        Instance.m_AudioResourceStorage.Release();
    }
}
