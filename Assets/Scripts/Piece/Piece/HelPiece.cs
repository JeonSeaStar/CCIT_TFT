using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelPiece : Piece
{
    [SerializeField] private GameObject helBullet;
    protected override IEnumerator Attack()
    {
        if (mana >= 90 && target != null)
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

    void ProjectionSkill()//미리 만들어진 총알 있기는 한데.. 사용하는지 모르겠음.. 내일 오면 물어보기!
    {
        if(target != null)
        {
            GameObject centaBullet = Instantiate(helBullet, transform.position, Quaternion.identity);
            Bullet b = centaBullet.GetComponent<HelBullet>();
            b.Shot(target.transform.position - transform.position);
        }
    }
}
