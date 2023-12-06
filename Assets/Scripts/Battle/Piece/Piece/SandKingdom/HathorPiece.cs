using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HathorPiece : Piece
{
    [SerializeField] private GameObject bullet;
    public override IEnumerator Attack()
    {
        if (mana >= maxMana && target != null)
        {
            StartSkill();
            mana = 0;
            yield return new WaitForSeconds(attackSpeed);
            StartNextBehavior();
        }
        else
        {
            DoAttack();
        }
    }

    public override IEnumerator Skill()
    {
        ProjectionSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)));
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    void ProjectionSkill(float damage)
    {
        if (target != null)
        {
            SoundManager.instance.Play("SandKingdom/S_Hathor", SoundManager.Sound.Effect);
            GameObject centaBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            Bullet b = centaBullet.GetComponent<HathorBullet>();
            b.parentPiece = this;
            b.damage = damage;
            b.Shot(target.transform.position - transform.position);
        }
    }
    public override void SkillUpdateText()
    {

        pieceData.skillExplain = string.Format("���� ��󿡰� ������ ȭ���� ��� {0}�� ���ظ� �����ϴ�. ", abilityPower * (1 + (abilityPowerCoefficient / 100)));
    }
}
