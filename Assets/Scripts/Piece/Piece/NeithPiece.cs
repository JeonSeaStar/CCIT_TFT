using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeithPiece : Piece
{
    [SerializeField] private GameObject bullet;
    protected override IEnumerator Attack()
    {
        if (mana <= 100 && target != null)
        {
            Skill();
            mana = 0;
            yield return new WaitForSeconds(attackSpeed);
            StartCoroutine(NextBehavior());
        }
        else
        {
            base.Attack();
        }
    }

    protected override IEnumerator Skill()
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
        yield return new WaitForSeconds(attackSpeed);
        StartCoroutine(NextBehavior());
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
