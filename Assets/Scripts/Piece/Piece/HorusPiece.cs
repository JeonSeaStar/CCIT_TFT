using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorusPiece : Piece
{
    [SerializeField] private GameObject bullet;
    int count = 0;
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
            HorusSkill(256f);
        }
        else if (star == 1)
        {
            HorusSkill(576f);
        }
        else if (star == 2)
        {
            HorusSkill(1776f);
        }
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    public void HorusSkill(float damage)
    {
        List<Piece> Piece = fieldManager.enemyFilePieceList;
        List<Vector3> PieceDis = new List<Vector3>();
        for(int i = 0; i < Piece.Count; i++)
        {
             PieceDis.Add(Piece[i].transform.position - this.transform.position);
        }
        PieceDis.Sort();

        count = PieceDis.Count;
        for(int k = 0; k < count; k++)
        {
            if(PieceDis.Count > 0)
            {
                if(count <= 5)
                {
                    count = 4;
                }
                if (count == 4)
                {
                    GameObject centaBullet = Instantiate(bullet, transform.position, Quaternion.identity);
                    Bullet b = centaBullet.GetComponent<NeithBullet>();
                    b.parentPiece = this;
                    b.damage = damage;
                    b.Shot(PieceDis[k]);
                }
                else if(count == 3)
                {
                    GameObject centaBullet = Instantiate(bullet, transform.position, Quaternion.identity);
                    Bullet b = centaBullet.GetComponent<NeithBullet>();
                    b.parentPiece = this;
                    b.damage = damage;
                    b.Shot(PieceDis[k]);
                }
                else if (count == 2)
                {
                    GameObject centaBullet = Instantiate(bullet, transform.position, Quaternion.identity);
                    Bullet b = centaBullet.GetComponent<NeithBullet>();
                    b.parentPiece = this;
                    b.damage = damage;
                    b.Shot(PieceDis[k]);
                }
                else if (count == 1)
                {
                    GameObject centaBullet = Instantiate(bullet, transform.position, Quaternion.identity);
                    Bullet b = centaBullet.GetComponent<NeithBullet>();
                    b.parentPiece = this;
                    b.damage = damage;
                    b.Shot(PieceDis[k]);
                }
            }
            else
            {
                return;
            }
        }
    }
}
