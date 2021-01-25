using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    [SerializeField] EntityStats playerStats;
    [SerializeField] Image healthFill;

    public float playerCurrentHealth;
    public float playerMaxHealth;
    public float playerHealthPercentage;

    void Start() {
        playerMaxHealth = playerStats.maxHealth;
    }

    void Update() {
        playerCurrentHealth = playerStats.currentHealth;

        playerHealthPercentage = (1f/playerMaxHealth) * playerCurrentHealth;
        healthFill.fillAmount = playerHealthPercentage;
    }
}
