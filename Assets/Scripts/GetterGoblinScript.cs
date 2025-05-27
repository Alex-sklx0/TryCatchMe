using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetterGoblinScript : MonoBehaviour
{
    public Transform cristian;
    public GameObject disparoPrefab;

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
        GameObject disparo = Instantiate(disparoPrefab, transform.position + direccion * 0.12f, Quaternion.identity);
        disparo.GetComponent<DisparoGetterGoblinScript>().Direccion = direccion;
    }

    public void Golpe()
    {
        _salud -= 1;
        if (_salud == 0) Destroy(gameObject);
    }
    
}
