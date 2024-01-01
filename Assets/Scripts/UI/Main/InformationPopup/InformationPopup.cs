using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationPopup : MonoBehaviour
{
    public GameObject piece;
    public GameObject synerge;
    public GameObject panel;

    [Header("유닛 도감")]
    public GameObject pieceElementPrefab;
    public List<PieceData> pieceDataList;
    public List<PieceData> currentSynergePiece = new List<PieceData>();
    public Transform pieceElementParent;
    public List<PieceElement> pieceElementList = new List<PieceElement>();
    public List<MythData> mythDatas;
    public List<AnimalData> animalDatas;
    public List<UnitedData> unitedDatas;
    public List<SynergeButton> synergeButtons;
    public Sprite[] buttonSprites;

    public void ClosePopup()
    {
        piece.SetActive(false);
        synerge.SetActive(false);
        panel.SetActive(false);
    }

    #region 기물 도감
    private void SetPieceData(PieceData.Myth myth)
    {
        currentSynergePiece = new List<PieceData>();

        foreach(var data in pieceDataList)
        {
            if(data.myth == myth && !currentSynergePiece.Contains(data))
                currentSynergePiece.Add(data);
        }
    }

    private void SetPieceData(PieceData.Animal animal)
    {
        currentSynergePiece = new List<PieceData>();

        foreach (var data in pieceDataList)
        {
            if (data.animal == animal && !currentSynergePiece.Contains(data))
                currentSynergePiece.Add(data);
        }
    }

    private void SetPieceData(PieceData.United united)
    {
        currentSynergePiece = new List<PieceData>();

        foreach (var data in pieceDataList)
        {
            if (data.united == united && !currentSynergePiece.Contains(data))
                currentSynergePiece.Add(data);
        }
    }

    private void SelectSynerge(SynergeButton synerge)
    {
        if(synerge.myth != PieceData.Myth.None)
            SetPieceData(synerge.myth);

        if (synerge.animal != PieceData.Animal.None)
            SetPieceData(synerge.animal);

        //if (synerge.united != PieceData.United.None)
        //    SetPieceData(synerge.united);

        PieceElementInstantiate();
    }

    private void PieceElementInstantiate()
    {
        foreach (var element in pieceElementList)
            Destroy(element.gameObject);
        pieceElementList.Clear();

        for (int i = 0; i < currentSynergePiece.Count; i++)
        {
            PieceElement element = Instantiate(pieceElementPrefab, pieceElementParent).GetComponent<PieceElement>();
            element.pieceFace.sprite = currentSynergePiece[i].piecePortrait;
            element.pieceName.text = currentSynergePiece[i].pieceName;
            element.pieceHp.text = currentSynergePiece[i].health[2].ToString() + "/" + currentSynergePiece[i].health[2].ToString();
            element.pieceMp.text = currentSynergePiece[i].mana[2].ToString() + "/" + currentSynergePiece[i].mana[2].ToString();
            element.attackDamage.text = currentSynergePiece[i].attackDamage[2].ToString();
            element.abilityPower.text = currentSynergePiece[i].abilityPower[2].ToString();
            element.attackSpeed.text = currentSynergePiece[i].attackSpeed[2].ToString();
            element.attackRange.text = currentSynergePiece[i].attackRange[2].ToString();
            element.skillIcon.sprite = currentSynergePiece[i].skilSprite;
            element.skillname.text = currentSynergePiece[i].skillName;
            element.skillExplain.text = currentSynergePiece[i].skillExplain;

            if (currentSynergePiece[i].myth != PieceData.Myth.None)
            {
                foreach (var myth in mythDatas)
                {
                    if (myth.myth == currentSynergePiece[i].myth)
                    {
                        element.synerges[0].synergeIcon.sprite = myth.synergeIconSprite;
                        element.synerges[0].synergeName.text = myth.synergeName;
                        element.synerges[0].synergeInfoGameObject.SetActive(true);
                        break;
                    }
                }
            }
            else
                element.synerges[0].synergeInfoGameObject.SetActive(false);

            if (currentSynergePiece[i].animal != PieceData.Animal.None)
            {
                foreach (var animal in animalDatas)
                {
                    if (animal.animal == currentSynergePiece[i].animal)
                    {
                        element.synerges[1].synergeIcon.sprite = animal.synergeIconSprite;
                        element.synerges[1].synergeName.text = animal.synergeName;
                        element.synerges[1].synergeInfoGameObject.SetActive(true);
                        break;
                    }
                }
            }
            else
                element.synerges[1].synergeInfoGameObject.SetActive(false);

            if (currentSynergePiece[i].united != PieceData.United.None)
            {
                element.synerges[2].synergeInfoGameObject.SetActive(false);
                //foreach (var united in unitedDatas)
                //{
                //    if (united.united == target.pieceData.united)
                //    {
                //        synerges[2].synergeIcon.sprite = united.synergeIconSprite;
                //        synerges[2].synergeName.text = united.synergeName;
                //        synerges[2].synergeInfoGameObject.SetActive(true);
                //        break;
                //    }
                //}
            }
            else
                element.synerges[2].synergeInfoGameObject.SetActive(false);

            pieceElementList.Add(element);
        }
    }

    public void OpenPiece()
    {
        panel.SetActive(true);
        piece.SetActive(true);
        synerge.SetActive(false);
        SynergeButtonClick(synergeButtons[0]);
    }

    public void SynergeButtonClick(SynergeButton synergeButton)
    {
        foreach (var image in synergeButtons)
            image.buttonImage.sprite = buttonSprites[0];

        synergeButton.buttonImage.sprite = buttonSprites[1];
        SelectSynerge(synergeButton);
    }
    #endregion

    #region 시너지 도감
    public void OpenSynerge()
    {
        panel.SetActive(true);
        piece.SetActive(false);
        synerge.SetActive(true);
    }
    #endregion
}
