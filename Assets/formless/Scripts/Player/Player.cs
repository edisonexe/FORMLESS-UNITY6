using UnityEngine;
using Formless.Core;
using Formless.Player.States;
using Formless.Core.Animations;
using Formless.Enemy;
using Formless.Enemy.States;
using Formless.Core.Utilties;

namespace Formless.Player
{
    public class Player : Entity
    {
        public static Player Instance { get; private set; }
        public Transform LastDamageSource { get; private set; }


        public BoxCollider2D boxCollider2D;
        public CapsuleCollider2D capsuleCollider2D;
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;
        private Material _material;

        private PlayerInputHandler _inputHandler;

        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _maxHealth;
        private bool _isBasicAttack;
        private bool _isStrongAttack;
        [SerializeField] public float damageBasicAttack;
        [SerializeField] public float damageStrongAttack;
        public PolygonCollider2D basicAttackCollider;
        public PolygonCollider2D strongAttackCollider;

        private bool _isInTeleport = false;

        private int _keysCount;

        protected override void Awake()
        {
            base.Awake();

            Instance = this;
            DontDestroyOnLoad(gameObject);

            _inputHandler = new PlayerInputHandler();
            _inputHandler.Enable();

            _animator = GetComponent<Animator>();
            boxCollider2D = GetComponent<BoxCollider2D>();
            capsuleCollider2D = GetComponent<CapsuleCollider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();

            var basicAttackObject = transform.Find("BasicAttack");
            var strongAttackObject = transform.Find("StrongAttack");

            basicAttackCollider = basicAttackObject.GetComponent<PolygonCollider2D>();
            strongAttackCollider = strongAttackObject.GetComponent<PolygonCollider2D>();
        }

        private void Start()
        {
            StateMachine.ChangeState(new PlayerIdleState(this, StateMachine, _inputHandler, _animator));
            Health = _maxHealth;
            basicAttackCollider.enabled = false;
            strongAttackCollider.enabled = false;
            _material = _spriteRenderer.material;

            UIManager.Instance.currentHealth = Health;
            Debug.Log(UIManager.Instance.currentHealth);
            UIManager.Instance.UpdateHeartsUI();
            UIManager.Instance.currentCountKeys = _keysCount;
            UIManager.Instance.UpdateKeysUI();
        }

        private void FixedUpdate()
        {
            StateMachine.FixedUpdate();
        }

        public void Move(Vector2 moveInput)
        {
            _rb.MovePosition(_rb.position + moveInput * (_moveSpeed * Time.fixedDeltaTime));
        }

        public void ChangePlayerFacingDirection(Vector2 moveInput)
        {
            if (moveInput.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (moveInput.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }

        public void DestroyObject()
        {
            Destroy(gameObject, 1.5f);
        }

        public void BasicAttack()
        {
            Debug.Log("Базовая атака");
        }

        public void StrongAttack()
        {
            Debug.Log("Сильная атака");
        }

        public override void TakeDamage(Transform damageSourcePosition, float damage)
        {
            base.TakeDamage(damageSourcePosition, damage);

            UIManager.Instance.currentHealth = Health;
            UIManager.Instance.UpdateHeartsUI();

            StateMachine.ChangeState(new PlayerHurtState(this, StateMachine, _inputHandler, _animator));
        }

        public void BasicAttackColliderEnable()
        {
            basicAttackCollider.enabled = true;
            _isBasicAttack = true;
        }

        public void BasicAttackColliderDisable()
        {
            basicAttackCollider.enabled = false;
        }

        public void StrongAttackColliderEnable()
        {
            strongAttackCollider.enabled = true;
            _isStrongAttack = true;
        }

        public void StrongAttackColliderDisable()
        {
            strongAttackCollider.enabled = false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Teleport"))
            {
                if (!_isInTeleport)
                {
                    _isInTeleport = true;
                    Debug.Log("Телепорт");
                }
            }

            if (collision.transform.TryGetComponent(out Enemy.Enemy enemy))
            {
                Debug.Log("Столкновение с врагом");
                if (_isBasicAttack)
                {
                    enemy.TakeDamage(transform, damageBasicAttack);
                    _isBasicAttack = false;
                }
                else if (_isStrongAttack) 
                {
                    enemy.TakeDamage(transform,damageStrongAttack);
                    _isStrongAttack = false;
                }
            }
            else
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

        private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Teleport"))
        {
            if (_isInTeleport)
            {
                _isInTeleport = false;
                Debug.Log("Игрок покинул телепорт");
            }
        }
    }

        public void StartFadeAndDestroy()
        {
            StartCoroutine(Utils.FadeOutAndDestroy(gameObject, _material));
        }

        private void ChangeHealth(int bonusHealth)
        {
            Health += bonusHealth;
            UIManager.Instance.currentHealth = Health;
            UIManager.Instance.UpdateHeartsUI();
        }
    
        private void AddKeys()
        {
            _keysCount += 1;
            UIManager.Instance.currentCountKeys = _keysCount;
            UIManager.Instance.UpdateKeysUI();
        }
    }
}

