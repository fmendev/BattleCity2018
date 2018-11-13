using UnityEngine;
using UnityEngine.EventSystems;

public class MenuSelectionSprite : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject selectionSprite;

    public void OnPointerEnter(PointerEventData eventData)
    {
        selectionSprite.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        selectionSprite.gameObject.SetActive(false);
    }
}
