using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
public class CursedCalm : MonoBehaviourPun, ISaveable {
    private Inventory inventory;
    private MeshRenderer textRenderer;

    [Header("Setup")]
    [Tooltip("Keycard Inventory ID")]
    public int keycardID;

    [Tooltip("Remove Keycard after access granted.")]
    public bool removeCard;

    [Header("Sounds")]
    public AudioClip accessGranted;
    public AudioClip accessDenied;
    public float volume = 1f;

    [Header("Events")]
    public UnityEvent OnAccessGranted;
    public UnityEvent OnAccessDenied;

    private bool granted;
    private bool denied;

    void Awake() {
        inventory = Inventory.Instance;
    }
       
    public void UseObject() {
        if(!denied && !granted) {
            if(inventory.CheckItemInventory(keycardID)) {
                if(accessGranted) {
                    AudioSource.PlayClipAtPoint(accessGranted, transform.position, volume);
                }
                OnAccessGranted.Invoke();
                granted = true;
                denied = false;
                StartCoroutine(AccessGranted());
                if(removeCard) {
                    inventory.RemoveItem(keycardID);
                }
            } else {
                if(accessDenied) { AudioSource.PlayClipAtPoint(accessDenied, transform.position, volume); }
                OnAccessDenied.Invoke();
                StartCoroutine(AccessDenied());
                denied = true;
            }
        }
    }

    IEnumerator AccessGranted() {
        yield return new WaitForSeconds(2);
        granted = false;
    }
    IEnumerator AccessDenied() {
        yield return new WaitForSeconds(2);
        denied = false;
    }

    public Dictionary<string, object> OnSave() {
        return new Dictionary<string, object>
        {
            {"granted", granted }
        };
    }

    public void OnLoad(JToken token) {
        granted = (bool)token["granted"];

        if(granted) {
            OnAccessGranted.Invoke();
        }
    }
}
