using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnidadMovimiento : MonoBehaviour
{
    
    Cuadricula cuadricula;

    

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

    public bool GenerarMovimiento(Unidad unidad, Celda cinicio, Celda cobjetivo)
    {
        
        movimientos.Add(new Movimiento(cinicio,cobjetivo));


        int movcolum = 999;
        int movfila = 999;

        movcolum = cobjetivo.columna - cinicio.columna;
        movfila = cobjetivo.fila - cinicio.fila;

        if(movcolum == 999 || movfila == 999){return false;}

        Debug.Log("movimiento COLUMNA " + movcolum + " movimiento FILA " + movfila + " ");

        switch(unidad.GetTipoUnidad())
        {
            case TipoUnidad.Peon:
            {

               
                if(DireccionUnidad(cobjetivo,cinicio,unidad.limiteDirMov ))
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
                   
                                    
                    
                if(DireccionUnidad(cobjetivo,cinicio,unidad.limiteDirMov ))
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

    public void CalculoMovimiento(Unidad unidad, Celda cinicio, int movfila, int movcolum)
    {

        Celda[] celdas = cuadricula.GetCeldas();

        for (int i = 0; i < (cuadricula.filas*cuadricula.columnas); i++) 
		{
			                                           
            DireccionUnidad(celdas[i],cinicio,unidad.limiteDirMov );
                 
        }

    }

    //bool DireccionUnidad(Celda celda, Celda celdainicio, List<Direccion> direcciones, int movfila, int movcolum)
    bool DireccionUnidad(Celda celda, Celda celdainicio, List<Unidad.limiteDireccionMovimiento> limDirMov)
    {

        //Debug.Log(celda.gameObject.name + " " + celdainicio.gameObject.name + " " + direcciones);

        for (int i = 0; i < limDirMov.Count; i++)
        {
            

            // LIMITES DE MOVIMIENTO
            if(celda.fila - celdainicio.fila > 0  && (celda.columna - celdainicio.columna) == 0 // La celda objetivo es Norte respecto a la celda inicial
                && limDirMov[i].direccionUnidad == Direccion.Norte                              // Norte
                && (celda.fila-celdainicio.fila)<=limDirMov[i].limiteMovimiento )               // Limite Movimiento       
                    {celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;return true;}
            if(celda.fila - celdainicio.fila < 0  && (celda.columna - celdainicio.columna) == 0  
                && limDirMov[i].direccionUnidad == Direccion.Sur                                // Sur
                && (celdainicio.fila-celda.fila)<=limDirMov[i].limiteMovimiento )        
                    {celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;return true;}
            if(celda.fila - celdainicio.fila == 0  && (celda.columna - celdainicio.columna) > 0 
                && limDirMov[i].direccionUnidad == Direccion.Este                               // Este
                && (celda.columna-celdainicio.columna)<=limDirMov[i].limiteMovimiento)       
                    {celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;return true;}
            if(celda.fila - celdainicio.fila == 0  && (celda.columna - celdainicio.columna) < 0 
                && limDirMov[i].direccionUnidad == Direccion.Oeste                              // Oeste
                && (celdainicio.columna-celda.columna)<=limDirMov[i].limiteMovimiento)       
                    {celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;return true;}
            if(celda.fila - celdainicio.fila > 0  && (celda.columna - celdainicio.columna) < 0 
                && (celda.fila - celdainicio.fila)==(celda.columna - celdainicio.columna)*-1  
                && limDirMov[i].direccionUnidad == Direccion.NorOeste                           // NorOeste
                && (celdainicio.fila-celda.fila)<=limDirMov[i].limiteMovimiento
                && (celdainicio.columna-celda.columna)<=limDirMov[i].limiteMovimiento)    
                    {celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;return true;}
            if(celda.fila - celdainicio.fila > 0  && (celda.columna - celdainicio.columna) > 0 
                && (celda.fila - celdainicio.fila)==(celda.columna - celdainicio.columna) 
                && limDirMov[i].direccionUnidad == Direccion.NorEste                            // NorEste
                && (celda.fila-celdainicio.fila)<=limDirMov[i].limiteMovimiento
                && (celda.columna-celdainicio.columna)<=limDirMov[i].limiteMovimiento)            
                    {celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;return true;}
            if(celda.fila - celdainicio.fila < 0  
                && (celda.columna - celdainicio.columna) < 0 
                && (celda.fila - celdainicio.fila)==(celda.columna - celdainicio.columna) 
                && limDirMov[i].direccionUnidad == Direccion.SurOeste                           // SurOeste
                && (celdainicio.fila-celda.fila)<=limDirMov[i].limiteMovimiento
                && (celdainicio.columna-celda.columna)<=limDirMov[i].limiteMovimiento)    
                    {celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;return true;}
            if(celda.fila - celdainicio.fila < 0  
                && (celda.columna - celdainicio.columna) > 0 
                && (celda.fila - celdainicio.fila)*-1==(celda.columna - celdainicio.columna) 
                && limDirMov[i].direccionUnidad == Direccion.SurEste                            // SurEste
                && (celda.fila-celdainicio.fila)<=limDirMov[i].limiteMovimiento
                && (celda.columna-celdainicio.columna)<=limDirMov[i].limiteMovimiento)             
                    {celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;return true;}

        }

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
