using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InheritrixScript : MonoBehaviour
{   
public GameObject disparoPrefab;         // Bala normal
public GameObject disparoHieloPrefab;    // Bala de hielo
public GameObject disparoFuegoPrefab;    // Bala de fuego
    private int contadorDisparos = 0;
    public Transform cristian;

    private int _salud = 3;
    private float _ultimoTiro;

    void Update()
    {
        if (cristian == null) return;

        Vector3 direccion = cristian.position - transform.position;
        if (direccion.x >= 0.0f) transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        else transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);

        float distancia = Mathf.Abs(cristian.position.x - transform.position.x);

        if (distancia < 1.0f && Time.time > _ultimoTiro + 1.25f) //*!Error al disparar
        {
            Disparo();
            _ultimoTiro = Time.time;
        }
    }
    
private void Disparo()
{
    Vector3 direccion = new Vector3(transform.localScale.x, 0.0f, 0.0f);
    Vector3 posicionDisparo = transform.position + direccion * 0.12f;

    int tipoDisparo = Random.Range(1, 4); // Número aleatorio entre 1 y 3

    GameObject disparo = null;

    switch (tipoDisparo)
    {
        case 1:
            disparo = Instantiate(disparoPrefab, posicionDisparo, Quaternion.identity);
            disparo.GetComponent<DisparoGetterGoblinScript>().Direccion = direccion;
            break;
        case 2:
            disparo = Instantiate(disparoHieloPrefab, posicionDisparo, Quaternion.identity);
            disparo.GetComponent<DisparoHieloScript>().Direccion = direccion;
            break;
        case 3:
            disparo = Instantiate(disparoFuegoPrefab, posicionDisparo, Quaternion.identity);
            disparo.GetComponent<DisparoFuegoScript>().Direccion = direccion;
            break;
    }
}


    public void Golpe()
    {
        _salud -= 1;
        if (_salud == 0) Destroy(gameObject);
    }
}
