using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour
{
    private float _bullet_speed = 25.0f;
    private Vector2 _velocity;
    [SerializeField] private Rigidbody2D _rb2d;
    private Vector2 _delta = new Vector2(0.0f, 0.0f);
    void Start()
    {
        _velocity = new Vector2(_bullet_speed, _bullet_speed);
       // _rb2d = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //_delta = _delta * _velocity * Time.deltaTime;
        //Vector2 newPosition = _rb2d.position + _delta;
        //_rb2d.MovePosition(newPosition);
    }
    public void Shoot(int dire)
    {
        switch(dire)
        {
            case 1:
                _delta = new Vector2(0f, _bullet_speed);
                break;
            case 2:
                _delta = new Vector2(_bullet_speed, 0f);
                break;
            case 3:
                _delta = new Vector2(0f, - _bullet_speed);
                break;
            case 4:
                _delta = new Vector2(- _bullet_speed, 0f);
                break;
            default: break;
        }
        Debug.Log("Rigidbody2D assigned: " + (_rb2d != null));
        Debug.Log(dire);
        Debug.Log(_delta.x);
        _rb2d.velocity = _delta;
    }
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.gameObject.tag == "wall")
        {
            Debug.Log(hitInfo);
            Destroy(gameObject);
        }
    }
}
