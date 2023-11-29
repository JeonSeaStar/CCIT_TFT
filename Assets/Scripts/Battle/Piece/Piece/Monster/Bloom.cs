using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloom : Piece
{
    [SerializeField] private GameObject bloomBullet;
    public override IEnumerator Attack()
    {
        if (mana >= 80 && target != null)
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
        for(int i = 0; i < 6; i++)
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
            GameObject centaBullet = Instantiate(bloomBullet, transform.position, Quaternion.identity);
            Bullet b = centaBullet.GetComponent<BloomBullet>();
            b.parentPiece = this;
            b.damage = damage;
            b.Shot(target.transform.position - transform.position);
        }
    }
}
