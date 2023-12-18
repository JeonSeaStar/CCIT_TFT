using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunBlossomBullet : Bullet
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
        if (target.gameObject == null)
            return;
        if (target.gameObject == parentPiece.target.gameObject)
        {
            Instantiate(effect, target.transform.position, Quaternion.identity);
            Damage();
        }
    }

    private void Damage() //���� �ִϸ��̼� ���� �� �̺�Ʈ�� ������ ����
    {
        parentPiece.Damage(damage);
        Destroy(gameObject);
    }
}

