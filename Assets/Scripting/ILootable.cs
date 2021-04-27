using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILootable
{
    bool IsConnected { get; }
    GameObject RewardPrefab { get; }
    void Disconnect();
    void Deposit();
    bool IsDepositing { get; }
}
