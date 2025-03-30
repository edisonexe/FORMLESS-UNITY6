using UnityEngine;
using Formless.SM;
using Formless.Enemy.States;
using Formless.Core;
using UnityEngine.AI;
using Formless.Core.Utilties;
using Formless.UI;
using Formless.Core.Managers;
using System;
using UnityEngine.Rendering.Universal; 
using Formless.Enemy.Projectile;

namespace Formless.Enemy
{
    public class Enemy : Entity
    {
        public NavMeshAgent navMeshAgent { get; private set; }

        [SerializeField] private EnemySO _enemySO;
        
        public BoxCollider2D boxCollider2D;
        public CapsuleCollider2D capsuleCollider2D;
        public Material _material;
        protected SpriteRenderer spriteRenderer;
        protected Animator animator;

        private Light2D lightSource;
        public event Action<Enemy> OnDie;

        public Vector2 startPosition;

        public float patrolDistanceMax;
        public float patrolDistanceMin;
        protected float movingSpeed;
        public float chasingSpeed;
        public float detectionRange;
        public float patrolTimerMax = 4.5f;

        protected bool _isBasicAttack;
        protected bool _isStrongAttack;
        public float damageBasicAttack;
        public float damageStrongAttack;
        public PolygonCollider2D basicAttackCollider;
        public PolygonCollider2D strongAttackCollider;

        public float attackRange = 1.8f;
        public float rangeAttackRange = 6f;
        public bool rangeAttacking = false;
        public GameObject projectilePrefab;
        public Transform projectileSpawnPoint;
        public float projectileSpeed = 5f;

        public float MovingSpeed => movingSpeed;

        protected override void Awake()
        {
            base.Awake();
            animator = GetComponent<Animator>();
            boxCollider2D = GetComponent<BoxCollider2D>();
            capsuleCollider2D = GetComponent<CapsuleCollider2D>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            navMeshAgent.updateRotation = false;
            navMeshAgent.updateUpAxis = false;

            var basicAttackObject = transform.Find("BasicAttack");
            var strongAttackObject = transform.Find("StrongAttack");

            basicAttackCollider = basicAttackObject.GetComponent<PolygonCollider2D>();
            strongAttackCollider = strongAttackObject.GetComponent<PolygonCollider2D>();
        }

        private void Start()
        {
            Health = _enemySO.maxHealth;
            movingSpeed = _enemySO.moveSpeed;
            navMeshAgent.speed = movingSpeed;
            chasingSpeed = movingSpeed * _enemySO.chasingSpeedMultiplier;
            patrolDistanceMax = _enemySO.patrolDistanceMax;
            patrolDistanceMin = _enemySO.patrolDistanceMin;
            detectionRange = _enemySO.detectionRange;
            startPosition = transform.position;

            damageBasicAttack = _enemySO.damageBasicAttack;
            damageStrongAttack = _enemySO.damageStrongAttack;

            basicAttackCollider.enabled = false;
            strongAttackCollider.enabled = false;

            _material = spriteRenderer.material;

            StateMachine.ChangeState(new EnemyIdleState(this, StateMachine, animator));
            lightSource = transform.Find("Light 2D")?.GetComponent<Light2D>();
        }

        private void FixedUpdate()
        {
            StateMachine.FixedUpdate();
        }

        public float BasicAttackDamage => damageBasicAttack;
        public float StrongAttackDamage => damageStrongAttack;

        public virtual bool CanSeePlayer()
        {
            if (Player.Player.Instance == null || !Player.Player.Instance.gameObject.activeInHierarchy) return false;
            return Vector2.Distance(transform.position, Player.Player.Instance.transform.position) <= detectionRange;
        }

        public virtual Vector2 GetRandomPatrolPosition()
        {
            Vector2 randomDirection = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
            float distance = UnityEngine.Random.Range(patrolDistanceMin, patrolDistanceMax);

            return startPosition + randomDirection * distance;
        }

