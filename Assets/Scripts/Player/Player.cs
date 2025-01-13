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
    [SerializeField] private Text _healthDisplay;
    private int _currentHealth;

    [Header("Moving")]
    [SerializeField] private int _movingSpeed = 5;
    private float _minMovingSpeed = 0.1f;

    [Header("Keys")]
    [SerializeField] private Text _keysDisplay;
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

    public void TakeDamage(Transform damageSourcePosition, int damage)
    {
        if (_currentHealth <= 0)
            return;

        OnHurt?.Invoke(this, EventArgs.Empty);
        _knockBack.GetKnockBack(damageSourcePosition);
        _currentHealth -= damage;
        _healthDisplay.text = "HP: " + _currentHealth;
        DetectDeath();
    }

    private void ChangeHealth(int bonusHealth)
    {
        _currentHealth += bonusHealth;
        _healthDisplay.text = "HP: " + _currentHealth;
    }
    
    private void AddKeys()
    {
        _keysCount += 1;
        _keysDisplay.text = "KEYS: " + _keysCount;
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
        if (collision.CompareTag("Heart"))
        {
            ChangeHealth(5);
            Destroy(collision.gameObject); ;
        }
        else if (collision.CompareTag("Key"))
        {
            AddKeys();
            Destroy(collision.gameObject);
        }
    }
}
