using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerController : MonoBehaviour {

    private static TowerController singletonInstance;

    public GameObject moneyDisplay;
    public GameObject towerAvailableDisplay;
    public GameObject towers;

    [SerializeField]
    private int currentMoney = 0;
    private int availableTowers = 3;

    private Color disabled;
    private Color available;

	private void Awake ()
    {
        InitializeSingleton();

        disabled = new Color32(0, 0, 0, 180);
        available = new Color32(0, 0, 0, 0);

        moneyDisplay.GetComponent<TextMeshProUGUI>().text = currentMoney.ToString();
        towerAvailableDisplay.GetComponent<TextMeshProUGUI>().text = availableTowers.ToString();

        UpdateTowerDisplay();
    }

    private void InitializeSingleton()
    {
        singletonInstance = this;
    }

    public static void AddMoney(int moneyToAdd)
    {
        singletonInstance.currentMoney += moneyToAdd;
        singletonInstance.UpdateMoneyDisplay();
        singletonInstance.UpdateTowerDisplay();
    }

    private void UpdateMoneyDisplay()
    {
        moneyDisplay.GetComponent<TextMeshProUGUI>().text = currentMoney.ToString();
    }

    private void UpdateTowerDisplay()
    {
        for (int i = 0; i < towers.transform.childCount; i++)
        {
            int cost = Int32.Parse(towers.transform.GetChild(i).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text);

            if (currentMoney >= cost)
            {
                towers.transform.GetChild(i).GetChild(5).gameObject.GetComponent<RawImage>().color = available;
            }
            else
            {
                towers.transform.GetChild(i).GetChild(5).gameObject.GetComponent<RawImage>().color = disabled;
            }
        }
    }
}
