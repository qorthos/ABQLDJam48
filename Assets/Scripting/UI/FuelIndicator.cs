using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelIndicator : MonoBehaviour
{
    public Image Image;
    public PlayerData PlayerData;

    public float MinRotation;
    public float MaxRotation;


    // Update is called once per frame
    void Update()
    {
        var pct = PlayerData.Fuel / PlayerData.MaxFuel;
        var rot = Mathf.Lerp(MinRotation, MaxRotation, pct);
        var transformRot = Image.rectTransform.rotation;
        transformRot.eulerAngles = new Vector3(0, 0, rot);
        Image.rectTransform.rotation = transformRot;
    }
}
