using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MapStage : MonoBehaviour
{
    [SerializeField] public List<Stage> stageList;
    public Sprite[] circleSprites;
    public Color lockColor = new Color(120, 120, 120);
    public RectTransform fadeGameObject;
    public List<Sprite> enemyImageList;
    public float activeSpeed;

    public StageSelect StageSelectPopup;
    public GameObject enemyImage;
    
    public void MapSwitch()
    {
        if(gameObject.activeSelf)
        {
            fadeGameObject.DOAnchorPos(new Vector3(0, 3253, 0), activeSpeed);
            Invoke("CloseMap", activeSpeed / 3);
        }
        else
        {
            fadeGameObject.DOAnchorPos(new Vector3(0, -3253, 0), activeSpeed);
            Invoke("OpenMap", activeSpeed / 3);
        }
    }

    private void OpenMap()
    {
        gameObject.SetActive(true);
        InitStage();
    }

    private void CloseMap()
    {
        gameObject.SetActive(false);
    }
    
    private void InitStage()
    {
        for(int i = 0; i < stageList.Count; i++)
        {

            stageList[i].stageNum.text = "스테이지 " + (i + 1).ToString();

            for(int j = 0; j < 3; j++)
                stageList[i].enemyImages[j].sprite = GetEnemyImage(stageList[i].data.enemyImage[j]);

            if(i == 0)
            {
                stageList[i].lockImage.enabled = false;
                for (int j = 0; j < 3; j++)
                    stageList[i].enemyImages[j].color = Color.white;
                stageList[i].circleImage.sprite = circleSprites[0];
            }

            if (i + 1 == stageList.Count)
                break;

            if (stageList[i].clear)
            {
                stageList[i + 1].lockImage.enabled = false;
                for (int j = 0; j < 3; j++)
                    stageList[i + 1].enemyImages[j].color = Color.white;
                stageList[i + 1].circleImage.sprite = circleSprites[0];
            }
            else
            {
                stageList[i + 1].lockImage.enabled = true;
                for (int j = 0; j < 3; j++)
                    stageList[i + 1].enemyImages[j].color = lockColor;
                stageList[i + 1].circleImage.sprite = circleSprites[1];
            }
        }
    }

    private Sprite GetEnemyImage(EnemyInformationData.EnemyImage enemyImage)
    {
        switch(enemyImage)
        {
            case EnemyInformationData.EnemyImage.BUD:
                return enemyImageList[0];
            case EnemyInformationData.EnemyImage.BLOOM:
                return enemyImageList[1];
            case EnemyInformationData.EnemyImage.BLOSSOM:
                return enemyImageList[2];
            case EnemyInformationData.EnemyImage.BOBM:
                return enemyImageList[3];
            case EnemyInformationData.EnemyImage.POISIONBOBM:
                return enemyImageList[4];
            case EnemyInformationData.EnemyImage.SNOWBOMB:
                return enemyImageList[5];
            case EnemyInformationData.EnemyImage.DRAGONSPARK:
                return enemyImageList[6];
            case EnemyInformationData.EnemyImage.DRAGONFIRE:
                return enemyImageList[7];
            case EnemyInformationData.EnemyImage.DRAGONINFERNO:
                return enemyImageList[8];
            case EnemyInformationData.EnemyImage.SNAKE:
                return enemyImageList[9];
            case EnemyInformationData.EnemyImage.SNAKELET:
                return enemyImageList[10];
            case EnemyInformationData.EnemyImage.SNAKENAGA:
                return enemyImageList[11];
            case EnemyInformationData.EnemyImage.SHELL:
                return enemyImageList[12];
            case EnemyInformationData.EnemyImage.SPIKE:
                return enemyImageList[13];
            case EnemyInformationData.EnemyImage.HERMITKING:
                return enemyImageList[14];
            case EnemyInformationData.EnemyImage.SUNBLOSSOM:
                return enemyImageList[15];
            case EnemyInformationData.EnemyImage.SUNFLOWER:
                return enemyImageList[16];
            case EnemyInformationData.EnemyImage.SUNFLORAPIXIE:
                return enemyImageList[17];
            case EnemyInformationData.EnemyImage.WOLFPUP:
                return enemyImageList[18];
            case EnemyInformationData.EnemyImage.WOLF:
                return enemyImageList[19];
            case EnemyInformationData.EnemyImage.WEREWOLF:
                return enemyImageList[20];
            case EnemyInformationData.EnemyImage.TARGETDUMMY:
                return enemyImageList[21];
            case EnemyInformationData.EnemyImage.PRACTICEDUMMY:
                return enemyImageList[22];
            case EnemyInformationData.EnemyImage.TRAININGDUMMY:
                return enemyImageList[23];
            default:
                return null;
        }
    }

    public void StageSeletPopupActive()
    {
        StageSelectPopup.StageSelectSwitch();
    }
}