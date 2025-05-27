using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Runtime.CompilerServices;

/// <summary>
/// Clase que controla la alerta visual de salud (marco rojo) en pantalla.
/// </summary>
public class AlertaSalud : MonoBehaviour
{
    // Referencia a la imagen del marco rojo en la UI.
    public Image imageMarcoRojoo;

    // Velocidad con la que el marco rojo se desvanece.
    public float velocidadDeDesvanecimiento;

    // Referencia a la corrutina de desvanecimiento para poder controlarla.
    private Coroutine desvanecerCoroutine;

    /// <summary>
    /// Activa la alerta y comienza el desvanecimiento del marco rojo.
    /// </summary>
    public void AparecerAlerta()
    {
        imageMarcoRojoo.enabled = true; // Muestra la imagen.
        desvanecerCoroutine = StartCoroutine(DesvanecerMarcoRojoo()); // Inicia la corrutina.
    }

    /// <summary>
    /// Corrutina que reduce gradualmente la opacidad del marco rojo hasta hacerlo invisible.
    /// </summary>
    IEnumerator DesvanecerMarcoRojoo()
    {
        float alpha = 1f; // Opacidad inicial.
        while(alpha > 0.0f)
        {
            alpha -= Time.deltaTime / velocidadDeDesvanecimiento; // Reduce la opacidad.
            imageMarcoRojoo.color = new Color(1.0f,1.0f,1.0f,alpha); // Aplica el nuevo valor de opacidad.
            yield return null;   // Espera un frame.
        }

    
    }

    
    
}
