using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentaurusPiece : Piece
{
    [SerializeField] private GameObject centaurusBullet;
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
            SoundManager.instance.Play("GreatMountain/S_Centaurus", SoundManager.Sound.Effect);
            GameObject _bullet = Instantiate(centaurusBullet, transform.position, Quaternion.identity);
            Bullet bullet = _bullet.GetComponent<CentaurusBullet>();
            bullet.parentPiece = this;
            bullet.damage = damage;
            bullet.Shot(target.transform.position - transform.position);
        }
    }
    public override void SkillUpdateText()
    {
        pieceData.skillExplain = string.Format("현재 대상에게 {0}의 피해를 입히는 화살을 쏩니다.", (abilityPower * (1 + (abilityPowerCoefficient / 100))));
    }
}
