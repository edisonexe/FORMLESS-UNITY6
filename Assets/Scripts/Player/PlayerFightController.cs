using System;
using UnityEngine;

public class PlayerFightController : MonoBehaviour
{
    public static PlayerFightController Instance { get; private set; }

    public event EventHandler OnFightControllerAttack1;
    public event EventHandler OnFightControllerAttack2;


    public PolygonCollider2D _attack1PolygonCollider;
    public PolygonCollider2D _attack2PolygonCollider;

    [SerializeField] private int _damageAttack1 = 3;
    [SerializeField] private int _damageAttack2 = 6;

    private bool _isAttack1 = false;
    private bool _isAttack2 = false;

    private void Awake()
    {
        Instance = this;

        var attack1Object = transform.Find("Attack1Collider");
        var attack2Object = transform.Find("Attack2Collider");

        _attack1PolygonCollider = attack1Object.GetComponent<PolygonCollider2D>();
        _attack2PolygonCollider = attack2Object.GetComponent<PolygonCollider2D>();
    }

    private void Start()
    {
        PlayerInput.Instance.OnPlayerInputAttack1 += FightController_OnPlayerAttack1;
        PlayerInput.Instance.OnPlayerInputAttack2 += FightController_OnPlayerAttack2;

        Attack1ColliderTurnOff();
        Attack2ColliderTurnOff();
    }

    private void Update()
    {
        ChangeCollidersFacingDirection();
    }

    private void FightController_OnPlayerAttack2(object sender, EventArgs e)
    {
        OnFightControllerAttack2?.Invoke(this, EventArgs.Empty);
        _isAttack2 = true;
    }

    private void FightController_OnPlayerAttack1(object sender, EventArgs e)
    {
        OnFightControllerAttack1?.Invoke(this, EventArgs.Empty);
        _isAttack1 = true;
    }

    public void Attack1ColliderTurnOn()
    {
        _attack1PolygonCollider.enabled = true;
    }

    public void Attack1ColliderTurnOff()
    {
        _attack1PolygonCollider.enabled = false;
    }

    public void Attack2ColliderTurnOn()
    {
        _attack2PolygonCollider.enabled = true;
    }

    public void Attack2ColliderTurnOff()
    {
        _attack2PolygonCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out Enemy enemy))
        {
            if (_isAttack1)
            {
                enemy.TakeDamage(_damageAttack1);
                _isAttack1 = false;
            }
            else if (_isAttack2) 
            {
                enemy.TakeDamage(_damageAttack2);
                _isAttack2 = false;
            }
        }
    }

    //private void OnDrawGizmos()
    //{
    //    if (_attack01PolygonCollider != null && _attack01PolygonCollider.enabled)
    //    {
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawWireCube(_attack01PolygonCollider.bounds.center, _attack01PolygonCollider.bounds.size);
    //    }

    //    if (_attack02PolygonCollider != null && _attack02PolygonCollider.enabled)
    //    {
    //        Gizmos.color = Color.blue;
    //        Gizmos.DrawWireCube(_attack02PolygonCollider.bounds.center, _attack02PolygonCollider.bounds.size);
    //    }
    //}

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
