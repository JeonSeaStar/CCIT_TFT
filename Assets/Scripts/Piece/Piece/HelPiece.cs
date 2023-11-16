using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelPiece : Piece
{
    public GameObject helBullet;
    protected override void Attack()
    {
        if (mana <= 90)
        {
            Skill();
            mana = 0;
        }
        else
        {
            base.Attack();
        }
    }

    protected override void Skill()
    {
        base.Skill();
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

    void ProjectionSkill()//�̸� ������� �Ѿ� �ֱ�� �ѵ�.. ����ϴ��� �𸣰���.. ���� ���� �����!
    {
        if(target != null)
        {
            Instantiate(helBullet, target.transform.position, Quaternion.identity);
        }
    }
}
