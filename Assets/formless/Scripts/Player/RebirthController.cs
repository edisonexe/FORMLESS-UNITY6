using System.Collections.Generic;
using UnityEngine;
using Formless.UI;

namespace Formless.Player.Rebirth
{
    public class RebirthController : MonoBehaviour
    {
        //private RebirthTimer _rebirthDuration;

        private bool _isOriginalState = true;

        private GameObject _lastKilledEnemy;
        private Animator _playerAnimator;
        private PolygonCollider2D _playerBasicAttackCollider;
        private PolygonCollider2D _playerStrongAttackCollider;
        private CapsuleCollider2D _playerCapsuleCollider;
        private BoxCollider2D _playerBoxCollider;
        private PlayerInputHandler _inputHandler;

        // Оригинальные параметры игрока
        private bool _rangeAttacking = false;
        private float _originalBasicAttackDamage;
        private float _originalStrongAttackDamage;
        private float _originalMovingSpeed;
        private RuntimeAnimatorController _originalAnimator;
        private Vector2[] _originalBasicAttackPoints;
        private Vector2[] _originalStrongAttackPoints;
        private Vector2 _originalCapsuleSize;
        private Vector2 _originalCapsuleOffset;
        private CapsuleDirection2D _originalCapsuleDirection;
        private Vector2 _originalBoxSize;
        private Vector2 _originalBoxOffset;
        private Dictionary<string, float> _animatorParams = new Dictionary<string, float>();
        private RebirthTimer _rebirthCooldown;

        private void Start()
        {
            // Инициализация компонентов игрока
            _playerAnimator = GetComponent<Animator>();
            _playerBasicAttackCollider = transform.Find("BasicAttack")?.GetComponent<PolygonCollider2D>();
            _playerStrongAttackCollider = transform.Find("StrongAttack")?.GetComponent<PolygonCollider2D>();
            _playerCapsuleCollider = GetComponent<CapsuleCollider2D>();
            _playerBoxCollider = GetComponent<BoxCollider2D>();
            _rangeAttacking = Player.Instance.rangeAttacking;

            // Сохраняем оригинальные параметры
            _originalAnimator = _playerAnimator.runtimeAnimatorController;
        
            if (_playerBasicAttackCollider != null)
                _originalBasicAttackPoints = _playerBasicAttackCollider.points;

            if (_playerStrongAttackCollider != null)
                _originalStrongAttackPoints = _playerStrongAttackCollider.points;

            if (_playerCapsuleCollider != null)
            {
                _originalCapsuleSize = _playerCapsuleCollider.size;
                _originalCapsuleOffset = _playerCapsuleCollider.offset;
                _originalCapsuleDirection = _playerCapsuleCollider.direction;
            }

            if (_playerBoxCollider != null)
            {
                _originalBoxSize = _playerBoxCollider.size;
                _originalBoxOffset = _playerBoxCollider.offset;
            }

            _originalMovingSpeed = Player.Instance.MovingSpeed;
            _originalBasicAttackDamage = Player.Instance.DamageBasicAttack;
            _originalStrongAttackDamage = Player.Instance.DamageStrongAttack;

            _rebirthCooldown = UIManager.Instance.RebirthCooldown;

            //_rebirthDuration = UIManager.Instance.RebirthDuration;
        }

        private void Update()
        {
            // Проверяем, что _inputHandler назначен
            if (_inputHandler == null)
            {
                Debug.LogError("_inputHandler is not assigned!");
                return;
            }

            // Проверяем, что UIManager.Instance существует
            if (UIManager.Instance == null)
            {
                Debug.LogError("UIManager.Instance is not initialized!");
                return;
            }

            // Проверяем, что _rebirthDuration назначен
            if (_rebirthCooldown == null)
            {
                Debug.LogError("_rebirthCooldown is not assigned!");
                return;
            }



            if (_inputHandler.IsRebirthPressed() && _lastKilledEnemy != null && UIManager.Instance.CanRebirth())
            {
                Debug.Log("Нажата R");
                Rebirth();
                //UIManager.Instance.StartRebirthCooldown();
            }
            //if (_rebirthDuration.IsCooldownOver() && !_isOriginalState)
            //{
            //    RestoreOriginalState();
            //}
        }

        public void OnEnemyKilled(GameObject killedEnemy)
        {
            _lastKilledEnemy = killedEnemy;
            //Debug.Log("Установлен п.убитый враг " + _lastKilledEnemy.name);
        }

        public void SetInputHandler(PlayerInputHandler inputHandler)
        {
            _inputHandler = inputHandler;
        }

        private void SaveAnimatorParameters()
        {
            _animatorParams.Clear();
            foreach (var param in _playerAnimator.parameters)
            {
                if (param.type == AnimatorControllerParameterType.Bool)
                {
                    _animatorParams[param.name] = _playerAnimator.GetBool(param.name) ? 1f : 0f;
                }
                else if (param.type == AnimatorControllerParameterType.Float)
                {
                    _animatorParams[param.name] = _playerAnimator.GetFloat(param.name);
                }
                else if (param.type == AnimatorControllerParameterType.Int)
                {
                    _animatorParams[param.name] = _playerAnimator.GetInteger(param.name);
                }
            }
        }

        private void RestoreAnimatorParameters()
        {
            foreach (var param in _animatorParams)
            {
                if (AnimatorHasParameter(_playerAnimator, param.Key))
                {
                    if (param.Value == 1f || param.Value == 0f)
                        _playerAnimator.SetBool(param.Key, param.Value == 1f);
                    else if (param.Value % 1 == 0)
                        _playerAnimator.SetInteger(param.Key, (int)param.Value);
                    else
                        _playerAnimator.SetFloat(param.Key, param.Value);
                }
            }
        }