        public virtual void ChangeFacingDirection(Vector2 from, Vector2 to)
        {
            Vector2 direction = (to - from).normalized;
            Vector3 currentScale = transform.localScale;
    
            if (direction.x > 0)
                transform.localScale = new Vector3(Mathf.Abs(currentScale.x), currentScale.y, currentScale.z);
            else if (direction.x < 0)
                transform.localScale = new Vector3(-Mathf.Abs(currentScale.x), currentScale.y, currentScale.z);
        }

        public virtual void LookAtPlayer()
        {
            if (Player.Player.Instance == null) return;
            ChangeFacingDirection(transform.position, Player.Player.Instance.transform.position);
        }

        public virtual void BasicAttackColliderEnable()
        {
            _isBasicAttack = true;
            basicAttackCollider.enabled = true;
        }

        public virtual void BasicAttackColliderDisable()
        {
            basicAttackCollider.enabled = false;
        }

        public virtual void StrongAttackColliderEnable()
        {
            _isStrongAttack = true;
            strongAttackCollider.enabled = true;
        }

        public virtual void StrongAttackColliderDisable()
        {
            strongAttackCollider.enabled = false;
        }

        public void OnAttackAnimationFinished() { }

        public virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Player.Player player))
            {
                if (_isBasicAttack)
                {
                    player.TakeDamage(transform, damageBasicAttack);
                    _isBasicAttack = false;
                }
                else if (_isStrongAttack)
                {
                    player.TakeDamage(transform, damageStrongAttack);
                    _isStrongAttack = false;
                }
            }
        }

        public virtual State GetHurtState()
        {
            return new EnemyHurtState(this, StateMachine, animator);
        }

        public override void TakeDamage(Transform damageSourcePosition, float damage)
        {
            base.TakeDamage(damageSourcePosition, damage);
            basicAttackCollider.enabled = false;
            strongAttackCollider.enabled = false;

            ShowDamageText(damage);
            StateMachine.ChangeState(GetHurtState());
        }

        //public override void TakeDamage(float damage)
        //{
        //    base.TakeDamage(damage);

        //    ShowDamageText(damage);
        //    StateMachine.ChangeState(GetHurtState());
        //}

        private void ShowDamageText(float damage)
        {
            GameObject damageText = Instantiate(PrefabManager.Instance.DamageTextPrefab, transform.position + Vector3.up / 2f, Quaternion.identity);
            damageText.transform.SetParent(transform);
            damageText.GetComponent<DamageText>().Initialize(damage);
        }

        public virtual void StartFadeAndDestroy()
        {
            OnDie?.Invoke(this);
            StartCoroutine(Utils.FadeOutAndDestroy(gameObject, _material));
            StartCoroutine(Utils.FadeLight(lightSource, 3f));
        }

        public void SpawnProjectile()
        {
            if (projectilePrefab != null && projectileSpawnPoint != null)
            {
                // Определяем направление к игроку
                Vector2 direction = (Player.Player.Instance.transform.position - projectileSpawnPoint.position).normalized;

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
                EnemyProjectile projectileScript = projectile.GetComponent<EnemyProjectile>();
                if (projectileScript != null)
                {
                    projectileScript.Initialize(damageBasicAttack); // Передаем урон
                    projectileScript.SetDirection(direction, projectileSpeed); // Задаем направление и скорость
                }
            }
        }

        //public void SpawnProjectile()
        //{
        //    if (projectilePrefab != null && projectileSpawnPoint != null)
        //    {
        //        Vector2 direction = (Player.Player.Instance.transform.position - projectileSpawnPoint.position).normalized;

        //        GameObject projectile = Instantiate(
        //            projectilePrefab,
        //            projectileSpawnPoint.position,
        //            Quaternion.identity
        //        );

        //        // Настройка снаряда
        //        EnemyProjectile projectileScript = projectile.GetComponent<EnemyProjectile>();
        //        if (projectileScript != null)
        //        {
        //            projectileScript.Initialize(damageBasicAttack); // Передаем урон
        //            projectileScript.SetDirection(direction, projectileSpeed); // Задаем направление и скорость
        //        }
        //    }
        //    else
        //    {
        //        Debug.LogError("Projectile prefab or spawn point is not assigned in the Enemy component.");
        //    }
        //}
    }
}
