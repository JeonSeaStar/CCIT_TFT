using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    public Piece parentPiece;
    [SerializeField] protected float speed;
    public float damage;
    [SerializeField] protected GameObject attackEffect; //���� �ִϸ��̼����� ����

    abstract protected void SetDamage(float damage);
    abstract public void Shot(Vector3 direction);
    abstract protected IEnumerator ShotBullet(Vector3 direction);
}
