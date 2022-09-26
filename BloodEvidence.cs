using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BloodEvidence : MonoBehaviour {
    Renderer rend;
    GameObject clout;
    //PhotonView photonMine;

    // Start is called before the first frame update
    void Start() {

    }
    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Clout") {
            GetComponent<PhotonView>().RPC("BloodClout", RpcTarget.All);
        }
    }
    [PunRPC]
    void BloodClout() {
        clout = GameObject.FindGameObjectWithTag("Clout");
        rend = clout.GetComponent<Renderer>();
        rend.material.color = Color.red;
        if(gameObject.tag == "Evidence") {
            clout.tag = "Evidence";
        }
    }

}

