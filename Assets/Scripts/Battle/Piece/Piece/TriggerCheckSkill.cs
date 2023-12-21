using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCheckSkill : MonoBehaviour
{
    public float damage;
    public GameObject effect;
    public BoxCollider boxCollider;
    public LayerMask layerMask;
    public bool ownerPiece;

    private void OnEnable()
    {
        if(ownerPiece)
        {
            Collider[] targets = Physics.OverlapBox(boxCollider.transform.position, boxCollider.size / 2, Quaternion.identity, layerMask);
            foreach (var cals in targets)
            {
                GameObject _targets = cals.gameObject;
                if (_targets == null)
                {
                    gameObject.SetActive(false);
                    return;
                }
                else
                {
                    Piece target = _targets.GetComponent<Piece>();
                    if (target == null)
                    {
                        gameObject.SetActive(false);
                        return;
                    }
                    else
                    {
                        if (!target.isOwned)
                        {
                            Instantiate(effect, target.transform.position, Quaternion.identity);
                            target.Damage(target, damage);
                            gameObject.SetActive(false);
                        }
                        else
                        {
                            gameObject.SetActive(false);
                            return;
                        }
                    }
                }
            }
        }
        else
        {
            Collider[] targets = Physics.OverlapBox(boxCollider.transform.position, boxCollider.size / 2, Quaternion.identity, layerMask);
            foreach (var cals in targets)
            {
                GameObject _targets = cals.gameObject;
                if (_targets == null)
                {
                    gameObject.SetActive(false);
                    return;
                }
                else
                {
                    Piece target = _targets.GetComponent<Piece>();
                    if (target == null)
                    {
                        gameObject.SetActive(false);
                        return;
                    }
                    else
                    {
                        if (target.isOwned)
                        {
                            Instantiate(effect, target.transform.position, Quaternion.identity);
                            target.Damage(target, damage);
                            gameObject.SetActive(false);
                        }
                        else
                        {
                            gameObject.SetActive(false);
                            return;
                        }
                    }
                }
            }
        }
    }
}
