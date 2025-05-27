using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// Clase principal que controla el inventario del jugador
public class ControlInventario : MonoBehaviour
{
    // Array de elementos visuales del inventario (UI)
    public ElementoInventarioUI[] elementosInventarioUI;
    // Array de datos de los elementos almacenados en el inventario
    public ElementoInventario[] elementoInventario;
    // Referencia al GameObject de la ventana del inventario
    public GameObject ventanaInventario;
    // Posición en el mundo donde se soltarán los objetos
    public Transform posicionSoltar;

    [Header("Elemento seleccionado")] // Agrupa estas variables en el inspector de Unity
    // Elemento actualmente seleccionado en el inventario
    public ElementoInventario elementoSeleccionado;
    // Índice del elemento seleccionado
    private int indiceElementoSeleccionado;
    // Texto para mostrar el nombre del elemento seleccionado
    public TextMeshProUGUI nombreElementoSeleccionado;
    // Texto para mostrar la descripción del elemento seleccionado
    public TextMeshProUGUI descripcionElementoSeleccionado;
    // Texto para mostrar la necesidad asociada al elemento seleccionado
    public TextMeshProUGUI nombreNecesidadElementoSeleccionado;
    // Texto para mostrar los valores de la necesidad asociada
    public TextMeshProUGUI valoresNecesidadElementoSeleccionado;
    // Botón para usar el elemento seleccionado
    public Button botonUsar;
    // Botón para soltar el elemento seleccionado
    public Button botonSoltar;

    // Cantidad que se suma al usar un elemento (por ejemplo, recuperar hambre o sed)
    public int cantidadSumar;

    // Referencia al controlador del jugador
    private ControlJugador controladorJugador;
    // Referencia al controlador de indicadores (hambre, sed, energía)
    private ControlIndicador controlIndicador;

    [Header("Eventos")] // Agrupa los eventos en el inspector
    // Evento que se dispara al abrir el inventario
    public UnityEvent onAbrirInventario;
    // Evento que se dispara al cerrar el inventario
    public UnityEvent onCerrarInventario;

    // Singleton para acceder fácilmente al inventario desde otras clases
    public static ControlInventario instancia;

    // Método Awake, se ejecuta al inicializar el script
    public void Awake()
    {
        instancia = this; // Asigna la instancia para el singleton

        // Obtiene el componente ControlJugador del mismo GameObject
        controladorJugador = GetComponent<ControlJugador>();
    }

    // Método Start, se ejecuta antes del primer frame
    private void Start()
    {
        ventanaInventario.SetActive(false); // Oculta la ventana del inventario al inicio

        // Inicializa el array de datos con el mismo tamaño que el array visual
        elementoInventario = new ElementoInventario[elementosInventarioUI.Length];

        // Inicializa cada casilla del inventario
        for (int i = 0; i < elementoInventario.Length; i++)
        {
            elementoInventario[i] = new ElementoInventario();
            elementosInventarioUI[i].indice = i;
            elementosInventarioUI[i].Limpiar();
        }
    }

    // Método Update, se ejecuta una vez por frame (vacío en este caso)
    void Update()
    {

    }

    // Abre o cierra la ventana del inventario
    public void AbrirCerrarVentanaInventario()
    {
        if (ventanaInventario.activeInHierarchy)
        {
            // Si está abierta, la cierra y reanuda el juego
            ventanaInventario.SetActive(false);
            controladorJugador.ModoInventario(false); // Cambia el modo del jugador
            Time.timeScale = 1f; // Reanuda el tiempo del juego
        }
        else
        {
            // Si está cerrada, la abre y pausa el juego
            ventanaInventario.SetActive(true);
            controladorJugador.ModoInventario(true); // Cambia el modo del jugador
            Time.timeScale = 0f; // Pausa el tiempo del juego
        }
    }

