using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Runtime.CompilerServices;

public class AlertaSalud : MonoBehaviour
{
    public Image imageMarcoRojoo;
    public float velocidadDeDesvanecimiento;
    private Coroutine desvanecerCoroutine;
    public void AparecerAlerta()
    {
        imageMarcoRojoo.enabled = true;
        desvanecerCoroutine = StartCoroutine(DesvanecerMarcoRojoo());

    }

    IEnumerator DesvanecerMarcoRojoo()
    {
        float alpha = 1f;
        while(alpha > 0.0f)
        {
        alpha -= Time.deltaTime / velocidadDeDesvanecimiento;
        imageMarcoRojoo.color = new Color(1.0f,1.0f,1.0f,alpha);
        yield return null;   
        }

    
    }

    
    
}
