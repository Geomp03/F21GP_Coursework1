using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinnerLoot : MonoBehaviour
{
    public GameObject WinnerReward;

    // Loot for the winner
    public void Reward()
    {
        Debug.Log("Enable Winner Reward");
        WinnerReward.SetActive(true);
    }
}
