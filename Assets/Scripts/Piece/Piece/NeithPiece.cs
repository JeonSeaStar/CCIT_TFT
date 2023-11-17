using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeithPiece : Piece
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
        if (target != null)
        {
            GameObject centaBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            Bullet b = centaBullet.GetComponent<NeithBullet>();
            b.Shot(target.transform.position - transform.position);
        }
    }
}
