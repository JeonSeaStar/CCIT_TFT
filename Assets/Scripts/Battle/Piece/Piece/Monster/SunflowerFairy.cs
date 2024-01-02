using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunflowerFairy : Piece
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
            SoundManager.instance.Play("Sunflower_Series/S_Attack_SunFlowerFairy", SoundManager.Sound.Effect);
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
        if (star == 0)
            GetMultiTickDamageSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)), 4);
        else if (star == 1)
            GetMultiTickDamageSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)), 5);
        else if (star == 2)
            GetMultiTickDamageSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)), 5);
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    public void GetMultiTickDamageSkill(float tickDamage, int time)
    {
        if (dead)
            return;
        SkillState();
        SoundManager.instance.Play("Sunflower_Series/S_Skill_SunFlowerFairy", SoundManager.Sound.Effect);
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
            else if (_targets.isOwned == true)
            {
                Instantiate(skillEffects, _targets.transform.position, Quaternion.identity);
                _targets.SetTickDamage(tickDamage, time);
            }
        }
    }

    public override void SkillUpdateText()
    {
        if (star == 0)
            pieceData.skillExplain = string.Format("주변2칸 범위에 꽃가루를 뿌려 {0}초 동안 초 마다 {1}의 피해를 줍니다.", 4, abilityPower * (1 + (abilityPowerCoefficient / 100)));
        else if (star == 1)
            pieceData.skillExplain = string.Format("주변2칸 범위에 꽃가루를 뿌려 {0}초 동안 초 마다 {1}의 피해를 줍니다.", 5, abilityPower * (1 + (abilityPowerCoefficient / 100)));
        else if (star == 2)
            pieceData.skillExplain = string.Format("주변2칸 범위에 꽃가루를 뿌려 {0}초 동안 초 마다 {1}의 피해를 줍니다.", 5, abilityPower * (1 + (abilityPowerCoefficient / 100)));
    }

    public override void Dead()
    {
        SoundManager.instance.Play("Sunflower_Series/S_Death_SunFlowerFairy", SoundManager.Sound.Effect);
        StopAllCoroutines();
        currentTile.InitTile();
        gameObject.SetActive(false);
    }
}
