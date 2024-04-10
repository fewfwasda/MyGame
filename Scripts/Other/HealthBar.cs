using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider heathSlider;
    public TMP_Text healthBarText;
    Damageble playerDamageble;
    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerDamageble = player.GetComponent<Damageble>();
    }
    void Start()
    {
        heathSlider.value = CalculateSliderPercentage(playerDamageble.Health, playerDamageble.MaxHealth);
        healthBarText.text = "HP " + playerDamageble.Health + " / " + playerDamageble.MaxHealth;
    }
    private void OnEnable()
    {
        playerDamageble.healthChanged.AddListener(OnPlayerHealthChanged);
    }
    private void OnDisable()
    {
        playerDamageble.healthChanged.RemoveListener(OnPlayerHealthChanged);
    }
    private float CalculateSliderPercentage(float currenrHealth, float maxHealth)
    {
        return currenrHealth / maxHealth;
    }
    private void OnPlayerHealthChanged(int newHealth, int newMaxHealth)
    {
        heathSlider.value = CalculateSliderPercentage(newHealth, newMaxHealth);
        healthBarText.text = "HP " + newHealth + " / " + newMaxHealth;
    }
}
