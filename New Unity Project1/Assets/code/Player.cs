using code;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speedPlayer;
    private float _moveX;
    private float _moveY;
    private Vector3 _move;
    public bool IsPlayerMove;

    private void Update()
    {
        Move();
    }

    public void Move()
    {
        
        _moveX = Input.GetAxis("Horizontal");
        _moveY = Input.GetAxis("Vertical");
        _move = new Vector3(_moveX, 0, _moveY).normalized; // исправил движение по диагонали
        IsPlayerMove = Vector3.zero != _move ? true: false;
        transform.Translate(_move * Time.deltaTime * _speedPlayer);
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            FindObjectOfType<EndGame>().Lose();
        }
    }
}
