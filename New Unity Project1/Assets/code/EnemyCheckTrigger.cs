using UnityEngine;

namespace code
{
    public class EnemyCheckTrigger : MonoBehaviour

    {
        public bool _checkTrigger;
        [SerializeField] private Enemy _enemy;

        private void Start()
        {
            _checkTrigger = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {
                _checkTrigger = true;
                _enemy.MovePlayer();
                Debug.Log(_checkTrigger);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {
                _checkTrigger = false;
                _enemy._isCheckTrigger = false;
                _enemy.MovePatrol();
            }
        }
    }
}