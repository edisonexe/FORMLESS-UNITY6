using UnityEngine;

public class KnockBack : MonoBehaviour
{
    [SerializeField] private float _knockBackImpact = 3f;
    [SerializeField] private float _knockBackMovingTimerMax = 0.3f;

    private Rigidbody2D _rb;
    private float _knockBackMovingTimer;

    public bool IsGetKnockedBack { get; private set; }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _knockBackMovingTimer -= Time.deltaTime;
        if ( _knockBackMovingTimer < 0)
        {
            StopKnockBackMoving();

        }
    }

    public void GetKnockBack(Transform damageSource)
    {
        IsGetKnockedBack = true;
        _knockBackMovingTimer = _knockBackMovingTimerMax;
        Vector2 difference = (transform.position - damageSource.position).normalized * _knockBackImpact / _rb.mass;
        _rb.AddForce(difference, ForceMode2D.Impulse);
    }

    public void StopKnockBackMoving()
    {
        _rb.linearVelocity = Vector2.zero;
        IsGetKnockedBack = false;
    }
}
