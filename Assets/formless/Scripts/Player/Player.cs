using UnityEngine;
using Formless.Core;
using Formless.Player.States;
using Formless.Core.Utilties;
using Formless.Core.Managers;
using Formless.Player.Rebirth;
using System.Collections.Generic;
using Formless.Items;
using Formless.UI;

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

        public PlayerInputHandler inputHandler;
        private RebirthController _rebirthController;
        private SphereSystem _sphereSystem;

        [SerializeField] private float _movingSpeed = 2.5f;
        [SerializeField] private float _maxHealth;
        [SerializeField] private float _damageBasicAttack = 10f;
        [SerializeField] private float _damageStrongAttack = 15f;
        public PolygonCollider2D basicAttackCollider;
        public PolygonCollider2D strongAttackCollider;

        private int _keysCount;
        private bool _hasBossKey;
        private int _bombsCount;

        private HashSet<Enemy.Enemy> _damagedEnemies = new HashSet<Enemy.Enemy>();

        public RebirthController RebirthController => _rebirthController;
        public int BombCount => _bombsCount;
        public float MovingSpeed => _movingSpeed;
        public float DamageBasicAttack => _damageBasicAttack;
        public float DamageStrongAttack => _damageStrongAttack;

        protected override void Awake()
        {
            base.Awake();

            Instance = this;
            //DontDestroyOnLoad(gameObject);

            inputHandler = new PlayerInputHandler();
            //inputHandler.Enable();

            _rebirthController = GetComponent<RebirthController>();
            _sphereSystem = GetComponent<SphereSystem>();

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
            StateMachine.ChangeState(new PlayerIdleState(this, StateMachine, inputHandler, _animator));
            Health = _maxHealth;
            basicAttackCollider.enabled = false;
            strongAttackCollider.enabled = false;
            _material = _spriteRenderer.material;

            UIManager.Instance.SetHealthCount(Health);
            UIManager.Instance.SetKeysCount(_keysCount);
        
            if (_rebirthController != null)
            {
                _rebirthController.SetInputHandler(inputHandler);
            }

            // Потом переделать
            //_sphereSystem.AddOrb();
            //_sphereSystem.AddOrb();
            //_sphereSystem.AddOrb();
            //_sphereSystem.AddOrb();


        }

        private void FixedUpdate()
        {
            StateMachine.FixedUpdate();
        }

        public void Move(Vector2 moveInput)
        {
            _rb.MovePosition(_rb.position + moveInput * (_movingSpeed * Time.fixedDeltaTime));
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
        
        public void SetMovingSpeed(float speed)
        {
            _movingSpeed = speed;
            Debug.Log($"Нынешняя скорость игрока: {_movingSpeed} ");
        }

        public void SetBasicAttackDamage(float damage)
        {
            _damageBasicAttack = damage;
            Debug.Log($"Нынешний урон обыч. атаки игрока: {_damageBasicAttack} ");
        }

        public void SetStrongAttackDamage(float damage)
        {
            Debug.Log($"Нынешний урон сильн. атаки игрока: {_damageStrongAttack} ");
        }

        public override void TakeDamage(Transform damageSourcePosition, float damage)
        {
            base.TakeDamage(damageSourcePosition, damage);

            UIManager.Instance.SetHealthCount(Health);

            StateMachine.ChangeState(new PlayerHurtState(this, StateMachine, inputHandler, _animator));
        }

        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
            UIManager.Instance.SetHealthCount(Health);
            StateMachine.ChangeState(new PlayerHurtState(this, StateMachine, inputHandler, _animator));
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

        public void OnAttackAnimationFinished()
        {
            _damagedEnemies.Clear();

            if (StateMachine.CurrentState is PlayerAttackState)
            {
                StateMachine.ChangeState(new PlayerIdleState(this, StateMachine, inputHandler, _animator));
            }
        }

        public void StartFadeAndDestroy()
        {
            StartCoroutine(Utils.FadeOutAndDestroy(gameObject, _material));
        }

        public void AddHealth()
        {
            Health += 10;

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
            return inputHandler.IsInteractionPressed();
        }

        public bool IsUseBombPressed()
        {
            return inputHandler.IsUseBombPressed();
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
                            enemy.TakeDamage(transform, _damageBasicAttack);
                        }
                        else if (attackState.IsStrongAttack)
                        {
                            enemy.TakeDamage(transform, _damageStrongAttack);
                        }

                        _damagedEnemies.Add(enemy);
                    }
                }
            }
        }
    }
}

