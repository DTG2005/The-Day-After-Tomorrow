using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UI;
using UnityEngine;
using UnityEngineInternal;

public class S_PlayerMovement : MonoBehaviour
{
    char mode = 'N';
    Dictionary<string, float> moveset = new Dictionary<string, float>();
    public CharacterController controller;
    public GameObject cam;
    public GameObject combatCam;
    public float speed = 6f;
    public float smoothTime = 0.1f;
    float smoothVelocity;
    public float maxHealth = 100;
    float secondTimer = 1f;
    public float currentHealth;
    public Dictionary<string, int> effects = new Dictionary<string, int>();

    public int charge = 0;

    public S_HealthBarScript healthBar;
    public bool isInCombat;
    public S_EnemyScript s1;

    public void TakeDamage(float dmg){
        currentHealth-= dmg;
        healthBar.SetHealth(currentHealth);
    }

    void Start(){
        moveset["block"] = -5f;
        moveset["attack"] = 20f;
        moveset["strong attack"] = 40f;

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        combatCam.SetActive(false);
    }

    void Update()
    {
        secondTimer-=Time.deltaTime;
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(isInCombat){
            controller.enabled = false;
            cam.SetActive(false);
            combatCam.SetActive(true);
            if(!s1.turn){
                if (charge == 0){
                    if(Input.GetKeyDown(KeyCode.Alpha0)){
                        currentHealth +=5f;
                        s1.turn = true;
                    } else if (Input.GetKeyDown(KeyCode.Alpha1)){
                        s1.health -= 20f;
                        s1.turn = true;
                    } else if (Input.GetKeyDown(KeyCode.Alpha2)){
                        s1.turn = true;
                        charge = 1;
                    }
                } else{
                    charge -=1;
                    if(charge == 0){
                        s1.health -= 40f;
                        s1.turn = true;
                    }
                }
                Debug.Log(s1.health);
            }
        } else if(combatCam.activeInHierarchy){
            combatCam.SetActive(false);
            cam.SetActive(true);
        }

        if(currentHealth <=20f){
            effects["Bleed"] = -1;
        }

        if (direction.magnitude>=0.1f){
            float targetAngle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothVelocity, smoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f); 
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDirection.normalized * speed * Time.deltaTime);
        }
        // TODO: Whenever the player takes damage in the future, subtract from the currentHealth float and call the healthBar.SetHealth()
        if(Input.GetKeyDown(KeyCode.Space)){
            TakeDamage(5f);
        }
        if(effects.ContainsKey("Bleed")){
            if(effects["Bleed"]==0){
                effects.Remove("Bleed");
            } else {
                if(secondTimer <= 0){
                    effects["Bleed"]-=1;
                    TakeDamage(2f);
                }
            }
        }
        if(secondTimer <= 0){
            secondTimer = 1;
        }
    }
}
