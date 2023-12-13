using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blossom : Piece
{
    PathFinding pathFinding;
    [SerializeField] private GameObject hitEffect;
    public override IEnumerator Attack()
    {
        if (mana >= pieceData.mana[star])
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

    public override void DoAttack()
    {
        if (stun || freeze)
        {
            pieceState = State.IDLE;
            return;
        }
        if (target != null)
        {
            invincible = false;
            SoundManager.instance.Play("Nepenthes_Seris/S_Attack_Blossom", SoundManager.Sound.Effect);
            //print(name + "(이)가" + target.name + "에게 일반 공격을 합니다.");
            Damage(attackDamage);
            //mana += 100;
            mana += manaRecovery;
            StartNextBehavior();
        }
        else
        {
            IdleState();
        }
    }

    public override IEnumerator Skill()
    {
        Damage(1500f);
        GetLocationMultiRangeSkill(currentTile, 700f);
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }
    void GetLocationMultiRangeSkill(Tile tiles, float damage)
    {
        if (dead)
            return;
        SoundManager.instance.Play("Nepenthes_Seris/S_Skill_Blossom", SoundManager.Sound.Effect);
        SkillState();
        pathFinding = FieldManager.Instance.pathFinding;
        Instantiate(skillEffects, new Vector3(transform.position.x, transform.position.y + 2.5f, transform.position.z), Quaternion.identity);
        List<Tile> _getNeigbor = pathFinding.GetNeighbor(tiles);
        foreach (var _Neigbor in _getNeigbor)
        {
            Piece _targets = _Neigbor.piece;
            if (_targets == null)
            {
                Debug.Log("대상없음");
            }
            else if (_targets.isOwned)
            {
                Instantiate(hitEffect, _targets.transform.position, Quaternion.identity);
                Damage(_targets, damage);
            }
        }
    }

    public override void SkillUpdateText()
    {
        pieceData.skillExplain = string.Format("가운데 적에게 {0}의 피해를 입히고, 주변 적들에게 {1}의 피해를 입힙니다.", 1500, 700);
    }

    public override void Dead()
    {
        SoundManager.instance.Play("Nepenthes_Seris/S_Death_Blossom", SoundManager.Sound.Effect);
        StopAllCoroutines();
        currentTile.InitTile();
        gameObject.SetActive(false);
    }
}
