using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunfloraPixie : Piece
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
        FindClosestEnemy(abilityPower * (1 + (abilityPowerCoefficient / 100)));
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    void FindClosestEnemy(float damage)
    {
        if (dead)
            return;
        SkillState();
        SoundManager.instance.Play("SandKingdom/Sound_for_Horus_01", SoundManager.Sound.Effect);
        Collider[] col = Physics.OverlapSphere(transform.position, radius, layerMask);
        foreach (var cols in col)
        {
            GameObject _targets = cols.gameObject;
            if (_targets == null)
            {
                return;
            }
            else
            {
                GameObject centaBullet = Instantiate(bullet, transform.position, Quaternion.identity);
                Bullet b = centaBullet.GetComponent<SunfloraPixieBullet>();
                b.parentPiece = this;
                b.damage = damage;
                b.Shot(_targets.transform.position - transform.position);
            }
        }
    }
    public override void SkillUpdateText()
    {
        pieceData.skillExplain = string.Format("가장 가까운 적 4명에게 {0}의 피해를 입히는 모래 탄환을 {1}개 발사합니다.", abilityPower * (1 + (abilityPowerCoefficient / 100)), 4);
    }

    public override void Dead()
    {
        SoundManager.instance.Play("Wolf_Series/S_Death_Were_Wolf", SoundManager.Sound.Effect);
        StopAllCoroutines();
        currentTile.InitTile();
        gameObject.SetActive(false);
    }
}
