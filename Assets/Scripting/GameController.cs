using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pixelplacement;

public class GameController : MonoBehaviour
{
    public PlayerData PlayerData;
    public GameEventChannel GameEventChannel;
    public Image BlackPanel;

    public float StartTime;


    private void Start()
    {
        PlayerData.Reset();
        StartTime = Time.realtimeSinceStartup;

        GameEventChannel.Broadcast(GameEventEnum.Dialogue, new DialogueEventArgs()
        {
            Msg = "Time is money friend! Drop off the crystals at the shaft entrance and we'll give you time, err, I mean gas!",
        });

        BlackPanel.gameObject.SetActive(true);
        Tween.Color(BlackPanel, Color.clear, 2f, 0f, completeCallback: () => BlackPanel.gameObject.SetActive(false));

        var allDwarves = FindObjectsOfType<DwarfController>();
        PlayerData.TotalDwarves = allDwarves.Length;
    }

    private void Update()
    {
        PlayerData.LevelElapsedSeconds = Time.realtimeSinceStartup - StartTime;
    }
}