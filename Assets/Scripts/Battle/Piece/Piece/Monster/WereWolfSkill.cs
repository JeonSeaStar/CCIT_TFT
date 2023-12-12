using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WereWolfSkill : MonoBehaviour
{
    public Piece parentPiece;
    public float damage;
    public GameObject effect;

    private void OnTriggerEnter(Collider other)
    {
        Piece _target = other.GetComponent<Piece>();
        if (_target == null)
        {
            return;
        }
        else if (_target.isOwned)
        {
            Instantiate(effect, other.transform.position, Quaternion.identity);
            parentPiece.Damage(_target, damage);
        }
    }
}
