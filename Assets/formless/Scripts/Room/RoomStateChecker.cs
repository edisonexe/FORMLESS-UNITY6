using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Formless.Core.Managers;
using Formless.Player.Rebirth;

namespace Formless.Room
{
    public class RoomStateChecker : MonoBehaviour
    {
        private List<Enemy.Enemy> _enemies = new List<Enemy.Enemy>();
        private DoorsController _doorsController;
        private RebirthController _rebirthController;

        private void Start()
        {
            _doorsController = GetComponent<DoorsController>();
            _rebirthController = Player.Player.Instance.RebirthController;
        }

        public void AddEnemy(Enemy.Enemy enemy)
        {
            if (enemy != null)
            {
                _enemies.Add(enemy);
                enemy.OnDie += RemoveEnemy;
                Debug.Log($"Добавлен враг {enemy.gameObject.name}, подписан на OnDie.");
            }
        }

        private void RemoveEnemy(Enemy.Enemy enemy)
        {
            if (enemy == null) return;

            Debug.Log("Убит враг " + enemy.gameObject.name);
            _rebirthController.OnEnemyKilled(enemy.gameObject);

            enemy.OnDie -= RemoveEnemy;
            _enemies.Remove(enemy);

            GameplayManager.Instance.EnemyKilled();
            if (_enemies.Count == 0)
            {
                StartCoroutine(OpenDoorsAfterDelay());
            }
        }

        private IEnumerator OpenDoorsAfterDelay()
        {
            yield return new WaitForSeconds(0.5f);
            _doorsController.OpenRegularDoors();
        }
    }
}
