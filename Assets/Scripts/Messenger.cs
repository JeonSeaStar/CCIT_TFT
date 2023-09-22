using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Messenger : MonoBehaviour
{
    public int level;
    public int experience;
    public int lifePoint = 100;
    public int gold = 0;
    [SerializeField] LayerMask groundMask;
    [SerializeField] FieldManager fieldManager;
    [SerializeField] Ease ease;
    [SerializeField] float moveSpeed;
    [SerializeField] bool owned;

    void Update()
    {
        if (Input.GetMouseButton(0) && owned)
            Aim();
    }

    //ÀÌµ¿ ÁÂÇ¥
    (bool success, Vector3 position) GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, groundMask))
        {
            Vector3 targetPosition = new Vector3(hitInfo.point.x, transform.position.y, hitInfo.point.z);
            return (success: true, position: targetPosition);
        }
        else
            return (success: false, position: Vector3.zero);
    }

    //
    void Aim()
    {
        var (success, position) = GetMousePosition();
        if(success)
        {
            var direction = position - transform.position;

            transform.forward = direction;

            if(!fieldManager.grab)
            {
                DOTween.Kill(transform);
                transform.DOMove(position, moveSpeed * Vector3.Distance(transform.position, position)).SetEase(ease);
                transform.Rotate(90, transform.rotation.y, transform.rotation.z);
            }
        }
    }
}
