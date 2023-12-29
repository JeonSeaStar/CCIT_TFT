using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : Piece
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
            SoundManager.instance.Play("Shell_Series/S_Attack_Wolf_Cub", SoundManager.Sound.Effect);
            Damage(attackDamage);
            mana += manaRecovery;
            StartNextBehavior();
        }
        else
            IdleState();
    }

    public override IEnumerator Skill()
    {
        if(star == 0)
            DamageAndHealSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)), 500);
        else if(star == 1)
            DamageAndHealSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)), 580);
        else if(star == 2)
            DamageAndHealSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)), 670);
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    public void DamageAndHealSkill(float heal,float damage)
    {
        if (dead)
            return;
        if (target != null)
        {
            SkillState();
            SoundManager.instance.Play("Shell_Series/S_Drauger", SoundManager.Sound.Effect);
            health = health + heal;

            pathFinding = FieldManager.Instance.pathFinding;
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
                    Instantiate(skillEffects, transform.position, Quaternion.identity);
                    Damage(_targets, damage);
                }

            }
        }
    }

    public override void SkillUpdateText()
    {
        if (star == 0)
            pieceData.skillExplain = string.Format("껍질 속에 숨어 {0}의 체력을 회복하고 인접1칸 적에게 {0}의 피해를 입힙니다.", abilityPower * (1 + (abilityPowerCoefficient / 100)), 500);
        else if (star == 1)
            pieceData.skillExplain = string.Format("껍질 속에 숨어 {0}의 체력을 회복하고 인접1칸 적에게 {0}의 피해를 입힙니다.", abilityPower * (1 + (abilityPowerCoefficient / 100)), 580);
        else if (star == 2)
            pieceData.skillExplain = string.Format("껍질 속에 숨어 {0}의 체력을 회복하고 인접1칸 적에게 {0}의 피해를 입힙니다.", abilityPower * (1 + (abilityPowerCoefficient / 100)), 670);
    }

    public override void Dead()
    {
        SoundManager.instance.Play("Shell_Series/S_Death_Were_Wolf", SoundManager.Sound.Effect);
        StopAllCoroutines();
        currentTile.InitTile();
        gameObject.SetActive(false);
    }
}
