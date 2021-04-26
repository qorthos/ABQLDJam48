using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{
    public PlayerData PlayerData;
    public TextMeshProUGUI Text1;
    public TextMeshProUGUI Text2;
    public TextMeshProUGUI Text3;

    // Start is called before the first frame update
    void Start()
    {
        Text2.text = $"You collected {PlayerData.CrystalsCollected} crystals";
        Text3.text = $"and saved {PlayerData.DwarvesSaved} of {PlayerData.TotalDwarves} dwarves!";
    }


}
