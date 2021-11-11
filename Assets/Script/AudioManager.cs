using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// サウンド管理クラス
/// </summary>
public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] AudioSource audioBGM;
    [SerializeField] AudioSource audioSE;

    [SerializeField] AudioClip[] bgmClips;
    [SerializeField] AudioClip[] seClips;


    /// <summary>
    /// 指定した番号の効果音を再生する。
    /// </summary>
    /// <param name="namber">効果音の番号</param>
    public void PlaySE(int namber)
    {
        audioSE.PlayOneShot(seClips[namber]);
    }

    /// <summary>
    /// 指定した番号の効果音を再生する。
    /// </summary>
    /// <param name="namber">効果音の番号</param>
    public void PlayBGM(int namber)
    {
        audioSE.PlayOneShot(bgmClips[namber]);
    }

    /// <summary>
    /// 指定したclipを再生する。
    /// </summary>
    /// <param name="clip">音</param>
    public void PlayClip(AudioClip clip)
    {
        audioSE.PlayOneShot(clip);
    }

    /// <summary>
    /// 再生されている効果音を全て止める。
    /// </summary>
    public void StopSE()
    {
        audioSE.Stop();
    }

}
