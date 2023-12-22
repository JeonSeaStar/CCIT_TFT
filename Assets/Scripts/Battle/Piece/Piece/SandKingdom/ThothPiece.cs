using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThothPiece : Piece
{
    PathFinding pathFinding;
    public override IEnumerator Attack()
    {
        if (mana >= maxMana && target != null)
        {
            StartSkill();
            mana = 0;
            yield return new WaitForSeconds(attackSpeed);
            StartNextBehavior();
        }
        else
        {
            DoAttack();
        }
    }

    public override IEnumerator Skill()
    {
        GetSideDamageSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)));
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    void GetSideDamageSkill(float damage)
    {
        if (target != null)
        {
            if (dead)
                return;
            SkillState();
            SoundManager.instance.Play("SandKingdom/S_Thoth", SoundManager.Sound.Effect);
            Instantiate(skillEffects, new Vector3(target.transform.position.x, target.transform.position.y + 0.8f, target.transform.position.z), Quaternion.identity);
            pathFinding = FieldManager.Instance.pathFinding;
            List<Tile> _getNeigbor = pathFinding.GetSide(currentTile);
            foreach (var _Neigbor in _getNeigbor)
            {
                Piece _targets = _Neigbor.piece;
                if (_targets == null)
                {
                    Debug.Log("������");
                }
                else if (!_targets.isOwned)
                {
                    Damage(_targets, damage);
                }
            }
        }
    }

    public override void SkillUpdateText()
    {
        pieceData.skillExplain = string.Format("���� ���� �¿� 1ĭ ������ ������ �縷�� �ּ��� {0}�� ���ظ� �����ϴ�.", abilityPower * (1 + (abilityPowerCoefficient / 100)));
    }
}
