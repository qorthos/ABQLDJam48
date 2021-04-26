using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public PlayerData PlayerData;
    public GameEventChannel GameEventChannel;

    public float StartTime;


    private void Start()
    {
        PlayerData.Reset();
        StartTime = Time.realtimeSinceStartup;

        GameEventChannel.Broadcast(GameEventEnum.Dialogue, new DialogueEventArgs()
        {
            Msg = "Time is money friend! Get us crystals and we'll give you time, err, I mean gas!",
        });

    }

    private void Update()
    {
        PlayerData.LevelElapsedSeconds = Time.realtimeSinceStartup - StartTime;
    }
}