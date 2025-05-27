using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Clase principal que controla los indicadores de salud, energía, hambre y sed del jugador.
public class ControlIndicador : MonoBehaviour, IDeterioro
{
    // Referencias a los indicadores de la UI.
    public Indicador indicadorSalud;
    public Indicador indicadorEnergia;
    public Indicador indicadorHambre;
    public Indicador indicadorSed;

    // Cantidad de salud que se reduce cuando el hambre o la sed llegan a cero.
    public float reduccionSaludConHambre;
    public float reduccionSaludConSed;

    // Texto que se muestra cuando el jugador muere.
    public TextMeshProUGUI textoGameOver;

    // Evento que se dispara cuando el jugador sufre deterioro.
    public UnityEvent OnSufrirDeterioro;

    void Start()
    {
        // Inicializamos los valores actuales de las barras con sus valores iniciales.
        indicadorSalud.valorActual = indicadorSalud.valorInicial;
        indicadorEnergia.valorActual = indicadorEnergia.valorInicial;
        indicadorHambre.valorActual = indicadorHambre.valorInicial;
        indicadorSed.valorActual = indicadorSed.valorInicial;
    }

    void Update()
    {
        // Actualizamos el llenado visual de las barras según su porcentaje actual.
        indicadorHambre.image.fillAmount = indicadorHambre.ObtenerPorcentaje();
        indicadorSed.image.fillAmount = indicadorSed.ObtenerPorcentaje();
        indicadorEnergia.image.fillAmount = indicadorEnergia.ObtenerPorcentaje();
        indicadorSalud.image.fillAmount = indicadorSalud.ObtenerPorcentaje();

        // Restamos valores a las barras de hambre, sed y energía según su índice de deterioro.
        indicadorHambre.RestarValor(indicadorHambre.indiceDeterioro * Time.deltaTime);
        indicadorSed.RestarValor(indicadorSed.indiceDeterioro * Time.deltaTime);
        indicadorEnergia.RestarValor(indicadorEnergia.indiceDeterioro * Time.deltaTime);

        // Si el hambre llega a cero, se reduce la salud.
        if (indicadorHambre.valorActual <= 0.0f)
        {
            indicadorSalud.RestarValor(reduccionSaludConHambre * Time.deltaTime);
        }
        // Si la sed llega a cero, se reduce la salud.
        if (indicadorSed.valorActual <= 0.0f)
        {
            indicadorSalud.RestarValor(reduccionSaludConSed * Time.deltaTime);
        }
        // Si la salud llega a cero, el jugador muere.
        if (indicadorSalud.valorActual <= 0.0f)
        {
            Morir();
        }
        // Recarga de hambre al presionar la tecla H (para pruebas).
        if(Input.GetKeyDown(KeyCode.H)){
            indicadorHambre.SumarValor(100);
        }
        // Recarga de sed al presionar la tecla B (para pruebas).
        if(Input.GetKeyDown(KeyCode.B)){
            indicadorSed.SumarValor(100);
        }
    }

    // Método que se ejecuta cuando el jugador muere.
    public void Morir(){
        textoGameOver.gameObject.SetActive(true); // Muestra el texto de Game Over.
        Time.timeScale = 0; // Detiene el tiempo del juego.
    }

    // Método para restaurar un valor específico de un indicador según el código recibido.
    public void Restaurar(float valorRecuperacion,string codigo){
        switch (codigo.ToLower())
        {
            case "salud":
                indicadorSalud.SumarValor(valorRecuperacion);
                break;
            case "energia":
                indicadorEnergia.SumarValor(valorRecuperacion);
                break;
            case "hambre":
                indicadorHambre.SumarValor(valorRecuperacion);
                break;
            case "sed":
                indicadorSed.SumarValor(valorRecuperacion);
                break;
            default:
                Debug.LogError("Codigo de restauracion " + codigo + " no valido");
                break;
        }
    }

    // Método que aplica deterioro a la salud y dispara el evento correspondiente.
    public void ProduccirDeterioro(float cantidad){
        indicadorSalud.RestarValor(cantidad);
        OnSufrirDeterioro?.Invoke();
    }
}

// Clase que representa un indicador (barra) de la UI.
[System.Serializable]
public class Indicador
{
    public Image image; // Imagen de la barra en la UI.
    [HideInInspector] public float valorActual; // Valor actual de la barra.
    public float valorInicial; // Valor inicial de la barra.

    public float indiceRecuperacion; // Velocidad de recuperación.
    public float indiceDeterioro;    // Velocidad de deterioro.

    // Suma valor a la barra, sin exceder el valor inicial.
    public void SumarValor(float valor)
    {
        valorActual += valor;
        if (valorActual > valorInicial)
        {
            valorActual = valorInicial;
        }
    }
    // Resta valor a la barra, sin bajar de cero.
    public void RestarValor(float valor)
    {
        valorActual -= valor;
        if (valorActual < 0.0f)
        {
            valorActual = 0.0f;
        }
    }

    // Devuelve el porcentaje actual de la barra (entre 0 y 1).
    public float ObtenerPorcentaje()
    {
        return valorActual / valorInicial;
    }
}

// Interfaz para objetos que pueden sufrir deterioro y restauración.
public interface IDeterioro
{
    void ProduccirDeterioro(float cantidad);
    void Restaurar(float valorRecuperacion, string codigo);
    void Morir();
}