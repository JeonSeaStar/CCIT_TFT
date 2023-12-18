using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeDummy : Piece
{
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
        PracticeSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)));
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    void PracticeSkill(float updamage)
    {
        if (dead)
            return;
        if (target != null)
        {
            SkillState();
            SoundManager.instance.Play("Nepenthes_Series/S_Skil_Bloom", SoundManager.Sound.Effect);
            Instantiate(skillEffects, target.transform.position, Quaternion.identity);
            List<Piece> _allPiece = fieldManager.myFilePieceList;
            foreach (var _Neigbor in _allPiece)
            {
                Piece _targets = _Neigbor.GetComponent<Piece>();
                if (_targets == null)
                {
                    return;
                }
                else if (_targets.isOwned)
                {
                    _targets.attackDamage = _targets.attackDamage + updamage;
                }
            }
        }
    }

    public override void SkillUpdateText()
    {
        pieceData.skillExplain = string.Format("모든 적 유닛의 공격력을 이번 전투동안 {0}만큼 증가시킵니다.", abilityPower * (1 + (abilityPowerCoefficient / 100)));
    }

    public override void Dead()
    {
        SoundManager.instance.Play("Wolf_Series/S_Death_Were_Wolf", SoundManager.Sound.Effect);
        StopAllCoroutines();
        currentTile.InitTile();
        gameObject.SetActive(false);
    }
}
