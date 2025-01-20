using System;
using UnityEngine;

public class PlayerFightController : MonoBehaviour
{
    public static PlayerFightController Instance { get; private set; }

    public event EventHandler OnFightControllerBasicAttack;
    public event EventHandler OnFightControllerStrongAttack;


    public PolygonCollider2D _basicAttackPolygonCollider;
    public PolygonCollider2D _strongAttackPolygonCollider;

    [SerializeField] private int _damageBasicAttack = 3;
    [SerializeField] private int _damageStrongAttack = 6;

    private bool _isBasicAttack = false;
    private bool _isStrongAttack = false;

    private void Awake()
    {
        Instance = this;

        var basicAttackObject = transform.Find("BasicAttack");
        var strongAttackObject = transform.Find("StrongAttack");

        _basicAttackPolygonCollider = basicAttackObject.GetComponent<PolygonCollider2D>();
        _strongAttackPolygonCollider = strongAttackObject.GetComponent<PolygonCollider2D>();
    }

    private void Start()
    {
        PlayerInput.Instance.OnPlayerInputBasicAttack += FightController_OnPlayerBasicAttack;
        PlayerInput.Instance.OnPlayerInputStrongAttack += FightController_OnPlayerStrongAttack;

        BasicAttackColliderDisable();
        StrongAttackColliderDisable();
    }

    private void Update()
    {
        ChangeCollidersFacingDirection();
    }

    private void OnDestroy()
    {
        PlayerInput.Instance.OnPlayerInputBasicAttack -= FightController_OnPlayerBasicAttack;
        PlayerInput.Instance.OnPlayerInputStrongAttack -= FightController_OnPlayerStrongAttack;
    }

    public void BasicAttackColliderEnable()
    {
        _basicAttackPolygonCollider.enabled = true;
    }

    public void BasicAttackColliderDisable()
    {
        _basicAttackPolygonCollider.enabled = false;
    }

    public void StrongAttackColliderEnable()
    {
        _strongAttackPolygonCollider.enabled = true;
    }

    public void StrongAttackColliderDisable()
    {
        _strongAttackPolygonCollider.enabled = false;
    }

    private void FightController_OnPlayerStrongAttack(object sender, EventArgs e)
    {
        OnFightControllerStrongAttack?.Invoke(this, EventArgs.Empty);
        _isStrongAttack = true;
    }

    private void FightController_OnPlayerBasicAttack(object sender, EventArgs e)
    {
        OnFightControllerBasicAttack?.Invoke(this, EventArgs.Empty);
        _isBasicAttack = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out Enemy enemy))
        {
            if (_isBasicAttack)
            {
                enemy.TakeDamage(_damageBasicAttack);
                _isBasicAttack = false;
            }
            else if (_isStrongAttack) 
            {
                enemy.TakeDamage(_damageStrongAttack);
                _isStrongAttack = false;
            }
        }
    }

    private void ChangeCollidersFacingDirection()
    {
        Vector3 playerPosition = Player.Instance.GetPlayerScreenPosition();
        Vector2 movementVector = PlayerInput.Instance.GetMovementVector();

        if (movementVector.sqrMagnitude > 0.01f)
        {
            if (movementVector.x < 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else if (movementVector.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }
}
