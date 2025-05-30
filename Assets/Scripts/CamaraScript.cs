using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraController : MonoBehaviour
{
    public Transform cristian;
    private const float velocidadCamara = 0.025f;

    // Ajusta este valor para que la cámara esté más arriba del jugador
    public Vector3 desplazamiento = new Vector3();

    private void LateUpdate()
    {
        if (cristian == null) return;

        // Calcula la posición deseada con el desplazamiento del personaje
        Vector3 posicionDeseada = cristian.position + desplazamiento;

        // forzar siempre el eje Z a -10 para que la camara capte todo el plano
        posicionDeseada.z = -10f;

        // Suavizar el movimiento de la cámara
        Vector3 posicionSuavizada = Vector3.Lerp(transform.position, posicionDeseada, velocidadCamara);

        // Aplicar la posición suavizada
        transform.position = posicionSuavizada;
    }
}

