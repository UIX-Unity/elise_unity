using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using static UserInfo;

[System.Serializable]
public class UserData
{
    public int Level;
    public int CurExp;
    public int Money;
    public int Cash;
    public ETutorialFlag TutorialFlag = ETutorialFlag.None;
}

public enum ETutorialFlag
{
    None = 0,   // 0000
    Test = 1,   // 0001
    Test2 = 2,  // 0010
    Test4 = 4,  // 0100
}

public abstract class InventoryBase<T> where T : class, new()
{
    public InventoryBase(Func<T> bindGetRefDataFunc)
    {
        GetRefData = bindGetRefDataFunc;
    }

    protected Func<T> GetRefData;
    protected T refData => GetRefData();
    public abstract string GetLog();
}

public class UserDataInventory : InventoryBase<UserData>
{
    public UserDataInventory(Func<UserData> bindGetRefDataFunc) : base(bindGetRefDataFunc)
    {

    }

    #region < Lv & Exp >

    public int CurLevel { get => refData.Level; set => refData.Level = value; }
    public int CurExp { get => refData.CurExp; set => refData.CurExp = value; }

    /// <summary>
    /// 이 값은 서버도 같이 처리해줘야함.
    /// </summary>
    public int MaxExp => 100 + (CurLevel * 100);
    public void AddExp(int addExp)
    {
        CurExp += addExp;

        while (CurExp >= MaxExp)
        {
            CurExp -= MaxExp;
            ++CurLevel;
        }
    }

    #endregion

    #region < Money & Cash >

    public int Money { get => refData.Money; set => refData.Money = value; }
    public void AddMoney(int amount)
    {
        Money += amount;
    }

    #endregion

    public override string GetLog()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("#### UserDataInventory Log ####");
        sb.AppendLine($"# Lv.{CurLevel} Exp {CurExp}");
        sb.AppendLine($"# Money({Money})");
        return sb.ToString();
    }
}