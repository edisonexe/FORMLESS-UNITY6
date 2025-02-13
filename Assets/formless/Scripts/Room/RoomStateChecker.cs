using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Formless.Core.Managers;

namespace Formless.Room
{
    public class RoomStateChecker : MonoBehaviour
    {
        private List<Enemy.Enemy> _enemies = new List<Enemy.Enemy>();
        private DoorsController _doorsController;

        private void Start()
        {
            _doorsController = GetComponent<DoorsController>();
        }

        public void AddEnemy(Enemy.Enemy enemy)
        {
            if (enemy != null)
            {
                _enemies.Add(enemy);
                enemy.OnDie += RemoveEnemy;
            }
        }

        private void RemoveEnemy(Enemy.Enemy enemy)
        {
            if (enemy == null) return;

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
