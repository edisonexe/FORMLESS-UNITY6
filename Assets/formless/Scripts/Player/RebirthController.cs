using UnityEngine;
using Formless.Core.Managers;

namespace Formless.Player
{
    public class RebirthController : MonoBehaviour
    {
        [SerializeField] private float reincarnationCooldown = 40f;
        private float _lastReincarnationTime;
        private SpriteRenderer _spriteRenderer;
        private Animator _animator;
        private BoxCollider2D _boxCollider;
        private CapsuleCollider2D _capsuleCollider;
        private RuntimeAnimatorController _defaultAnimator;
        private Player _player;
        private PlayerInputHandler _inputHandler;

        private void Awake()
        {
            _player = GetComponent<Player>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
            _boxCollider = GetComponent<BoxCollider2D>();
            _capsuleCollider = GetComponent<CapsuleCollider2D>();
            _inputHandler = new PlayerInputHandler();

            // —охран€ем аниматор игрока, чтобы можно было вернуть обратно
            if (_animator != null)
                _defaultAnimator = _animator.runtimeAnimatorController;

            // —разу доступно перерождение при старте игры
            _lastReincarnationTime = Time.time - reincarnationCooldown;
        }

        private void Update()
        {
            // ѕровер€ем нажатие кнопки перерождени€ (R) и врем€ перерождени€
            if (_inputHandler.IsRebirthPressed() && Time.time >= _lastReincarnationTime + reincarnationCooldown)
            {
                TryReincarnate();
            }
        }

       private void TryReincarnate()
       {
            // ѕолучаем данные о последнем убитом враге
            EnemyData lastEnemy = GameplayManager.Instance.LastKilledEnemyData;
            if (lastEnemy == null)
            {
                Debug.Log("Ќет убитых врагов дл€ перерождени€.");
                return;
            }

            _lastReincarnationTime = Time.time; // сбрасываем таймер после перерождени€

            // ќбновл€ем спрайт
            if (lastEnemy.enemySprite != null)
            {
                _spriteRenderer.sprite = lastEnemy.enemySprite;
                Debug.Log($"—прайт игрока обновлен: {lastEnemy.enemySprite.name}");
            }
            else
            {
                Debug.LogWarning("—прайт дл€ перерождени€ не найден!");
            }

            // «агружаем аниматор по имени из данных врага
            if (!string.IsNullOrEmpty(lastEnemy.animatorControllerName))
            {
                string path = "Animators/Enemies/" + lastEnemy.animatorControllerName + "/" + lastEnemy.animatorControllerName; // ѕуть к аниматору в папке Resources
                RuntimeAnimatorController animatorController = Resources.Load<RuntimeAnimatorController>(path);
                if (animatorController != null)
                {
                    _animator.runtimeAnimatorController = animatorController;
                    Debug.Log($"јнимаци€ игрока обновлена: {path}");
                }
                else
                {
                    Debug.LogError($"Ќе удалось загрузить аниматор по пути {path}");
                }
            }
            else
            {
                Debug.LogWarning("јниматор дл€ перерождени€ не найден!");
            }

            // ќбновл€ем коллайдеры
            UpdateCollider(_boxCollider, lastEnemy.boxColliderSize, lastEnemy.boxColliderOffset);
            UpdateCollider(_capsuleCollider, lastEnemy);

            //  опируем коллайдеры атак
            CopyAttackCollider("BasicAttack", lastEnemy);
            CopyAttackCollider("StrongAttack", lastEnemy);

            Debug.Log($"»грок переродилс€ в {lastEnemy.enemyType}");
       }

        private void UpdateCollider(BoxCollider2D collider, Vector2 size, Vector2 offset)
        {
            if (collider != null)
            {
                collider.size = size;
                collider.offset = offset;
                Debug.Log($"BoxCollider2D обновлен: размер {collider.size}, смещение {collider.offset}");
            }
        }

        private void UpdateCollider(CapsuleCollider2D collider, EnemyData lastEnemy)
        {
            if (collider != null)
            {
                collider.size = new Vector2(lastEnemy.capsuleColliderSizeX, lastEnemy.capsuleColliderSizeY);
                collider.offset = new Vector2(lastEnemy.capsuleColliderOffsetX, lastEnemy.capsuleColliderOffsetY);
                collider.direction = (CapsuleDirection2D)lastEnemy.capsuleColliderDirection;
                Debug.Log($"CapsuleCollider2D обновлен: размер {collider.size}, смещение {collider.offset}, направление {collider.direction}");
            }
        }


        private void CopyAttackCollider(string attackName, EnemyData lastEnemy)
        {
            // ѕоиск коллайдера дл€ атаки у врага
            GameObject attackObject = GameObject.Find(attackName);
            if (attackObject != null)
            {
                PolygonCollider2D attackCollider = attackObject.GetComponent<PolygonCollider2D>();
                if (attackCollider != null)
                {
                    // ѕоиск у игрока коллайдера атаки с таким же именем
                    GameObject playerAttackObject = GameObject.Find($"Player/{attackName}");
                    if (playerAttackObject != null)
                    {
                        PolygonCollider2D playerAttackCollider = playerAttackObject.GetComponent<PolygonCollider2D>();
                        if (playerAttackCollider != null)
                        {
                            //  опируем параметры из данных врага, если они есть
                            if (lastEnemy != null)
                            {
                                if (lastEnemy.basicAttackCollider != null)
                                {
                                    for (int i = 0; i < lastEnemy.basicAttackCollider.pathCount; i++)
                                    {
                                        playerAttackCollider.SetPath(i, lastEnemy.basicAttackCollider.paths[i]);
                                    }
                                    Debug.Log($" опирование коллайдера атаки дл€ {attackName} выполнено.");
                                }
                                else
                                {
                                    Debug.LogWarning($" оллайдер атаки {attackName} не найден у врага!");
                                }
                            }
                        }
                    }
                }
            }
        }

        public void ResetToDefault()
        {
            // ¬озвращаем оригинальный аниматор и спрайт
            if (_defaultAnimator != null)
                _animator.runtimeAnimatorController = _defaultAnimator;

            // ћожно добавить возврат оригинального спрайта, если нужно
            Debug.Log("ќригинальный аниматор и спрайт восстановлены.");
        }
    }
}
