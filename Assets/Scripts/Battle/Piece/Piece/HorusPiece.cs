using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorusPiece : Piece
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask layerMask;
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
        FindClosestEnemy(abilityPower * (1 + (abilityPowerCoefficient / 100)));
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    void FindClosestEnemy(float damage)
    {
        Collider[] col = Physics.OverlapSphere(transform.position, radius, layerMask);
        foreach(var cols in col)
        {
            GameObject _targets = cols.gameObject;
            if (_targets == null)
            {
                return;
            }
            else
            {
                GameObject centaBullet = Instantiate(bullet, transform.position, Quaternion.identity);
                Bullet b = centaBullet.GetComponent<HorusBullet>();
                b.parentPiece = this;
                b.damage = damage;
                b.Shot(_targets.transform.position - transform.position);
            }
        }
    }
}
