using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TextCore;

public class Declare_Winner : MonoBehaviour
{
    public GameObject Win_P1;
    public GameObject Win_P2;

    // Declare winner
    public void DeclareWinner(int Winner)
    {
        Debug.Log("DeclareWinner called");
        if(Winner==1)
        {
            Win_P1.SetActive(true);
            Win_P2.SetActive(false);
        }
        else if(Winner==2)
        {
            Win_P1.SetActive(false);
            Win_P2.SetActive(true);
        }
        else
        {
            Win_P1.SetActive(false);
            Win_P2.SetActive(false);
        }
    }
}
