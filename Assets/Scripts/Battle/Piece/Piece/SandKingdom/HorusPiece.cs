using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorusPiece : Piece
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask layerMask;
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
        SkillState();
        FindClosestEnemy(abilityPower * (1 + (abilityPowerCoefficient / 100)));
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    void FindClosestEnemy(float damage)
    {
        SoundManager.instance.Play("SandKingdom/Sound_for_Horus_01", SoundManager.Sound.Effect);
        SoundManager.instance.Play("SandKingdom/Sound_for_Horus_02", SoundManager.Sound.Effect);
        SoundManager.instance.Play("SandKingdom/Sound_for_Horus_03", SoundManager.Sound.Effect);
        SoundManager.instance.Play("SandKingdom/Sound_for_Horus_04", SoundManager.Sound.Effect);
        Collider[] col = Physics.OverlapSphere(transform.position, radius, layerMask);
        foreach(var cols in col)
        {
            GameObject _targets = cols.gameObject;
            if (_targets == null)
            {
                return;
            }
            else
            {
                GameObject centaBullet = Instantiate(bullet, transform.position, Quaternion.identity);
                Bullet b = centaBullet.GetComponent<HorusBullet>();
                b.parentPiece = this;
                b.damage = damage;
                b.Shot(_targets.transform.position - transform.position);
            }
        }
    }
    public override void SkillUpdateText()
    {
        if (star == 0)
            pieceData.skillExplain = string.Format("가장 가까운 적 4명에게 {0}의 피해를 입히는 모래 탄환을 {1}개 발사합니다.", abilityPower * (1 + (abilityPowerCoefficient / 100)), 4);
        else if (star == 1)
            pieceData.skillExplain = string.Format("가장 가까운 적 4명에게 {0}의 피해를 입히는 모래 탄환을 {1}개 발사합니다.", abilityPower * (1 + (abilityPowerCoefficient / 100)), 5);
        else if (star == 2)
            pieceData.skillExplain = string.Format("가장 가까운 적 4명에게 {0}의 피해를 입히는 모래 탄환을 {1}개 발사합니다.", abilityPower * (1 + (abilityPowerCoefficient / 100)), 6);
    }
}
