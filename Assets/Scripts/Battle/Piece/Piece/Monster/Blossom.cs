using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blossom : Piece
{
    PathFinding pathFinding;
    [SerializeField] private Piece[] targets;

    [SerializeField] private GameObject beforeEffect;
    [SerializeField] private GameObject afterEffect;
    public override IEnumerator Attack()
    {
        if (mana >= 150)
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
        FindTargetThree(500f);
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    void FindTargetThree(float damage) //랜덤 세마리 받아야됨
    {
        if (fieldManager.myFilePieceList.Count > 3)
        {
            for(int i = 0; i < 3; i++)
            {
                targets[i] = fieldManager.myFilePieceList[i];
                Instantiate(beforeEffect, targets[i].currentTile.transform.position, Quaternion.identity);
                Instantiate(skillEffects, targets[i].transform.position, Quaternion.identity);
                Damage(targets[i], damage);
                GetLocationMultiRangeSkill(targets[i].currentTile, 200f);
            }
        }
        else if(fieldManager.myFilePieceList.Count > 2)
        {
            for (int i = 0; i < 2; i++)
            {
                targets[i] = fieldManager.myFilePieceList[i];
                Instantiate(skillEffects, targets[i].transform.position, Quaternion.identity);
                Damage(targets[i], damage);
                GetLocationMultiRangeSkill(targets[i].currentTile, 200f);
            }
        }
        else
        {
            return;
        }
    }

    void GetLocationMultiRangeSkill(Tile tiles, float damage)
    {
        pathFinding = ArenaManager.Instance.fieldManagers[0].pathFinding;
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
                Instantiate(afterEffect, _targets.transform.position, Quaternion.identity);
                Damage(_targets, damage);
            }
        }
    }
}
