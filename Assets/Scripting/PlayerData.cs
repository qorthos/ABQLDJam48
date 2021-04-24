using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/PlayerData")]
public class PlayerData : ScriptableObject
{
    public float StartingThrottle = 100;
    public float StartingMaxThrottle = 130;
    public float StartingMinThrottle = 070;
    public float RestingThrottle = 100;
    private float throttle;
    private float maxThrottle;
    private float minThrottle;


    public FloatUnityEvent OnThrottleChanged;
    public FloatUnityEvent OnMaxThrottleChanged;
    public FloatUnityEvent OnMinThrottleChanged;

    public float Throttle { get => throttle; set { throttle = value; OnThrottleChanged.Invoke(value); } }
    public float MaxThrottle { get => maxThrottle; set { maxThrottle = value; OnMaxThrottleChanged.Invoke(value); } }
    public float MinThrottle { get => minThrottle; set { minThrottle = value; OnMinThrottleChanged.Invoke(value); } }

    private void OnEnable()
    {
        Throttle = StartingThrottle;
        MaxThrottle = StartingMaxThrottle;
        MinThrottle = StartingMinThrottle;
    }

    private void OnDisable()
    {
        Throttle = StartingThrottle;
        MaxThrottle = StartingMaxThrottle;
        MinThrottle = StartingMinThrottle;
    }

}
