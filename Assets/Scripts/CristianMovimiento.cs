using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CristianMovimiento : MonoBehaviour
{
    public float velocidad;
    private float _velocidadBase;
    private Coroutine _corutinaRalentizacion;
    public float fuerzaSalto;
    private bool _saltoBloqueado = false;
    public GameObject disparoPrefab;
    public SpriteRenderer spriteRenderer;
    private bool _disparosBloqueados = false;
    private float tiempoUltimoDisparo;
    public float cooldownDisparo = 0.5f;
    private bool _stuneado = false;
    private int _saltosNecesarios = 0;
    private int _saltosRealizados = 0;

    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private float _horizontal;
    private bool _tocaSuelo;
    private float _ultimoDisparo;
    private float _salud = 5;
    public float Salud
    {
        get
        {
            return _salud;
        }
    }

    private void Start()
    {
        _velocidadBase = velocidad; // Guarda l
                                    // a velocidad original
        spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Movimiento
        _horizontal = Input.GetAxisRaw("Horizontal");

        // Rotación del sprite
        if (_horizontal < 0.0f) transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        else if (_horizontal > 0.0f) transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        // Animación de correr
        _animator.SetBool("corriendo", _horizontal != 0.0f);

        // Detección de suelo
        Debug.DrawRay(transform.position, Vector3.down * 0.12f, Color.red);
        _tocaSuelo = Physics2D.Raycast(transform.position, Vector3.down, 0.12f);

        // Salto normal
        if (Input.GetKeyDown(KeyCode.W) && _tocaSuelo && !_saltoBloqueado)
        {
            Salto();

            // Contar saltos solo si está stuneado
            if (_stuneado)
            {
                _saltosRealizados++;
                Debug.Log($"Saltos realizados: {_saltosRealizados}/{_saltosNecesarios}");
            }
        }

        // Disparo (solo si no está bloqueado)
        if (Input.GetKey(KeyCode.Space) && Time.time > _ultimoDisparo + 0.25f && !_disparosBloqueados)
        {
            Disparo();
            _ultimoDisparo = Time.time;
        }
    }
    private void FixedUpdate()
    {
        if (_stuneado)
        {
            // El jugador no puede moverse lateralmente mientras esté stuneado
            _rigidbody2D.linearVelocity = new Vector2(0f, _rigidbody2D.linearVelocity.y);
        }
        else
        {
            _rigidbody2D.linearVelocity = new Vector2(_horizontal * velocidad, _rigidbody2D.linearVelocity.y);
        }
    }

    private void Salto()
{
    if (_saltoBloqueado) return;
    
    _rigidbody2D.AddForce(Vector2.up * fuerzaSalto);
}

    public void BloquearDisparos(bool estado)
    {
        _disparosBloqueados = estado;
    }

    private void Disparo()
    {
        if (_disparosBloqueados) return;

        if (Time.time < tiempoUltimoDisparo + cooldownDisparo) return; // <-- Este es el cooldown

        // Resto del código de disparo
        Vector3 direccion = transform.localScale.x == 1.0f ? Vector3.right : Vector3.left;
        GameObject disparo = Instantiate(disparoPrefab, transform.position + direccion * 0.12f, Quaternion.identity);
        disparo.GetComponent<DisparoScript>().Direccion = direccion;

        tiempoUltimoDisparo = Time.time; // <-- Actualizamos el tiempo del último disparo
    }


    public void Golpe(float dano)
    {
        _salud = _salud - dano;
        if (_salud <= 0) Destroy(gameObject);
    }

    public void AplicarStun()
    {
        if (!_stuneado)
        {
            _stuneado = true;
            _saltosNecesarios = 3;
            _saltosRealizados = 0;
            StartCoroutine(StunCoroutine());

            // Feedback adicional
            Debug.Log("¡Estás aturdido! Salta 3 veces para liberarte");
        }
    }

    IEnumerator StunCoroutine()
    {
        // Guardar estado original
        bool podiaDisparar = !_disparosBloqueados;

        // Aplicar stun (bloquear disparos)
        _disparosBloqueados = true;

        // Efecto visual
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        Color originalColor = renderer.color;
        renderer.color = Color.yellow;

        // Esperar a que el jugador salte 3 veces
        while (_saltosRealizados < _saltosNecesarios)
        {
            yield return null;
        }

        // Restaurar estado
        _stuneado = false;
        _disparosBloqueados = !podiaDisparar; // Restaurar estado original de los disparos
        renderer.color = originalColor;

        // Resetear contador de saltos
        _saltosRealizados = 0;
        _saltosNecesarios = 0;
    }


    public void AplicarRalentizacion(float factor, float duracion)
    {
        // Detener ralentización existente
        StopAllCoroutines();

        // Aplicar nueva ralentización
        StartCoroutine(EfectoRalentizacion(factor, duracion));
    }

    private IEnumerator EfectoRalentizacion(float factor, float duracion)
    {
        // Aplicar efecto
        velocidad = _velocidadBase * factor;

        // Efecto visual (opcional)
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.blue;
        }

        // Esperar duración
        yield return new WaitForSeconds(duracion);

        // Restaurar valores
        velocidad = _velocidadBase;


        spriteRenderer.color = Color.white;

    }

    public void BloquearSalto(float duracion)
{
    if (!_saltoBloqueado)
    {
        StartCoroutine(BloqueoSaltoCoroutine(duracion));
    }
}

private IEnumerator BloqueoSaltoCoroutine(float duracion)
{
    _saltoBloqueado = true;
    
    // Efecto visual opcional
    SpriteRenderer renderer = GetComponent<SpriteRenderer>();
    Color originalColor = renderer.color;
    renderer.color = Color.magenta;
    
    yield return new WaitForSeconds(duracion);
    
    _saltoBloqueado = false;
    renderer.color = originalColor;
}
}