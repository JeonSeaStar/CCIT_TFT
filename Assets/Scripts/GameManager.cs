using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public EnemyInformationData selectedStage;

    void Awake()
    {
        //�̱������� �ٲټ�. ��´�. ����.
        instance = this;
        DontDestroyOnLoad(this);
    }
}
