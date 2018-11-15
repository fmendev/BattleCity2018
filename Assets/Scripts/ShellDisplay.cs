using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShellDisplay : MonoBehaviour
{
    private static ShellDisplay singletonInstance;

    public List<GameObject> shellDisplay;
    public static List<int> isReloading;

    private Texture2D disabledShell;
    private Texture2D availableShell;

    //TODO: make it work for additional player
    private void Awake()
    {
        InitializeSingleton();

        disabledShell = Resources.Load<Texture2D>("ImprintShell");
        availableShell = Resources.Load<Texture2D>("Shell");
    }

    private void Start()
    {
        InitializeReloadingStatus();
        UpdateAmmoDisplay();
    }

    private void InitializeSingleton()
    {
        singletonInstance = this;
    }

    public void ReloadShell(int shellFiredIndex)
    {
        shellDisplay[shellFiredIndex].GetComponent<RawImage>().texture = disabledShell;
        shellDisplay[shellFiredIndex].GetComponent<ShellWipe>().enabled = true;
    }

    private void InitializeReloadingStatus()
    {
        isReloading = new List<int>();

        for (int i = 0; i < shellDisplay.Count; i++)
        {
            isReloading.Add(0);
        }
    }

    public static void UpdateAmmoDisplay()
    {
        for (int i = 0; i < singletonInstance.shellDisplay.Count; i++)
        {
            if (isReloading[i] == 0)
            {
                singletonInstance.shellDisplay[i].GetComponent<RawImage>().texture = singletonInstance.availableShell;
                singletonInstance.shellDisplay[i].GetComponent<RawImage>().color = new Color32(255, 255, 255, 255);
            }
        }

        for (int i = singletonInstance.shellDisplay.Count - 1; i > WeaponsController.GetCurrentMaxAmmo() - 1; i--)
        {
            singletonInstance.shellDisplay[i].GetComponent<RawImage>().texture = singletonInstance.disabledShell;
            singletonInstance.shellDisplay[i].GetComponent<RawImage>().color = new Color32(75, 75, 75, 255);
        }
    }
}
