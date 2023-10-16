using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killplayer : MonoBehaviour
{
    public GameObject playerPool;
    public Player[] TestPlayers;

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

    public void TestKillPlayer()
    {
        int k = Random.Range(0, 8);
        if (TestPlayers[k])
        {
            //Protocol.TestMessage message;
            //message = new Protocol.TestMessage(TestPlayers[k].GetIndex());
            //if (BackEndMatchManager.GetInstance().IsHost())
            //{
            //    BackEndMatchManager.GetInstance().TestAddMsgToLocalQueue(message);
            //}
            //else
            //{
            //    BackEndMatchManager.GetInstance().SendDataToInGame<Protocol.TestMessage>(message);
            //}
        }
    }

    public void buyOBJ()
    {
        
    }

}
