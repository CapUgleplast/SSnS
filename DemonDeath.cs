using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DemonDeath : MonoBehaviour {

    public CursedManager cursed;

    public void DestroyDemon() {
        cursed = GameObject.FindGameObjectWithTag("Cursed").GetComponent<CursedManager>();
        this.gameObject.GetComponent<PhotonView>().RPC("PunDestroyDemon", RpcTarget.MasterClient);
        Debug.LogError("Killed");
    }

    [PunRPC]
    public void PunDestroyDemon() {
        if(!PhotonNetwork.IsMasterClient) return;
        CursedManager.demonOn = false;
        if(!CursedManager.Bloodbath) {
            if(!CursedManager.PhaseThree) {
                CursedManager.timer = 120;
            } else {
                CursedManager.timer = 20;
            }
        } else {
            CursedManager.timer = 5;
        }
        Debug.LogError("DestroyDemon");
        PhotonNetwork.Destroy(this.gameObject);
    }
}
