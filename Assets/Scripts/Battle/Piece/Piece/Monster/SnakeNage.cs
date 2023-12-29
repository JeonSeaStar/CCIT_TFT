using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeNage : Piece
{
    public TriggerCheckSkill snakeNageSkill;
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
            for(int i = 0; i < 2; i++)
            {
                SoundManager.instance.Play("Wolf_Series/S_Attack_Wolf_Cub", SoundManager.Sound.Effect);
                Damage(attackDamage);
                mana += manaRecovery;
            }
            StartNextBehavior();
        }
        else
            IdleState();
    }

    public override IEnumerator Skill() //���İ��� ���� �� ���� - ���� �ʿ� �� �� ����
    {
        GetLocationMultiRangeSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)), 100f, 5);
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    void GetLocationMultiRangeSkill(float damage, float tickDamage, float time)
    {
        if (dead)
            return;
        SkillState();
        SoundManager.instance.Play("GreatMountain/S_Griffin", SoundManager.Sound.Effect);
        Quaternion rotation = transform.rotation;
        Instantiate(skillEffects, transform.position, rotation);
        snakeNageSkill.gameObject.SetActive(true);
        snakeNageSkill.damage = damage;
        snakeNageSkill.isTickTrue = true;
        snakeNageSkill.tickDamage = tickDamage;
        snakeNageSkill.time = time;
    }

    public override void SkillUpdateText()
    {
        pieceData.skillExplain = string.Format("��ä�� ������ �꼺 �극���� �վ� {0}�� ���ظ� �ְ�, 5�ʵ��� ���ʸ��� {1}�� ���� ���ظ� �ݴϴ�.", abilityPower * (1 + (abilityPowerCoefficient / 100)),2);
    }

    public override void Dead()
    {
        SoundManager.instance.Play("Wolf_Series/S_Death_Were_Wolf", SoundManager.Sound.Effect);
        StopAllCoroutines();
        currentTile.InitTile();
        gameObject.SetActive(false);
    }
}