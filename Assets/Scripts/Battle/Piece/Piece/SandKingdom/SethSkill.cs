using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SethSkill : MonoBehaviour
{
    public BoxCollider boxCollider;
    public float damage;
    public Piece par;
    public LayerMask layerMask;
    private void Start()
    {
        Collider[] targets = Physics.OverlapBox(boxCollider.transform.position, boxCollider.size / 2, Quaternion.identity, layerMask);
        foreach (var cals in targets)
        {
            GameObject _targets = cals.gameObject;
            if (_targets == null)
            {
                Destroy(gameObject);
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
                        //Instantiate(effect, target.transform.position, Quaternion.identity);
                        target.Damage(target, damage);
                        Destroy(gameObject);
                    }
                    else
                    {
                        Destroy(gameObject);
                        return;
                    }
                }
            }
        }
    }
}
