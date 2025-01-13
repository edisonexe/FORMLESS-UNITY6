using Formless.Utils;
using TMPro;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Material _material;

    [SerializeField] private PlayerFightController _fightController;
    private const string ATTACK1 = "Attack1";
    private const string ATTACK2 = "Attack2";
    private const string IS_ROAMING = "IsRoaming";
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
        PlayerFightController.Instance.OnFightControllerAttack1 += PlayerVisual_OnPlayerAttack1;
        PlayerFightController.Instance.OnFightControllerAttack2 += PlayerVisual_OnPlayerAttack2;
        Player.Instance.OnDie += Player_OnDie;
        Player.Instance.OnHurt += Player_OnHurt;
    }

    private void Update()
    {
        _animator.SetBool(IS_ROAMING, Player.Instance.GetIsWalking());
        ChangePlayerFacingDirection();
    }

    private void OnDestroy()
    {
        PlayerFightController.Instance.OnFightControllerAttack1 -= PlayerVisual_OnPlayerAttack1;
        PlayerFightController.Instance.OnFightControllerAttack2 -= PlayerVisual_OnPlayerAttack2;
        Player.Instance.OnDie -= Player_OnDie;
        Player.Instance.OnHurt -= Player_OnHurt;
    }

    public void TriggerStartAttack1()
    {
        PlayerFightController.Instance.Attack1ColliderTurnOn();
    }

    public void TriggerStartAttack2()
    {
        PlayerFightController.Instance.Attack2ColliderTurnOn();
    }

    public void TriggerEndAttack1()
    {
        PlayerFightController.Instance.Attack1ColliderTurnOff();
    }

    public void TriggerEndAttack2()
    {
        PlayerFightController.Instance.Attack2ColliderTurnOff();
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

    private void PlayerVisual_OnPlayerAttack2(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(ATTACK2);
    }

    private void PlayerVisual_OnPlayerAttack1(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(ATTACK1);
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
        //else // Если персонаж стоит
        //{
        //    // Поворот в зависимости от положения мыши
        //    if (mousePosition.x < playerPosition.x)
        //    {
        //        _spriteRenderer.flipX = true;
        //    }
        //    else
        //    {
        //        _spriteRenderer.flipX = false;
        //    }
        //}
    }

}
