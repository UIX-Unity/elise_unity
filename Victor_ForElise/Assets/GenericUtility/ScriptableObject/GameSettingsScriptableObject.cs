using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObject/GameSettings")]
public class GameSettingsScriptableObject : ScriptableObject
{
    [SerializeField]
    private bool m_CreateNewUser = false;
    public bool createNewUser
    {
        get
        {
            return m_CreateNewUser;
        }
    }

    [SerializeField]
    private int m_CreateNewUserGold = 1000;
    public int createNewUserGold
    {
        get
        {
#if UNITY_EDITOR
            return m_CreateNewUserGold;
#else 
            return 0;
#endif
        }
    }

    public float powerUpRatioPerLevel_Unit = 0.25f;

    [SerializeField]
    private string[] m_StartBuildingList;
    public string[] startBuildingList
    {
        get
        {
            return m_StartBuildingList;
        }
    }
}
