using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum TipoUnidad {Peon,Torre,Caballo,Alfil,Reina,Rey};
public enum Direccion {Norte,NorEste,Este,SurEste,Sur,SurOeste,Oeste,NorOeste}


public class Unidad : MonoBehaviour
{

    public string celdaIniciost;


    
    [SerializeField] TipoUnidad tipoUnidad; 

    

    [System.Serializable] 
    public class limiteDireccionMovimiento {
        public Direccion direccionUnidad;
        public int limiteMovimiento;

        public limiteDireccionMovimiento(Direccion direccionUnidad, int limiteMovimiento)
        {
            this.direccionUnidad = direccionUnidad;
            this.limiteMovimiento = limiteMovimiento;
        }


    }
    public List<limiteDireccionMovimiento> limiteDirMov;


    private void Start()
    {
        //"Celda 3 7"
       GameObject.Find(celdaIniciost).GetComponent<Celda>().SetUnidadEnCelda(this);
    
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
