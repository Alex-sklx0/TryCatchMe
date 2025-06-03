
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraController : MonoBehaviour
{
    public Transform cristian;
    private const float velocidadCamara = 0.025f;
    private const float PosicionInicialZ = -10f;


    // Ajusta este valor para que la cámara esté más arriba del jugador
    public Vector3 desplazamiento = new Vector3();

    private void LateUpdate()
    {
        try
        {
            if (cristian == null)
            {
                Debug.LogWarning("[CamaraController] No se asignó el transform de Cristian.");
                return;
            }

            // Calcular la posición deseada con desplazamiento
            Vector3 posicionDeseada = cristian.position + desplazamiento;

            // Forzar Z a -10 para mantener la cámara en 2D
            posicionDeseada.z = PosicionInicialZ;

            // Suavizar el movimiento
            Vector3 posicionSuavizada = Vector3.Lerp(transform.position, posicionDeseada, velocidadCamara);

            // Aplicar la nueva posición
            transform.position = posicionSuavizada;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[CamaraController] Error en LateUpdate: {e.Message}", this);
        }
    }
}
