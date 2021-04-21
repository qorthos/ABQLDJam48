using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class LogicComponent : MonoBehaviour
{
    public LogicComponentEvent OnLogicFinished = new LogicComponentEvent();
    internal LogicGroup LogicGroup;

    private void Start()
    {
        
    }

    public virtual void StartLogic(GameEventEnum gameEventType) 
    {

    }

    protected virtual void EndLogic(GameEventEnum gameEventType)
    {
        OnLogicFinished.Invoke(this, gameEventType);
    }

    private void OnDestroy()
    {
        OnLogicFinished.RemoveAllListeners();
    }

}

