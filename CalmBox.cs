using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EmeraldAI;
using Photon.Pun;
using UnityEngine.AI;

public class CalmBox : MonoBehaviour
{
    public GameObject box;
    public bool charged;
    public GameObject calmZone;
    public bool Collided;
    public int StopTime;

    private void Start() {
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Flesh") {
            StartCoroutine(MusicTimer(other));  
        }
    }


    private void Update() {
        if(charged) {
            if(box.transform.position.y <= 0.2f) {
                Collided = true;
            } else {
                Collided = false;
            }
        }
        if(Collided && charged) {
            calmZone.GetComponent<BoxCollider>().enabled = true;
        } else {
            calmZone.GetComponent<BoxCollider>().enabled = false;
        }

    }

    [PunRPC]
    void DemonOff() {
       
    }

    [PunRPC]
    void DemonOn() {

    }

    IEnumerator MusicTimer(Collider other) {
            other.gameObject.GetComponent<NavMeshAgent>().enabled = false;
            other.transform.GetChild(2).GetComponent<BoxCollider>().enabled = false;
            other.gameObject.GetComponent<EmeraldAISystem>().enabled = false;
            other.gameObject.GetComponent<EmeraldAIBehaviors>().enabled = false;
            other.gameObject.GetComponent<EmeraldAIEventsManager>().enabled = false;
            other.gameObject.GetComponent<Animator>().enabled = false;

            charged = false;
            yield return new WaitForSeconds(StopTime);
            other.gameObject.GetComponent<EmeraldAISystem>().enabled = true;
            other.gameObject.GetComponent<EmeraldAIBehaviors>().enabled = true;
            other.gameObject.GetComponent<EmeraldAIEventsManager>().enabled = true;
            other.gameObject.GetComponent<Animator>().enabled = true;
            other.gameObject.GetComponent<NavMeshAgent>().enabled = true;
            other.transform.GetChild(2).GetComponent<BoxCollider>().enabled = true;
            GameObject.FindGameObjectWithTag("EvidenceManager").GetComponent<EvidenceManager>().PunCalmBoxSpawn();
            PhotonNetwork.Destroy(this.transform.parent.GetComponent<PhotonView>());
        

    }

    public void MusicUp() {
        charged = true;
        Debug.LogError(charged);
    }
}
