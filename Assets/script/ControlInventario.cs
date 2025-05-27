using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ControlInventario : MonoBehaviour
{

    public ElementoInventarioUI[] elementosInventarioUI;
    public ElementoInventario[] elementoInventario;
    public GameObject ventanaInventario;
    public Transform posicionSoltar;

    [Header("Elemento seleccionado")]
    public ElementoInventario elementoSeleccionado;
    private int indiceElementoSeleccionado;
    public TextMeshProUGUI nombreElementoSeleccionado;
    public TextMeshProUGUI descripcionElementoSeleccionado;
    public TextMeshProUGUI nombreNecesidadElementoSeleccionado;
    public TextMeshProUGUI valoresNecesidadElementoSeleccionado;
    public Button botonUsar;
    public Button botonSoltar;

    public int cantidadSumar;

    //necesitamos el control del jugador
    private ControlJugador controladorJugador;
    private ControlIndicador controlIndicador;

    //el inventario no esta permanente en la pantalla
    [Header("Eventos")]
    public UnityEvent onAbrirInventario;
    public UnityEvent onCerrarInventario;

    //singleton
    public static ControlInventario instancia;

    public void Awake()
    {
        instancia = this;

        //Aprovecho el awake para tomar el control del jugador
        controladorJugador = GetComponent<ControlJugador>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        ventanaInventario.SetActive(false);

        // Inicializar el array de datos con el mismo tamaño que el array visual
        elementoInventario = new ElementoInventario[elementosInventarioUI.Length];

        for (int i = 0; i < elementoInventario.Length; i++)
        {
            elementoInventario[i] = new ElementoInventario();
            elementosInventarioUI[i].indice = i;
            elementosInventarioUI[i].Limpiar();

        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AbrirCerrarVentanaInventario()
    {
        if (ventanaInventario.activeInHierarchy)
        {
            //cerrar
            ventanaInventario.SetActive(false);
            controladorJugador.ModoInventario(false);// Cambiar el modo del jugador a no inventario
            Time.timeScale = 1f; // Reanudar el tiempo del juego
        }
        else
        {
            //abrir
            ventanaInventario.SetActive(true);
            controladorJugador.ModoInventario(true); // Cambiar el modo del jugador a inventario
            Time.timeScale = 0f; // Pausar el tiempo del juego
        }

    }


    public void OnBottonInventario(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            AbrirCerrarVentanaInventario();
        }
    }

    //consulta si la vemtana de inventario esta abierta
    private bool EstaAbierta()
    {
        return ventanaInventario.activeInHierarchy;
    }

    //actualiza el casillero del inventario
    public void AnadirElemento(DatosElemento elemento)
    {

        Debug.Log("Añadiendo elemento: " + elemento.nombre);
        ElementoInventario elementoparaAlmacenar = ObtenerElementoAlmacenado(elemento);
        if (elementoparaAlmacenar != null)
        {
            elementoparaAlmacenar.cantidad++;
            ActualizarUI();
            return;
        }

        ElementoInventario elementoVacio = ObtenerObjetoVacio();
        if (elementoVacio != null)
        {
            elementoVacio.cantidad = 1;
            elementoVacio.elemento = elemento;
            ActualizarUI();
            return;

        }
        else
        {
            Debug.Log("NO hay casilla vacia");
            SoltarElemento(elemento);
        }
    }

    private void SoltarElemento(DatosElemento elemento)
    {
        Debug.Log("Soltando elemento: " );
        Instantiate(elemento.prefab, posicionSoltar.position, Quaternion.identity);
    }

    private void ActualizarUI()
    {
        for (int i = 0; i < elementoInventario.Length; i++)
        {
            if (elementoInventario[i].elemento != null)
            {
                Debug.Log("Actualizando UI del elemento: " + elementoInventario[i].elemento.nombre);
                elementosInventarioUI[i].Establecer(elementoInventario[i]);
            }
            else
            {
                elementosInventarioUI[i].Limpiar();
            }
        }

    }

    ElementoInventario ObtenerElementoAlmacenado(DatosElemento elemento)
    {
        for (int i = 0; i < elementoInventario.Length; i++)
        {
            if (elementoInventario[i].elemento == elemento)
            {
                return elementoInventario[i];
                
            }
        }
        return null;
    }

    ElementoInventario ObtenerObjetoVacio()
    {
        for (int i = 0; i < elementoInventario.Length; i++)
        {
            if (elementoInventario[i].elemento == null)
            {
                return elementoInventario[i];
            }
        }
        return null;
    }

    public void ElementoSeleccionado(int indice)
    {
        Debug.Log("Elemento seleccionado en el inventario: " + indice);
        Debug.Log("Elemento seleccionado: " + elementoInventario[indice].elemento?.nombre);
        if (elementoInventario[indice] != null)
        {
            //si no hay elemento seleccionado, selecciono el elemento
            elementoSeleccionado = elementoInventario[indice];
            indiceElementoSeleccionado = indice;

            nombreElementoSeleccionado.text = elementoSeleccionado.elemento.nombre;
            descripcionElementoSeleccionado.text = elementoSeleccionado.elemento.descripcion;

            botonSoltar.gameObject.SetActive(true);
            botonUsar.gameObject.SetActive(true);
        }
        else
        {
            //si ya hay un elemento seleccionado, lo deselecciono
            elementoSeleccionado = null;
            indiceElementoSeleccionado = -1;
        }
    }

    void EliminarElementoSeleccionado(int indice)
    {
        elementoSeleccionado.cantidad--;
        if (elementoSeleccionado.cantidad <= 0)
        {
            elementoSeleccionado.elemento = null; // Elimino el elemento
            LimpiarElementoSeleccionado();
        }
        
    }

    private void LimpiarElementoSeleccionado()
    {
        elementoSeleccionado = null;
        nombreElementoSeleccionado.text = string.Empty;
        descripcionElementoSeleccionado.text = string.Empty;
        botonSoltar.gameObject.SetActive(false);
        botonUsar.gameObject.SetActive(false);
        // Actualizar la UI
        ActualizarUI();
    }

    public void OnBotonUsar()
    {
        switch (elementoSeleccionado.elemento.tipoElemento)
        {
            case TipoElemento.Comida:
                controlIndicador.indicadorHambre.SumarValor(cantidadSumar);
                break;
            case TipoElemento.Bebida:
                controlIndicador.indicadorSed.SumarValor(cantidadSumar);
                break;
            case TipoElemento.Descanso:
                controlIndicador.indicadorEnergia.SumarValor(cantidadSumar);                    
                break;
            default:
                Debug.LogWarning("Tipo de elemento no reconocido: " + elementoSeleccionado.elemento.tipoElemento);
                break;
        }
        
        EliminarElementoSeleccionado(indiceElementoSeleccionado);
    }

    public void OnBotonSoltar()
    {
        Debug.Log("estoy en on boton soltar");
        Debug.Log("Soltando elemento seleccionado: " + elementoSeleccionado);
        SoltarElemento(elementoSeleccionado.elemento);
        EliminarElementoSeleccionado(indiceElementoSeleccionado);
    }


}

public class ElementoInventario
{
    public DatosElemento elemento;
    public int cantidad;
}