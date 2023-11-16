using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HathorBullet : Bullet
{
    private void Start()
    {
        SetDamage();
    }

    protected override void SetDamage()
    {
        damage = defaultDamage + parentPiece.abilityPower;
    }

    public override void Shot(Vector3 direction)
    {
        StartCoroutine(ShotBullet(direction));
    }

    protected override IEnumerator ShotBullet(Vector3 direction)
    {
        transform.Translate(direction * speed);
        yield return new WaitForSeconds(0.1f);
        ShotBullet(direction);
    }

    private void OnTriggerEnter(Collider target)
    {
        Damage();
    }

    private void Damage() //추후 애니메이션 실행 및 이벤트로 나눠서 구현
    {
        //parentPiece.Damage(damage);
        StopAllCoroutines();
        Destroy(gameObject);
    }
}
