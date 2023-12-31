using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    public Piece parentPiece;
    [SerializeField] protected float speed;
    public float damage;
    [SerializeField] protected GameObject attackEffect; //추후 애니메이션으로 변경
    public GameObject effect;
    public Vector3 dir;

    abstract protected void SetDamage(float damage);
    abstract public void Shot(Vector3 direction);
    
}
