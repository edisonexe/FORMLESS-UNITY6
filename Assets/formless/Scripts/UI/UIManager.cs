using System;
using UnityEngine;
using UnityEngine.UI;
using Formless.Player.Rebirth;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }


    [Header("Health and hearts")]
    private float _currentHealth;
    private float _maxCountHearts;
    [SerializeField] private Image[] _hearts;
    [SerializeField] private Sprite _fullHeart;
    [SerializeField] private Sprite _halfHeart;
    [SerializeField] private Sprite _emptyHeart;

    [Header("Keys")]
    private int _currentCountKeys;
    private int _maxCountKeys;
    [SerializeField] private Image[] _keys;

    [Header("BossHealth")]
    [SerializeField] private Image _bossHealthBar;
    [SerializeField] private Image _bossHealthLine;
    public float bossHealth;
    public float bossMaxHealth;

    [SerializeField] private Image _bossKey;
    private bool _hasBossKey;

    [SerializeField] private Image _cooldownImage;
    private RebirthTimer _rebirthCooldown;

    [SerializeField] private Image _durationImage;
    private RebirthTimer _rebirthDuration;
    public RebirthTimer RebirthDuration => _rebirthDuration;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _maxCountHearts = _hearts.Length;
        _maxCountKeys = _keys.Length;
        _rebirthCooldown = new RebirthTimer(_cooldownImage, 5f);
        _rebirthDuration = new RebirthTimer(_durationImage, 15f);
    }
    private void Update()
    {
        _rebirthCooldown.UpdateTimer(Time.deltaTime);
        _rebirthDuration.UpdateTimerReverseFill(Time.deltaTime);
    }

    public void StartRebirthCooldown()
    {
        _rebirthCooldown.StartCooldown();
    }

    public void StartRebirthDuration()
    {
        _rebirthDuration.StartCooldownReverseFill();
    }

    public bool CanRebirth()
    {
        return _rebirthCooldown.IsCooldownOver();
    }

    public void UpdateHeartsUI()
    {
        for (int i = 0; i < _maxCountHearts; i++)
        {
            int heartHP = i * 10; // Каждое сердце отвечает за 10 HP

            if (_currentHealth >= heartHP + 10) // Полное сердце
            {
                _hearts[i].sprite = _fullHeart;
            }
            else if (_currentHealth >= heartHP + 5) // Половина сердца
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
            if (i < _currentCountKeys)
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
        //Debug.LogFormat("текущее {0} макс {1}", bossHealth, bossMaxHealth);
        _bossHealthLine.fillAmount = bossHealth / bossMaxHealth;
    }

    public void HasBossKey()
    {
        _hasBossKey = true;
    }

    public void UseBossKey()
    {
        _hasBossKey = false;
        UpdateBossKeyUI();
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

    public void PickupHeart()
    {
        _currentHealth += 10;
        UpdateHeartsUI();
    }

    public void SetHealthCount(float count)
    {
        _currentHealth = count;
        UpdateHeartsUI();
    }

    public void SetKeysCount(int count)
    {
        _currentCountKeys = count;
        UpdateKeysUI();
    }
}
