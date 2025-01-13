using Formless.Utils;
using UnityEngine;

public class EnemyVisual : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private Material _material;

    [SerializeField] private EnemyAI _enemyAI;
    [SerializeField] private Enemy _enemy;

    private const string IS_ROAMING = "IsRoaming";
    private const string CHASING_SPEED_MULTIPLIER = "ChasingSpeedMultiplier";
    private const string IS_DIE = "IsDie";
    private const string ATTACK1 = "Attack1";
    private const string ATTACK2 = "Attack2";
    private const string HURT = "Hurt";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _material = _spriteRenderer.material;
    }

    private void Start()
    {
        _enemyAI.OnEnemyAttack1 += enemyAI_OnEnemyAttack1;
        _enemyAI.OnEnemyAttack2 += enemyAI_OnEnemyAttack2;
        _enemy.OnTakeHit += enemy_OnTakeHit;
        _enemy.OnDie += enemy_OnDie;
    }

    private void Update()
    {
        _animator.SetBool(IS_ROAMING, _enemyAI.IsRoaming());
        _animator.SetFloat(CHASING_SPEED_MULTIPLIER, _enemyAI.GetRoamingAnimationSpeed());
    }

    private void OnDestroy()
    {
        _enemyAI.OnEnemyAttack1 -= enemyAI_OnEnemyAttack1;
        _enemyAI.OnEnemyAttack2 -= enemyAI_OnEnemyAttack2;
        _enemy.OnTakeHit -= enemy_OnTakeHit;
        _enemy.OnDie -= enemy_OnDie;
    }


    public void TriggerStartAttack1()
    {
        _enemy.Attack1ColliderTurnOn();
    }

    public void TriggerStartAttack2()
    {
        _enemy.Attack2ColliderTurnOn();
    }

    public void TriggerEndAttack1()
    {
        _enemy.Attack1ColliderTurnOff();
    }

    public void TriggerEndAttack2()
    {
        _enemy.Attack2ColliderTurnOff();
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

    private void enemyAI_OnEnemyAttack2(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(ATTACK2);
    }

    private void enemyAI_OnEnemyAttack1(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(ATTACK1);
    }
}
