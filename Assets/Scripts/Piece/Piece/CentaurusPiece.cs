using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentaurusPiece : Piece
{
    [SerializeField] private GameObject bullet;

    protected override void Skill()
    {
        GameObject centaBullet = Instantiate(bullet, transform.position, Quaternion.identity);
        Bullet b = centaBullet.GetComponent<CentaurusBullet>();
        b.Shot(target.transform.position - transform.position);
    }
}
