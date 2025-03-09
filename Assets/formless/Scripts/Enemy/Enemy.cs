using UnityEngine;
using Formless.SM;
using Formless.Enemy.States;
using Formless.Core;
using UnityEngine.AI;
using Formless.Core.Utilties;
using Formless.UI;
using Formless.Core.Managers;
using System;

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

        public event Action<Enemy> OnDie;

        public Vector2 startPosition;

        public float patrolDistanceMax;
        public float patrolDistanceMin;
        public float movingSpeed;
        public float chasingSpeed;
        public float detectionRange;
        public float attackRange = 1f;
        public float patrolTimerMax = 4.5f;

        protected bool _isBasicAttack;
        protected bool _isStrongAttack;
        public float damageBasicAttack;
        public float damageStrongAttack;
        public PolygonCollider2D basicAttackCollider;
        public PolygonCollider2D strongAttackCollider;

        private bool _isHitThisAttack = false;
        public bool IsHitThisAttack => _isHitThisAttack;

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
        }

        private void FixedUpdate()
        {
            StateMachine.FixedUpdate();
        }

        public virtual bool CanSeePlayer()
        {
            if (Player.Player.Instance == null) return false;
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

            _isHitThisAttack = true;

            ShowDamageText(damage);
            StateMachine.ChangeState(GetHurtState());
        }

        public void ResetIsHitThisAttack()
        {
            _isHitThisAttack = false;
        }

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
        }
    }
}
