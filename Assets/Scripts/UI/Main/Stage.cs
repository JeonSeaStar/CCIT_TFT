using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Stage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public MapStage map;
    public EnemyInformationData data;
    public TextMeshProUGUI stageNum;
    public Image baseImage;
    public Image circleImage;
    public List<Image> enemyImages;
    public Image lockImage;
    public bool clear;

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GameManager.instance.selectedStage = data;
        SetEnemyImage();
        map.StageSeletPopupActive();
    }

    private void SetEnemyImage()
    {
        List<Sprite> sprites = new List<Sprite>();
        for(int i = 0; i < data.enemy.Count; i++)
        {
            for (int j = 0; j < data.enemy[i].enemyInformation.Count; j++)
            {
                if (!sprites.Contains(data.enemy[i].enemyInformation[j].piece.GetComponent<Piece>().pieceData.piecePortrait))
                    sprites.Add(data.enemy[i].enemyInformation[j].piece.GetComponent<Piece>().pieceData.piecePortrait);
            }
        }

        for(int i = 0; i < sprites.Count; i++)
        {
            GameObject enemyImage = Instantiate(map.enemyImage, map.StageSelectPopup.enemyImageParent);
            enemyImage.transform.GetChild(0).GetComponent<Image>().sprite = sprites[i];
            enemyImage.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = sprites[i].bounds.size * 100;
            enemyImage.name = "BG[" + i + "]";
        }
    }
}