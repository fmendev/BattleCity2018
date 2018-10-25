using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShellDisplay : MonoBehaviour
{
    public List<GameObject> shellDisplay;
    public static List<int> isReloading;

    private Texture2D disabledShell;
    private Texture2D availableShell;

    //TODO: make it work for additional player
    private void Awake()
    {
        disabledShell = Resources.Load<Texture2D>("ImprintShell");
        availableShell = Resources.Load<Texture2D>("Shell");
    }

    private void Start()
    {
        InitializeReloadingStatus();
        UpdateAmmoDisplay();
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

    private void UpdateAmmoDisplay()
    {
            for (int i = shellDisplay.Count - 1; i > TankGun.GetCurrentMaxAmmo() - 1; i--)
            {
                shellDisplay[i].GetComponent<RawImage>().texture = disabledShell;
                shellDisplay[i].GetComponent<RawImage>().color = new Color32(75, 75, 75, 255);
            }
    }
}
