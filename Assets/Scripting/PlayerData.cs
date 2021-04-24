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
        Throttle = StartingThrottle;
        MaxThrottle = StartingMaxThrottle;
        MinThrottle = StartingMinThrottle;

        Fuel = StartingFuel;

        connectedObjects.Clear();
    }

    private void OnDisable()
    {
        Throttle = StartingThrottle;
        MaxThrottle = StartingMaxThrottle;
        MinThrottle = StartingMinThrottle;

        Fuel = StartingFuel;

        connectedObjects.Clear();
    }

    public void AddConnectedObject(GameObject newObject)
    {
        connectedObjects.Add(newObject);
        OnConnectedObjectAdded.Invoke();
    }

    public bool TryPopConnectedObject(out GameObject oldObject)
    {
        if (connectedObjects.Count > 0)
        {
            oldObject = connectedObjects[ConnectedObjectsCount - 1];
            OnConnectedObjectRemove.Invoke();
            return true;
        }

        oldObject = null;
        return false;
    }

}
