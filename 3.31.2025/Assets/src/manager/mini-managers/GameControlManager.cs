using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;


public class GameControlManager {

    private GameObject moveObject;

    public GameControlManager() {
        moveObject = GameObject.Find("Move");
    }
    

    public void toggleMovement(bool toggle) {
        moveObject.SetActive(toggle);
    }
}