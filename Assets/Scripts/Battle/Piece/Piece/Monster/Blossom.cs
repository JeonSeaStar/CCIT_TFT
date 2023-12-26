using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blossom : Piece
{
    PathFinding pathFinding;
    [SerializeField] private GameObject hitEffect;
    public override IEnumerator Attack()
    {
        if (mana >= pieceData.mana[star])
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
            SoundManager.instance.Play("Nepenthes_Series/S_Attack_Blossom", SoundManager.Sound.Effect);
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
        if(star == 0)
        {
            Damage(abilityPower * (1 + (abilityPowerCoefficient / 100)));
            GetLocationMultiRangeSkill(currentTile, 230f);
        }
        else if(star == 1)
        {
            Damage(abilityPower * (1 + (abilityPowerCoefficient / 100)));
            GetLocationMultiRangeSkill(currentTile, 280f);
        }
        else if(star == 2)
        {
            Damage(abilityPower * (1 + (abilityPowerCoefficient / 100)));
            GetLocationMultiRangeSkill(currentTile, 350f);
        }
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }
    void GetLocationMultiRangeSkill(Tile tiles, float damage)
    {
        if (dead)
            return;
        SoundManager.instance.Play("Nepenthes_Series/S_Skill_Blossom", SoundManager.Sound.Effect);
        SkillState();
        pathFinding = FieldManager.Instance.pathFinding;
        Instantiate(skillEffects, new Vector3(transform.position.x, transform.position.y + 2.5f, transform.position.z), Quaternion.identity);
        List<Tile> _getNeigbor = pathFinding.GetNeighbor(tiles);
        foreach (var _Neigbor in _getNeigbor)
        {
            Piece _targets = _Neigbor.piece;
            if (_targets == null)
            {
                Debug.Log("대상없음");
            }
            else if (_targets.isOwned)
            {
                Instantiate(hitEffect, _targets.transform.position, Quaternion.identity);
                Damage(_targets, damage);
            }
        }
    }

    public override void SkillUpdateText()
    {
        if (star == 0)
        {
            pieceData.skillExplain = string.Format("가운데 적에게 {0}의 피해를 입히고, 주변 적들에게 {1}의 피해를 입힙니다.", abilityPower * (1 + (abilityPowerCoefficient / 100)), 230);
        }
        else if (star == 1)
        {
            pieceData.skillExplain = string.Format("가운데 적에게 {0}의 피해를 입히고, 주변 적들에게 {1}의 피해를 입힙니다.", abilityPower * (1 + (abilityPowerCoefficient / 100)), 280);
        }
        else if (star == 2)
        {
            pieceData.skillExplain = string.Format("가운데 적에게 {0}의 피해를 입히고, 주변 적들에게 {1}의 피해를 입힙니다.", abilityPower * (1 + (abilityPowerCoefficient / 100)), 350);
        }
    }

    public override void Dead()
    {
        SoundManager.instance.Play("Nepenthes_Series/S_Death_Blossom", SoundManager.Sound.Effect);
        StopAllCoroutines();
        currentTile.InitTile();
        gameObject.SetActive(false);
    }
}
