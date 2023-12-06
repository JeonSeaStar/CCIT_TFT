using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeimdallrPiece : Piece
{
    PathFinding pathFinding;
    public override IEnumerator Attack()
    {
        if (mana >= maxMana)
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
        SkillState();
        GetLocationMultiRangeSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)));
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    void GetLocationMultiRangeSkill(float heal)
    {
        SoundManager.instance.Play("FrostyWind/S_Heimdallr", SoundManager.Sound.Effect);
        pathFinding = ArenaManager.Instance.fieldManagers[0].pathFinding;
        List<Tile> _getNeigbor = pathFinding.GetNeighbor(currentTile);
        foreach (var _Neigbor in _getNeigbor)
        {
            Piece _targets = _Neigbor.piece;
            if (_targets == null)
            {
                Debug.Log("대상없음");
            }
            else if (_targets.isOwned)
            {
                Instantiate(skillEffects, _targets.transform.position, Quaternion.identity);
                _targets.health += heal;
            }

        }
    }
    public override void SkillUpdateText()
    {
        pieceData.skillExplain = string.Format("뿔피리를 불어 주변 1칸 범위의 아군 기물들의 체력을 {0}만큼 회복시킵니다.", (abilityPower * (1 + (abilityPowerCoefficient / 100))));
    }
}
