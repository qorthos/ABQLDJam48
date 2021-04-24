using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelIndicator : MonoBehaviour
{
    public Image FillImage;
    public PlayerData PlayerData;


    // Update is called once per frame
    void Update()
    {
        FillImage.fillAmount = PlayerData.Fuel / PlayerData.MaxFuel;
        //FillImage.color
    }
}
