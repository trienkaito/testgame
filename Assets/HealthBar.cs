using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Nhớ thêm thư viện này để sử dụng Image

public class HealthBar : MonoBehaviour
{
    public Image healthBar; // Tham chiếu đến thanh máu
    public float maxHealth = 100f; // Máu tối đa
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        healthBar.fillAmount = currentHealth / maxHealth;
    }
}
