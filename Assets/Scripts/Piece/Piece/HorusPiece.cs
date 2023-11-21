using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorusPiece : Piece
{
    [SerializeField] private GameObject bullet;
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
            HorusSkill(attackDamage * 1f, 16);
        }
        else if (star == 1)
        {
            HorusSkill(attackDamage * 3f, 20);
        }
        else if (star == 2)
        {
            HorusSkill(attackDamage * 8f, 24);
        }
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    public void HorusSkill(float damage, int count)
    {
        List<Piece> Piece = fieldManager.enemyFilePieceList;
        List<Vector3> PieceDis = new List<Vector3>();
        for(int i = 0; i < Piece.Count; i++)
        {
             PieceDis.Add(Piece[i].transform.position - this.transform.position);
        }
        PieceDis.Sort();

        for(int k = 0; k < PieceDis.Count; k++)
        {
            for(int j = 0; j < 4; j++)
            {
                GameObject centaBullet = Instantiate(bullet, transform.position, Quaternion.identity);
                Bullet b = centaBullet.GetComponent<NeithBullet>();
                b.parentPiece = this;
                b.damage = damage;
                b.Shot(target.transform.position - transform.position);
            }
        }
    }
}
