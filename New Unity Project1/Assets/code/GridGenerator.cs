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
        [Range(0,60)]
        [SerializeField] private int _countWall;
        [SerializeField] private GameObject _nullPrefab;
        private Vector3[,] _points;
        private int[,,] _path;
        private Vector3 _currentWallPosition;
        private float _maxSize = 4.5f;
        private float _step = 1f;
        private float _currentX;
        private float _currentY;
        private int _countArray = 10;
        private bool _isIdentityObj = false;
        private List<GameObject> _listWall;


        private void Awake()
        {
            DrawPath();
            _listWall = new List<GameObject>();
            _points = new Vector3[_countArray, _countArray];
            _currentX = -_maxSize;
            _currentY = -_maxSize;
            GenerateArray();
            Spawn();
            GenerateWall();
        }

        private void Start()
        {
            NavMeshBuilder.BuildNavMesh();
        }

        public void DrawPath()
        {
            _path = new int[,,]
            {
                {
                    {1, 1, 1, 0, 0, 0, 0, 1, 1, 1},
                    {0, 0, 1, 1, 1, 1, 1, 1, 0, 0},
                    {0, 0, 0, 0, 0, 1, 0, 0, 0, 0},
                    {0, 0, 0, 0, 1, 1, 0, 0, 0, 0},
                    {0, 0, 0, 0, 1, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 1, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 1, 0, 0, 0, 0, 0},
                    {0, 0, 0, 1, 1, 0, 0, 0, 0, 0},
                    {0, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                    {1, 1, 0, 0, 0, 0, 0, 0, 0, 1}
                },
                {
                    {1, 1, 1, 1, 0, 0, 0, 1, 1, 1},
                    {0, 0, 0, 1, 0, 0, 0, 1, 1, 1},
                    {0, 0, 0, 1, 1, 1, 1, 1, 0, 0},
                    {0, 0, 0, 0, 1, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 1, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 1, 1, 1, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 1, 0, 0, 0},
                    {0, 0, 0, 0, 0, 1, 1, 0, 0, 0},
                    {0, 0, 0, 0, 0, 1, 0, 0, 0, 0},
                    {1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
                },
                {
                    {1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                    {0, 0, 0, 1, 0, 0, 0, 0, 0, 1},
                    {0, 0, 1, 1, 0, 0, 0, 0, 0, 1},
                    {0, 0, 1, 0, 0, 0, 0, 0, 0, 1},
                    {0, 0, 1, 0, 0, 0, 0, 0, 0, 1},
                    {0, 0, 1, 0, 0, 0, 0, 0, 0, 1},
                    {0, 1, 1, 0, 0, 0, 0, 0, 0, 1},
                    {0, 1, 0, 0, 0, 0, 0, 0, 0, 1},
                    {0, 1, 0, 0, 0, 0, 0, 0, 0, 1},
                    {1, 1, 0, 0, 0, 0, 0, 0, 0, 1}
                },
                {
                    {1, 1, 1, 0, 0, 0, 0, 0, 0, 1},
                    {0, 0, 1, 0, 0, 0, 0, 0, 0, 1},
                    {0, 0, 1, 1, 0, 0, 0, 0, 0, 1},
                    {0, 0, 0, 1, 0, 0, 0, 0, 0, 1},
                    {0, 0, 0, 1, 0, 0, 0, 0, 0, 1},
                    {0, 0, 0, 1, 1, 0, 0, 0, 0, 1},
                    {0, 0, 0, 0, 1, 1, 1, 1, 1, 1},
                    {0, 0, 0, 0, 0, 0, 0, 1, 0, 1},
                    {0, 0, 0, 0, 0, 0, 0, 1, 0, 1},
                    {1, 1, 1, 1, 1, 1, 1, 1, 0, 1}
                },
            };
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

        public void FreePath(int road)
        {
            Debug.Log(road);
            for (int i = 0; i < _countArray; i++)
            {
                for (int j = 0; j < _countArray; j++)
                {
                    if (_path[road, i, j] == 1)
                    {
                        var a = Instantiate(_nullPrefab, _points[i, j], Quaternion.identity);
                        _listWall.Add(a);
                    }
                }
            }
        }

        private void GenerateWall()
        {
            Vector3 _startPosPlayer = new Vector3(_maxSize, 0.5f, -_maxSize);
            int NumberRoad = Random.Range(0, 3);
            FreePath(NumberRoad);
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
                    _isIdentityObj = true;
                }
            }

            if (_isIdentityObj)
            {
                _isIdentityObj = false;
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