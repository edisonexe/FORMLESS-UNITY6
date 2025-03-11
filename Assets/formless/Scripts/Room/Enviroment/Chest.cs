using Formless.Core.Managers;
using Formless.Items;
using UnityEngine;

namespace Formless.Room.Enviroment
{
    public class Chest: MonoBehaviour
    {
        private BoxCollider2D _boxCollider;
        private Animator _animator;
        private bool _isOpened = false;
        private const string OPEN = "Open";

        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider2D>();
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            _boxCollider.enabled = true;
        }

        public void SpawnItem()
        {
            Instantiate(PrefabManager.Instance.BombPrefabWith, transform.position, Quaternion.identity);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                if (!_isOpened)
                {
                    _isOpened = true;
                    _boxCollider.enabled = false;
                    _animator.SetTrigger(OPEN);

                    Debug.Log("Сундук открыт!");
                }
            }
        }
    }
}