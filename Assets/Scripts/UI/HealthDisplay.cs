using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private Sprite fullHeartSprite;
    [SerializeField] private Sprite halfHeartSprite;
    [SerializeField] private Sprite emptyHeartSprite;

    [SerializeField] private GameObject heartPrefab;

    [SerializeField] private Transform heartsPanel; // Панель, где находятся сердечки

    private List<GameObject> hearts = new List<GameObject>();
    private int maxHealth = 50; // Максимальное здоровье
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHearts();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        UpdateHearts();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        UpdateHearts();
    }

    private void UpdateHearts()
    {
        Debug.Log("Обновление UI");
        // Удаляем старые сердечки
        foreach (GameObject heart in hearts)
        {
            Destroy(heart);
        }
        hearts.Clear();

        // Создаем новые сердечки
        for (int i = 0; i < maxHealth / 10; i++)
        {
            GameObject newHeart = Instantiate(heartPrefab, heartsPanel);
            Image heartImage = newHeart.GetComponent<Image>();

            if (i < currentHealth)
            {
                // Полное сердечко
                heartImage.sprite = fullHeartSprite;
            }
            else if (i < currentHealth + 0.5f)
            {
                // Половинка сердечка
                heartImage.sprite = halfHeartSprite;
            }
            else
            {
                // Пустое сердечко
                heartImage.sprite = emptyHeartSprite;
            }

            hearts.Add(newHeart);
        }
    }

}
