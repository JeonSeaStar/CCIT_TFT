using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelPiece : Piece
{
    [SerializeField] private GameObject helBullet;
    public override IEnumerator Attack()
    {
        if (mana >= 90 && target != null)
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
            GameObject centaBullet = Instantiate(helBullet, transform.position, Quaternion.identity);
            Bullet b = centaBullet.GetComponent<HelBullet>();
            b.parentPiece = this;
            b.damage = damage;
            b.Shot(target.transform.position - transform.position);
        }
    }
}
