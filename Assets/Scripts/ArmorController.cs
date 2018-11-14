using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmorController : MonoBehaviour
{
    private static ArmorController singletonInstance;

    public GameObject armorBar;

    [SerializeField]
    private int currentArmor;
    [SerializeField]
    private int maxArmor;

    private Color green, red, disabled;

    private void Awake ()
    {
        InitializeSingleton();

        currentArmor = maxArmor = 1;
        green = new Color32(76, 255, 0, 255);
        red = new Color32(127, 0, 0, 255);
        disabled = new Color32(0, 0, 0, 128);
	}

    private void Start()
    {
        UpdateArmorBar();
    }

    private void UpdateArmorBar()
    {
        for (int i = 0; i < currentArmor; i++)
        {
            Color c = armorBar.transform.GetChild(i).GetComponent<RawImage>().color;
            armorBar.transform.GetChild(i).GetComponent<RawImage>().color = green; 
        }

        for (int i = currentArmor; i < maxArmor; i++)
        {
            armorBar.transform.GetChild(i).GetComponent<RawImage>().color = red;
        }

        for (int i = 3; i >= maxArmor; i--)
        {
            armorBar.transform.GetChild(i).GetComponent<RawImage>().color = disabled;
        }
    }

    private void InitializeSingleton()
    {
        singletonInstance = this;
    }

    public static int GetArmor()
    {
        return singletonInstance.currentArmor;
    }

    public static void IncreaseArmor()
    {
        singletonInstance.currentArmor++;
        singletonInstance.UpdateArmorBar();
    }

    public static void TakeDamage()
    {
        singletonInstance.currentArmor--;
        singletonInstance.UpdateArmorBar();
    }

    public static void SetArmor(int armorAmount)
    {
        singletonInstance.currentArmor = armorAmount;
        singletonInstance.UpdateArmorBar();
    }

    public static int GetMaxArmor()
    {
        return singletonInstance.maxArmor;
    }

    public static void IncreaseMaxArmor()
    {
        singletonInstance.currentArmor++;
        singletonInstance.UpdateArmorBar();
    }
}
