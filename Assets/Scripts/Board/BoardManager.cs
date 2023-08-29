using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MyTileInfo
{

}

public class BoardManager : MonoBehaviour
{
    #region Singleton
    private static BoardManager instance = null;

    private void Awake()
    {
        if (instance == null) //instance�� null. ��, �ý��ۻ� �����ϰ� ���� ������
        {
            instance = this; //���ڽ��� instance�� �־��ݴϴ�.
            DontDestroyOnLoad(gameObject); //OnLoad(���� �ε� �Ǿ�����) �ڽ��� �ı����� �ʰ� ����
        }
        else
        {
            if (instance != this) //instance�� ���� �ƴ϶�� �̹� instance�� �ϳ� �����ϰ� �ִٴ� �ǹ�
                Destroy(this.gameObject); //�� �̻� �����ϸ� �ȵǴ� ��ü�̴� ��� AWake�� �ڽ��� ����
        }
    }
    #endregion

    public MyTileInfo mytileinfo = new MyTileInfo();

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            FieldManager.instance.roundType = FieldManager.RoundType.BATTLE;    
        }
        if(Input.GetKeyDown(KeyCode.K))
        {
            FieldManager.instance.getPieceCount++;
        }

        if(Input.GetKeyUp(KeyCode.L))
        {
            FieldManager.instance.setPieceCount++;
        }
    }
}
