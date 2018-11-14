using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TowerController : MonoBehaviour {

    private static TowerController singletonInstance;

    public GameObject moneyDisplay;
    public GameObject towerAvailableDisplay;

    private int currentMoney = 0;
    private int availableTowers = 3;

	private void Start ()
    {
        InitializeSingleton();
        moneyDisplay.GetComponent<TextMeshProUGUI>().text = currentMoney.ToString();
        towerAvailableDisplay.GetComponent<TextMeshProUGUI>().text = availableTowers.ToString();
    }

    private void InitializeSingleton()
    {
        singletonInstance = this;
    }

    private void UpdateMoneyDisplay()
    {
        moneyDisplay.GetComponent<TextMeshProUGUI>().text = currentMoney.ToString();
    }

    public static void AddMoney(int moneyToAdd)
    {
        singletonInstance.currentMoney += moneyToAdd;
        singletonInstance.UpdateMoneyDisplay();
    }
}