    // Método para manejar la entrada del botón de inventario
    public void OnBottonInventario(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            AbrirCerrarVentanaInventario();
        }
    }

    // Devuelve true si la ventana del inventario está abierta
    private bool EstaAbierta()
    {
        return ventanaInventario.activeInHierarchy;
    }

    // Añade un elemento al inventario
    public void AnadirElemento(DatosElemento elemento)
    {
        Debug.Log("Añadiendo elemento: " + elemento.nombre);
        // Busca si ya existe el elemento en el inventario
        ElementoInventario elementoparaAlmacenar = ObtenerElementoAlmacenado(elemento);
        if (elementoparaAlmacenar != null)
        {
            elementoparaAlmacenar.cantidad++;
            ActualizarUI();
            return;
        }

        // Busca una casilla vacía
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
            SoltarElemento(elemento); // Si no hay espacio, suelta el objeto
        }
    }

    // Instancia el objeto en la posición de soltar
    private void SoltarElemento(DatosElemento elemento)
    {
        Debug.Log("Soltando elemento: " );
        Instantiate(elemento.prefab, posicionSoltar.position, Quaternion.identity);
    }

    // Actualiza la interfaz visual del inventario
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

    // Busca si el elemento ya está almacenado en el inventario
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

    // Busca una casilla vacía en el inventario
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

    // Selecciona un elemento del inventario
    public void ElementoSeleccionado(int indice)
    {
        Debug.Log("Elemento seleccionado en el inventario: " + indice);
        Debug.Log("Elemento seleccionado: " + elementoInventario[indice].elemento?.nombre);
        if (elementoInventario[indice] != null)
        {
            // Selecciona el elemento y actualiza la UI
            elementoSeleccionado = elementoInventario[indice];
            indiceElementoSeleccionado = indice;

            nombreElementoSeleccionado.text = elementoSeleccionado.elemento.nombre;
            descripcionElementoSeleccionado.text = elementoSeleccionado.elemento.descripcion;

            botonSoltar.gameObject.SetActive(true);
            botonUsar.gameObject.SetActive(true);
        }
        else
        {
            // Deselecciona si no hay elemento
            elementoSeleccionado = null;
            indiceElementoSeleccionado = -1;
        }
    }

    // Elimina una unidad del elemento seleccionado
    void EliminarElementoSeleccionado(int indice)
    {
        elementoSeleccionado.cantidad--;
        if (elementoSeleccionado.cantidad <= 0)
        {
            elementoSeleccionado.elemento = null; // Elimina el elemento si la cantidad es 0
            LimpiarElementoSeleccionado();
        }
    }

    // Limpia la selección y actualiza la UI
    private void LimpiarElementoSeleccionado()
    {
        elementoSeleccionado = null;
        nombreElementoSeleccionado.text = string.Empty;
        descripcionElementoSeleccionado.text = string.Empty;
        botonSoltar.gameObject.SetActive(false);
        botonUsar.gameObject.SetActive(false);
        // Actualiza la UI del inventario
        ActualizarUI();
    }

    // Acción al pulsar el botón de usar
    public void OnBotonUsar()
    {
        if (elementoSeleccionado == null || elementoSeleccionado.elemento == null)
        {
            Debug.LogWarning("No hay elemento seleccionado para usar.");
            return;
        }
        if (controlIndicador == null)
        {
            controlIndicador = FindObjectOfType<ControlIndicador>();
            if (controlIndicador == null)
            {
                Debug.LogWarning("No se encontró el ControlIndicador.");
                return;
            }
        }
        switch (elementoSeleccionado.elemento.tipoElemento)
        {
            case TipoElemento.Comida:
                if (controlIndicador.indicadorHambre != null)
                    controlIndicador.indicadorHambre.SumarValor(cantidadSumar);
                break;
            case TipoElemento.Bebida:
                if (controlIndicador.indicadorSed != null)
                    controlIndicador.indicadorSed.SumarValor(cantidadSumar);
                break;
            case TipoElemento.Descanso:
                if (controlIndicador.indicadorEnergia != null)
                    controlIndicador.indicadorEnergia.SumarValor(cantidadSumar);
                break;
            default:
                Debug.LogWarning("Tipo de elemento no reconocido: " + elementoSeleccionado.elemento.tipoElemento);
                break;
        }
        EliminarElementoSeleccionado(indiceElementoSeleccionado);
        ActualizarUI();
    }

    // Acción al pulsar el botón de soltar
    public void OnBotonSoltar()
    {
        Debug.Log("estoy en on boton soltar");
        Debug.Log("Soltando elemento seleccionado: " + elementoSeleccionado);
        SoltarElemento(elementoSeleccionado.elemento);
        EliminarElementoSeleccionado(indiceElementoSeleccionado);
        ActualizarUI();
    }
}

// Clase que representa un elemento almacenado en el inventario
public class ElementoInventario
{
    public DatosElemento elemento; // Referencia a los datos del elemento
    public int cantidad; // Cantidad de ese elemento en el inventario
}