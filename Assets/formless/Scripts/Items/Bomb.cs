using UnityEngine;

namespace Formless.Items
{
    public class Bomb : MonoBehaviour
    {
        private Animator _animator;
        [SerializeField] private AnimationClip _clipBomb;
        [SerializeField] private GameObject _explosionObj;
        private const string ACTIVATE_BOMB = "ActivateBomb";

        private Vector2 _targetPosition;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            _targetPosition = (Vector2)transform.position + new Vector2(0f, -0.5f);
        }

        private void Update()
        {
            transform.position = Vector2.MoveTowards(transform.position, _targetPosition, 2f * Time.deltaTime);
        }

        public void ActivateBombPrepare()
        {
            _animator.SetTrigger(ACTIVATE_BOMB);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                var collectible = gameObject.GetComponent<Collectible>();
                if (collectible != null && collectible.isCollected) return;

                collectible.isCollected = true;
                Player.Player.Instance.PickupBomb();
                Destroy(gameObject);
            }
        }

        private void Fuse()
        {
            if (_explosionObj)
            {
                GameObject explosion = Instantiate(_explosionObj, transform.position, Quaternion.identity);
                
                Explosion explosionScript = explosion.GetComponent<Explosion>();

                if (explosionScript != null)
                {
                    explosionScript.TriggerExplosion();
                }
            }

            Destroy(gameObject);
        }

        public void EndPrepareAnimation()
        {
            Fuse();
        }
    }
}
