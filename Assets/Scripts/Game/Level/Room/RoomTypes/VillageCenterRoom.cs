using UnityEngine;
using System.Collections;

public class VillageCenterRoom : VillageRoom {

    public Transform GetRightCameraBorder() {
        return this.transform.Find("CameraBorders/RightCameraBorder");
    }
    
    public Transform GetLeftCameraBorder() {
        return this.transform.Find("CameraBorders/LeftCameraBorder");
    }

    public Transform GetUpCameraBorder() {
        return this.transform.Find("CameraBorders/UpCameraBorder");
    }

    public Transform GetDownCameraBorder() {
        return this.transform.Find("CameraBorders/DownCameraBorder");
    }
}
