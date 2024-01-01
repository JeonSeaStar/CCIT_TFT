using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SynergeButton : MonoBehaviour, IPointerClickHandler
{
    public PieceData.Myth myth;
    public PieceData.Animal animal;
    public PieceData.United united;
    public Image buttonImage;
    public InformationPopup ip;

    public void OnPointerClick(PointerEventData eventData)
    {
        ip.SynergeButtonClick(this);
    }
}
