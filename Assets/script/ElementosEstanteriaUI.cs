using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ElementoInventarioUI : MonoBehaviour
{

    public Button boton; // Botón asociado a este elemento en la UI
    public Image icono; // Imagen que muestra el icono del elemento
    public TextMeshProUGUI textoCantidad; // Texto que muestra la cantidad del elemento

    public int indice; // Indice del elemento en el inventario
    public ElementoInventario elementoActual; // Referencia al elemento de inventario actual

    // Método para establecer la información del elemento en la UI
    public void Establecer(ElementoInventario elementoInventario)
    {
        elementoActual = elementoInventario; // Asigna el elemento actual
        icono.gameObject.SetActive(true); // Activa el objeto de la imagen del icono
        icono.sprite = elementoInventario.elemento.icono; // Establece el sprite del icono

        //si solo tengo un elemento, aparece el icono. Si es más de una unidad, aparece también la cantidad
        textoCantidad.text = elementoInventario.cantidad > 1 ? elementoInventario.cantidad.ToString() : string.Empty;
    }

    // Método para limpiar la información del elemento en la UI
    public void Limpiar()
    {
        //no elimino el elemento, lo desactivo para poder reutilizarlo
        icono.gameObject.SetActive(false); // Desactiva el objeto de la imagen del icono
        textoCantidad.text = string.Empty; // Limpia el texto de la cantidad
    }
    
    
    // Método llamado cuando se hace clic en el botón del elemento
    public void OnButtonClick()
    {
        ColorBlock colorBlock = boton.colors; // Obtiene los colores del botón
        colorBlock.normalColor = Color.white; // Establece el color normal del botón a blanco
        colorBlock.pressedColor = Color.green; // Establece el color presionado del botón a verde
        Debug.Log("estoy en ElementoInventarioUI OnButtonClick");
        Debug.Log("indice: " + indice);
        
        ControlInventario.instancia.ElementoSeleccionado(indice); // Llama al método ElementoSeleccionado en ControlInventario
    }
    
}