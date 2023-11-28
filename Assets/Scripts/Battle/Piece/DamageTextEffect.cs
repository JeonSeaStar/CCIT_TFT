using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DamageTextEffect : MonoBehaviour
{
    [SerializeField] private float destroyTime;
    [SerializeField] private Animator animator;
    [SerializeField] private Vector3 offset;

    void Start()
    {
        Destroy(gameObject, destroyTime);

        transform.localPosition = new Vector3(Random.Range(-offset.x, offset.x), Random.Range(-offset.y, offset.y), Random.Range(-offset.z, offset.z));
    }

    void Effect()
    {

    }
}
