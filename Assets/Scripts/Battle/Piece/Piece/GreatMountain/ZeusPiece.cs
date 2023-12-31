using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeusPiece : Piece
{
    [SerializeField] private GameObject zeusBullet;

    public override IEnumerator Attack()
    {
        if (mana >= maxMana)
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
            SoundManager.instance.Play("GreatMountain/S_Zeus", SoundManager.Sound.Effect);
            Quaternion rot = transform.rotation;
            //dInstantiate(skillEffects, transform.position, rot);
            GameObject centaBullet = Instantiate(zeusBullet, transform.position, Quaternion.identity);
            Bullet b = centaBullet.GetComponent<ZeusBullet>();
            b.parentPiece = this;
            b.damage = damage;
            b.Shot(target.transform.position - transform.position);
        }
    }
    public override void SkillUpdateText()
    {
        pieceData.skillExplain = string.Format("현재 대상에게 {0}의 피해를 입히는 번개를 방출합니다. 번개를 방출하는 동안에는 움직일 수 없습니다.", (abilityPower * (1 + (abilityPowerCoefficient / 100))));
    }
}
