using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killplayer : MonoBehaviour
{
    public GameObject playerPool;
    public Player[] TestPlayers;

    public GameObject TestObject;
    public GameObject[] TestObjects;

    public GameObject instanveObj;

    public void GetKillPlayer()
    {
        for (int i = 0; i < playerPool.transform.childCount; i++)
        {
            TestPlayers[i] = playerPool.gameObject.transform.GetChild(i).gameObject.GetComponent<Player>();
        }
    }

    public void KillPlayer()
    {
        int k = Random.Range(0, 8);
        if (TestPlayers[k])
        {
            if (BackEndMatchManager.GetInstance().IsHost() == false)
            {
                return;
            }
            Protocol.PlayerDeadMessage message =
                    new Protocol.PlayerDeadMessage(TestPlayers[k].GetIndex());
            BackEndMatchManager.GetInstance().SendDataToInGame<Protocol.PlayerDeadMessage>(message);
        }
    }
    public void TestObjectActive()
    {
        int k = Random.Range(0, 8);
        if (TestObjects[k])
        {
            TestObjects[k].SetActive(false);
        }
    }

    public void InstantiateObj()
    {
        Instantiate(instanveObj);
    }

}
