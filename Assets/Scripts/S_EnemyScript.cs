using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_EnemyScript : MonoBehaviour
{
    public S_PlayerMovement plmvmt;

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision){
        if(collision.body.tag == "Player"){
            plmvmt.isInCombat = true;
        }
    }
}
