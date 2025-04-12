using System;
using UnityEngine;
using UnityEngine.UI;
using Formless.Player.Rebirth;
using System.Collections;

namespace Formless.UI
{
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

        [Header("BossHealth")]
        [SerializeField] private Image _bossHealthBar;
        [SerializeField] private Image _bossHealthLine;
        public float bossHealth;
        public float bossMaxHealth;

        private bool _hasBossKey;

        [SerializeField] private Image _cooldownImage;
        private RebirthTimer _rebirthCooldown;

        [SerializeField] private Text _floorNumberText;


        [SerializeField] private Text _damageText;
        [SerializeField] private Text _speedText;

        [SerializeField] private Text _keysCountText;
        [SerializeField] private Text _bossKeysCountText;
        private int _bombsCount;
        [SerializeField] private Text _bombsCountText;
        //[SerializeField] private Image _durationImage;
        //private RebirthTimer _rebirthDuration;

        public GameObject endPanel;

        //public RebirthTimer RebirthDuration => _rebirthDuration;
        public RebirthTimer RebirthCooldown => _rebirthCooldown;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            //DontDestroyOnLoad(gameObject);
            _rebirthCooldown = new RebirthTimer(_cooldownImage, 30f);
        }

        private void Start()
        {
            _maxCountHearts = _hearts.Length;
            DungeonGenerator.OnDungeonFullGenerated += ShowFloorNumber;
            //_rebirthDuration = new RebirthTimer(_durationImage, 15f);
        }

        private void Update()
        {
            _rebirthCooldown.UpdateTimer(Time.deltaTime);
            //_rebirthDuration.UpdateTimerReverseFill(Time.deltaTime);
        }

        public void StartRebirthCooldown()
        {
            _rebirthCooldown.StartCooldown();
        }

        //public void StartRebirthDuration()
        //{
        //    _rebirthDuration.StartCooldownReverseFill();
        //}

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

        public void UpdateMaxHearts(float maxHealth)
        {
            int requiredHearts = Mathf.CeilToInt(maxHealth / 10f);

            for (int i = 0; i < _hearts.Length; i++)
            {
                if (i < requiredHearts)
                {
                    _hearts[i].gameObject.SetActive(true);
                }
                else
                {
                    _hearts[i].gameObject.SetActive(false);
                }
            }

            UpdateHeartsUI();
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
                _bossKeysCountText.text = 1.ToString();
            }
            else
            {
                _bossKeysCountText.text = 0.ToString();
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
            _keysCountText.text = count.ToString();
        }

        public void PickupBomb()
        {
            _bombsCount += 1;
            UpdateBombsUI();
        }

        public void UseBomb()
        {
            _bombsCount -= 1;
            UpdateBombsUI();
        }

        private void UpdateBombsUI()
        {
            _bombsCountText.text = _bombsCount.ToString();
        }

        public void EndPanelEnable()
        {
            endPanel.SetActive(true);
        }

        public void EndPanelDisable()
        {
            endPanel.SetActive(false);
        }

        public void ShowFloorNumber()
        {
            _floorNumberText.color = new Color(1,1,1,1);
            int floorNumber = DungeonGenerator.Instance.CountDungeons;
            _floorNumberText.text = "FLOOR  " + floorNumber.ToString();
            _floorNumberText.gameObject.SetActive(true);
            StartCoroutine(FadeOutAndDeactivate());
        }

        private IEnumerator FadeOutAndDeactivate()
        {
            Color startColor = _floorNumberText.color;
            float delay = 2f;
            float duration = 2f;
            float timeElapsed = 0f;
            yield return new WaitForSeconds(delay);

            while (timeElapsed < duration)
            {
                timeElapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, timeElapsed / duration);
                _floorNumberText.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

                yield return null;
            }
            _floorNumberText.color = new Color(0,0,0,0);
            
            _floorNumberText.gameObject.SetActive(false);
        }

        public void SetDamageText(float basicDamage, float strongDamage)
        {
            _damageText.text = basicDamage.ToString() + "\n" + strongDamage.ToString();
        }

        public void SetSpeedText(float speed)
        {
            _speedText.text = speed.ToString();
        }
    }
}
