using UnityEngine;
using UnityEngine.UI;
using System;

[SelectionBase]
public class Player : MonoBehaviour
{
    public static Player Instance {  get; private set; }
    private KnockBack _knockBack;
    private BoxCollider2D _boxCollider2D;
    private Rigidbody2D _rb;

    [Header("Health")]
    [SerializeField] private int _maxHealth;
    private float _currentHealth;

    [Header("Damage")]
    [SerializeField] public float damageBasicAttack = 0.5f;
    [SerializeField] public float damageStrongAttack = 1f;

    [Header("Moving")]
    [SerializeField] private int _movingSpeed = 5;
    private float _minMovingSpeed = 0.1f;

    [Header("Keys")]
    private int _keysCount;

    public event EventHandler OnDie;
    public event EventHandler OnHurt;

    private bool _isRoaming = false;

    private void Awake()
    {
        Instance = this;
        _rb = GetComponent<Rigidbody2D>();
        _knockBack = GetComponent<KnockBack>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        _currentHealth = _maxHealth;
        UIManager.Instance.currentHealth = _currentHealth;
        Debug.Log(UIManager.Instance.currentHealth);
        UIManager.Instance.UpdateHeartsUI();
        UIManager.Instance.currentCountKeys = _keysCount;
        UIManager.Instance.UpdateKeysUI();
    }

    private void FixedUpdate()
    {
        if (_knockBack.IsGetKnockedBack)
            return;

        HandleMovement();
    }

    public bool GetIsWalking()
    {
        return _isRoaming;
    }

    public Vector3 GetPlayerScreenPosition()
    {
        Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        return playerScreenPosition;
    }

    public void TakeDamage(Transform damageSourcePosition, float damage)
    {
        if (_currentHealth <= 0)
            return;

        if (damageSourcePosition == null)
        {
            Debug.LogWarning("Damage source position is null");
            return; // Игнорируем, если позиция источника урона не установлена
        }

        OnHurt?.Invoke(this, EventArgs.Empty);
        _knockBack.GetKnockBack(damageSourcePosition);
        _currentHealth -= damage;
        UIManager.Instance.currentHealth = _currentHealth; // Обновляем состояние здоровья в HealthManager
        UIManager.Instance.UpdateHeartsUI(); // Обновляем UI здоровья
        DetectDeath();
    }

    private void ChangeHealth(int bonusHealth)
    {
        _currentHealth += bonusHealth;
        UIManager.Instance.currentHealth = _currentHealth;
        UIManager.Instance.UpdateHeartsUI();
    }
    
    private void AddKeys()
    {
        _keysCount += 1;
        Debug.LogFormat("ключей стало {0}", _keysCount);
        UIManager.Instance.currentCountKeys = _keysCount;
        UIManager.Instance.UpdateKeysUI();
        //_keysDisplay.text = "KEYS: " + _keysCount;
    }

    private void DetectDeath()
    {
        if (_currentHealth <= 0)
        {
            OnDie?.Invoke(this, EventArgs.Empty);
            PlayerInput.Instance.DisablePlayerInput();
            _boxCollider2D.enabled = false;
            Debug.Log("PLAYER DIED");
        }
    }

    private void HandleMovement()
    {
        Vector2 inputVector = PlayerInput.Instance.GetMovementVector();
        _rb.MovePosition(_rb.position + inputVector * (_movingSpeed * Time.fixedDeltaTime));

        if (Mathf.Abs(inputVector.x) > _minMovingSpeed || Mathf.Abs(inputVector.y) > _minMovingSpeed)
        {
            _isRoaming = true;
        }
        else 
        {
            _isRoaming = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var collectible = collision.GetComponent<Collectible>();
        if (collectible != null && collectible.isCollected) return;

        if (collision.CompareTag("Heart"))
        {
            collectible.isCollected = true;
            ChangeHealth(1);
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Key"))
        {
            collectible.isCollected = true;
            AddKeys();
            Destroy(collision.gameObject);
        }
    }


}
