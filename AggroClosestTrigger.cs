using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThunderWire.EmeraldAI;

public class AggroClosestTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<BoxCollider>().enabled = true;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player") {
            Debug.LogError("RetargetToClosest");
            transform.parent.GetComponent<EmeralAISendDamage>().ApplyDamage(2, other.transform);
            Debug.LogError("Retargeted");
        }
    }
}
