using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, Killable, Damageable<int>
{

    //[SerializeField]
    public  int health { get; set; }

    [Range(3, 10)]
    [SerializeField]
    private int maxHealth = 3;

    void Start()
    {
        health = maxHealth; // This very likely needs to change but how does serialization work lol   
    }

    public void kill()
    {
        health = 0;
        Debug.Log("you died");
    }

    public void damage(int amount)
    {
        health -= Mathf.Clamp(0, health, amount);
        if(health == 0)
        {
            kill();
        }
    }

}
