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
    public UnityEvent OnLogicEnded;
    public UnityEvent OnLogicStarted;

    readonly Queue<LogicGroup> logicGroups = new Queue<LogicGroup>();
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

        logicGroups.Clear();
        IsPlayingLogic = false;

        OnLogicEnded.RemoveAllListeners();
        OnLogicStarted.RemoveAllListeners();
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

    public LogicGroup BroadcastLogic(GameEventEnum gameEventEnum, LogicGroupArgs args = null)
    {
        var newLogicGroup = AppendLogic(gameEventEnum);

        if (args == null)
        {
            args = new LogicGroupArgs();
        }

        args.LogicGroup = newLogicGroup;

        Broadcast(gameEventEnum, args);

        return newLogicGroup;
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

    public LogicGroup AppendLogic(GameEventEnum gameEventType)
    {
        var newLogicGroupGO = new GameObject($"LogicGroup: {gameEventType}");
        var newLogicGroup = newLogicGroupGO.AddComponent<LogicGroup>();
        DontDestroyOnLoad(newLogicGroupGO);
        newLogicGroup.GameEventChannel = this;
        newLogicGroup.GameEventType = gameEventType;        
        newLogicGroup.OnFinished.AddListener(LogicGroup_OnFinished);

#if EventDebug
        Debug.Log($"<color=yellow>▲ </color><color=#72d47f><b>Logic Group Started: {gameEventType}</b></color>");
#endif

        if (IsPlayingLogic == false)
        {
            IsPlayingLogic = true;
            newLogicGroup.IsPlaying = true;
            OnLogicStarted.Invoke();
        }
        else
        {
            logicGroups.Enqueue(newLogicGroup);
        }

        return newLogicGroup;
    }

    private void LogicGroup_OnFinished(LogicGroup group)
    {
        Destroy(group.gameObject);

#if EventDebug
        Debug.Log($"<color=yellow>▼ </color><color=#72d47f><b>Logic Group Ended: {group.GameEventType}</b></color>");
#endif

        if (logicGroups.Count > 0)
        {
            logicGroups.Dequeue().IsPlaying = true;
        }
        else
        {
            IsPlayingLogic = false;
            OnLogicEnded.Invoke();
        }
    }
}
