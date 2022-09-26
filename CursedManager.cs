using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using EmeraldAI;

public class CursedManager : MonoBehaviour {
    public float timeOfTimer;
    public static float timer;
    public GameObject[] demonSpawns;
    private GameObject demon;
    public GameObject demonPrefab;
    public static int demonSpeed = 5;

    //[HideInInspector]
    public static bool demonOn = false;
    public static bool gameStart = false;
    Renderer rend;
    public float disturbRectInt;
    public static float x = 0;
    public GameObject RageCalmTrigger;
    public GameObject[] CalmsSpawns;

    public static bool Bloodbath = false;
    public static bool PhaseThree = false;

    private bool rageBool;
    public AudioClip rage;

    void Start() {
        rend = GetComponent<Renderer>();
        demonSpawns = GameObject.FindGameObjectsWithTag("demonSpawn");
        CalmsSpawns = GameObject.FindGameObjectsWithTag("CalmSpawn");
        timer = timeOfTimer;
        if(PhotonNetwork.IsMasterClient) {
            var rand = Random.Range(0, CalmsSpawns.Length);
            DontDestroyOnLoad(PhotonNetwork.Instantiate("Prefabs/CalmCard", CalmsSpawns[rand].transform.position, Quaternion.identity));
        }
    }

    void CalmDown() {
       if(demonOn == true) {
            demon = GameObject.FindGameObjectWithTag("Flesh");
            PhotonNetwork.Destroy(demon);
            rend.material.color = Color.white;
            timer = timeOfTimer;
            demonOn = false;
            rageBool = false;
            if(PhotonNetwork.IsMasterClient) {
                var rand = Random.Range(0, CalmsSpawns.Length);
                DontDestroyOnLoad(PhotonNetwork.Instantiate("Prefabs/CalmCard", CalmsSpawns[rand].transform.position, Quaternion.identity));
            }
        }
    }

    void Disturb() {
        if(demonOn == false) {
            rend.material.color = Color.yellow;
            StartCoroutine(DisturbCD());
            IEnumerator DisturbCD() {
                yield return new WaitForSeconds(4);
                timer = timer - disturbRectInt;
                rend.material.color = Color.white;
            }

        }
    }
    void LateUpdate() {
        if(demon == null && rend.material.color == Color.red) {
            rageBool = false;
            rend.material.color = Color.white;
        } else if(demon == null) {
            demon = GameObject.FindGameObjectWithTag("Flesh");
            demonOn = false;
        } 
        if(demon != null){
            if(!rageBool) {
                AudioSource.PlayClipAtPoint(rage, transform.position, 1);
                rageBool = true;
            }
            rend.material.color = Color.red;
            demonOn = true;
        } 

        if (PhotonNetwork.IsMasterClient) {
            if(timer >= 0 && demonOn == false && gameStart) {
                timer -= Time.deltaTime + x;
            } else if(timer < 0 && demonOn == false) {
                var rand = Random.Range(0, demonSpawns.Length);
                demon = PhotonNetwork.Instantiate("Prefabs/"+ demonPrefab.name, demonSpawns[rand].transform.position, Quaternion.identity);
                demon.GetComponent<EmeraldAISystem>().RunSpeed = demonSpeed;
                demon.GetComponent<EmeraldAISystem>().RunAnimationSpeed = demonSpeed/5;
                DontDestroyOnLoad(demon);
                //Debug.LogError(timer + "Spawn");
                demonOn = true;
            }
        }
        //Debug.LogError(timer);
    }
   
}
