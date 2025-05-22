using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ElementoInventarioUI : MonoBehaviour
{

    public Button boton;
    public Image icono;
    public TextMeshProUGUI textoCantidad;

    public int indice;
    public ElementoInventario elementoActual;

    public void Establecer(ElementoInventario elementoInventario)
    {
        elementoActual = elementoInventario;
        icono.gameObject.SetActive(true);
        icono.sprite = elementoInventario.elemento.icono;

        //si solo tengo un elemento, aparece el icono. Si es más de una unidad, aparece también la cantidad
        textoCantidad.text = elementoInventario.cantidad > 1 ? elementoInventario.cantidad.ToString() : string.Empty;
    }

    public void Limpiar()
    {
        //no elimino el elemento, lo desactivo para poder reutilizarlo
        icono.gameObject.SetActive(false);
        textoCantidad.text = string.Empty;
    }

    
    public void OnButtonClick()
    {

    }
    
}