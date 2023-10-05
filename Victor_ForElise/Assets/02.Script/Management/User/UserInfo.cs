using System.Text;

public enum EUserDataType
{
    All = 0,
    Data = 1,
}

public class UserInfo : BaseSingleton<UserInfo>, IBaseSingleton
{
    #region < Keys >

    /*
        [ 암호 생성기 ] 
        https://passwordsgenerator.net/kr/
        * 특수문자 미포함 (구글 클라우드 저장할 때 에러남)
        * 16글자로 생성
    */
    private static readonly string USER_DATA_KEY = "yryND77ZBYJpa3Pd";

    #endregion

    #region < Datas >

    /*
        원본 Data는 무조건 Private!!
        ※ 클래스 외부에서 직접 접근할 수 없도록 관리
    */
    private UserData data = new UserData();

    #endregion

    #region < DataInventory >

    private UserDataInventory m_DataInv;
    public static UserDataInventory dataInv => Instance.m_DataInv;

    #endregion

    public void OnCreateInstance()
    {
        LoadFromLocal();

        m_DataInv = new UserDataInventory(() => data);
    }

    public void OnDestroyInstance()
    {
        SaveToLocal(EUserDataType.All);
    }

    public void LoadFromLocal()
    {
        if (GameSettingManager.createNewUser)
        {
            // USER
            data = new UserData();
            //data.Money = GameSettingManager.createNewUserGold;
        }
        else
        {
            if (!DataManagement.LoadDataFromLocal_Des(USER_DATA_KEY, out data))
            {
                data = new UserData();
            }
        }
    }

    public void SaveToLocal(params EUserDataType[] dataTypes)
    {
        foreach (var dataType in dataTypes)
        {
            switch (dataType)
            {
                case EUserDataType.Data:
                    DataManagement.SaveDataToLocal_Enc(USER_DATA_KEY, data);
                    break;
                case EUserDataType.All:
                default:
                    DataManagement.SaveDataToLocal_Enc(USER_DATA_KEY, data);
                    break;
            }
        }
    }

    public string GetLog()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("#### UserInfo Log ####");
        sb.AppendLine("\n" + dataInv.GetLog());
        return sb.ToString();
    }
}
