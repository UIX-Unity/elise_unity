using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioClipCollection", menuName = "Collection/AudioClipCollection")]
public class AudioClipCollection : ScriptableObject {
    
    [System.Serializable]
    public class Data
    {
        public Data()
        {
            key = string.Empty;
            src = null;
        }

        public Data(string _key, AudioClip _src)
        {
            key = _key;
            src = _src;
        }

        public string key;
        public AudioClip src;

        public void SetData(Data _data)
        {
            key = _data.key;
            src = _data.src;
        }

        public void Initialize()
        {
            key = string.Empty;
            src = null;
        }
    }
    
    [SerializeField]
    public List<Data> datas = new List<Data>();

    public int Count { get { return datas.Count; } }

    /// <summary>
    /// 찾을 수 없는 데이터일 경우 DefaultData를 반환한다.
    /// </summary>
    /// <param name="_srcName"></param>
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

    public bool TryGetData(string _srcName, out Data _data)
    {
        foreach (var data in datas)
        {
            if (data.key.Equals(_srcName))
            {
                _data = data;
                return true;
            }
        }
        _data = null;
        return false;
    }

    public bool TryGetValue(string _srcName, out AudioClip _src)
    {
        foreach (var data in datas)
        {
            if (data.key.Equals(_srcName))
            {
                _src = data.src;
                return true;
            }
        }

        _src = null; 
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

    public void AddToDictionary(Dictionary<string, AudioClip> dic, bool _log = false)
    {
        foreach (var data in datas)
        {
            if (dic.ContainsKey(data.key))
            {
                Debug.LogError($"AddToDictionary : Already contain Key({data.key})");
            }
            else
            {
                dic.Add(data.key, data.src);
                if (_log) Debug.Log("AddToDictionary Key : " + data.key + " , AudioClip : " + data.src);
            }
        }
    }

    public void CopyTo(AudioClipCollection collection)
    {
        collection.datas.Clear();

        foreach (var d in datas)
        {
            collection.datas.Add(d);
        }
    }
}
