using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class EvidenceManager : MonoBehaviour {

    public PhotonView photonMine;

    public DynamicObject door;

    [Header("Evidence Generator")]
    public List<GameObject> evidenceList = new List<GameObject>();

    public string[] EvidencePathsSpawnList;
    public string[] EvidenceAOPathsSpawnList;

    public GameObject[] AnxiousObjectsList;

    #region Spawn Lists
    [HideInInspector]
    public GameObject[] BookShelfSpawnList;

    [HideInInspector]
    public GameObject[] RamHeadSpawnList;

    [HideInInspector]
    public GameObject[] BloodNapkinSpawnList;

    [HideInInspector]
    public GameObject[] FleshSpawnList;

    [HideInInspector]
    public GameObject[] CrossEvidenceSpawnList;

    [HideInInspector]
    public GameObject[] DemonMirrorSpawnList;

    [HideInInspector]
    public GameObject[] DemonPaintingSpawnList;

    [HideInInspector]
    public GameObject[] DemonDollSpawnList;

    [HideInInspector]
    public GameObject[] AnxiousObjectsSpawnList;

    [HideInInspector]
    public GameObject[] RatSpawnList;
    #endregion

    public List<GameObject[]> evidenceSpawnList = new List<GameObject[]>();

    private int rand;
    private int AOrand;
    private int TrueDemon;
    public bool[] evTrueList = new bool[10];
    public bool[] evFalse1List = new bool[10];
    public bool[] evFalse2List = new bool[10];
    public int[] evTrueCells = new int[5];
    public Toggle[] evIRLList;
    public Text demonType;
    public Button AcceptTheory;
    public string[] demonTypesList;
    private int demonFound;
    private bool isAccepted = false;
    public bool noColor = true;


    [Header("Cursed Objects Generator")]
    public Color curseColor;
    public GameObject cursedLight;
    public GameObject IRscanLight;
    Vector3 cursLightVector;

    GameObject[] CursedObjectsSpawnA;
    GameObject[] CursedObjectsSpawnB;
    GameObject[] CursedObjectsSpawnC;
    public List<GameObject[]> CursedObjectsSpawnList = new List<GameObject[]>();
    public GameObject[] CursedObjectsList;

    [Header("CalmBox Spawner")]
    public GameObject CalmBox;
    public GameObject[] CalmBoxSpawnList;


    private void Start() {
        photonMine = PhotonView.Get(this);
        Random.InitState(System.DateTime.Now.Millisecond);

        Debug.LogError("start");

    }

    public void BindToggleToListener() {
        foreach(var t in evIRLList) {
            t.onValueChanged.AddListener(EvListCompare);
        }
        Debug.LogError("bind");
    }

    [PunRPC]
    List<GameObject[]> EvidenceSpawnsStack() {

        BookShelfSpawnList = GameObject.FindGameObjectsWithTag("BSSpawn");
        evidenceSpawnList.Add(BookShelfSpawnList);

        RamHeadSpawnList = GameObject.FindGameObjectsWithTag("RHSpawn");
        evidenceSpawnList.Add(RamHeadSpawnList);

        BloodNapkinSpawnList = GameObject.FindGameObjectsWithTag("BNSpawn");
        evidenceSpawnList.Add(BloodNapkinSpawnList);

        FleshSpawnList = GameObject.FindGameObjectsWithTag("FSpawn");
        evidenceSpawnList.Add(FleshSpawnList);

        CrossEvidenceSpawnList = GameObject.FindGameObjectsWithTag("CESpawn");
        evidenceSpawnList.Add(CrossEvidenceSpawnList);

        DemonMirrorSpawnList = GameObject.FindGameObjectsWithTag("DMSpawn");
        evidenceSpawnList.Add(DemonMirrorSpawnList);

        DemonPaintingSpawnList = GameObject.FindGameObjectsWithTag("DPSpawn");
        evidenceSpawnList.Add(DemonPaintingSpawnList);

        DemonDollSpawnList = GameObject.FindGameObjectsWithTag("DDSpawn");
        evidenceSpawnList.Add(DemonDollSpawnList);

        RatSpawnList = GameObject.FindGameObjectsWithTag("RSpawn");
        evidenceSpawnList.Add(RatSpawnList);

        AnxiousObjectsSpawnList = GameObject.FindGameObjectsWithTag("AOSpawn");
        evidenceSpawnList.Add(AnxiousObjectsSpawnList);

        CalmBoxSpawnList = GameObject.FindGameObjectsWithTag("CalmBoxSpawn");

        CursedObjectsSpawnA = GameObject.FindGameObjectsWithTag("CursObjsSpawnA");
        CursedObjectsSpawnList.Add(CursedObjectsSpawnA);
        CursedObjectsSpawnB = GameObject.FindGameObjectsWithTag("CursObjsSpawnB");
        CursedObjectsSpawnList.Add(CursedObjectsSpawnB);
        CursedObjectsSpawnC = GameObject.FindGameObjectsWithTag("CursObjsSpawnC");
        CursedObjectsSpawnList.Add(CursedObjectsSpawnC);
        IRscanLight = GameObject.FindGameObjectWithTag("IRscanner");

        Debug.LogError("spawnlist");
        return evidenceSpawnList;
    }

    void EvListCompare(bool _) {
        Debug.LogError("compare");

        bool equalEvs = true;
        for(int i = 0; i < 10; i++) {
            if(evIRLList[i].isOn != evTrueList[i]) {
                equalEvs = false;
            }
        }
        if(equalEvs) {
            demonType.text = demonTypesList[0];
            AcceptTheory.gameObject.SetActive(true);
            demonFound = 0;
            return;
        }

        equalEvs = true;
        for(int i = 0; i < 10; i++) {
            if(evIRLList[i].isOn != evFalse1List[i]) {
                equalEvs = false;
            }
        }
        if(equalEvs) {
            demonType.text = demonTypesList[1];
            AcceptTheory.gameObject.SetActive(true);
            demonFound = 1;
            return;
        }

        equalEvs = true;
        for(int i = 0; i < 10; i++) {
            if(evIRLList[i].isOn != evFalse2List[i]) {
                equalEvs = false;
            }
        }
        if(equalEvs) {
            demonType.text = demonTypesList[2];
            AcceptTheory.gameObject.SetActive(true);
            demonFound = 2;
            return;
        }
        demonType.text = "";
        AcceptTheory.gameObject.SetActive(false);
    }

    void GenerateEvidences() {
        Debug.LogError("rpcStart");

        photonMine.RPC("RPCGenerateEvidences", RpcTarget.MasterClient);
        photonMine.RPC("RPCCloseDoor", RpcTarget.All);//Photon

        Debug.LogError("rpcend");
    }
    [PunRPC]
    public void RPCCloseDoor() {
        door.CloseDoors();
        door.isLocked = true;
    }

    [PunRPC]
    public void RPCGenerateEvidences() {
        Debug.LogError("rpcStart");

        door.CloseDoors();
        door.isLocked = true;
        //SceneManager.UnloadSceneAsync("Shop");

        TrueDemon = (int)Mathf.Ceil(Random.Range(-1f, demonTypesList.Length - 1));
        string SwapDemon;
        SwapDemon = demonTypesList[0];
        demonTypesList[0] = demonTypesList[TrueDemon];
        demonTypesList[TrueDemon] = SwapDemon;
        Debug.LogError("demon");


        AOrand = (int)Mathf.Ceil(Random.Range(-1f, AnxiousObjectsList.Length - 1));
        EvidencePathsSpawnList[9] = EvidenceAOPathsSpawnList[AOrand];
        foreach(var t in AnxiousObjectsList) {
            if(t != AnxiousObjectsList[AOrand]) {
                t.SetActive(false);
            }
        }
        Debug.LogError("AO");
        photonMine.RPC("EvidenceSpawnsStack", RpcTarget.All); //Photon
                                                              //EvidenceSpawnsStack();

        for(int i = 0; i < EvidencePathsSpawnList.Length; i++) {
            rand = (int)Mathf.Ceil(Random.Range(-1f, evidenceSpawnList[i].Length - 1));
            GameObject evidence = PhotonNetwork.Instantiate("Prefabs/" + EvidencePathsSpawnList[i], evidenceSpawnList[i][rand].transform.position, Quaternion.identity);
            
            photonMine.RPC("EvlistSynch", RpcTarget.All, EvidencePathsSpawnList[i]); //Photon
        }
        Debug.LogError("evidencelist");


        var j = 0;
        while(j < 5) {
            rand = (int)Mathf.Ceil(Random.Range(-1f, evidenceList.Count - 1));
            if(evTrueList[rand] != true) {
                photonMine.RPC("TrueEvsAssignment", RpcTarget.All, rand); //Photon
                j++;
                Debug.LogError("trueEv " + (rand + 1));
            }
        }


        j = 0;
        while(j < 5) {
            rand = (int)Mathf.Ceil(Random.Range(-1f, evidenceList.Count - 1));
            if(evFalse1List[rand] != true) {
                photonMine.RPC("FalseEvs1Assignment", RpcTarget.All, rand); //Photon
                j++;
                //Debug.LogError("FalseEv1 " + (rand + 1));
            }
        }


        j = 0;
        while(j < 5) {
            rand = (int)Mathf.Ceil(Random.Range(-1f, evidenceList.Count - 1));
            if(evFalse2List[rand] != true /*|| evFalse2List[rand] != evFalse1List[rand]*/) {
                photonMine.RPC("FalseEvs2Assignment", RpcTarget.All, rand); //Photon
                j++;
                //Debug.LogError("FalseEv2 " + (rand + 1));
            }
        }

        PunCalmBoxSpawn();
        CursedManager.gameStart = true;
        //evFalse1List = (bool[])evTrueList.Clone();
    }

    [PunRPC]
    void EvlistSynch(string EvPaths) {
        DontDestroyOnLoad(GameObject.Find(EvPaths + "(Clone)"));
        evidenceList.Add(GameObject.Find(EvPaths + "(Clone)"));
    }

    [PunRPC]
    void TrueEvsAssignment(int rand) {
        evidenceList[rand].tag = "Evidence";
        evTrueList[rand] = true;
    }

    [PunRPC]
    void FalseEvs1Assignment(int rand) {
        evFalse1List[rand] = true;
    }

    [PunRPC]
    void FalseEvs2Assignment(int rand) {
        evFalse2List[rand] = true;
    }

    public void AcceptDemon() {
        if((demonType.text == demonTypesList[0] || demonType.text == demonTypesList[1] || demonType.text == demonTypesList[2]) && !isAccepted) {
            photonMine.RPC("DemonAccepted", RpcTarget.All); //Photon
            photonMine.RPC("CursedObjsSpawn", RpcTarget.MasterClient); //Photon
        }
    }

    [PunRPC]
    void DemonAccepted() {
        isAccepted = true;
        Debug.LogError("Accepted");
    }


    [PunRPC]
    void CursedObjsSpawn() {
        for(int i = 0; i < CursedObjectsSpawnList[0].Length; i++) {
            GameObject cursObjA = PhotonNetwork.Instantiate("Prefabs/" + CursedObjectsList[0].name, CursedObjectsSpawnList[0][i].transform.position, Quaternion.identity);
            DontDestroyOnLoad(cursObjA);
            //CursedObjectsSpawnList[0][i] = cursObjA;
            GameObject cursObjB = PhotonNetwork.Instantiate("Prefabs/" + CursedObjectsList[1].name, CursedObjectsSpawnList[1][i].transform.position, Quaternion.identity);
            DontDestroyOnLoad(cursObjB);
            //CursedObjectsSpawnList[1][i] = cursObjB;
            GameObject cursObjC = PhotonNetwork.Instantiate("Prefabs/" + CursedObjectsList[2].name, CursedObjectsSpawnList[2][i].transform.position, Quaternion.identity);
            DontDestroyOnLoad(cursObjC);
            // CursedObjectsSpawnList[2][i] = cursObjC;
            //cursObj.transform.SetParent(CursedObjectsSpawnList[i].transform);
        }
        photonMine.RPC("FindCursObjs", RpcTarget.All);

        rand = (int)Mathf.Ceil(Random.Range(-1f, demonTypesList.Length - 1));
        if(rand == 0) {
            curseColor = Color.red;
        }
        if(rand == 1) {
            curseColor = new Color(0.6f, 0, 1, 1);
        }
        if(rand == 2) {
            curseColor = new Color(0, 1, 0.36f, 1); ;
        }

        cursLightVector = new Vector3(curseColor.r, curseColor.g, curseColor.b);
        photonMine.RPC("ColorChange", RpcTarget.All, cursLightVector);

        for(int j = 0; j < CursedObjectsSpawnList.Count; j++) {
            rand = (int)Mathf.Ceil(Random.Range(-1f, CursedObjectsSpawnList[j].Length - 1));
            photonMine.RPC("CursedObjAssignment", RpcTarget.All, j, rand, TrueDemon);
        }

    }

    [PunRPC]
    void FindCursObjs() {
        for(int i = 0; i < CursedObjectsSpawnList.Count; i++) {
            CursedObjectsSpawnList[i] = GameObject.FindGameObjectsWithTag("CursObj" + (i + 1));
            foreach(var t in CursedObjectsSpawnList[i]) {
                t.tag = "FalseCursObj";
            }
        }

    }
    [PunRPC]
    void ColorChange(Vector3 cursLightVector) {
        Debug.LogError("ColorChange");
        cursedLight.GetComponent<Light>().color = new Color(cursLightVector.x, cursLightVector.y, cursLightVector.z, 1);
        cursedLight.SetActive(true);
        IRscanLight = GameObject.FindGameObjectWithTag("IRscanner");
        IRscanLight.GetComponent<Light>().color = new Color(cursLightVector.x, cursLightVector.y, cursLightVector.z, 1);
        Debug.LogError("ColorChanged");
    }

    [PunRPC]
    void CursedObjAssignment(int j, int rand, int TrueDemon) {
        CursedObjectsSpawnList[j][rand].transform.GetChild(0).gameObject.layer = 6;
        if(j == TrueDemon) {
            CursedObjectsSpawnList[j][rand].tag = "TrueCursObj";
        }
    }

    public void PunCalmBoxSpawn() {
        photonMine.RPC("CalmBoxSpawn", RpcTarget.MasterClient); //Photon
    }

    [PunRPC]
    void CalmBoxSpawn() {
        rand = (int)Mathf.Ceil(Random.Range(-1f, CalmBoxSpawnList.Length - 1));
        DontDestroyOnLoad(PhotonNetwork.Instantiate("Prefabs/" + CalmBox.name, CalmBoxSpawnList[rand].transform.position, Quaternion.identity));

    }


}
