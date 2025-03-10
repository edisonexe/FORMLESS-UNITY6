using UnityEngine;

namespace Formless.Items
{
    public class Bomb : MonoBehaviour
    {
        private CircleCollider2D _circleCollider;
        private float _fuseTime;
        [SerializeField] private AnimationClip _clipBomb;
        [SerializeField] private GameObject _explosionObj;

        private void Start()
        {
            _fuseTime = _clipBomb.length;
            Invoke(nameof(Fuse), _fuseTime);
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
    }
}
