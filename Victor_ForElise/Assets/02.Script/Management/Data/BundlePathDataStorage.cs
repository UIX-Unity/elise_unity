using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BundlePathDataStorage : StorageBase
{
    private Dictionary<ESceneLocate, string> m_BundlePathData = new Dictionary<ESceneLocate, string>();
    public Dictionary<ESceneLocate, string>.KeyCollection GetBundlePathDataKeys => m_BundlePathData.Keys;
    public Dictionary<ESceneLocate, string>.ValueCollection GetBundlePathDataValues => m_BundlePathData.Values;

    public bool TryGetBundlePath(ESceneLocate locate, out string data)
    {
        return m_BundlePathData.TryGetValue(locate, out data);
    }

    public override void LoadData()
    {
    }

    public override void Release()
    {
        throw new System.NotImplementedException();
    }
    public override string GetLog()
    {
        throw new System.NotImplementedException();
    }

}
