using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthHUD : MonoBehaviour
{
    private Text text;
    private int currentHealth;
    PlayerHealth player;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerHealth>();
        currentHealth = player.health;
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = player.health;
        text.text = "Health: " + currentHealth;
    }
}
