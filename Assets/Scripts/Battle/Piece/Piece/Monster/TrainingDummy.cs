using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingDummy : Piece
{
    PathFinding pathFinding;
    public override IEnumerator Attack()
    {
        if (mana >= pieceData.mana[star] && target != null)
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

    public override void DoAttack()
    {
        if (stun || freeze)
        {
            pieceState = State.IDLE;
            return;
        }
        if (target != null)
        {
            invincible = false;
            SoundManager.instance.Play("Wolf_Series/S_Attack_Were_Wolf", SoundManager.Sound.Effect);
            Damage(attackDamage);
            mana += manaRecovery;
            StartNextBehavior();
        }
        else
        {
            IdleState();
        }
    }

    public override IEnumerator Skill()
    {
        GetLocationMultiRangeSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)), 100, 5);
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    public void GetLocationMultiRangeSkill(float damage, float tickDamage, int time)
    {
        if (dead)
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
            else if (_targets.isOwned == true)
            {
                Instantiate(skillEffects, _targets.transform.position, Quaternion.identity);
                Damage(_targets, damage);
                _targets.SetTickDamage(tickDamage, time);
            }
        }
    }

    
    public override void SkillUpdateText()
    {
        pieceData.skillExplain = string.Format("회전 공격을 시전해 주변 1칸 범위 내 모든 적에게 {0}의 피해를 입히고 5초동안 {1}의 출혈 피해를 입힙니다.",abilityPower * (1 + (abilityPowerCoefficient / 100)), 500);
    }

    public override void Dead()
    {
        SoundManager.instance.Play("Wolf_Series/S_Death_Were_Wolf", SoundManager.Sound.Effect);
        StopAllCoroutines();
        currentTile.InitTile();
        gameObject.SetActive(false);
    }
}
