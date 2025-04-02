using UnityEngine;
using Formless.Boss.States;
using Formless.SM;
using System;
using Formless.Core.Utilties;
using Formless.UI;

namespace Formless.Boss
{
    public class Boss : Enemy.Enemy
    {
        [SerializeField] private BossSO _bossSO;

        public new event Action<Boss> OnDie;
        public float damageSpecialAttack;
        public PolygonCollider2D specialAttackFirstCollider;
        public PolygonCollider2D specialAttackSecondCollider;

        protected override void Awake()
        {
            base.Awake();
            var specialAttackObject = transform.Find("SpecialAttack");
            var specialAttackFirstObject = specialAttackObject.Find("SpecialAttackFirst");
            var specialAttackSecondObject = specialAttackObject.Find("SpecialAttackSecond");
            specialAttackFirstCollider = specialAttackFirstObject.GetComponent<PolygonCollider2D>();
            specialAttackSecondCollider = specialAttackSecondObject.GetComponent<PolygonCollider2D>();
        }

        private void Start()
        {
            Health = _bossSO.maxHealth;
            movingSpeed = _bossSO.moveSpeed;
            navMeshAgent.speed = movingSpeed;
            chasingSpeed = movingSpeed * _bossSO.chasingSpeedMultiplier;
            patrolDistanceMax = _bossSO.patrolDistanceMax;
            patrolDistanceMin = _bossSO.patrolDistanceMin;
            detectionRange = _bossSO.detectionRange;
            startPosition = transform.position;

            damageBasicAttack = _bossSO.damageBasicAttack;
            damageStrongAttack = _bossSO.damageStrongAttack;
            damageSpecialAttack = _bossSO.damageSpecialAttack;

            basicAttackCollider.enabled = false;
            strongAttackCollider.enabled = false;
            specialAttackFirstCollider.enabled = false;
            specialAttackSecondCollider.enabled = false;

            UIManager.Instance.bossHealth = Health;
            UIManager.Instance.bossMaxHealth = Health;
            UIManager.Instance.EnableBossHealthBar();

            _material = spriteRenderer.material;

            StateMachine.ChangeState(new BossIdleState(this, StateMachine, animator));
        }

        public override void BasicAttackColliderEnable()
        {
            base.BasicAttackColliderEnable();
        }

        public override void BasicAttackColliderDisable()
        {
            base.BasicAttackColliderDisable();
        }

        public override void StrongAttackColliderEnable()
        {
            base.StrongAttackColliderEnable();
        }

        public override void StrongAttackColliderDisable()
        {
            base.StrongAttackColliderDisable();
        }

        public void SpecialAttackFirstColliderEnable()
        {
            specialAttackFirstCollider.enabled = true;
        }

        public void SpecialAttackFirstColliderDisable()
        {
            specialAttackFirstCollider.enabled = false;
        }

        public void SpecialAttackSecondColliderEnable()
        {
            specialAttackSecondCollider.enabled = true;
        }

        public void SpecialAttackSecondColliderDisable()
        {
            specialAttackSecondCollider.enabled = false;
        }

        public override State GetHurtState()
        {
            return new BossHurtState(this, StateMachine, animator);
        }

        public override void TakeDamage(Transform damageSourcePosition, float damage)
        {
            base.TakeDamage(damageSourcePosition, damage);
            UIManager.Instance.bossHealth = Health;
            UIManager.Instance.UpdateBossHealthBar();
        }

        public override void ChangeFacingDirection(Vector2 from, Vector2 to)
        {
            base.ChangeFacingDirection(from, to);
        }

        public override void LookAtPlayer()
        {
            base.LookAtPlayer();
        }

        public override bool CanSeePlayer()
        {
            return base.CanSeePlayer();
        }

        public override void StartFadeAndDestroy()
        {
            OnDie?.Invoke(this);
            StartCoroutine(Utils.FadeOutAndDestroy(gameObject, _material));
            UIManager.Instance.DisableBossHealthBar();
        }

        public override Vector2 GetRandomPatrolPosition()
        {
            return base.GetRandomPatrolPosition();
        }
    }
}
