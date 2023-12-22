using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeusPiece : Piece
{
    [SerializeField] private GameObject zeusBullet;

    public override IEnumerator Attack()
    {
        if (mana >= maxMana)
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
        if (dead)
            return;
        if (target != null)
        {
            SkillState();
            SoundManager.instance.Play("GreatMountain/S_Zeus", SoundManager.Sound.Effect);
            GameObject _bullet = Instantiate(zeusBullet, transform.position, Quaternion.identity);
            Bullet bullet = _bullet.GetComponent<ZeusBullet>();
            bullet.parentPiece = this;
            bullet.damage = damage;
            bullet.Shot(target.transform.position - transform.position);
        }
    }
    public override void SkillUpdateText()
    {
        pieceData.skillExplain = string.Format("���� ��󿡰� {0}�� ���ظ� ������ ������ �����մϴ�. ������ �����ϴ� ���ȿ��� ������ �� �����ϴ�.", (abilityPower * (1 + (abilityPowerCoefficient / 100))));
    }
}
