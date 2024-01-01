using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AlphaButton : MonoBehaviour, IPointerClickHandler
{
    public InformationPopup ip;
    public bool b;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (b) ip.OpenPiece();
        else ip.OpenSynerge();
    }
}
