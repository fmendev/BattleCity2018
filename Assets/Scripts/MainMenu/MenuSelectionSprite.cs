using UnityEngine;
using UnityEngine.EventSystems;

public class MenuSelectionSprite : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject selectionSprite;

    private Vector3 buttonPosition;

    float offsetX = -10;

    void Awake()
    {
        buttonPosition = gameObject.transform.position;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("In!");
        selectionSprite.gameObject.SetActive(true);
        selectionSprite.GetComponent<RectTransform>().position = new Vector3(buttonPosition.x + offsetX, buttonPosition.y);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Out!");
        selectionSprite.gameObject.SetActive(false);
    }
}
