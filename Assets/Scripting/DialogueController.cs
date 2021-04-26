using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DialogueController : MonoBehaviour
{
    public PlayerData PlayerData;
    public GameEventChannel GameEventChannel;

    public float UpTime;
    public TextMeshProUGUI Text;
    public GameObject DialoguePanel;


    private float timer = 0;

    private void Awake()
    {
        GameEventChannel.RegisterListener(GameEventEnum.Dialogue, OnDialogue);
    }

    private void OnDialogue(GameEventEnum arg0, EventArgs arg1)
    {
        var args = arg1 as DialogueEventArgs;
        DisplayMessage(args.Msg);
    }

    public void DisplayMessage(string msg)
    {
        timer = 0;
        Text.text = msg;
        UpTime = 2 + msg.Length * 0.05f;
        Text.maxVisibleCharacters = 0;
        DialoguePanel.SetActive(true);
    }

    private void Update()
    {
        timer += Time.deltaTime;

        Text.maxVisibleCharacters = (int)(Text.text.Length * timer / (UpTime - 2f));

        if (timer >= UpTime)
        {
            DialoguePanel.SetActive(false);
        }

    }


}
