using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCheckSkill : MonoBehaviour
{
    public float damage;
    public GameObject effect;

    private void OnTriggerEnter(Collider other)
    {
        Piece test = other.GetComponent<Piece>();
        if (test == null)
        {
            gameObject.SetActive(false);
            return;
        }
        else if(!test.isOwned)
        {
            Instantiate(effect, test.transform.position, Quaternion.identity);
            test.Damage(test, damage);
            gameObject.SetActive(false);
        }
    }
}
