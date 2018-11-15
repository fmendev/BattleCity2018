using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum Tooltip { Eagle, Enemy, Ammo, HealthBar, Lives};

public class TooltipController : MonoBehaviour
{
    private static TooltipController singletonInstance;

    public GameObject tooltip;
    public GameObject crosshair;

    public GameObject crosshairEaglePosition;
    public GameObject crosshairEnemyPosition;
    public GameObject crosshairAmmoPosition;
    public GameObject crosshairHealthBarPosition;
    public GameObject crosshairLivesPosition;
    public GameObject tooltipEaglePosition;
    public GameObject tooltipEnemyPosition;
    public GameObject tooltipAmmoPosition;
    public GameObject tooltipHealthBarPosition;
    public GameObject tooltipLivesPosition;

    private Dictionary<Tooltip, string> tooltipMessages;
    private Dictionary<Tooltip, GameObject> tooltipPositions;
    private Dictionary<Tooltip, GameObject> crosshairPositions;
    private string continueMessage;

    private GameObject activeTooltip;
    private float tooltipDelay = 1f;

    private void Awake()
    {
        InitializeSingleton();

        continueMessage = "Press [SPACE] or [CLICK] to continue...";
        tooltipMessages = new Dictionary<Tooltip, string>()
        {
            { Tooltip.Eagle, "This is the Condor, symbol of our land\nDefend at all costs!!!" }
        };

        tooltipPositions = new Dictionary<Tooltip, GameObject>()
        {
            { Tooltip.Eagle, tooltipEaglePosition }
        };

        crosshairPositions = new Dictionary<Tooltip, GameObject>()
        {
            { Tooltip.Eagle, crosshairEaglePosition }
        };
    }

    private void InitializeSingleton()
    {
        singletonInstance = this;
    }

    public static void PlayTooltipAnimation(Tooltip key)
    {
        singletonInstance.ShowCrosshair(key);
        singletonInstance.StartCoroutine(singletonInstance.ShowToolTip(key));
    }

    private void ShowCrosshair(Tooltip key)
    {
        GameObject position = singletonInstance.crosshairPositions[key];
        singletonInstance.activeTooltip = Instantiate(singletonInstance.crosshair, position.transform);
    }

    private IEnumerator ShowToolTip(Tooltip key)
    {
        yield return new WaitForSeconds(tooltipDelay);
        GameObject position = singletonInstance.tooltipPositions[key];
        singletonInstance.activeTooltip = Instantiate(singletonInstance.tooltip, position.transform);
        
        singletonInstance.activeTooltip.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = singletonInstance.tooltipMessages[key];
        singletonInstance.activeTooltip.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = singletonInstance.continueMessage;
    }


}
