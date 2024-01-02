using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Piece
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
            SoundManager.instance.Play("Bomb_Series/S_Attack_Bomb", SoundManager.Sound.Effect);
            Damage(attackDamage);
            mana += manaRecovery;
            StartNextBehavior();
        }
        else
            IdleState();
    }

    public override IEnumerator Skill()
    {
        BombSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)));
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    public void BombSkill(float damage)
    {
        if (dead)
            return;
        SkillState();
        SoundManager.instance.Play("Bomb_Series/S_Skill_Bomb", SoundManager.Sound.Effect);
        pathFinding = FieldManager.Instance.pathFinding;
        Instantiate(skillEffects, transform.position, Quaternion.identity);
        List<Tile> _getNeigbor = pathFinding.WideGetNeighbor(currentTile);
        _getNeigbor.Add(currentTile);
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
        health = 0;
        DeadState();
    }

    public override void SkillUpdateText()
    {
        pieceData.skillExplain = string.Format("현재 공격대상에게 빠르게 굴러가 {0}의 피해를 입히고 {1}초 동안 기절시킵니다.", abilityPower * (1 + (abilityPowerCoefficient / 100)), 1);
    }

    public override void Dead()
    {
        SoundManager.instance.Play("Bomb_Series/S_Death_Bomb", SoundManager.Sound.Effect);
        StopAllCoroutines();
        currentTile.InitTile();
        gameObject.SetActive(false);
    }
}
