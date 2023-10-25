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
    private float speed = 1.0f;
    public int howPlayerPiece;
    public void Start()
    {
        pos = this.transform.position;
        pos.x += 1f;
        howPlayerPiece = howPlayerPiece * 100;
    }

    public void Update()
    {
        this.transform.position += pos * Time.deltaTime * speed;
        if(Input.GetKeyDown(KeyCode.D))
        {
            pos.x -= 1f;
            pos.z += 1f;
            this.transform.position += pos * Time.deltaTime * speed;
        }
    }

}
