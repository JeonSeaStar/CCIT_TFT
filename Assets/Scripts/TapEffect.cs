using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TapEffect : MonoBehaviour
{
    private Vector2 mousePosition;
    private Vector2 localPosition;
    [SerializeField] private Camera cam;
    [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform rt;
    [SerializeField] private GameObject tapEffect;

    private void Awake()
    {
        CameraStack();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            SpawnTapEffect();
        }
    }

    public void CameraStack()
    {
        var CameraData = Camera.main.GetComponent<UniversalAdditionalCameraData>();
        CameraData.cameraStack.Add(cam);
    }

    private void SpawnTapEffect()
    {
        mousePosition = Input.mousePosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, mousePosition, cam, out localPosition);

        GameObject effect = Instantiate(tapEffect, canvas.transform);
        effect.transform.localPosition = new Vector3(localPosition.x, localPosition.y, 0);
    }
}
