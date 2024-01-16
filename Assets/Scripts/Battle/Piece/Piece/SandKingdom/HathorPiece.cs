using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HathorPiece : Piece
{
    [SerializeField] private GameObject Hathorbullet;
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
            SoundManager.instance.Play("SandKingdom/S_Hathor", SoundManager.Sound.Effect);
            GameObject _bullet = Instantiate(Hathorbullet, transform.position, Quaternion.identity);
            Bullet bullet = _bullet.GetComponent<HathorBullet>();
            bullet.parentPiece = this;
            bullet.damage = damage;
            bullet.Shot(target.transform.position - transform.position);
        }
    }
    public override void SkillUpdateText()
    {

        pieceData.skillExplain = string.Format("현재 대상에게 강력한 화살을 쏘아 {0}의 피해를 입힙니다. ", abilityPower * (1 + (abilityPowerCoefficient / 100)));
    }
}
