using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletServer : MonoBehaviour
{

    public bool isLive = true;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            isLive = false;
            this.gameObject.SetActive(false);
            if (BackEndMatchManager.GetInstance().IsHost() == false)
            {
                return;
            }
            Player tmp = collider.gameObject.GetComponent<Player>();
            if (tmp)
            {
                Protocol.PlayerDamegedMessage message =
                    new Protocol.PlayerDamegedMessage(tmp.GetIndex(), this.transform.position.x, this.transform.position.y, this.transform.position.z);
                BackEndMatchManager.GetInstance().SendDataToInGame<Protocol.PlayerDamegedMessage>(message);
            }
        }
    }
}
