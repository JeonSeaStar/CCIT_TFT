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

    private void Damage() //���� �ִϸ��̼� ���� �� �̺�Ʈ�� ������ ����
    {
        //parentPiece.Damage(damage);
        StopAllCoroutines();
        Destroy(gameObject);
    }
}
