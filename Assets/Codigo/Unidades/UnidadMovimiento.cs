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
        
        


        int movcolum = 999;
        int movfila = 999;

        movcolum = cobjetivo.columna - cinicio.columna;
        movfila = cobjetivo.fila - cinicio.fila;

        if(movcolum == 999 || movfila == 999){return false;}

        //Debug.Log("movimiento COLUMNA " + movcolum + " movimiento FILA " + movfila + " ");

        switch(unidad.GetTipoUnidad())
        {
            case TipoUnidad.Peon:
            {

               
                if(DireccionUnidad(cobjetivo,cinicio,unidad.limiteDirMov,false ))
                {

                    cobjetivo.SetUnidadEnCelda(cinicio.GetUnidadEnCelda()); // La celda donde muevo la unidad ahora tiene esa unidad
                    cinicio.SetUnidadEnCelda(null); // Esta celda ya no tiene unidad
                    movimientos.Add(new Movimiento(cinicio,cobjetivo));
                }
                else
                {

                    cinicio.SetUnidadEnCelda(cinicio.GetUnidadEnCelda()); 

                }

                break;
            }
            case TipoUnidad.Reina:
            {
                   
                                    
                    
                if(DireccionUnidad(cobjetivo,cinicio,unidad.limiteDirMov, false ))
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

    public void CalculoMovimiento(Unidad unidad, Celda cinicio)
    {

        Celda[] celdas = cuadricula.GetCeldas();

        Celda celdabloqNorte = null;
        Celda celdabloqNorEste = null;
        Celda celdabloqEste = null;
        Celda celdabloqSurEste = null;
        Celda celdabloqSur = null;
        Celda celdabloqSurOeste = null;
        Celda celdabloqOeste = null;
        Celda celdabloqNorOeste = null;

                for (int z = 0; z < (cuadricula.filas*cuadricula.columnas); z++) // Bloquear movimiento por otra unidad en la direccion
                {
                   
                    if(celdas[z].GetUnidadEnCelda()==cinicio.GetUnidadEnCelda()){continue; } // Si es la misma unidad no cuenta para el calculo de bloquear
                    else if(celdas[z].GetUnidadEnCelda()!=null)
                    {
                        
                        if(celdas[z].fila - cinicio.fila > 0 && celdas[z].columna - cinicio.columna == 0) // Norte
                        {if(celdabloqNorte==null){celdabloqNorte = celdas[z];}}
                        if(celdas[z].fila - cinicio.fila > 0  && celdas[z].columna - cinicio.columna > 0 // NorEste
                        && (celdas[z].fila - cinicio.fila)==(celdas[z].columna - cinicio.columna)) 
                        {if(celdabloqNorEste==null){celdabloqNorEste = celdas[z];}}
                        if(celdas[z].fila - cinicio.fila == 0  && (celdas[z].columna - cinicio.columna) > 0) // Este
                        {if(celdabloqEste==null){celdabloqEste = celdas[z];}} 
                        if(celdas[z].fila - cinicio.fila < 0  && (celdas[z].columna - cinicio.columna) > 0  // SurEste
                        && (celdas[z].fila - cinicio.fila)*-1==(celdas[z].columna - cinicio.columna))
                        {if(celdabloqSurEste==null){celdabloqSurEste = celdas[z];}}  
                        if(celdas[z].fila - cinicio.fila < 0 && celdas[z].columna - cinicio.columna == 0) // Sur
                        {if(celdabloqSur==null){celdabloqSur = celdas[z];}}
                        if(celdas[z].fila - cinicio.fila < 0  && (celdas[z].columna - cinicio.columna) < 0  // SurOeste
                        && (celdas[z].fila - cinicio.fila)==(celdas[z].columna - cinicio.columna))
                        {if(celdabloqSurOeste==null){celdabloqSurOeste = celdas[z];}}  
                        if(celdas[z].fila - cinicio.fila == 0 && celdas[z].columna - cinicio.columna < 0) // Oeste
                        {if(celdabloqOeste==null){celdabloqOeste = celdas[z];}}
                        if(celdas[z].fila - cinicio.fila > 0  && (celdas[z].columna - cinicio.columna) < 0  // NorOeste
                        && (celdas[z].fila - cinicio.fila)==(celdas[z].columna - cinicio.columna)*-1)
                        {if(celdabloqNorOeste==null){celdabloqNorOeste = celdas[z];}}  
                        

  
                    }  
                    
                   
                            
                }
                for (int i = 0; i < (cuadricula.filas*cuadricula.columnas); i++) 
		        {

                    if(celdabloqNorte!=null && celdabloqNorte.fila<celdas[i].fila && celdabloqNorte.columna==celdas[i].columna){Debug.Log("Bloqueo Norte");continue;}
                    if(celdabloqNorEste!=null && celdabloqNorEste.fila<celdas[i].fila && celdabloqNorEste.columna<celdas[i].columna){Debug.Log("Bloqueo NorEste");continue;}
                    if(celdabloqEste!=null  && celdabloqEste.fila==celdas[i].fila && celdabloqEste.columna<celdas[i].columna){Debug.Log("Bloqueo Este");continue;} 
                    if(celdabloqSurEste!=null && celdabloqSurEste.fila>celdas[i].fila && celdabloqSurEste.columna<celdas[i].columna){Debug.Log("Bloqueo SurEste");continue;}
                    if(celdabloqSur!=null && celdabloqSur.fila>celdas[i].fila && celdabloqSur.columna==celdas[i].columna){Debug.Log("Bloqueo Sur");continue;}
                    if(celdabloqSurOeste!=null && celdabloqSurOeste.fila>celdas[i].fila && celdabloqSurOeste.columna>celdas[i].columna){Debug.Log("Bloqueo SurOeste");continue;}
                    if(celdabloqOeste!=null && celdabloqOeste.fila==celdas[i].fila && celdabloqOeste.columna>celdas[i].columna){Debug.Log("Bloqueo Oeste");continue;}
                    if(celdabloqNorOeste!=null && celdabloqNorOeste.fila<celdas[i].fila && celdabloqNorOeste.columna>celdas[i].columna){Debug.Log("Bloqueo NorOeste");continue;}
                    DireccionUnidad(celdas[i],cinicio,unidad.limiteDirMov ,true);

                }

        // for (int i = 0; i < (cuadricula.filas*cuadricula.columnas); i++) 
		// {

        //     if(celdas[i].fila == cinicio.fila && celdas[i].columna == cinicio.columna){continue; } // La misma casilla no cuenta para el calculo
    
        //     if(celdabloqNorte != null 
        //         && celdas[i].fila - cinicio.fila > 0  
        //         && celdas[i].columna - cinicio.columna == 0 
        //         && celdabloqNorte.fila<celdas[i].fila){continue;}

        //     if(celdabloqNorEste != null 
        //         && celda.fila - celdainicio.fila > 0  && (celda.columna - celdainicio.columna) > 0 
        //         && (celda.fila - celdainicio.fila)==(celda.columna - celdainicio.columna) 
                                        
        //         && (celda.fila-celdainicio.fila)<=limDirMov[i].limiteMovimiento
        //         && (celda.columna-celdainicio.columna)<=limDirMov[i].limiteMovimiento)     

        //     DireccionUnidad(celdas[i],cinicio,unidad.limiteDirMov ,true);
                 
        // }

    }

    //bool DireccionUnidad(Celda celda, Celda celdainicio, List<Direccion> direcciones, int movfila, int movcolum)
    bool DireccionUnidad(Celda celda, Celda celdainicio, List<Unidad.limiteDireccionMovimiento> limDirMov, bool calculo)
    {
        // PARA EL TEMA DE LAS UNIDADES BLOQUEANDO EL MOVIMIENTO
        // Si hay unidad en la direccion volver a llamar DireccionUnidad como celda objetivo la celda de la unidad y break

        //Debug.Log(celda.gameObject.name + " " + celdainicio.gameObject.name + " " + direcciones);

        Celda[] celdas_unidadbloq = cuadricula.GetCeldas();

  


        for (int i = 0; i < limDirMov.Count; i++)
        {

            // if(calculo==false)
            // {
            //     for (int z = 0; z < (cuadricula.filas*cuadricula.columnas); z++) // Bloquear movimiento por otra unidad en la direccion
            //     {
                   
            //         if(celdas_unidadbloq[z].GetUnidadEnCelda()==celdainicio.GetUnidadEnCelda()){continue; } // Si es la misma unidad no cuenta para el calculo de bloquear
            //         else if(celdas_unidadbloq[z].GetUnidadEnCelda()!=null)
            //         {
                    
            //             if(limDirMov[i].direccionUnidad == Direccion.Norte && celdas_unidadbloq[z].fila - celdainicio.fila > 0  && celdas_unidadbloq[z].columna - celdainicio.columna == 0 )
            //             {//celda = celdas_unidadbloq[z];
            //              Debug.Log(celdas_unidadbloq[z].gameObject.name);break;}

            //         }
            //         //DireccionUnidad(celdas[z],celda,limDirMov ,true);
                            
            //     }
            // }

            // LIMITES DE MOVIMIENTO
            if(celda.fila - celdainicio.fila > 0  && (celda.columna - celdainicio.columna) == 0 // La celda objetivo es Norte respecto a la celda inicial
                && limDirMov[i].direccionUnidad == Direccion.Norte                              // Norte
                && (celda.fila-celdainicio.fila)<=limDirMov[i].limiteMovimiento )               // Limite Movimiento       
                    {if(calculo == true){celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;}return true;}
            if(celda.fila - celdainicio.fila < 0  && (celda.columna - celdainicio.columna) == 0  
                && limDirMov[i].direccionUnidad == Direccion.Sur                                // Sur
                && (celdainicio.fila-celda.fila)<=limDirMov[i].limiteMovimiento )        
                    {if(calculo == true){celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;}return true;}
            if(celda.fila - celdainicio.fila == 0  && (celda.columna - celdainicio.columna) > 0 
                && limDirMov[i].direccionUnidad == Direccion.Este                               // Este
                && (celda.columna-celdainicio.columna)<=limDirMov[i].limiteMovimiento)       
                    {if(calculo == true){celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;}return true;}
            if(celda.fila - celdainicio.fila == 0  && (celda.columna - celdainicio.columna) < 0 
                && limDirMov[i].direccionUnidad == Direccion.Oeste                              // Oeste
                && (celdainicio.columna-celda.columna)<=limDirMov[i].limiteMovimiento)       
                    {if(calculo == true){celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;}return true;}
            if(celda.fila - celdainicio.fila > 0  && (celda.columna - celdainicio.columna) < 0 
                && (celda.fila - celdainicio.fila)==(celda.columna - celdainicio.columna)*-1  
                && limDirMov[i].direccionUnidad == Direccion.NorOeste                           // NorOeste
                && (celdainicio.fila-celda.fila)<=limDirMov[i].limiteMovimiento
                && (celdainicio.columna-celda.columna)<=limDirMov[i].limiteMovimiento)    
                    {if(calculo == true){celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;}return true;}
            if(celda.fila - celdainicio.fila > 0  && (celda.columna - celdainicio.columna) > 0 
                && (celda.fila - celdainicio.fila)==(celda.columna - celdainicio.columna) 
                && limDirMov[i].direccionUnidad == Direccion.NorEste                            // NorEste
                && (celda.fila-celdainicio.fila)<=limDirMov[i].limiteMovimiento
                && (celda.columna-celdainicio.columna)<=limDirMov[i].limiteMovimiento)            
                    {if(calculo == true){celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;}return true;}
            if(celda.fila - celdainicio.fila < 0  
                && (celda.columna - celdainicio.columna) < 0 
                && (celda.fila - celdainicio.fila)==(celda.columna - celdainicio.columna) 
                && limDirMov[i].direccionUnidad == Direccion.SurOeste                           // SurOeste
                && (celdainicio.fila-celda.fila)<=limDirMov[i].limiteMovimiento
                && (celdainicio.columna-celda.columna)<=limDirMov[i].limiteMovimiento)    
                    {if(calculo == true){celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;}return true;}
            if(celda.fila - celdainicio.fila < 0  
                && (celda.columna - celdainicio.columna) > 0 
                && (celda.fila - celdainicio.fila)*-1==(celda.columna - celdainicio.columna) 
                && limDirMov[i].direccionUnidad == Direccion.SurEste                            // SurEste
                && (celda.fila-celdainicio.fila)<=limDirMov[i].limiteMovimiento
                && (celda.columna-celdainicio.columna)<=limDirMov[i].limiteMovimiento)             
                    {if(calculo == true){celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;}return true;}


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
