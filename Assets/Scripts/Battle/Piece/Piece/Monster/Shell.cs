using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : Piece
{
    
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
        AttackSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)), 1);
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    //가장 먼 적을 어떻게 찾을까?
    public void AttackSkill(float damage, float time)
    {
        if (dead)
            return;
        if (target != null)
        {
            SkillState();
            SoundManager.instance.Play("FrostyWind/S_Drauger", SoundManager.Sound.Effect);
            Instantiate(skillEffects, target.transform.position, Quaternion.identity);
            target.SetStun(time);
            Damage(damage);
        }
    }

    public override void SkillUpdateText()
    {
        pieceData.skillExplain = string.Format("가장 먼 적을 향해 빠르게 굴러가 {0}의 피해를 입히고 {1}초 동안 기절시킵니다.", abilityPower * (1 + (abilityPowerCoefficient / 100)), 1);
    }

    public override void Dead()
    {
        SoundManager.instance.Play("Wolf_Series/S_Death_Were_Wolf", SoundManager.Sound.Effect);
        StopAllCoroutines();
        currentTile.InitTile();
        gameObject.SetActive(false);
    }
}
