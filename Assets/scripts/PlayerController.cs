using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using TMPro;

public class PlayerController : NetworkBehaviour
{

    [SerializeField] private float _player_speed = 2.0f;
    private Vector2 _velocity;
    private Rigidbody2D _rb2d;

    public GameObject _bulletTemplate;
    private int _dir=1;
    private int _Health = 100;
    private Vector3 _Spawn_Point = new Vector3(0.0f, 0.0f, 0.0f);
    private float _Shooting_Offset = 0.8f;

    //private readonly NetworkVariable<Color> _netColor = new();
    [SerializeField] private SpriteRenderer _renderer;
    private readonly Color[] _colors = { Color.yellow, Color.blue, Color.green, Color.red, Color.black, Color.white, Color.magenta, Color.gray };
    public int _index;

    void Start()
    {
        _velocity = new Vector2(_player_speed, _player_speed);
        _rb2d = GetComponent<Rigidbody2D>();
        _index = GameMnager.Instance.CountPlayer();
        ColorSet(_index);
    }

    //public override void OnNetworkSpawn()
    //{
    //    if (!IsOwner) Destroy(this);
    //}

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        float horiMove = Input.GetAxisRaw("Horizontal");
        float vertyMove = Input.GetAxisRaw("Vertical");
        Vector2 delta = new Vector2(0.0f, 0.0f);
        if (horiMove != 0f)
        {
            delta = new Vector2(horiMove, 0f);
            if (horiMove > 0) { _dir = 2; } else { _dir = 4; }
        }
        else if (vertyMove != 0f)
        {
            delta = new Vector2(0f, vertyMove);
            if (vertyMove > 0) { _dir = 1; } else { _dir = 3; }
        }
        else
        {
            delta = Vector2.zero;
        }
        delta = delta * _velocity * Time.deltaTime;
        Vector2 newPosition = _rb2d.position + delta;
        _rb2d.MovePosition(newPosition);

        if (Input.GetKeyDown(KeyCode.E))//render bullet
        {
            switch (_dir)
            {
                case 1:
                    _Spawn_Point = new Vector3(transform.position.x, transform.position.y+ _Shooting_Offset, 0.0f);
                    break;
                case 2:
                    _Spawn_Point = new Vector3(transform.position.x + _Shooting_Offset, transform.position.y, 0.0f);
                    break;
                case 3:
                    _Spawn_Point = new Vector3(transform.position.x, transform.position.y - _Shooting_Offset, 0.0f);
                    break;
                case 4:
                    _Spawn_Point = new Vector3(transform.position.x - _Shooting_Offset, transform.position.y, 0.0f);
                    break;
                default: break;
            }
            //GameMnager.Instance.SpawnBullet(_Spawn_Point, _dir);
            RequestFireServerRpc(_Spawn_Point, _dir);

            // Fire locally immediately
            ExecuteShoot(_Spawn_Point, _dir);

            
        }
        
    }

    [ServerRpc]
    private void RequestFireServerRpc(Vector3 _Spawn_Point, int _dir_)
    {
        FireClientRpc(_Spawn_Point, _dir_);
    }

    [ClientRpc]
    private void FireClientRpc(Vector3 _Spawn_Point, int _dir_)
    {
        if (!IsOwner) ExecuteShoot(_Spawn_Point, _dir_);
    }

    private void ExecuteShoot(Vector3 _Spawn_Point, int _dir_)
    {
        GameObject my_bullet = Instantiate(_bulletTemplate, _Spawn_Point, transform.rotation);
        my_bullet.GetComponent<bulletScript>().Shoot(_dir_);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (!IsOwner) return;
        if (hitInfo.gameObject.tag == "bullet")
        {
            Debug.Log(hitInfo);
            _Health -= 10;
            HealthSlider.Instance.SetHealth(_Health);
            if (HealthSlider.Instance.AmiDeadAlready()) { 
                GameMnager.Instance.GameEnd(false, _index);
                
                transform.position = new Vector3 (123f, 0f,0f);
                RequestCheakForSurvivorsServerRpc();
                //Destroy(gameObject);
            }
            
        }
        if (hitInfo.gameObject.tag == "coin")
        {
            Debug.Log(hitInfo);
            CoinScriptUI.Instance.Add();
        }
    }

    [ServerRpc]
    private void RequestCheakForSurvivorsServerRpc()
    {
        CheakClientRpc();
        Debug.Log("gonnacheak");
    }

    [ClientRpc]
    private void CheakClientRpc()
    {
        GameMnager.Instance.SmbDied();
        if (GameMnager.Instance.AmITheLastManStanding()) { Debug.Log("last one"); } else { Debug.Log("not one"); }
        if (!IsOwner&& GameMnager.Instance.AmITheLastManStanding())
        {
            
            GameMnager.Instance.GameEnd(true, _index);
        }
        
    }
    private void ColorSet(int id)
    {
        if (id > 8) id = 1;
        _renderer.color = _colors[id - 1];
    }
}
