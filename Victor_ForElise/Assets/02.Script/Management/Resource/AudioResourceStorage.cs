using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioResourceStorage : StorageBase
{
    private Dictionary<string, AudioClip> m_AudioClips = new Dictionary<string, AudioClip>();
    public Dictionary<string, AudioClip>.KeyCollection GetAudioClipsKeys() => m_AudioClips.Keys;
    public Dictionary<string, AudioClip>.ValueCollection GetAudioClipsValues() => m_AudioClips.Values;

    public bool TryGetAudioClip(string id, out AudioClip clip)
    {
        return m_AudioClips.TryGetValue(id, out clip);
    }

    public override void LoadData()
    {
        throw new System.NotImplementedException();
    }
    public override string GetLog()
    {
        throw new System.NotImplementedException();
    }

    public override void Release()
    {
        m_AudioClips.Clear();
    }
}
