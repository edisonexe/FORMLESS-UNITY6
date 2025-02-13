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

            // ��������� �������� ������, ����� ����� ���� ������� �������
            if (_animator != null)
                _defaultAnimator = _animator.runtimeAnimatorController;

            // ����� �������� ������������ ��� ������ ����
            _lastReincarnationTime = Time.time - reincarnationCooldown;
        }

        private void Update()
        {
            // ��������� ������� ������ ������������ (R) � ����� ������������
            if (_inputHandler.IsRebirthPressed() && Time.time >= _lastReincarnationTime + reincarnationCooldown)
            {
                TryReincarnate();
            }
        }

       private void TryReincarnate()
       {
            // �������� ������ � ��������� ������ �����
            EnemyData lastEnemy = GameplayManager.Instance.LastKilledEnemyData;
            if (lastEnemy == null)
            {
                Debug.Log("��� ������ ������ ��� ������������.");
                return;
            }

            _lastReincarnationTime = Time.time; // ���������� ������ ����� ������������

            // ��������� ������
            if (lastEnemy.enemySprite != null)
            {
                _spriteRenderer.sprite = lastEnemy.enemySprite;
                Debug.Log($"������ ������ ��������: {lastEnemy.enemySprite.name}");
            }
            else
            {
                Debug.LogWarning("������ ��� ������������ �� ������!");
            }

            // ��������� �������� �� ����� �� ������ �����
            if (!string.IsNullOrEmpty(lastEnemy.animatorControllerName))
            {
                string path = "Animators/Enemies/" + lastEnemy.animatorControllerName + "/" + lastEnemy.animatorControllerName; // ���� � ��������� � ����� Resources
                RuntimeAnimatorController animatorController = Resources.Load<RuntimeAnimatorController>(path);
                if (animatorController != null)
                {
                    _animator.runtimeAnimatorController = animatorController;
                    Debug.Log($"�������� ������ ���������: {path}");
                }
                else
                {
                    Debug.LogError($"�� ������� ��������� �������� �� ���� {path}");
                }
            }
            else
            {
                Debug.LogWarning("�������� ��� ������������ �� ������!");
            }

            // ��������� ����������
            UpdateCollider(_boxCollider, lastEnemy.boxColliderSize, lastEnemy.boxColliderOffset);
            UpdateCollider(_capsuleCollider, lastEnemy);

            // �������� ���������� ����
            CopyAttackCollider("BasicAttack", lastEnemy);
            CopyAttackCollider("StrongAttack", lastEnemy);

            Debug.Log($"����� ����������� � {lastEnemy.enemyType}");
       }

        private void UpdateCollider(BoxCollider2D collider, Vector2 size, Vector2 offset)
        {
            if (collider != null)
            {
                collider.size = size;
                collider.offset = offset;
                Debug.Log($"BoxCollider2D ��������: ������ {collider.size}, �������� {collider.offset}");
            }
        }

        private void UpdateCollider(CapsuleCollider2D collider, EnemyData lastEnemy)
        {
            if (collider != null)
            {
                collider.size = new Vector2(lastEnemy.capsuleColliderSizeX, lastEnemy.capsuleColliderSizeY);
                collider.offset = new Vector2(lastEnemy.capsuleColliderOffsetX, lastEnemy.capsuleColliderOffsetY);
                collider.direction = (CapsuleDirection2D)lastEnemy.capsuleColliderDirection;
                Debug.Log($"CapsuleCollider2D ��������: ������ {collider.size}, �������� {collider.offset}, ����������� {collider.direction}");
            }
        }


        private void CopyAttackCollider(string attackName, EnemyData lastEnemy)
        {
            // ����� ���������� ��� ����� � �����
            GameObject attackObject = GameObject.Find(attackName);
            if (attackObject != null)
            {
                PolygonCollider2D attackCollider = attackObject.GetComponent<PolygonCollider2D>();
                if (attackCollider != null)
                {
                    // ����� � ������ ���������� ����� � ����� �� ������
                    GameObject playerAttackObject = GameObject.Find($"Player/{attackName}");
                    if (playerAttackObject != null)
                    {
                        PolygonCollider2D playerAttackCollider = playerAttackObject.GetComponent<PolygonCollider2D>();
                        if (playerAttackCollider != null)
                        {
                            // �������� ��������� �� ������ �����, ���� ��� ����
                            if (lastEnemy != null)
                            {
                                if (lastEnemy.basicAttackCollider != null)
                                {
                                    for (int i = 0; i < lastEnemy.basicAttackCollider.pathCount; i++)
                                    {
                                        playerAttackCollider.SetPath(i, lastEnemy.basicAttackCollider.paths[i]);
                                    }
                                    Debug.Log($"����������� ���������� ����� ��� {attackName} ���������.");
                                }
                                else
                                {
                                    Debug.LogWarning($"��������� ����� {attackName} �� ������ � �����!");
                                }
                            }
                        }
                    }
                }
            }
        }

        public void ResetToDefault()
        {
            // ���������� ������������ �������� � ������
            if (_defaultAnimator != null)
                _animator.runtimeAnimatorController = _defaultAnimator;

            // ����� �������� ������� ������������� �������, ���� �����
            Debug.Log("������������ �������� � ������ �������������.");
        }
    }
}
