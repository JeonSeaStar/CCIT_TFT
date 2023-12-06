using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaPiece : Piece
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
        if (star == 0)
        {
            RaSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)), 0.7f);
        }
        else if (star == 1)
        {
            RaSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)), 1.2f);
        }
        else if (star == 2)
        {
            RaSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)), 1.7f);
        }
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    public void RaSkill(float damage, float time)
    {
        SoundManager.instance.Play("SandKingdom/S_Ra_01", SoundManager.Sound.Effect);
        if (fieldManager.enemyFilePieceList != null)
        {
            for (int i = 0; i < fieldManager.enemyFilePieceList.Count; i++)
            {
                Instantiate(skillEffects, fieldManager.enemyFilePieceList[i].transform.position, Quaternion.identity);
                fieldManager.enemyFilePieceList[i].SetStun(time);
                SetDebuff("Stun", time, fieldManager.enemyFilePieceList[i]);
                Invoke("AfterSound", 0.5f);
                Damage(fieldManager.enemyFilePieceList[i], damage);
            }
        }
    }

    public void AfterSound()
    {
        SoundManager.instance.Play("SandKingdom/S_Ra_02", SoundManager.Sound.Effect);
    }

    public override void SkillUpdateText()
    {
        if(star == 0)
        {
            pieceData.skillExplain = string.Format("모든 적에게 {0}의 피해를 주고 {1}초 동안 기절시킵니다.", abilityPower * (1 + (abilityPowerCoefficient / 100)), 0.7f);
        }
        else if (star == 1)
        {
            pieceData.skillExplain = string.Format("모든 적에게 {0}의 피해를 주고 {1}초 동안 기절시킵니다.", abilityPower * (1 + (abilityPowerCoefficient / 100)), 1.2f);
        }
        else if (star == 2)
        {
            pieceData.skillExplain = string.Format("모든 적에게 {0}의 피해를 주고 {1}초 동안 기절시킵니다.", abilityPower * (1 + (abilityPowerCoefficient / 100)), 1.7f);
        }
        
    }
}
