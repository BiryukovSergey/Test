using System.Collections.Generic;
using UnityEditor.AI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace code
{
    public class GridGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject _wall;
        [SerializeField] private GameObject _exitPrefab;
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private GameObject _enemyPrefab;
        [SerializeField] private int _countWall;
        private Vector3[,] _points;
        private Vector3 _currentWallPosition;
        private float _maxSize = 4.5f;
        private float _step = 1f;
        private float _currentX;
        private float _currentY;
        private int _countArray = 10;
        private List<GameObject> _listWall;
        private bool identityObj = false;


        private void Awake()
        {
            _listWall = new List<GameObject>();
            _points = new Vector3[_countArray, _countArray];
            _currentX = -_maxSize;
            _currentY = -_maxSize;
            GenerateArray();
            Spawn();
            GenerateWall();
            NavMeshBuilder.BuildNavMesh();
        }

        private void Spawn()
        {
            var player = Instantiate(_playerPrefab, new Vector3(_maxSize, 0.5f, -_maxSize), Quaternion.identity);
            var exit = Instantiate(_exitPrefab, new Vector3(-_maxSize, 0.5f, _maxSize), Quaternion.identity);
            var enemy1 = Instantiate(_enemyPrefab, new Vector3(_maxSize, 0.5f, _maxSize), Quaternion.identity);
            var enemy2 = Instantiate(_enemyPrefab, new Vector3(-_maxSize, 0.5f, -_maxSize), Quaternion.identity);

            _listWall.Add(player);
            _listWall.Add(exit);
            _listWall.Add(enemy1);
            _listWall.Add(enemy2);
        }

        private void GenerateWall()
        {
            for (int i = 0; i < _countWall; i++)
            {
                _currentWallPosition = FreePosition();

                var wall = Instantiate(_wall, _currentWallPosition, Quaternion.identity);
                _listWall.Add(wall);
            }
        }

        public Vector3 FreePosition()
        {
            int randomI;
            int randomJ;

            randomI = Random.Range(0, _countArray);
            randomJ = Random.Range(0, _countArray);

            _currentWallPosition = _points[randomI, randomJ];

            foreach (var obj in _listWall)
            {
                if (obj.transform.position == _currentWallPosition)
                {
                    identityObj = true;
                }
            }

            if (identityObj)
            {
                identityObj = false;
                FreePosition();
            }

            return _currentWallPosition;
        }

        private void GenerateArray()
        {
            for (int i = 0; i < _countArray; i++)
            {
                for (int j = 0; j < _countArray; j++)
                {
                    _points[i, j] = new Vector3(_currentX, 0.5f, _currentY);
                    _currentY += _step;
                }

                _currentX += _step;
                _currentY = -_maxSize;
            }
        }
    }
}