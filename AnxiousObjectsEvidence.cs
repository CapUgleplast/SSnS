using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AnxiousObjectsEvidence : MonoBehaviour
{
    public bool WardrobeTick;
    public AudioClip bangarang;
    public GameObject Wardrobe;
    public Animator WardrobeAnim;
    public float rand;
    public bool PlateRackTick;
    public GameObject PlateRack;
    public Animator PlateRackAnim;
    public GameObject plate;
    private Vector3 targetPlayer;
    public static bool plateAttack = false;

    void Start() {
        if(WardrobeTick) {
            WardrobeAnim = Wardrobe.GetComponent<Animator>();
        }
        if(PlateRackTick) {
            PlateRackAnim = PlateRack.GetComponent<Animator>();
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(gameObject.tag == "Evidence") {
            if(other.tag == "Player" && WardrobeTick) {
                rand = Random.Range(3, 10);
                StartCoroutine(AnxiousWardrobe(rand));
                gameObject.GetComponent<BoxCollider>().enabled = false;
            }
            if(other.tag == "Player" && PlateRackTick && PlateRackAnim.GetBool("Hover") == false) {
                rand = Random.Range(3, 10);
                StartCoroutine(AnxiousPlate(rand));
                gameObject.GetComponent<BoxCollider>().enabled = false;
            }
            if(other.tag == "Player" && PlateRackTick && PlateRackAnim.GetBool("Hover") == true) {
                //targetPlayer = new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z);
                PlateRackAnim.enabled = false;
                gameObject.GetComponent<BoxCollider>().enabled = false;
                plate.GetComponent<BoxCollider>().enabled = true;
                rand = Random.Range(1, 4);
                plateAttack = true;
            }
        }
    }
    
    IEnumerator AnxiousPlate(float rand) {
        yield return new WaitForSeconds(rand);
        PlateRackAnim.SetBool("Hover", true);
        yield return new WaitForSeconds(15); 
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }

    IEnumerator AnxiousWardrobe(float rand) {
        yield return new WaitForSeconds(rand);
        
        WardrobeAnim.SetBool("Bangarang", true);
        AudioSource.PlayClipAtPoint(bangarang, transform.position, 4);
    }

    private void FixedUpdate() {
        if(plateAttack && rand == 1) {
            plate.transform.Translate(Vector3.forward * Time.deltaTime * 12, PlateRack.transform);
        }
        if(plateAttack && rand == 2) {
            plate.transform.Translate(Vector3.back * Time.deltaTime * 12, PlateRack.transform);
        }
        if(plateAttack && rand == 3) {
            plate.transform.Translate(Vector3.right * Time.deltaTime * 12, PlateRack.transform);
        }
        if(plateAttack && rand == 4) {
            plate.transform.Translate(Vector3.left * Time.deltaTime * 12, PlateRack.transform);
        }
    }

}
