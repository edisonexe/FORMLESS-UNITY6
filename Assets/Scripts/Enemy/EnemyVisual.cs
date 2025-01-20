using Formless.Utils;
using UnityEngine;

public class EnemyVisual : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    protected Animator _animator;
    private Material _material;

    [SerializeField] private EnemyAI _enemyAI;
    [SerializeField] private Enemy _enemy;

    private const string CHASING_SPEED_MULTIPLIER = "ChasingSpeedMultiplier";
    protected const string IS_MOVING = "IsMoving";
    protected const string IS_DIE = "IsDie";
    protected const string BASIC_ATTACK = "BasicAttack";
    protected const string STRONG_ATTACK = "StrongAttack";
    protected const string HURT = "Hurt";

    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _material = _spriteRenderer.material;
    }

    protected virtual void Start()
    {
        _enemyAI.OnEnemyBasicAttack += enemyAI_OnEnemyBasicAttack;
        _enemyAI.OnEnemyStrongAttack += enemyAI_OnEnemyStrongAttack;
        _enemy.OnTakeHit += enemy_OnTakeHit;
        _enemy.OnDie += enemy_OnDie;
    }

    protected virtual void Update()
    {
        _animator.SetBool(IS_MOVING, _enemyAI.IsRoaming());

        if (HasAnimatorParameter(CHASING_SPEED_MULTIPLIER))
        {
            _animator.SetFloat(CHASING_SPEED_MULTIPLIER, _enemyAI.GetRoamingAnimationSpeed());
        }
        //_animator.SetFloat(CHASING_SPEED_MULTIPLIER, _enemyAI.GetRoamingAnimationSpeed());
    }

    private void OnDestroy()
    {
        _enemyAI.OnEnemyBasicAttack -= enemyAI_OnEnemyBasicAttack;
        _enemyAI.OnEnemyStrongAttack -= enemyAI_OnEnemyStrongAttack;
        _enemy.OnTakeHit -= enemy_OnTakeHit;
        _enemy.OnDie -= enemy_OnDie;
    }


    public void TriggerStartBasicAttack()
    {
        _enemy.BasicAttackColliderEnable();
    }

    public void TriggerStartStrongAttack()
    {
        _enemy.StrongAttackColliderEnable();
    }

    public void TriggerEndBasicAttack()
    {
        _enemy.BasicAttackColliderDisable();
    }

    public void TriggerEndStrongAttack()
    {
        _enemy.StrongAttackColliderDisable();
    }

    public void TriggerHandleDeath()
    {
        _enemy.HandleDeath();
    }

    private void enemy_OnDie(object sender, System.EventArgs e)
    {
        _animator.SetBool(IS_DIE, true);
        _spriteRenderer.sortingOrder = -1;
        StartCoroutine(FormlessUtils.FadeOutAndDestroy(gameObject, _material));
    }

    private void enemy_OnTakeHit(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(HURT);
    }

    private void enemyAI_OnEnemyStrongAttack(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(STRONG_ATTACK);
    }

    private void enemyAI_OnEnemyBasicAttack(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(BASIC_ATTACK);
    }

    private bool HasAnimatorParameter(string parameterName)
    {
        foreach (AnimatorControllerParameter param in _animator.parameters)
        {
            if (param.name == parameterName)
            {
                return true;
            }
        }
        return false;
    }

}
