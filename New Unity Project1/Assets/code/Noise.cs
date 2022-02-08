using UnityEngine;
using UnityEngine.UI;

public class Noise : MonoBehaviour
{
    [SerializeField] private Image _imageFrontNoise;
    private int _minNoise = 0;
    private int _maxNoise = 10;
    private int _stepNoiseUp = 3;
    private float _currentNoise;
    private float _currentTime;
    private float _finalTime;
    private Player _player;

    public float CurrentNoise => _currentNoise;


    private void Start()
    {
        _player = FindObjectOfType<Player>();
        _imageFrontNoise.fillAmount = 0.0f;
    }

    private void Update()
    {
        NoisePlayer();
    }

    private void NoisePlayer()
    {
        _currentTime += Time.deltaTime;
        if (_player.IsPlayerMove)
        {
            _finalTime = 1;
        }
        else
        {
            _finalTime = 0.5f;
        }

        if (_currentTime >= _finalTime)
        {
            _currentTime = 0;

            if (_finalTime == 1)
            {
                ChangeNoise(3);
            }
            else if (_finalTime == 0.5f)
            {
                ChangeNoise(-1);
            }
        }
    }

    private void ChangeNoise(float change)
    {
        var test = _currentNoise + change;
        if (test > _maxNoise)
        {
            test = _maxNoise;
        }
        else if(test < _minNoise)
        {
            test = _minNoise;
        }

        _currentNoise = test;
        _imageFrontNoise.fillAmount = _currentNoise / 10;
    }

}
