using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AresBullet : Bullet
{
    private void Start()
    {
        SetDamage(damage);
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

        if (target.gameObject == parentPiece.target.gameObject)
        {
            Instantiate(effect, new Vector3(target.transform.position.x, target.transform.position.y + 0.8f, target.transform.position.z), Quaternion.identity);
            Damage();
        }
    }

    private void Damage() //추후 애니메이션 실행 및 이벤트로 나눠서 구현
    {
        parentPiece.Damage(damage);
        Destroy(gameObject);
    }
}
