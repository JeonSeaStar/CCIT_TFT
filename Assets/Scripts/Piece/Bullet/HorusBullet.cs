using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorusBullet : Bullet
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
