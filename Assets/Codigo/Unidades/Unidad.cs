using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum TipoUnidad {Peon,Torre,Caballo,Alfil,Reina,Rey};
public class Unidad : MonoBehaviour
{

    [SerializeField] private UnidadMovimiento unidadMovimiento = null;

    
    [SerializeField] LayerMask layerUnidad;
    
    [SerializeField] TipoUnidad tipoUnidad; 


    private void Start()
    {

       GameObject.Find("Celda 3 3").GetComponent<Celda>().SetUnidadEnCelda(this);

    }

    public Unidad GetUnidad()
    {

        return this;

    }

    public TipoUnidad GetTipoUnidad()
    {

        return tipoUnidad;

    }




}
