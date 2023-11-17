using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThothPiece : Piece
{
    [SerializeField] private GameObject bullet;
    protected override void Attack()
    {
        if (mana <= 100 && target != null)
        {
            Skill();
            mana = 0;
            Invoke("NextBehavior", attackSpeed);
        }
        else
        {
            base.Attack();
        }
    }

    protected override void Skill()
    {
        if (star == 0)
        {
            ProjectionSkill();
        }
        else if (star == 1)
        {
            ProjectionSkill();
        }
        else if (star == 2)
        {
            ProjectionSkill();
        }
    }

    void ProjectionSkill()
    {
        GameObject centaBullet = Instantiate(bullet, transform.position, Quaternion.identity);
        Bullet b = centaBullet.GetComponent<ThothBullet>();
        b.Shot(target.transform.position - transform.position);
    }
}
