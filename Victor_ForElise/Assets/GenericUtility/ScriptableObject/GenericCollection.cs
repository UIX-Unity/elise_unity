using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GenericCollection", menuName = "Collection/GenericCollection")]
public class GenericCollection : ScriptableObject {

    [System.Serializable]
    public class Data
    {
        public Data()
        {
            key = string.Empty;
            res = null;
        }

        public Data(string _key, Object _res)
        {
            key = _key;
            res = _res;
        }

        public string key;
        public Object res;

        public void SetData(Data _data)
        {
            key = _data.key;
            res = _data.res;
        }

        public void Initialize()
        {
            key = string.Empty;
            res = null;
        }
    }

    [SerializeField]
    public List<Data> datas = new List<Data>();

    /// <summary>
    /// 찾을 수 없는 데이터일 경우 DefaultData를 반환한다.
    /// </summary>
    /// <param name="_spriteName"></param>
    /// <returns></returns>
    public Data this[string _key]
    {
        get
        {
            foreach (var data in datas)
            {
                if (data.key.Equals(_key))
                {
                    return data;
                }
            }
            return null;
        }
    }

    public bool TryGetData(string _spriteName, out Data _data)
    {
        foreach (var data in datas)
        {
            if (data.key.Equals(_spriteName))
            {
                _data = data;
                return true;
            }
        }

        _data = null;
        return false;
    }

    public bool TryGetValue<T>(string _spriteName, out T _res) where T : Object
    {
        foreach (var data in datas)
        {
            if (data.key.Equals(_spriteName))
            {
                _res = data.res as T;
                return true;
            }
        }

        _res = null;
        return false;
    }

    public bool ContainsKey(string _key)
    {
        foreach (var data in datas)
        {
            if (data.key.Equals(_key))
            {
                return true;
            }
        }
        return false;
    }

    public void AddData(Data _data)
    {
        datas.Add(_data);
    }

    public void AddToDictionary<T>(Dictionary<string, T> dic) where T : Object
    {
        foreach (var data in datas)
        {
            if (dic.ContainsKey(data.key))
            {
                Debug.LogError($"AddToDictionary : Already contain Key({data.key})");
            }
            else
            {
                dic.Add(data.key, data.res as T);
            }
        }
    }

    public void CopyTo(GenericCollection collection)
    {
        collection.datas.Clear();

        foreach (var d in datas)
        {
            collection.datas.Add(d);
        }
    }
}
