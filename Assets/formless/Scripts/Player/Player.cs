using UnityEngine;
using Formless.Core;
using Formless.Player.States;
using Formless.Core.Utilties;
using Formless.Core.Managers;
using Formless.Player.Rebirth;
using System.Collections.Generic;
using Formless.Items;
using Formless.UI;
using Formless.Enemy.Projectile;
using Formless.Player.Projectile;
using Formless.Audio;

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
        [SerializeField] private float _damageBasicAttack = 10f;
        [SerializeField] private float _damageStrongAttack = 15f;
        [SerializeField] private float _damageRangeAttack = 5f;
        public PolygonCollider2D basicAttackCollider;
        public PolygonCollider2D strongAttackCollider;
        public bool IsMoving { get; set;} = false;

        private float _attackCooldown = 0.3f;
        private float _attackCooldownTimer;

        // Дальняя атака
        public GameObject projectilePrefab;
        public Transform projectileSpawnPoint;
        public bool rangeAttacking = false;
        public float projectileSpeed = 5f;

        public bool CanAttack => _attackCooldownTimer <= 0f;

        // Кол-во предметов
        private int _keysCount;
        private bool _hasBossKey;
        private int _bombsCount;

        private HashSet<Enemy.Enemy> _damagedEnemies = new HashSet<Enemy.Enemy>(8);

        public RebirthController RebirthController => _rebirthController;
        public int BombCount => _bombsCount;
        public float MovingSpeed => _movingSpeed;
        public float DamageBasicAttack => _damageBasicAttack;
        public float DamageStrongAttack => _damageStrongAttack;

        protected override void Awake()
        {
            base.Awake();

            Instance = this;

            inputHandler = new PlayerInputHandler();

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
            Health = MaxHealth;
            basicAttackCollider.enabled = false;
            strongAttackCollider.enabled = false;
            _material = _spriteRenderer.material;

            UIManager.Instance.UpdateMaxHearts(MaxHealth);
            UIManager.Instance.SetHealthCount(Health);
            UIManager.Instance.SetKeysCount(_keysCount);
            UIManager.Instance.SetDamageText(DamageBasicAttack, DamageStrongAttack);
            UIManager.Instance.SetSpeedText(MovingSpeed);

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

        public void ResetAttackCooldown()
        {
            _attackCooldownTimer = _attackCooldown;
        }

        protected override void Update()
        {
            base.Update();
            if (_attackCooldownTimer > 0f)
            {
                _attackCooldownTimer -= Time.deltaTime;
            }
        }


        public void RCSetInputHandler()
        {
            if (_rebirthController != null)
            {
                _rebirthController.SetInputHandler(inputHandler);
            }
        }


        public void Move(Vector2 moveInput)
        {
            IsMoving = true;
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
            Debug.Log($"Старая скорость {_movingSpeed}");
            _movingSpeed = speed;
            Debug.Log($"Новая скорость: {_movingSpeed} ");
            UIManager.Instance.SetSpeedText( _movingSpeed );
        }

        public void SetBasicAttackDamage(float damage)
        {
            _damageBasicAttack = damage;
            //Debug.Log($"Нынешний урон обыч. атаки игрока: {_damageBasicAttack} ");
        }

        public void SetStrongAttackDamage(float damage)
        {
            _damageStrongAttack = damage;
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
            AudioManager.Instance.PlaySound(AudioManager.Instance.findItem);
            Health += 10;

            GameplayManager.Instance.HeartCollected();

            UIManager.Instance.PickupHeart();
        }
    
        public void AddKey()
        {
            AudioManager.Instance.PlaySound(AudioManager.Instance.findItem);
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
            AudioManager.Instance.PlaySound(AudioManager.Instance.findItem);
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
            UIManager.Instance.UseBomb();
        }

        public void PickupBomb()
        {
            AudioManager.Instance.PlaySound(AudioManager.Instance.findItem);
            _bombsCount++;
            UIManager.Instance.PickupBomb();

            //Debug.Log($"Количество бомб = {_bombsCount}");
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

        public void SpawnProjectile()
        {
            if (projectilePrefab != null && projectileSpawnPoint != null)
            {
                // Получаем позицию курсора мыши в мировых координатах
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0; // Убедимся, что z-координата равна 0 (для 2D)

                // Определяем направление от точки спавна снаряда к курсору
                Vector2 direction = (mousePosition - projectileSpawnPoint.position).normalized;

                // Создаем снаряд в точке спавна
                GameObject projectile = Instantiate(
                    projectilePrefab,
                    projectileSpawnPoint.position, // Позиция спавна снаряда
                    Quaternion.identity
                );

                // Разворачиваем снаряд в направлении цели
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Вычисляем угол
                projectile.transform.rotation = Quaternion.Euler(0, 0, angle); // Применяем поворот

                // Настройка снаряда
                PlayerProjectile projectileScript = projectile.GetComponent<PlayerProjectile>();
                if (projectileScript != null)
                {
                    projectileScript.Initialize(_damageRangeAttack); // Передаем урон
                    projectileScript.SetDirection(direction, projectileSpeed); // Задаем направление и скорость
                }

                if (StateMachine.CurrentState is PlayerAttackState)
                {
                    StateMachine.ChangeState(new PlayerIdleState(this, StateMachine, inputHandler, _animator));
                }
            }
        }
    }
}

