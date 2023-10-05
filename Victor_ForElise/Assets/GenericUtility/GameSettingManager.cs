using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;
using Newtonsoft.Json;

public class InAppSettings
{
    /// <summary>
    /// 배경음 On/Off 여부
    /// </summary>
    public bool isOffBGM = false;

    /// <summary>
    /// 효과음 On/Off 여부
    /// </summary>
    public bool isOffSFX = false;

    public double cameraScrollSensitivity = 10f;
    public double cameraZoomSensitivity = 10f;

}


public class GameSettingManager : BaseSingleton<GameSettingManager>, IBaseSingleton
{
    private InAppSettings inAppSettings;
    private GameSettingsScriptableObject data;

    public int TimeAdsRewardTurm;

    public void OnCreateInstance()
    {
        Load();
    }

    public void OnDestroyInstance()
    {
        Save();
    }

    public static void Load()
    {
        Instance.data = Resources.Load<GameSettingsScriptableObject>("GameSettings");

        var inAppSettingsJson = PlayerPrefs.GetString("InAppSettings");

        Instance.inAppSettings = JsonConvert.DeserializeObject<InAppSettings>(inAppSettingsJson);
        if (Instance.inAppSettings == null)
        {
            Instance.inAppSettings = new InAppSettings();
        }
    }

    public static void Save()
    {
        var inAppSettingsJson = JsonConvert.SerializeObject(Instance.inAppSettings);
        PlayerPrefs.SetString("InAppSettings", inAppSettingsJson);
        PlayerPrefs.Save();
    }

    #region < In App Settings >

    public static bool IsOffBGM
    {
        get { return Instance.inAppSettings.isOffBGM; }
        set {Instance.inAppSettings.isOffBGM = value; Instance.OnChangedBGM?.Invoke(Instance.inAppSettings.isOffBGM); }
    }

    public static bool IsOffSFX
    {
        get { return Instance.inAppSettings.isOffSFX; }
        set { Instance.inAppSettings.isOffSFX = value; Instance.OnChangedSFX?.Invoke(Instance.inAppSettings.isOffSFX); }
    }

    public static float SFXVolume => 1f;
    public static float BGMVolume => 1f;

    public UnityAction<bool> OnChangedBGM;
    public UnityAction<bool> OnChangedSFX;

    #endregion

    #region < In Editor Settings >

    public static bool createNewUser => Instance.data.createNewUser;
    public static int createNewUserGold => Instance.data.createNewUserGold;

    public static float powerUpRatioPerLevel_Unit => Instance.data.powerUpRatioPerLevel_Unit;

    public static string[] startBuildingList => Instance.data.startBuildingList;

    #endregion
}
