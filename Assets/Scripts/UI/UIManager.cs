using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Health and hearts")]
    public float currentHealth;
    private float _maxCountHearts;
    [SerializeField] private Image[] _hearts;
    [SerializeField] private Sprite _fullHeart;
    [SerializeField] private Sprite _halfHeart;
    [SerializeField] private Sprite _emptyHeart;

    [Header("Keys")]
    public int currentCountKeys;
    private int _maxCountKeys;
    [SerializeField] private Image[] _keys;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Уничтожаем дубликат
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _maxCountHearts = _hearts.Length;
        _maxCountKeys = _keys.Length;
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

    public void UpdateKeysUI()
    {
        for (int i = 0; i < _maxCountKeys; i++)
        {
            if (i < currentCountKeys)
            {
                _keys[i].gameObject.SetActive(true);
            }
            else
            {
                _keys[i].gameObject.SetActive(false);
            }
        }
    }


}
