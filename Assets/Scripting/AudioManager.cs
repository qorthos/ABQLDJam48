using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public GameEventChannel GameEventChannel;

    public GameObject GlobalAudioPrefab;
    public GameObject LocalAudioPrefab;
    public GameObject VoiceAudioPrefab;

    public AudioSource MusicSource;

    private void Awake()
    {
        GameEventChannel.RegisterListener(GameEventEnum.PlayLocalAudio, OnPlayLocalAudio);
        GameEventChannel.RegisterListener(GameEventEnum.PlayGlobalAudio, OnPlayGlobalAudio);
        GameEventChannel.RegisterListener(GameEventEnum.PlayVoiceAudio, OnPlayVoiceAudio);
    }

    private void OnPlayVoiceAudio(GameEventEnum arg0, EventArgs arg1)
    {
        var audioArgs = arg1 as AudioEventArgs;
        var newGO = Instantiate(VoiceAudioPrefab, transform);
        newGO.transform.position = audioArgs.Position;
        var newAS = newGO.GetComponent<AudioSource>();
        newAS.clip = audioArgs.AudioClip;
        newAS.Play();

    }

    private void OnPlayGlobalAudio(GameEventEnum arg0, EventArgs arg1)
    {
        var audioArgs = arg1 as AudioEventArgs;
        var newGO = Instantiate(GlobalAudioPrefab, transform);
        newGO.transform.position = audioArgs.Position;
        var newAS = newGO.GetComponent<AudioSource>();
        newAS.clip = audioArgs.AudioClip;
        newAS.Play();
    }

    private void OnPlayLocalAudio(GameEventEnum arg0, EventArgs arg1)
    {
        var audioArgs = arg1 as AudioEventArgs;
        var newGO = Instantiate(LocalAudioPrefab, transform);
        newGO.transform.position = audioArgs.Position;
        var newAS = newGO.GetComponent<AudioSource>();
        newAS.clip = audioArgs.AudioClip;
        newAS.Play();
    }
}
