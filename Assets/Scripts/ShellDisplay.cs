using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShellDisplay : MonoBehaviour
{
    public List<GameObject> shellDisplay;

    private GameObject player1;
    private Texture2D disabledShell;
    private Texture2D availableShell;

    //TODO: make it work for additional player
    private void Awake()
    {
        player1 = GameObject.FindGameObjectWithTag("Player");
        disabledShell = Resources.Load<Texture2D>("ImprintShell");
        availableShell = Resources.Load<Texture2D>("Shell");
    }

    private void Start()
    {
        UpdateAmmoDisplay();
            }

    public void ReloadShell(int shellAmmo)
    {
        int shellFiredIndex = shellAmmo - 1;

        shellDisplay[shellFiredIndex].GetComponent<ShellWipe>().enabled = true;
    }

    private void UpdateAmmoDisplay()
    {
            for (int i = shellDisplay.Count - 1; i > GetAvailableShellCount() - 1; i--)
            {
                shellDisplay[i].GetComponent<RawImage>().texture = disabledShell;
                shellDisplay[i].GetComponent<RawImage>().color = new Color32(75, 75, 75, 255);
            }
    }

    private int GetAvailableShellCount()
    {
        return player1.GetComponent<TankGun>().shellAmmo;
    }
}
