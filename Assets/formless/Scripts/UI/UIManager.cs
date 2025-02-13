using System;
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

    [Header("BossHealth")]
    [SerializeField] private Image _bossHealthBar;
    [SerializeField] private Image _bossHealthLine;
    public float bossHealth;
    public float bossMaxHealth;

    [SerializeField] private Image _bossKey;
    private bool _hasBossKey;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // ���������� ��������
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
            if (i < currentHealth - 0.5f) // ������ ������
            {
                _hearts[i].sprite = _fullHeart;
            }
            else if (i < currentHealth) // �������� ������
            {
                _hearts[i].sprite = _halfHeart;
            }
            else // ������ ������
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

    public void EnableBossHealthBar()
    {
        _bossHealthBar.gameObject.SetActive(true);
        UpdateBossHealthBar();
    }

    public void DisableBossHealthBar()
    {
        _bossHealthBar.gameObject.SetActive(false);
    }

    public void UpdateBossHealthBar()
    {
        Debug.LogFormat("������� {0} ���� {1}", bossHealth, bossMaxHealth);
        _bossHealthLine.fillAmount = bossHealth / bossMaxHealth;
    }

    public void UseKey()
    {
        currentCountKeys -= 1;
    }

    public void HasBossKey()
    {
        _hasBossKey = true;
    }

    public void UseBossKey()
    {
        _hasBossKey = false;
    }

    public void UpdateBossKeyUI()
    {
        if (_hasBossKey)
        {
            _bossKey.gameObject.SetActive(true);
        }
        else
        {
            _bossKey.gameObject.SetActive(false);
        }
    }
}
