using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnidadMovimiento : MonoBehaviour
{
    
    Cuadricula cuadricula;

    public enum Direccion {Norte,NorEste,Este,SurEste,Sur,SurOeste,Oeste,NorOeste}

    [System.Serializable]public struct Movimiento
    {
        public Celda cinicio;
        public Celda cobjetivo;

        public Movimiento(Celda cinicio, Celda cobjetivo) : this()
        {
            this.cinicio = cinicio;
            this.cobjetivo = cobjetivo;
        }
    }

    [SerializeField] List<Movimiento> movimientos;

    void Start()
    {

        movimientos = new List<Movimiento>();
        cuadricula = GameObject.Find("Cuadricula").GetComponent<Cuadricula>();

        

    }

    public bool GenerarMovimiento(TipoUnidad tipo, Celda cinicio, Celda cobjetivo)
    {
        
        movimientos.Add(new Movimiento(cinicio,cobjetivo));


        int movcolum = 999;
        int movfila = 999;

        movcolum = cobjetivo.columna - cinicio.columna;
        movfila = cobjetivo.fila - cinicio.fila;

        if(movcolum == 999 || movfila == 999){return false;}

        Debug.Log("movimiento COLUMNA " + movcolum + " movimiento FILA " + movfila + " ");

        switch(tipo)
        {
            case TipoUnidad.Peon:
            {

                 List<Direccion> direccionPeon = new List<Direccion>();
                
                direccionPeon.Add(Direccion.Norte);

                if(DireccionUnidad(cobjetivo,cinicio,direccionPeon))
                {

                    //Debug.Log(" movfila " + movfila + " movcolum " + movcolum + " columna " + columna + " fila " + fila);
                    cobjetivo.SetUnidadEnCelda(cinicio.GetUnidadEnCelda()); // La celda donde muevo la unidad ahora tiene esa unidad
                    cinicio.SetUnidadEnCelda(null); // Esta celda ya no tiene unidad
                                       
                }
                else
                {

                    cinicio.SetUnidadEnCelda(cinicio.GetUnidadEnCelda()); 

                }

                break;
            }
            case TipoUnidad.Reina:
            {
                   
                    //if(movcolum!=0 && (movfila>0 || movfila<0) || (movcolum>0 || movcolum<0) && movfila!=0){// Limite para la torre
                              List<Direccion> direccionReina = new List<Direccion>();
                
                        direccionReina.Add(Direccion.Norte);
                        direccionReina.Add(Direccion.Este);
                        direccionReina.Add(Direccion.Sur);
                        direccionReina.Add(Direccion.Oeste);
                        direccionReina.Add(Direccion.NorEste);
                        direccionReina.Add(Direccion.NorOeste);
                        direccionReina.Add(Direccion.SurEste);
                        direccionReina.Add(Direccion.SurOeste);
                    
                    
                if(DireccionUnidad(cobjetivo,cinicio,direccionReina))
                {

                    //Debug.Log(" movfila " + movfila + " movcolum " + movcolum + " columna " + columna + " fila " + fila);
                    cobjetivo.SetUnidadEnCelda(cinicio.GetUnidadEnCelda()); // La celda donde muevo la unidad ahora tiene esa unidad
                    cinicio.SetUnidadEnCelda(null); // Esta celda ya no tiene unidad
                                       
                }
                else
                {

                    cinicio.SetUnidadEnCelda(cinicio.GetUnidadEnCelda()); 

                }


                
                break;    
            }
            default:
            {

                break;
            }

        }



        return true;

    }

    public void CalculoMovimiento(TipoUnidad tipo, Celda cinicio)
    {

        //Debug.Log(Cuadricula.filas + " " + Cuadricula.columnas);

        Celda[] celdas = cuadricula.GetCeldas();

        for (int i = 0; i < (cuadricula.filas*cuadricula.columnas); i++) 
		{
			

            switch(tipo)
            {
                case TipoUnidad.Peon:
                {

                    List<Direccion> direccionPeon = new List<Direccion>();
                
                        direccionPeon.Add(Direccion.Norte);
                                            
                    DireccionUnidad(celdas[i],cinicio,direccionPeon);
                   
                    break;
                }
                case TipoUnidad.Reina:
                {

                    List<Direccion> direccionReina = new List<Direccion>();
                
                        direccionReina.Add(Direccion.Norte);
                        direccionReina.Add(Direccion.Este);
                        direccionReina.Add(Direccion.Sur);
                        direccionReina.Add(Direccion.Oeste);
                        direccionReina.Add(Direccion.NorEste);
                        direccionReina.Add(Direccion.NorOeste);
                        direccionReina.Add(Direccion.SurEste);
                        direccionReina.Add(Direccion.SurOeste);
                    
                    DireccionUnidad(celdas[i],cinicio,direccionReina);
                   
                    break;
                }

            }
            
        //Debug.Log(celda.gameObject.name);
        }

    }

    bool DireccionUnidad(Celda celda, Celda celdainicio, List<Direccion> direcciones)
    {

        //Debug.Log(celda.gameObject.name + " " + celdainicio.gameObject.name + " " + direcciones);

        if(celda.fila - celdainicio.fila > 0  && (celda.columna - celdainicio.columna) == 0 && direcciones.Contains(Direccion.Norte))       // Norte
        {celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;return true;}
        if(celda.fila - celdainicio.fila < 0  && (celda.columna - celdainicio.columna) == 0  && direcciones.Contains(Direccion.Sur))   // Sur
        {celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;return true;}
        if(celda.fila - celdainicio.fila == 0  && (celda.columna - celdainicio.columna) > 0 && direcciones.Contains(Direccion.Este))   // Este
        {celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;return true;}
        if(celda.fila - celdainicio.fila == 0  && (celda.columna - celdainicio.columna) < 0 && direcciones.Contains(Direccion.Oeste))  // Oeste
        {celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;return true;}
        if(celda.fila - celdainicio.fila > 0  && (celda.columna - celdainicio.columna) < 0 
        && (celda.fila - celdainicio.fila)==(celda.columna - celdainicio.columna)*-1  && direcciones.Contains(Direccion.NorOeste))          // NorOeste
        {celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;return true;}
        if(celda.fila - celdainicio.fila > 0  && (celda.columna - celdainicio.columna) > 0 
        && (celda.fila - celdainicio.fila)==(celda.columna - celdainicio.columna) && direcciones.Contains(Direccion.NorEste))               // NorEste
        {celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;return true;}
        if(celda.fila - celdainicio.fila < 0  && (celda.columna - celdainicio.columna) < 0 
        && (celda.fila - celdainicio.fila)==(celda.columna - celdainicio.columna) && direcciones.Contains(Direccion.SurOeste))              // SurOeste
        {celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;return true;}
        if(celda.fila - celdainicio.fila < 0  && (celda.columna - celdainicio.columna) > 0 
        && (celda.fila - celdainicio.fila)*-1==(celda.columna - celdainicio.columna) && direcciones.Contains(Direccion.SurEste))            // SurEste
        {celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;return true;}

        return false;
    }

    public void ReiniciarCalculo()
    {

        Celda[] celdas = cuadricula.GetCeldas();

        for (int i = 0; i < (cuadricula.filas*cuadricula.columnas); i++) 
		{

            celdas[i].gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = false;

        }


    }

}
