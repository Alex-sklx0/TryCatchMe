using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CristianMovimiento : MonoBehaviour
{
    public float velocidad;
    public float fuerzaSalto;
    public GameObject disparoPrefab;

    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private float _horizontal;
    private bool _tocaSuelo;
    private float _ultimoDisparo;
    private int _salud = 5;
    public int Salud
    {
        get
        {
            return _salud;
        }
    }

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Movimiento
        _horizontal = Input.GetAxisRaw("Horizontal");

        //para que cristian gire el sprite hacia atras cuando se teclee "a"
        if (_horizontal < 0.0f) transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        else if (_horizontal > 0.0f) transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        // para saber si hay que hacer animacion de correr o no
        _animator.SetBool("corriendo", _horizontal != 0.0f);

        // Detectar Suelo
         Debug.DrawRay(transform.position, Vector3.down * 0.12f, Color.red);
        if (Physics2D.Raycast(transform.position, Vector3.down, 0.12f))
        {
            _tocaSuelo = true;
        }
        else _tocaSuelo = false;

        // Salto
        if (Input.GetKeyDown(KeyCode.W) && _tocaSuelo)
        {
            Salto();
        }

        // Disparar
        if (Input.GetKey(KeyCode.Space) && Time.time > _ultimoDisparo + 0.25f)
        {
            Disparo();
            _ultimoDisparo = Time.time;
        }
    }

    private void FixedUpdate()
    {
        _rigidbody2D.linearVelocity = new Vector2(_horizontal * velocidad, _rigidbody2D.linearVelocity.y);
    }

    private void Salto()
    {
        _rigidbody2D.AddForce(Vector2.up * fuerzaSalto);
    }

    private void Disparo()
    {
        Vector3 direccion;
//establecer direccion de la bala segun la direccion del personaje
        if (transform.localScale.x == 1.0f) direccion = Vector3.right;
        else direccion = Vector3.left;
//llamar a la clase disparo
        GameObject disparo = Instantiate(disparoPrefab, transform.position + direccion * 0.12f, Quaternion.identity);
        disparo.GetComponent<DisparoScript>().Direccion=direccion;
    }

    public void Golpe()
    {
        _salud -= 1;
        if (_salud == 0) Destroy(gameObject);
    }
}