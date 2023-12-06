using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelBullet : Bullet
{
    private void Start()
    {
        SetDamage(damage);
        Invoke("DestroyBullet", 2f);
        Invoke("NextMove", 1f);
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
            if(_target == null)
            {
                return;
            }
            else if(!_target.isOwned)
            {
                Instantiate(effect, new Vector3(target.transform.position.x, target.transform.position.y + 0.8f, target.transform.position.z), Quaternion.identity);
                parentPiece.Damage(_target, damage);
                NextMove();
            }
        }
    }

    private void Damage() //���� �ִϸ��̼� ���� �� �̺�Ʈ�� ������ ����
    {
        parentPiece.Damage(damage);
    }

    void DestroyBullet()
    {
        Destroy(gameObject);
    }
    void NextMove()
    {
        parentPiece.StartNextBehavior();
    }
}
