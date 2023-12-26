using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunBlossom : Piece
{
    [SerializeField] private GameObject bloomBullet;
    [SerializeField] private Transform effectPos;
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
            SoundManager.instance.Play("Nepenthes_Series/S_Attack_Bloom", SoundManager.Sound.Effect);
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
        Instantiate(skillEffects, effectPos.position, Quaternion.identity);
        ProjectionSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)));
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    void ProjectionSkill(float damage)
    {
        if (dead)
            return;
        if (target != null)
        {
            SkillState();
            SoundManager.instance.Play("Nepenthes_Series/S_Skil_Bloom", SoundManager.Sound.Effect);
            GameObject _bullet = Instantiate(bloomBullet, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), Quaternion.identity);
            Bullet bullet = _bullet.GetComponent<SunBlossomBullet>();
            bullet.parentPiece = this;
            bullet.damage = damage;
            bullet.Shot(target.transform.position - transform.position);
        }
    }

    public override void SkillUpdateText()
    {
        pieceData.skillExplain = string.Format("현재 공격 대상에게 각 {0}의 피해를 입히는 씨앗을 쏩니다.", (abilityPower * (1 + (abilityPowerCoefficient / 100))));
    }

    public override void Dead()
    {
        SoundManager.instance.Play("Nepenthes_Series/S_Death_Bloom", SoundManager.Sound.Effect);
        StopAllCoroutines();
        currentTile.InitTile();
        gameObject.SetActive(false);
    }
}
