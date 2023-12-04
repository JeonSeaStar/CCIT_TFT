using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloom : Piece
{
    [SerializeField] private GameObject bloomBullet;
    [SerializeField] private Transform effectPos;
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
        SkillState();
        Instantiate(skillEffects, effectPos.position, Quaternion.identity);
        ProjectionSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)) * 6);
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    void ProjectionSkill(float damage)
    {

        if (target != null)
        {
            GameObject centaBullet = Instantiate(bloomBullet, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), Quaternion.identity);
            Bullet b = centaBullet.GetComponent<BloomBullet>();
            b.parentPiece = this;
            b.damage = damage;
            b.Shot(target.transform.position - transform.position);
        }
    }
}
