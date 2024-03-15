using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UI;
using UnityEngine;

public class S_PlayerMovement : MonoBehaviour
{
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

    public S_HealthBarScript healthBar;
    public bool isInCombat;

    void Start(){
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
            currentHealth -= 5f;
            healthBar.SetHealth(currentHealth);
        }
        if(effects.ContainsKey("Bleed")){
            if(effects["Bleed"]==0){
                effects.Remove("Bleed");
            } else {
                if(secondTimer <= 0){
                    effects["Bleed"]-=1;
                    currentHealth -= 2f;
                    healthBar.SetHealth(currentHealth);
                }
            }
        }
        if(secondTimer <= 0){
            secondTimer = 1;
        }
    }
}
