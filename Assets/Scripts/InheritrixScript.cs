using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inheritrix : MonoBehaviour, IDanable
{   
    private const float TiempoEntreDisparos = 1.25f;
    private const float RangoDisparo = 1f;
    private const float ConstanteEjeSprite = 1f; //constante para cuando se gira el srite en direccion y,z

    private const float ConstanteDireccionDisparo = 0.0f; //constante para cuando genera un disparo en direccion y,z
    private const float ConstanteHorizontalDisparo = 0.12f; //constante para cuando genera un disparo en direccion y,z

    private const int SaludMin = 0;

    [SerializeField] private GameObject _disparoPrefab;         // Bala normal
    [SerializeField] private GameObject _disparoHieloPrefab;    // Bala de hielo
    [SerializeField] private GameObject _disparoFuegoPrefab;    // Bala de fuego
    [SerializeField] private Transform _cristianPosicion;           // Referencia al jugador (asignable desde el inspector)

    private int _salud = 3;
    private float _ultimoTiro;
    private Vector2 _direccionMovimiento;


    void Update()
    {
        if (_cristianPosicion == null) return;
        ActualizarOrientacion();
        ControlarDisparo();
    }
   private void ActualizarOrientacion()
    {
            _direccionMovimiento = (_cristianPosicion.position - transform.position).normalized;
            transform.localScale = new Vector3(Mathf.Sign(_direccionMovimiento.x), ConstanteEjeSprite, ConstanteEjeSprite);
    }
    private void ControlarDisparo()
    {
        float distanciaAlJugador = Vector3.Distance(_cristianPosicion.position, transform.position);

        // Disparar si esta cerca y ha pasado suficiente tiempo
        if (distanciaAlJugador <= RangoDisparo && Time.time > _ultimoTiro + TiempoEntreDisparos) 
        {
            Disparo();
            _ultimoTiro = Time.time;
        }
    }

private void Disparo()
    {
        Vector3 direccion = new Vector3(transform.localScale.x, ConstanteDireccionDisparo, ConstanteDireccionDisparo);
        Vector3 posicionDisparo = transform.position + direccion * ConstanteHorizontalDisparo;

        int tipoDisparo = Random.Range(1, 4); // Número aleatorio entre 1 y 3 (1=normal, 2=hielo, 3=fuego)

        GameObject disparo = null;

        switch (tipoDisparo)
        {
            case 1:
                disparo = Instantiate(_disparoPrefab, posicionDisparo, Quaternion.identity);
                disparo.GetComponent<DisparoGetterGoblin>().Direccion = direccion;
                break;
            case 2:
                disparo = Instantiate(_disparoHieloPrefab, posicionDisparo, Quaternion.identity);
                disparo.GetComponent<DisparoHielo>().Direccion = direccion;
                break;
            case 3:
                disparo = Instantiate(_disparoFuegoPrefab, posicionDisparo, Quaternion.identity);
                disparo.GetComponent<DisparoFuego>().Direccion = direccion;
                break;
        }
    }


   
  private void RecibirDano()
    {
        _salud--;
        if (_salud <= SaludMin) Destroy(gameObject);//destruir con tiempo para aplciar la animacion } // Método público que llama al privado  
    }
    public void Golpe()
    {
        RecibirDano();

    }
}