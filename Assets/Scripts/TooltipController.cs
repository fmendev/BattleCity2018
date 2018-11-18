using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum Tooltip { Eagle, FirstWave, Ammo, HealthBar, Lives};

public class TooltipController : MonoBehaviour
{
    private static TooltipController singletonInstance;

    public GameObject tooltip;
    public GameObject crosshair;

    public GameObject crosshairEaglePosition;
    public GameObject crosshairFirstWavePosition1;
    public GameObject crosshairFirstWavePosition2;
    public GameObject crosshairFirstWavePosition3;
    public GameObject crosshairAmmoPosition;
    public GameObject crosshairHealthBarPosition;
    public GameObject crosshairLivesPosition;
    public GameObject tooltipEaglePosition;
    public GameObject tooltipFirstWavePosition;
    public GameObject tooltipAmmoPosition;
    public GameObject tooltipHealthBarPosition;
    public GameObject tooltipLivesPosition;

    private Dictionary<Tooltip, string> tooltipMessages;
    private Dictionary<Tooltip, GameObject> tooltipPositions;
    private Dictionary<Tooltip, List<GameObject>> crosshairPositions;
    private string continueMessage;

    private GameObject activeTooltip;
    private float tooltipDelay = 1f;

    private void Awake()
    {
        InitializeSingleton();

        continueMessage = "Press [SPACE] or [CLICK] to continue...";
        tooltipMessages = new Dictionary<Tooltip, string>()
        {
            { Tooltip.Eagle, "This is the Condor, symbol of our land\nDefend at all costs!!!" },
            { Tooltip.FirstWave, "Foreign invaders, defiling our land\nDestroy them all" },
            { Tooltip.Ammo, "Ammo" },
            { Tooltip.HealthBar, "Health" },
            { Tooltip.Lives, "Lives" }
        };

        tooltipPositions = new Dictionary<Tooltip, GameObject>()
        {
            { Tooltip.Eagle, tooltipEaglePosition },
            { Tooltip.FirstWave, tooltipFirstWavePosition },
            { Tooltip.Ammo, tooltipAmmoPosition },
            { Tooltip.HealthBar, tooltipHealthBarPosition },
            { Tooltip.Lives, tooltipLivesPosition }
        };

        crosshairPositions = new Dictionary<Tooltip, List<GameObject>>
        {
            { Tooltip.Eagle, new List<GameObject>() { crosshairEaglePosition } },
            { Tooltip.FirstWave, new List<GameObject>() { crosshairFirstWavePosition1, crosshairFirstWavePosition2, crosshairFirstWavePosition3 } },
            { Tooltip.Ammo, new List<GameObject>() { crosshairAmmoPosition } },
            { Tooltip.HealthBar, new List<GameObject>() { crosshairHealthBarPosition } },
            { Tooltip.Lives, new List<GameObject>() { crosshairLivesPosition } }
        };
    }

    private void InitializeSingleton()
    {
        singletonInstance = this;
    }

    public static void PlayTooltipAnimation(Tooltip key, bool isSelfClosing)
    {
        //Shows crosshair, and after a given delay shows tooltip

        singletonInstance.ShowCrosshair(key, isSelfClosing);
        singletonInstance.StartCoroutine(singletonInstance.ShowToolTip(key, isSelfClosing));
    }

    private void ShowCrosshair(Tooltip key, bool isSelfClosing)
    {
        List<GameObject> positions = singletonInstance.crosshairPositions[key];

        foreach (var position in positions)
        {
            GameObject crosshair = Instantiate(singletonInstance.crosshair, position.transform);
            crosshair.GetComponent<CrosshairBehavior>().isSelfClosing = isSelfClosing;
        }
    }

    private IEnumerator ShowToolTip(Tooltip key, bool isSelfClosing)
    {
        yield return new WaitForSeconds(tooltipDelay);
        GameObject position = singletonInstance.tooltipPositions[key];
        singletonInstance.activeTooltip = Instantiate(singletonInstance.tooltip, position.transform);

        singletonInstance.activeTooltip.GetComponent<TooltipBehavior>().isSelfClosing = isSelfClosing;
        singletonInstance.activeTooltip.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = singletonInstance.tooltipMessages[key];
        singletonInstance.activeTooltip.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = singletonInstance.continueMessage;
    }


}
