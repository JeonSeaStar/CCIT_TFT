using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCheckSkill : MonoBehaviour
{
    [HideInInspector] public float damage;
    public GameObject effect;
    public BoxCollider boxCollider;
    public LayerMask layerMask;
    [HideInInspector] public GameObject tickEffect;
    [HideInInspector] public float tickDamage;
    [HideInInspector] public float time;
    [HideInInspector] public bool isTickTrue = false;

    private void OnEnable()
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
                    Instantiate(effect, target.transform.position, Quaternion.identity);
                    target.Damage(target, damage);
                    if (isTickTrue)
                        target.SetTickDamage(tickEffect, tickDamage, time);
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
