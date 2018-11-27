using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerSelection : MonoBehaviour, IPointerDownHandler
{
    public GameObject turret;
    public GameObject towerDeployed;
    public GameObject towerParent;

    private bool towerSelected = false;
    private Vector2 mousePos;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (TowerController.GetTowerAvailability(0))
        {
            //SoundManager.PlaySfx(SFX.MouseOnOption);
            //towerSelected = true;
            //towerDeployed = Instantiate(turret, towerParent.transform);
        }
    }

    //public void OnMouseDown()
    //{
    //    if (towerSelected)
    //    {
    //        towerSelected = false;
    //        TowerController.SpendMoney(200);
    //        SoundManager.PlaySfx(SFX.MouseOnOption);
    //    }
    //}

    void Update()
    {
        if (towerSelected)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            towerDeployed.transform.position = mousePos;
        }
    }
}
