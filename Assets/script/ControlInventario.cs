using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ControlInventarioUI : MonoBehaviour
{

    public ElementoInventarioUI[] elementosInventarioUI;
    public ElementoInventario[] elementoInventario;
    public GameObject ventanaInventario;
    public Transform posicionSoltar;

    [Header("Elemento seleccionado")]
    private ElementoInventario elementoSeleccionado;
    private int indiceElementoSeleccionado;
    public TextMeshProUGUI nombreElementoSeleccionado;
    public TextMeshProUGUI descripcionElementoSeleccionado;
    public TextMeshProUGUI nombreNecesidadElementoSeleccionado;
    public TextMeshProUGUI valoresNecesidadElementoSeleccionado;
    public Button botonUsar;
    public Button botonSoltar;

    //necesitamos el control del jugador
    private ControlJugador controladorJugador;

    //el inventario no esta permanente en la pantalla
    [Header("Eventos")]
    public UnityEvent onAbrirInventario;
    public UnityEvent onCerrarInventario;

    //singleton
    public static ControlInventarioUI instancia;

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

    private void AbrirCerrarVentanaInventario()
    {

    }

    //consulta si la vemtana de inventario esta abierta
    private bool EstaAbierta()
    {
        return ventanaInventario.activeInHierarchy;
    }

    //actualiza el casillero del inventario
    private void AnadirElemento(DatosElemento elemento)
    {
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
            //mensaje de error
        }
    }

    private void SoltarElemento(DatosElemento elemento)
    {

    }

    private void ActualizarUI()
    {

    }

    ElementoInventario ObtenerElementoAlmacenado(DatosElemento elemento)
    {
        return null;
    }

    ElementoInventario ObtenerObjetoVacio()
    {
        return null;
    }

    void ElementoSeleccionado(int indice)
    {

    }

    void EliminarElementoSeleccionado(int indice)
    {

    }

    public void OnBotonUsar()
    {

    }

    public void OnBotonSoltar()
    {
        
    }
    
    
}

public class ElementoInventario
{
    public DatosElemento elemento;
    public int cantidad;
}