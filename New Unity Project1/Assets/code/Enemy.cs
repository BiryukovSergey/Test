using UnityEngine;
using UnityEngine.AI;

namespace code
{
    public class Enemy : MonoBehaviour
    {
        public bool _isCheckTrigger;
        private GridGenerator _gridGenerator;
        private GameObject _player;
        private NavMeshAgent _navMeshAgent;
        private Noise _noise;
        private Vector3 _start;
        private Vector3 _finish;
        private bool _isMoveFinish = true;
        [SerializeField]private EnemyCheckTrigger[] _enemyCheckTrigger;

        private void Start()
        {
            _player = FindObjectOfType<Player>().gameObject;
            _gridGenerator = FindObjectOfType<GridGenerator>();
            _noise = FindObjectOfType<Noise>();

            _navMeshAgent = GetComponent<NavMeshAgent>();

            _start = transform.position;
            _finish = _gridGenerator.FreePosition();
        }

        private void FixedUpdate()
        {
            
            for (int i = 0; i < _enemyCheckTrigger.Length; i++)
            {
                if (_enemyCheckTrigger[i]._checkTrigger == true)
                {
                    _isCheckTrigger = true;
                }
            }
            if (_noise.CurrentNoise == 10 && _isCheckTrigger == false)
            {
                MovePlayer();
            }

            else if (_isCheckTrigger == false)
            {
                MovePatrol();
            }
           
        }
        
        public void MovePlayer()
        {
            _navMeshAgent.SetDestination(_player.transform.position);
        }

        public void MovePatrol()
        {
            if (Vector3.Distance(transform.position, _finish) < 0.5f)
            {
                _isMoveFinish = false;
            }
            else if (Vector3.Distance(transform.position, _start) < 0.5f)
            {
                _isMoveFinish = true;
            }

            if (_isMoveFinish)
            {
                _navMeshAgent.SetDestination(_finish);
            }
            else
            {
                _navMeshAgent.SetDestination(_start);
            }
        }
    }
}