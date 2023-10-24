using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPieceInspector : MonoBehaviour
{
    public float test1 = 10;
    public float test2 = 20;
    public float test3 = 20;
    public int test4 = 100;
    public Vector3 pos;

    public void Start()
    {
        pos = this.transform.position;
    }
}
