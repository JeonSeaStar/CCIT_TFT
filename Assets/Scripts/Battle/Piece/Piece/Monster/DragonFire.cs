using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonFire : Piece
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
            SoundManager.instance.Play("Wolf_Series/S_Attack_Wolf_Cub", SoundManager.Sound.Effect);
            Damage(attackDamage);
            mana += manaRecovery;
            StartNextBehavior();
        }
        else
            IdleState();
    }

    public override IEnumerator Skill()
    {
        for(int i = 0; i < 3; i++)
        {
            AttackSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)));
        }
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    public void AttackSkill(float damage)
    {
        if (dead)
            return;
        if (target != null)
        {
            SkillState();
            SoundManager.instance.Play("FrostyWind/S_Drauger", SoundManager.Sound.Effect);
            Instantiate(skillEffects, transform.position, Quaternion.identity);
            pathFinding = FieldManager.Instance.pathFinding;
            List<Tile> _getNeigbor = pathFinding.WideGetNeighbor(target.currentTile);
            _getNeigbor.Add(target.currentTile);
            foreach (var _Neigbor in _getNeigbor)
            {
                Piece _targets = _Neigbor.piece;

                if (_targets == null)
                {
                    Debug.Log("대상없음");
                }
                else if (_targets.isOwned)
                {
                    Damage(_targets, damage);
                }
            }
        }
    }

    public override void SkillUpdateText()
    {
        pieceData.skillExplain = string.Format("땅을 세게 찍어 주변 2칸 범위의 모든 적에게 {0}의 피해를 3번 입힙니다.", abilityPower * (1 + (abilityPowerCoefficient / 100)));
    }

    public override void Dead()
    {
        SoundManager.instance.Play("Wolf_Series/S_Death_Were_Wolf", SoundManager.Sound.Effect);
        StopAllCoroutines();
        currentTile.InitTile();
        gameObject.SetActive(false);
    }
}
