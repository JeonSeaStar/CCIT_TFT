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
        if (target != null)
        {
            Quaternion rot = transform.rotation;
            //dInstantiate(skillEffects, transform.position, rot);
            GameObject centaBullet = Instantiate(zeusBullet, transform.position, Quaternion.identity);
            Bullet b = centaBullet.GetComponent<ZeusBullet>();
            b.parentPiece = this;
            b.damage = damage;
            b.Shot(target.transform.position - transform.position);
        }
    }
}
