using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrottleIndicator : MonoBehaviour
{
    public Image FillImage;
    public PlayerData PlayerData;


    // Update is called once per frame
    void Update()
    {
        FillImage.fillAmount = (PlayerData.Throttle - PlayerData.MinThrottle) / (PlayerData.MaxThrottle - PlayerData.MinThrottle);
    }
}
