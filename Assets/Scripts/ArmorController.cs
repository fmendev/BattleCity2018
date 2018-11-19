using UnityEngine;
using UnityEngine.UI;

public class ArmorController : MonoBehaviour
{
    private static ArmorController singletonInstance;

    public GameObject armorBar;
    public GameObject flashBar;

    [SerializeField]
    private int currentArmor;
    [SerializeField]
    private int maxArmor;

    private int limit;
    private float alphaDelta = .04f;
    private bool flashingAdded = false;
    private bool flashingRemoved = false;
    private Color green, red, disabled, white;

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

    private void Update()
    {
        if (flashingAdded)
            limit = currentArmor;
        else if (flashingRemoved)
            limit = currentArmor + 1;

        if (flashingAdded == true || flashingRemoved == true)
        {
            for (int i = 0; i < limit; i++)
            {
                if (flashBar.transform.GetChild(i).gameObject.activeSelf)
                {
                    float alpha = flashBar.transform.GetChild(i).GetComponent<RawImage>().color.a;
                    Color flashColor = new Color(255, 255, 255, alpha - alphaDelta);
                    flashBar.transform.GetChild(i).GetComponent<RawImage>().color = flashColor;

                    if (flashColor.a <= 0)
                    {
                        flashBar.transform.GetChild(i).GetComponent<RawImage>().color = Color.white;

                        flashingAdded = flashingRemoved = false;
                        flashBar.transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
            }
        }
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
        singletonInstance.FlashRemovedBar();
        singletonInstance.UpdateArmorBar();
    }

    public static void SetArmor(int armorAmount)
    {
        int addedArmor = armorAmount - singletonInstance.currentArmor;
        singletonInstance.FlashAddedBars();
        singletonInstance.currentArmor = armorAmount;
        singletonInstance.UpdateArmorBar();
    }

    public static int GetMaxArmor()
    {
        return singletonInstance.maxArmor;
    }

    public static void IncreaseMaxArmor()
    {
        singletonInstance.maxArmor++;
        singletonInstance.UpdateArmorBar();
    }

    private void FlashAddedBars()
    {
        //For it to work with added bars, needs to be executed after maxArmor is increased, and before current armor is increased
        for (int i = currentArmor; i < singletonInstance.maxArmor; i++)
        {
            flashBar.transform.GetChild(i).gameObject.SetActive(true);
            flashBar.transform.GetChild(i).GetComponent<RawImage>().color = Color.white;
            flashingAdded = true;
        }
    }

    private void FlashRemovedBar()
    {
        //For it to work with a removed bar, needs to be executed after current armor is decreased
        flashBar.transform.GetChild(currentArmor).gameObject.SetActive(true);
        flashBar.transform.GetChild(currentArmor).GetComponent<RawImage>().color = Color.white;
        flashingRemoved = true;
    }
}

