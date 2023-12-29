using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JormungandPiece : Piece
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
        if (star == 0)
        {
            GetMultiRangeTickDamageSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)), (abilityPower * (1 + (abilityPowerCoefficient / 100))) * 0.8f, 6);
        }
        else if (star == 1)
        {
            GetMultiRangeTickDamageSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)), (abilityPower * (1 + (abilityPowerCoefficient / 100))) * 1.25f, 6);
        }
        else if (star == 2)
        {
            GetMultiRangeTickDamageSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)), (abilityPower * (1 + (abilityPowerCoefficient / 100))) * 4.25f, 10);
        }
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    public void GetMultiRangeTickDamageSkill(float damage, float tickDamage, int time)
    {
        if (dead)
            return;
        if (pieceState == State.DANCE)
            return;
        
        SkillState();
        SoundManager.instance.Play("FrostyWind/S_Jormungand", SoundManager.Sound.Effect);
        pathFinding = FieldManager.Instance.pathFinding;
        List<Tile> _getNeigbor = pathFinding.GetNeighbor(target.currentTile);
        _getNeigbor.Add(target.currentTile);
        foreach (var _Neigbor in _getNeigbor)
        {
            Piece _targets = _Neigbor.piece;

            if (_targets == null)
            {
                Debug.Log("대상없음");
            }
            else if (_targets.isOwned == false)
            {
                Instantiate(skillEffects, _targets.transform.position, Quaternion.identity);
                Damage(_targets, damage);
                _targets.SetTickDamage(tickDamage, time);
            }
        }
    }

    public override void SkillUpdateText()
    {
        if (star == 0)
            pieceData.skillExplain = string.Format("강력한 산성독을 내뱉어 {0}초 동안 {1}의 피해를 주는 산성지대를 생성합니다.", 6 ,(abilityPower * (1 + (abilityPowerCoefficient / 100))));
        else if (star == 1)
            pieceData.skillExplain = string.Format("강력한 산성독을 내뱉어 {0}초 동안 {1}의 피해를 주는 산성지대를 생성합니다.", 6 ,(abilityPower * (1 + (abilityPowerCoefficient / 100))));
        else if (star == 2)
            pieceData.skillExplain = string.Format("강력한 산성독을 내뱉어 {0}초 동안 {1}의 피해를 주는 산성지대를 생성합니다.", 10 ,(abilityPower * (1 + (abilityPowerCoefficient / 100))));
        
    }
}
