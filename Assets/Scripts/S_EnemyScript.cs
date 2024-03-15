using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class S_EnemyScript : MonoBehaviour
{
    public S_PlayerMovement plmvmt;
    public bool turn;
    public string type;

    public Dictionary<string, float> moveset = new Dictionary<string, float>();

    public int chargeCount = 0;
    public float health = 100;
    public string[] logs;

    void Start(){
        moveset["block"] = -5f;
        moveset["attack"] = 5f;
        moveset["strong attack"] = 15f;
        health = 100f;
    }

    // Update is called once per frame
    void Update()
    {
        if(plmvmt.isInCombat){
            if (turn){
                if (chargeCount > 0){
                    chargeCount -=1;
                    if(chargeCount == 0){
                        plmvmt.TakeDamage(30f);
                    }
                } else {
                    List<string> choices = new List<string>(moveset.Keys);
                    string choice;
                    if(health < 20){
                        if(Random.Range(1, 10) > 3){
                            choice = "block";
                        } else choice = "attack";
                    } else {
                        choice = choices[Random.Range(0, 2)];
                    }
                    if(moveset[choice]<0){
                        health-=moveset[choice];
                    } else {
                        plmvmt.TakeDamage(moveset[choice]);
                    }
                }
                turn = false;
            }
        }
    }

    void OnCollisionEnter(Collision collision){
        if(collision.body.tag == "Player"){
            plmvmt.isInCombat = true;
            plmvmt.s1 = this;
        }
    }
}
