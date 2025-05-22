using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NuevoElemento", menuName = "Inventario/Elemento")]
public class DatosElemento : ScriptableObject
{

    [Header("Datos del Elemento")]
    public string nombre;
    public string descripcion;
    public Sprite icono;
    public GameObject prefab;

}
