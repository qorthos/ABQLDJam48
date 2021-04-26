using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

public class ToolboxController : MonoBehaviour
{
    public GameEventChannel GameEventChannel;
    public PlayerData PlayerData;
    public AudioClip CollectClip;

    public void Awake()
    {
        transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
    }

    public void Start()
    {
        Tween.LocalScale(transform, Vector3.one, 0.3f, 0f, Tween.EaseInBack);
    }

    public void Trigger()
    {
        PlayerData.Damage = Mathf.Clamp(PlayerData.Damage - 0.240f, 0, 1);

        GameEventChannel.Broadcast(GameEventEnum.PlayLocalAudio, new AudioEventArgs()
        {
            AudioClip = CollectClip,
            Position = transform.position,
        });

        Destroy(gameObject);
    }
}