        private bool AnimatorHasParameter(Animator animator, string paramName)
        {
            foreach (var param in animator.parameters)
            {
                if (param.name == paramName)
                    return true;
            }
            return false;
        }

        private void SetNewMaxHealth(string enemyName)
        {
            float maxHealth = 0f;
            switch (enemyName)
            {
                case "Wizard":
                    maxHealth = 40f; 
                    break;
                case "Skeleton":
                    maxHealth = 50f;
                    break;
                case "Orc":
                    maxHealth = 60f;
                    break;
                case "Armored Orc":
                    maxHealth = 80f; 
                    break;
                default:
                    break;
            }
            Debug.Log($"Имя врага -{enemyName}, Новое макс.зд - {maxHealth}");
            Player.Instance.MaxHealth = maxHealth;
            UIManager.Instance.UpdateMaxHearts(maxHealth);
        }


        private void Rebirth()
        {
            if (_lastKilledEnemy == null) return;

            Animator enemyAnimator = _lastKilledEnemy.GetComponent<Animator>();

            if (_playerAnimator.runtimeAnimatorController == enemyAnimator.runtimeAnimatorController)
            {
                Debug.Log("Аниматор уже установлен. Перерождение отменено.");
                return;
            }

            SaveAnimatorParameters();

            Enemy.Enemy enemy = _lastKilledEnemy.GetComponent<Enemy.Enemy>();
            if (enemy != null)
            {
                float newMovingSpeed = enemy.ChasingSpeed + 1f;;
                Player.Instance.SetMovingSpeed(newMovingSpeed);
                Player.Instance.SetBasicAttackDamage(enemy.BasicAttackDamage);
                Player.Instance.SetStrongAttackDamage(enemy.StrongAttackDamage);
                SetNewMaxHealth(enemy.enemyName);
            }

            PolygonCollider2D enemyBasicAttackCollider = _lastKilledEnemy.transform.Find("BasicAttack")?.GetComponent<PolygonCollider2D>();
            PolygonCollider2D enemyStrongAttackCollider = _lastKilledEnemy.transform.Find("StrongAttack")?.GetComponent<PolygonCollider2D>();
            CapsuleCollider2D enemyCapsuleCollider = _lastKilledEnemy.GetComponent<CapsuleCollider2D>();
            BoxCollider2D enemyBoxCollider = _lastKilledEnemy.GetComponent<BoxCollider2D>();

            // Анимация
            _playerAnimator.runtimeAnimatorController = enemyAnimator.runtimeAnimatorController;
            RestoreAnimatorParameters();

            bool enemyIsRangeAttacking = enemy.rangeAttacking;
            Player.Instance.rangeAttacking = enemyIsRangeAttacking;

            // Копируем коллайдеры
            CopyPolygonCollider(enemyBasicAttackCollider, _playerBasicAttackCollider);
            CopyPolygonCollider(enemyStrongAttackCollider, _playerStrongAttackCollider);
            CopyCapsuleCollider(enemyCapsuleCollider, _playerCapsuleCollider);
            CopyBoxCollider(enemyBoxCollider, _playerBoxCollider);

            UIManager.Instance.StartRebirthCooldown();
            _isOriginalState = false;
        }

        public void RestoreOriginalState()
        {
            //Debug.Log("ВОЗВРАТ К ОРИГИНАЛЬНОМУ СОСТОЯНИЮ!");
            SaveAnimatorParameters();
            _playerAnimator.runtimeAnimatorController = _originalAnimator;
            RestoreAnimatorParameters();
            if (_playerBasicAttackCollider != null && _originalBasicAttackPoints != null)
                _playerBasicAttackCollider.points = _originalBasicAttackPoints;

            if (_playerStrongAttackCollider != null && _originalStrongAttackPoints != null)
                _playerStrongAttackCollider.points = _originalStrongAttackPoints;

            if (_playerCapsuleCollider != null)
            {
                _playerCapsuleCollider.size = _originalCapsuleSize;
                _playerCapsuleCollider.offset = _originalCapsuleOffset;
                _playerCapsuleCollider.direction = _originalCapsuleDirection;
            }

            if (_playerBoxCollider != null)
            {
                _playerBoxCollider.size = _originalBoxSize;
                _playerBoxCollider.offset = _originalBoxOffset;
            }

            Player.Instance.SetMovingSpeed(_originalMovingSpeed);
            Player.Instance.SetBasicAttackDamage(_originalBasicAttackDamage);
            Player.Instance.SetStrongAttackDamage(_originalStrongAttackDamage);

            _isOriginalState = true;
        }

        private void CopyPolygonCollider(PolygonCollider2D source, PolygonCollider2D target)
        {
            if (source == null || target == null) return;

            target.pathCount = source.pathCount;
            for (int i = 0; i < source.pathCount; i++)
            {
                target.SetPath(i, source.GetPath(i));
            }
        }

        private void CopyCapsuleCollider(CapsuleCollider2D source, CapsuleCollider2D target)
        {
            if (source == null || target == null) return;

            target.size = source.size;
            target.offset = source.offset;
            target.direction = source.direction;
        }

        private void CopyBoxCollider(BoxCollider2D source, BoxCollider2D target)
        {
            if (source == null || target == null) return;

            target.size = source.size;
            target.offset = source.offset;
        }
    }

}
