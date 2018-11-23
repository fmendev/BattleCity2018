using UnityEngine;
using System.Collections;

public class CanvasToWorldSpacePositioner : MonoBehaviour
{
    private static CanvasToWorldSpacePositioner singletonInstance;

    void Awake()
    {
        InitializeSingleton();
    }

    private void InitializeSingleton()
    {
        singletonInstance = this;
    }

    public static void TranslateCanvasToWorldPosition(RectTransform UI_Element, GameObject worldObject)
    {
        RectTransform canvasRect = singletonInstance.GetComponent<RectTransform>();

        //0,0 for the canvas is at the center of the screen, whereas WorldToViewPortPoint treats the lower left corner as 0,0.
        //Because of this, you need to subtract the height / width of the canvas * 0.5f to get the correct position.

        Vector2 viewportPosition = Camera.main.WorldToViewportPoint(worldObject.transform.position);
        Vector2 worldObjectScreenPosition = new Vector2(
            ((viewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
            ((viewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));

        //now you can set the position of the ui element
        Instantiate(UI_Element, singletonInstance.transform);
        UI_Element.anchoredPosition = worldObjectScreenPosition;
    }
}

