using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LokiPiece : Piece
{
    PathFinding pathFinding;
    int randomCount;
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
        if (star == 0)
        {
            GetAdbilityTarget(1.3f);
        }
        else if (star == 1)
        {
            GetAdbilityTarget(1.6f);
        }
        else if (star == 2)
        {
            GetAdbilityTarget(1.9f);
        }
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    public void GetAdbilityTarget(float percentage)
    {
        if (dead)
            return;
        SkillState();
        SoundManager.instance.Play("FrostyWind/S_Loki", SoundManager.Sound.Effect);
        pathFinding = ArenaManager.Instance.fieldManagers[0].pathFinding;
        List<Tile> _getFirstLineTiles = pathFinding.GetFrontLine(fieldManager.lokiPieceSkillPosition);
        foreach (var _Neigbor in _getFirstLineTiles)
        {
            Piece _targets = _Neigbor.GetComponent<Piece>();
            if (_targets == null)
            {
                Debug.Log("대상없음");
            }
            else if (_targets.isOwned)
            {
                Instantiate(skillEffects, _targets.transform.position, Quaternion.identity);
                RandomCapability(_targets, percentage);
            }
        }
    }

    void RandomCapability(Piece Target,float percentage)
    {//체력, 공격력, 공속, 꽝
        randomCount = Random.Range(0, 5);
        if (randomCount == 0)
        {
            Target.health = Target.health * percentage;
        }
        else if (randomCount == 1)
        {
            Target.attackDamage = Target.attackDamage * percentage;
        }
        else if (randomCount == 2)
        {
            Target.attackSpeed = Target.attackSpeed - (percentage % 3f);
        }
        else if (randomCount == 3)
        {
            Target.health = Target.health - (attackDamage * percentage);
        }
        else if (randomCount == 4)
        {
            Target.attackDamage = Target.attackDamage - (10f * percentage);
        }
        else if (randomCount == 5)
        {
            Target.attackSpeed = Target.attackSpeed + ( 1f - percentage);
        }
    }
    public override void SkillUpdateText()
    {
        if (star == 0)
            pieceData.skillExplain = string.Format("자신을 제외한 전열에 존재하는 모든 아군 기물의 무작위 능력치를 증가시키는 주술을 사용합니다. 공격력, 스킬증폭: {0}, 체력: {1}, 공격속도: {2}", 1.3 * attackDamage, 1.3 * health, 1.3 / 100);
        else if (star == 1)
            pieceData.skillExplain = string.Format("자신을 제외한 전열에 존재하는 모든 아군 기물의 무작위 능력치를 증가시키는 주술을 사용합니다. 공격력, 스킬증폭: {0}, 체력: {1}, 공격속도: {2}", 1.6 * attackDamage, 1.6 * health, 1.6 / 100);
        else if (star == 2)
            pieceData.skillExplain = string.Format("자신을 제외한 전열에 존재하는 모든 아군 기물의 무작위 능력치를 증가시키는 주술을 사용합니다. 공격력, 스킬증폭: {0}, 체력: {1}, 공격속도: {2}", 1.9 * attackDamage, 1.9 * health, 1.9 / 100);
    }
}
