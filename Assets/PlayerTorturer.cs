using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTorturer : MonoBehaviour {

    void Update() {
        if(Input.GetKeyDown("q")){
            GameObject.FindGameObjectWithTag("Player").GetComponent<EntityStats>().TakeDamage(10);
        }
    }
}
