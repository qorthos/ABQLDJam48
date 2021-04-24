using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadIndicator : MonoBehaviour
{
    public Image FillImage;
    public PlayerData PlayerData;


    // Update is called once per frame
    void Update()
    {
        FillImage.fillAmount = PlayerData.ConnectedObjectsCount / 20f;
    }
}
