using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum TipoUnidad {Null,Peon,Torre,Caballo,Alfil,Reina,Rey};
public enum Direccion {Norte,NorEste,Este,SurEste,Sur,SurOeste,Oeste,NorOeste}


public class Unidad : MonoBehaviour
{

    //public string celdaIniciost;

    public Jugador UnidadJugador;
    
    [SerializeField] TipoUnidad tipoUnidad; 


    public bool MovimientoConAtaque = true;
    public bool MovimientoDirecto = false;

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

    [System.Serializable] 
    public class limiteDireccionAtaque {
        public Direccion direccionUnidad;
        public int limiteAtaque;

        public limiteDireccionAtaque(Direccion direccionUnidad, int limiteAtaque)
        {
            this.direccionUnidad = direccionUnidad;
            this.limiteAtaque = limiteAtaque;
        }


    }

    [System.Serializable] 
    public class limiteDireccionDirecta {
        public Direccion direccionUnidadFila;
        public int limiteAtaqueFila;
        public Direccion direccionUnidadColumna;
        public int limiteAtaqueColumna;

    }

    public List<limiteDireccionMovimiento> limiteDirMov;
    public List<limiteDireccionAtaque> limiteDirAtaq;
    public List<limiteDireccionDirecta> limiteDirDirec;



    private void Start()
    {
        //"Celda 3 7"
           
    }

    public Unidad GetUnidad()
    {

        return this;

    }

    public TipoUnidad GetTipoUnidad()
    {

        return tipoUnidad;

    }

    public void SetCelda(string celdaIniciost)
    {

        GameObject.Find(celdaIniciost).GetComponent<Celda>().SetUnidadEnCelda(this);

    }




}
