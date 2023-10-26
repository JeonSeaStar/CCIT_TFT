using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    protected Piece parentPiece;
    [SerializeField] protected float speed;
    [SerializeField] protected float defaultDamage;
    protected float damage;
    [SerializeField] protected GameObject attackEffect; //추후 애니메이션으로 변경

    abstract protected void SetDamage();
    abstract public void Shot(Vector3 direction);
    abstract protected IEnumerator ShotBullet(Vector3 direction);
}
