using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorusPiece : Piece
{
    [SerializeField] private GameObject bullet;
    int count = 0;
    public override IEnumerator Attack()
    {
        if (mana >= 70 && target != null)
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
            ProjectionSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)));
        }
        else if (star == 1)
        {
            ProjectionSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)));
        }
        else if (star == 2)
        {
            ProjectionSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)));
        }
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    void ProjectionSkill(float damage)
    {
        if (target != null)
        {
            GameObject centaBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            Bullet b = centaBullet.GetComponent<HathorBullet>();
            b.parentPiece = this;
            b.damage = damage;
            b.Shot(target.transform.position - transform.position);
        }
    }
}
