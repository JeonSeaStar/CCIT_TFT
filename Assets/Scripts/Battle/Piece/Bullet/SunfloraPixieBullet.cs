using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunfloraPixieBullet : Bullet
{
    public GameObject effect2;
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
        if (target.CompareTag("Piece"))
        {
            Instantiate(effect, new Vector3(target.transform.position.x, target.transform.position.y + 0.8f, target.transform.position.z), Quaternion.identity);
            Damage();
        }
    }

    private void Damage() //���� �ִϸ��̼� ���� �� �̺�Ʈ�� ������ ����
    {
        parentPiece.Damage(damage);
        Destroy(gameObject);
    }
}