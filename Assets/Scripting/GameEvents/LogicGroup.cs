using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class LogicGroup : MonoBehaviour
{
    public GameEventChannel GameEventChannel;
    public LogicGroupEvent OnFinished = new LogicGroupEvent();
    public bool IsPlaying = false;
    public GameEventEnum GameEventType;

    List<LogicComponent> componentsQueued = new List<LogicComponent>();
    int playingComponents = 0;

    readonly Queue<LogicGroup> logicGroups = new Queue<LogicGroup>();
    public bool IsPlayingLogic = false;

    public void AddComponent(LogicComponent component)
    {
        component.LogicGroup = this;
        componentsQueued.Add(component);
        component.OnLogicFinished.AddListener(Component_OnFinished);
    }

    private void Update()
    {
        if (IsPlaying == false)
            return;

        if ((playingComponents == 0) && (componentsQueued.Count == 0) && (logicGroups.Count == 0) && (IsPlayingLogic == false))
        {
            Debug.LogWarning($"Warning! No components were ever added to logic group: {GameEventType}!");
            OnFinished.Invoke(this);
        }

        while ((componentsQueued.Count != 0) && (IsPlayingLogic == false))
        {
            var newComponents = componentsQueued.ToArray();
            componentsQueued.Clear();

            for (int i = 0; i < newComponents.Length; i++)
            {
                newComponents[i].StartLogic(GameEventType);
                playingComponents++;
            }
        }

        if ((logicGroups.Count > 0) && (IsPlayingLogic == false) && (playingComponents == 0))
        {
            IsPlayingLogic = true;
            var newlg = logicGroups.Dequeue();
            newlg.IsPlaying = true;
            Debug.Log($"<color=yellow>▲▲</color>playing nested logic group: {newlg.GameEventType}");
        }
    }

    public LogicGroup BroadcastLogic(GameEventEnum gameEventEnum, LogicGroupArgs args = null)
    {
        var newLogicGroup = AppendLogic(gameEventEnum);

        if (args == null)
        {
            args = new LogicGroupArgs();
        }

        args.LogicGroup = newLogicGroup;

        GameEventChannel.Broadcast(gameEventEnum, args);

        return newLogicGroup;
    }

    public LogicGroup AppendLogic(GameEventEnum gameEventType)
    {
        var newLogicGroupGO = new GameObject($"LogicGroup: {gameEventType}");
        newLogicGroupGO.transform.parent = transform;
        var newLogicGroup = newLogicGroupGO.AddComponent<LogicGroup>();
        newLogicGroup.GameEventChannel = GameEventChannel;
        newLogicGroup.GameEventType = gameEventType;
        newLogicGroup.OnFinished.AddListener(LogicGroup_OnFinished);

#if EventDebug
        Debug.Log($"<color=yellow>▲ </color><color=#72d47f><b>Logic Group Started: {gameEventType}</b></color>");
#endif

        logicGroups.Enqueue(newLogicGroup);

        return newLogicGroup;
    }

    private void Component_OnFinished(LogicComponent logicComponent, GameEventEnum gameEventType)
    {
        if (gameEventType != GameEventType)
            return;

        playingComponents--;
        logicComponent.OnLogicFinished.RemoveListener(Component_OnFinished);

        if ((playingComponents == 0) && (IsPlayingLogic == false) && (componentsQueued.Count == 0) && (logicGroups.Count == 0))
        {
            OnFinished.Invoke(this);
        }
    }

    private void LogicGroup_OnFinished(LogicGroup group)
    {
        Destroy(group.gameObject);

#if EventDebug
        Debug.Log($"<color=yellow>▼ </color><color=#72d47f><b>Logic Group Ended: {group.GameEventType}</b></color>");
#endif

        IsPlayingLogic = false;

        if ((playingComponents == 0) && (logicGroups.Count == 0) && (componentsQueued.Count == 0))
        {
            OnFinished.Invoke(this);
        }
    }
}

