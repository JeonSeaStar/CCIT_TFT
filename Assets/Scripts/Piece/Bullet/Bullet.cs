using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    protected Piece parentPiece;
    [SerializeField] protected float speed;
    [SerializeField] protected float defaultDamage;
    protected float damage;
    [SerializeField] protected GameObject attackEffect; //���� �ִϸ��̼����� ����

    abstract protected void SetDamage();
    abstract public void Shot(Vector3 direction);
    abstract protected IEnumerator ShotBullet(Vector3 direction);
}
