using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum TipoElemento
{
    Comida,
    Bebida,
    Descanso,
}

[CreateAssetMenu(fileName = "NuevoElemento", menuName = "Inventario/Elemento")]
public class DatosElemento : ScriptableObject
{

    [Header("Datos del Elemento")]
    public string nombre;
    public string descripcion;
    public Sprite icono;
    public GameObject prefab;
    public TipoElemento tipoElemento;

}
