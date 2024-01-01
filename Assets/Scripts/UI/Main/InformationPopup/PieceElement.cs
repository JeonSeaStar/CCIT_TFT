using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class PieceElement : MonoBehaviour
{
    public Image pieceFace;
    public TextMeshProUGUI pieceName;
    public TextMeshProUGUI pieceHp;
    public TextMeshProUGUI pieceMp;
    public SynergeDisplay[] synerges;
    public TextMeshProUGUI attackDamage;
    public TextMeshProUGUI abilityPower;
    public TextMeshProUGUI attackSpeed;
    public TextMeshProUGUI attackRange;
    public Image skillIcon;
    public TextMeshProUGUI skillname;
    public TextMeshProUGUI skillExplain;
}
