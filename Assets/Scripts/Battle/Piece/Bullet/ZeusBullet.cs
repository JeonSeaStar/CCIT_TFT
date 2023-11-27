using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeusBullet : Bullet
{
    private void Start()
    {
        SetDamage(damage);
        Invoke("DestroyBullet", 4f);
    }

    protected override void SetDamage(float damage)
    {
        damage = this.damage;
    }

    public override void Shot(Vector3 direction)
    {
        dir = direction;
    }

    private void Update()
    {
        transform.Translate(dir * speed);
    }

    private void OnTriggerEnter(Collider target)
    {
        //if(target.CompareTag("Enemy"))
        {
            Piece _target = target.GetComponent<Piece>();
            if (_target == null)
            {
                return;
            }
            else if (!_target.isOwned)
            {
                Instantiate(effect, target.transform.position, Quaternion.identity);
                parentPiece.Damage(_target, damage);
            }
        }
    }

    private void Damage() //추후 애니메이션 실행 및 이벤트로 나눠서 구현
    {
        parentPiece.Damage(damage);
    }

    void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
