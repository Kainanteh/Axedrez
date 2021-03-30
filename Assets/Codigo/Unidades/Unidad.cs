using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum TipoUnidad {Peon,Torre,Caballo,Alfil,Reina,Rey};
public enum Direccion {Norte,NorEste,Este,SurEste,Sur,SurOeste,Oeste,NorOeste}

public class Unidad : MonoBehaviour
{

    // [SerializeField] private UnidadMovimiento unidadMovimiento = null;

    
    // [SerializeField] LayerMask layerUnidad;
    
    [SerializeField] TipoUnidad tipoUnidad; 


    // public int movimientofila = 1;
    // public int movimientocolumna = 1;

    // public int movimientoNorte = 1;
    // public int movimientoNorEste = 1;
    // public int movimientoEste = 1;
    // public int movimientoSurEste = 1;
    // public int movimientoSur = 1;
    // public int movimientoSurOeste = 1;
    // public int movimientoOeste = 1;
    // public int movimientoNorOeste = 1;

    

    [System.Serializable] 
    public struct limiteDireccionMovimiento {
        public Direccion direccionUnidad;
        public int limiteMovimiento;
   
    }
    public List<limiteDireccionMovimiento> limiteDirMov;


    private void Start()
    {

       GameObject.Find("Celda 3 7").GetComponent<Celda>().SetUnidadEnCelda(this);
         //limiteMovimientosDireccion = new Dictionary<string, int>();
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
