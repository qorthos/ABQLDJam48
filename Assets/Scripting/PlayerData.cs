using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/PlayerData")]
public class PlayerData : ScriptableObject
{
    public float StartingThrottle = 100;
    public float StartingMaxThrottle = 130;
    public float StartingMinThrottle = 070;
    public float RestingThrottle = 100;

    public float StartingFuel;
    public float MaxFuel;
    public float Fuel;

    public float Damage;

    public int DwarvesSaved;
    public int CrystalsCollected;
    public float LevelElapsedSeconds;

    private float throttle;
    private float maxThrottle;
    private float minThrottle;
    private List<GameObject> connectedObjects;

    public FloatUnityEvent OnThrottleChanged;
    public FloatUnityEvent OnMaxThrottleChanged;
    public FloatUnityEvent OnMinThrottleChanged;
    public UnityEvent OnConnectedObjectAdded;
    public UnityEvent OnConnectedObjectRemove;

    public float Throttle { get => throttle; set { throttle = value; OnThrottleChanged.Invoke(value); } }
    public float MaxThrottle { get => maxThrottle; set { maxThrottle = value; OnMaxThrottleChanged.Invoke(value); } }
    public float MinThrottle { get => minThrottle; set { minThrottle = value; OnMinThrottleChanged.Invoke(value); } }
    public int ConnectedObjectsCount { get => connectedObjects.Count; }


    private void OnEnable()
    {
        Reset();
        connectedObjects = new List<GameObject>();
    }

    private void OnDisable()
    {
        Reset();
    }

    public void Reset()
    {
        Throttle = StartingThrottle;
        MaxThrottle = StartingMaxThrottle;
        MinThrottle = StartingMinThrottle;
        Damage = 0;

        DwarvesSaved = 0;
        CrystalsCollected = 0;
        LevelElapsedSeconds = 0;

        Fuel = StartingFuel;
        connectedObjects.Clear();
    }

    public void AddConnectedObject(GameObject newObject)
    {
        connectedObjects.Add(newObject);
        OnConnectedObjectAdded.Invoke();
    }

    public void RemoveConnectedObject(GameObject oldObject)
    {
        if (connectedObjects.Contains(oldObject))
            connectedObjects.Remove(oldObject);
    }

    public bool TryPopConnectedObject(out GameObject oldObject)
    {
        if (connectedObjects.Count > 0)
        {
            oldObject = connectedObjects[ConnectedObjectsCount - 1];
            connectedObjects.Remove(oldObject);
            OnConnectedObjectRemove.Invoke();
            return true;
        }

        oldObject = null;
        return false;
    }

}
