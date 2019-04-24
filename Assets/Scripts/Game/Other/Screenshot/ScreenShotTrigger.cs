using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ScreenShotComponent))]
public class ScreenShotTrigger : MonoBehaviour {

    void Start() {}

    public void OnTriggerEnter(Collider coll) {
        Player player = coll.gameObject.GetComponent<Player>();
        if(player) {
             GetComponent<ScreenShotComponent>().SaveScreenShot();
        }
    }
}
