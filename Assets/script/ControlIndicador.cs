using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;






public class ControlIndicador : MonoBehaviour, IDeterioro
{
    public Indicador indicadorSalud;
    public Indicador indicadorEnergia;
    public Indicador indicadorHambre;
    public Indicador indicadorSed;
    public float reduccionSaludConHambre;
    public float reduccionSaludConSed;
    public TextMeshProUGUI textoGameOver;
    public UnityEvent OnSufrirDeterioro;


    void Start()
    {
        //inicializamos los valores de las barras
        indicadorSalud.valorActual = indicadorSalud.valorInicial;
        indicadorEnergia.valorActual = indicadorEnergia.valorInicial;
        indicadorHambre.valorActual = indicadorHambre.valorInicial;
        indicadorSed.valorActual = indicadorSed.valorInicial;
    }

    void Update()
    {
        indicadorHambre.image.fillAmount = indicadorHambre.ObtenerPorcentaje();
        indicadorSed.image.fillAmount = indicadorSed.ObtenerPorcentaje();
        indicadorEnergia.image.fillAmount = indicadorEnergia.ObtenerPorcentaje();
        indicadorSalud.image.fillAmount = indicadorSalud.ObtenerPorcentaje();
        indicadorHambre.RestarValor(indicadorHambre.indiceDeterioro * Time.deltaTime);
        indicadorSed.RestarValor(indicadorSed.indiceDeterioro * Time.deltaTime);
        indicadorEnergia.RestarValor(indicadorEnergia.indiceDeterioro * Time.deltaTime);
        if (indicadorHambre.valorActual <= 0.0f)
        {
            indicadorSalud.RestarValor(reduccionSaludConHambre * Time.deltaTime);
        }
        if (indicadorSed.valorActual <= 0.0f)
        {
            indicadorSalud.RestarValor(reduccionSaludConSed * Time.deltaTime);
        }if (indicadorSalud.valorActual <= 0.0f)
        {
            Morir();
        }if(Input.GetKeyDown(KeyCode.H)){
            indicadorHambre.SumarValor(100);
        }if(Input.GetKeyDown(KeyCode.B)){
            indicadorSed.SumarValor(100);
        }}

    public void Morir(){
        
            textoGameOver.gameObject.SetActive(true);
            Time.timeScale = 0;
        
    }
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

    

    public void ProduccirDeterioro(float cantidad){
        indicadorSalud.RestarValor(cantidad);
        OnSufrirDeterioro?.Invoke();
    }

    

}
[System.Serializable]
public class Indicador
{
    public Image image;
    [HideInInspector] public float valorActual;
    public float valorInicial;

    public float indiceRecuperacion;
    public float indiceDeterioro;


    //metodo para sumar el valor de la barra
    public void SumarValor(float valor)
    {
        valorActual += valor;
        if (valorActual > valorInicial)
        {
            valorActual = valorInicial;
        }
    }
    //metodo para restar el valor de la barra
    public void RestarValor(float valor)
    {
        valorActual -= valor;
        if (valorActual < 0.0f)
        {
            valorActual = 0.0f;
        }
    }

    //metodo obtener porcentaje de la barra
    public float ObtenerPorcentaje()
    {
        return valorActual / valorInicial;
    }



}

public interface IDeterioro
{
    void ProduccirDeterioro(float cantidad);
    void Restaurar(float valorRecuperacion, string codigo);
    void Morir();
}