using UnityEngine;
using Formless.Core;
using Formless.Player.States;
using Formless.Core.Utilties;
using Formless.Core.Managers;
using Formless.Player.Rebirth;
using System.Collections.Generic;

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
        private RebirthController _rebirthController;

        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _maxHealth;
        [SerializeField] public float damageBasicAttack;
        [SerializeField] public float damageStrongAttack;
        public PolygonCollider2D basicAttackCollider;
        public PolygonCollider2D strongAttackCollider;

        private int _keysCount;
        private bool _hasBossKey;
        private int _bombsCount;

        private HashSet<Enemy.Enemy> _damagedEnemies = new HashSet<Enemy.Enemy>();

        public RebirthController RebirthController => _rebirthController;
        public int BombCount => _bombsCount;

        protected override void Awake()
        {
            base.Awake();

            Instance = this;
            //DontDestroyOnLoad(gameObject);

            _inputHandler = new PlayerInputHandler();
            _inputHandler.Enable();

            _rebirthController = GetComponent<RebirthController>();

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

            UIManager.Instance.SetHealthCount(Health);
            UIManager.Instance.SetKeysCount(_keysCount);
        
            if (_rebirthController != null)
            {
                _rebirthController.SetInputHandler(_inputHandler);
            }    
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

        public override void TakeDamage(Transform damageSourcePosition, float damage)
        {
            base.TakeDamage(damageSourcePosition, damage);

            UIManager.Instance.SetHealthCount(Health);

            StateMachine.ChangeState(new PlayerHurtState(this, StateMachine, _inputHandler, _animator));
        }

        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
            UIManager.Instance.SetHealthCount(Health);
            StateMachine.ChangeState(new PlayerHurtState(this, StateMachine, _inputHandler, _animator));
        }

        public void BasicAttackColliderEnable()
        {
            basicAttackCollider.enabled = true;
        }

        public void BasicAttackColliderDisable()
        {
            basicAttackCollider.enabled = false;
        }

        public void StrongAttackColliderEnable()
        {
            strongAttackCollider.enabled = true;
        }

        public void StrongAttackColliderDisable()
        {
            strongAttackCollider.enabled = false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (StateMachine.CurrentState is PlayerAttackState attackState)
            {
                if (collision.transform.TryGetComponent(out Enemy.Enemy enemy))
                {
                    if (!_damagedEnemies.Contains(enemy))
                    {

                        if (attackState.IsBasicAttack)
                        {
                            enemy.TakeDamage(transform, damageBasicAttack);
                        }
                        else if (attackState.IsStrongAttack)
                        {
                            enemy.TakeDamage(transform, damageStrongAttack);
                        }

                        _damagedEnemies.Add(enemy);
                    }
                }
            }
        }

        public void OnAttackAnimationFinished()
        {
            _damagedEnemies.Clear();

            if (StateMachine.CurrentState is PlayerAttackState)
            {
                StateMachine.ChangeState(new PlayerIdleState(this, StateMachine, _inputHandler, _animator));
            }
        }

        public void StartFadeAndDestroy()
        {
            StartCoroutine(Utils.FadeOutAndDestroy(gameObject, _material));
        }

        public void AddHealth()
        {
            Health += 1;

            GameplayManager.Instance.HeartCollected();

            UIManager.Instance.PickupHeart();
        }
    
        public void AddKey()
        {
            _keysCount += 1;

            GameplayManager.Instance.KeyCollected();
            UIManager.Instance.SetKeysCount(_keysCount);
        }

        public void UseKey()
        {
            if ( _keysCount > 0)
            {
                _keysCount--;
                GameplayManager.Instance.SetPlayerKeysCount(_keysCount);
                UIManager.Instance.SetKeysCount(_keysCount);
            }
        }

        public void PickupBossKey()
        {
            _hasBossKey = true;
            GameplayManager.Instance.PickupBossKey();
            UIManager.Instance.HasBossKey();
        }

        public void UseBossKey()
        {
            if (_hasBossKey == true)
            {
                _hasBossKey = false;
                GameplayManager.Instance.UseBossKey();
                UIManager.Instance.UseBossKey();
            }
        }

        public bool IsInteractionPressed()
        {
            return _inputHandler.IsInteractionPressed();
        }

        public bool IsUseBombPressed()
        {
            return _inputHandler.IsUseBombPressed();
        }

        public void UseBomb()
        {
            _bombsCount--;
        }

        public void PickupBomb()
        {
            _bombsCount++;
            Debug.Log($"Количество бомб = {_bombsCount}");
        }
    }
}

