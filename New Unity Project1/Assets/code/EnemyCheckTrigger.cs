using UnityEngine;

namespace code
{
    public class EnemyCheckTrigger : MonoBehaviour

    {
        [SerializeField] private Enemy _enemy;
        
        public bool _checkTrigger;


        private void Start()
        {
            _checkTrigger = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {
                _enemy._isCheckTrigger= true;
                _checkTrigger = true;
                _enemy.MovePlayer();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {
                _enemy._isCheckTrigger= false;
                _checkTrigger = false;
                _enemy.MovePatrol();
            }
        }
    }
}