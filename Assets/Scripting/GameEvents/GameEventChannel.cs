using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(fileName = "GameEventChannel", menuName = "Channels/GameEventChannel")]
public class GameEventChannel : ScriptableObject
{
    private Dictionary<GameEventEnum, BroadcastEvent> broadcastEventLib;
    public bool IsPlayingLogic = false;

    private void OnEnable()
    {
        broadcastEventLib = new Dictionary<GameEventEnum, BroadcastEvent>();
    }

    private void OnDisable()
    {
        foreach (BroadcastEvent broadcastEvent in broadcastEventLib.Values)
        {
            broadcastEvent.RemoveAllListeners();
        }

        broadcastEventLib.Clear();

        IsPlayingLogic = false;
    }

    public void Broadcast(GameEventEnum gameEventType, EventArgs args)
    {
        Debug.Log($"Broadcasting: {gameEventType}");
        if (broadcastEventLib.ContainsKey(gameEventType) == false)
        {
            broadcastEventLib.Add(gameEventType, new BroadcastEvent());
        }

        broadcastEventLib[gameEventType].Invoke(gameEventType, args);
    }

    public void RegisterListener(GameEventEnum gameEventType, UnityAction<GameEventEnum, EventArgs> callback)
    {
        if (broadcastEventLib.ContainsKey(gameEventType) == false)
        {
            broadcastEventLib.Add(gameEventType, new BroadcastEvent());
        }

        broadcastEventLib[gameEventType].AddListener(callback);
    }

    public void RemoveListener(GameEventEnum gameEventType, UnityAction<GameEventEnum, EventArgs> callback)
    {
        if (broadcastEventLib.ContainsKey(gameEventType) == false)
        {
            Debug.LogError("Attempting to remove a listener from a BroadcastEvent that never existed in the first place?!");
            return;
        }

        broadcastEventLib[gameEventType].RemoveListener(callback);
    }

}
