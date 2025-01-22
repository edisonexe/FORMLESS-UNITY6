using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public float currentHealth;
    private float _maxCountHearts;
    [SerializeField] private Image[] _hearts;
    [SerializeField] private Sprite _fullHeart;
    [SerializeField] private Sprite _halfHeart;
    [SerializeField] private Sprite _emptyHeart;

    private void Start()
    {
        _maxCountHearts = _hearts.Length;
        UpdateHeartsUI();
        Debug.Log(_maxCountHearts);
    }

    public void UpdateHeartsUI()
    {
        for (int i = 0; i < _maxCountHearts; i++)
        {
            if (i < currentHealth - 0.5f) // Полное сердце
            {
                _hearts[i].sprite = _fullHeart;
            }
            else if (i < currentHealth) // Половина сердца
            {
                _hearts[i].sprite = _halfHeart;
            }
            else // Пустое сердце
            {
                _hearts[i].sprite = _emptyHeart;
            }
        }
    }

}
