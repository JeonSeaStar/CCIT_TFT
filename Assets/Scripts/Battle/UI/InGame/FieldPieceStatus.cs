using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FieldPieceStatus : MonoBehaviour
{
    public TextMeshProUGUI currentPieceCount;
    public TextMeshProUGUI maxPieceCount;
    public GameObject fieldPieceStatusGameObject;

    public void UpdateFieldStatus(int current, int max)
    {
        if (current == max)
            fieldPieceStatusGameObject.SetActive(false);
        else
            fieldPieceStatusGameObject.SetActive(true);

        currentPieceCount.text = current.ToString();
        maxPieceCount.text = max.ToString();
    }
}
