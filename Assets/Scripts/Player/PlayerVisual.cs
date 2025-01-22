using Formless.Utils;
using TMPro;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Material _material;

    [SerializeField] private PlayerFightController _fightController;
    private const string BASIC_ATTACK = "BasicAttack";
    private const string STRONG_ATTACK = "StrongAttack";
    private const string IS_MOVING = "IsMoving";
    private const string IS_DIE = "IsDie";
    private const string HURT = "Hurt";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _material = _spriteRenderer.material;
    }

    private void Start()
    {
        PlayerFightController.Instance.OnFightControllerBasicAttack += PlayerVisual_OnPlayerBasicAttack;
        PlayerFightController.Instance.OnFightControllerStrongAttack += PlayerVisual_OnPlayerStrongAttack;
        Player.Instance.OnDie += Player_OnDie;
        Player.Instance.OnHurt += Player_OnHurt;
    }

    private void Update()
    {
        _animator.SetBool(IS_MOVING, Player.Instance.GetIsWalking());
        ChangePlayerFacingDirection();
    }

    private void OnDestroy()
    {
        PlayerFightController.Instance.OnFightControllerBasicAttack -= PlayerVisual_OnPlayerBasicAttack;
        PlayerFightController.Instance.OnFightControllerStrongAttack -= PlayerVisual_OnPlayerStrongAttack;
        Player.Instance.OnDie -= Player_OnDie;
        Player.Instance.OnHurt -= Player_OnHurt;
    }

    public void TriggerStartBasicAttack()
    {
        PlayerFightController.Instance.BasicAttackColliderEnable();
    }

    public void TriggerStartStrongAttack()
    {
        PlayerFightController.Instance.StrongAttackColliderEnable();
    }

    public void TriggerEndBasicAttack()
    {
        PlayerFightController.Instance.BasicAttackColliderDisable();
    }

    public void TriggerEndStrongAttack()
    {
        PlayerFightController.Instance.StrongAttackColliderDisable();
    }

    private void Player_OnHurt(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(HURT);
    }

    private void Player_OnDie(object sender, System.EventArgs e)
    {
        _animator.SetBool(IS_DIE, true);
        StartCoroutine(FormlessUtils.FadeOutAndDestroy(gameObject, _material));
    }

    private void PlayerVisual_OnPlayerStrongAttack(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(STRONG_ATTACK);
    }

    private void PlayerVisual_OnPlayerBasicAttack(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(BASIC_ATTACK);
    }

    private void ChangePlayerFacingDirection()
    {
        Vector3 mousePosition = PlayerInput.Instance.GetMousePositions();
        Vector3 playerPosition = Player.Instance.GetPlayerScreenPosition();
        Vector2 movementVector = PlayerInput.Instance.GetMovementVector();

        // Если персонаж двигается
        if (movementVector.sqrMagnitude > 0.01f) // Используем sqrMagnitude для оптимизации
        {
            if (movementVector.x < 0) // Движение влево
            {
                _spriteRenderer.flipX = true;
            }
            else if (movementVector.x > 0) // Движение вправо
            {
                _spriteRenderer.flipX = false;
            }
        }
    }

}
