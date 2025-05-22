using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectoElemento : MonoBehaviour, IInteractuable
{
    public DatosElemento datosElemento; // Referencia al ScriptableObject que contiene los datos del elemento
    public void Interactuar()
    {
        // Aquí puedes implementar la lógica de interacción, como recoger el objeto o mostrar información.
        // Por ejemplo, podrías destruir el objeto al recogerlo:
        Destroy(gameObject);
        
    }

    public string ObtenerMensajeInteraccion()
    {
    return "Dale a la E para conquistar el mundo  " +  datosElemento.nombre;
    }
}


