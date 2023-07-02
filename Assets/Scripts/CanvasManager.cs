using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] GameObject colorSelector;
    [SerializeField] GameObject[] playerColors, gunColors;

    
    public void SetColorSelectoractivity(bool activity)
    {
        colorSelector.SetActive(activity);
    }
    public void SetPlayerColor(int siblingIndex)
    {
        for (int i = 0; i < playerColors.Length; i++)
            playerColors[i].GetComponent<Outline>().enabled = false;
        playerColors[siblingIndex].GetComponent<Outline>().enabled = true;
       
        MyPlayer.LocalPlayerInstance.GetComponent<MyPlayer>().ChangeMyColor((PlayerColor)siblingIndex);
        
    }
    public void SetGunColor(int siblingIndex)
    {
        for (int i = 0; i < gunColors.Length; i++)
            gunColors[i].GetComponent<Outline>().enabled = false;
        gunColors[siblingIndex].GetComponent<Outline>().enabled = true;

        MyPlayer.LocalPlayerInstance.GetComponent<MyPlayer>().ChangeMyGunColor((GunColor)siblingIndex);
    }
    public void StartButton()
    {
        MyPlayer.LocalPlayerInstance.GetComponent<MyPlayer>().OnStart();
        MyPlayer.LocalPlayerInstance.GetComponent<InputManager>().OnStart();
        SetColorSelectoractivity(false);
    }
}
