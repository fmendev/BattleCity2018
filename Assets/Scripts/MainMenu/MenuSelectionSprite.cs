using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuSelectionSprite : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject selectionSprite;

    public void OnPointerEnter(PointerEventData eventData)
    {
        selectionSprite.gameObject.SetActive(true);
        gameObject.GetComponent<Outline>().enabled = true;
        SoundManager.PlaySfx(SFX.MouseOnOption);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        selectionSprite.gameObject.SetActive(false);
        gameObject.GetComponent<Outline>().enabled = false;
    }
}
