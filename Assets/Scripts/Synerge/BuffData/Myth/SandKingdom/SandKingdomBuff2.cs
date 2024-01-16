using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Buff Datas/Myth/SandKingdomBuff2")]
public class SandKingdomBuff2 : BuffData
{
    public override void DirectEffect(Piece piece, bool isAdd) => piece.pieceData.CalculateBuff(piece, this, isAdd);
    public override void BattleStartEffect(bool isAdd) => FieldManager.Instance.buffManager.sandKingdomWind.SetActive(isAdd);
    public override void CoroutineEffect() => FieldManager.Instance.StartCoroutine(SandKingdom());

    IEnumerator SandKingdom()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            foreach (var enemy in FieldManager.Instance.enemyFilePieceList)
            {
                if (enemy.gameObject.activeSelf == true)
                {
                    enemy.Damage(enemy, 10);
                    Debug.Log("모래바람 시너지가 적 전체에게 데미지를 줍니다.");
                }
            }
        }
    }
}
