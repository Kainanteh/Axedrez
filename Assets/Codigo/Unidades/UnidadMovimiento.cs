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

    public bool GenerarMovimiento(Unidad unidad, Celda cinicio, Celda cobjetivo, bool ataque)
    {
        
        


        // int movcolum = 999;
        // int movfila = 999;

        // movcolum = cobjetivo.columna - cinicio.columna;
        // movfila = cobjetivo.fila - cinicio.fila;

        // if(movcolum == 999 || movfila == 999){return false;}

        //Debug.Log("movimiento COLUMNA " + movcolum + " movimiento FILA " + movfila + " ");

        switch(unidad.GetTipoUnidad())
        {
            case TipoUnidad.Peon:
            {

           

                    if(DireccionUnidad(cobjetivo,cinicio,unidad,false ) && ataque == true)
                    {

                        
                    
                        cobjetivo.GetUnidadEnCelda().gameObject.SetActive(false);cobjetivo.SetUnidadEnCelda(null);  // La unidad objetiva se esconde 
                                                                                                                    // y se quita la ref en la celda objetivo

                                                                                                                    // Peon tiene movimiento 2:1-N pero no puedo atacar
                                                                                                                    // si hay objetivo tiene movmiento de ataque 1-NO-NE
                            
                        cobjetivo.SetUnidadEnCelda(cinicio.GetUnidadEnCelda()); // La celda donde muevo la unidad ahora tiene esa unidad
                        cinicio.SetUnidadEnCelda(null); // Esta celda ya no tiene unidad
                        movimientos.Add(new Movimiento(cinicio,cobjetivo));
                        NuevoMovimiento(unidad,Direccion.Norte,1);
                    
                        
                    }
                    if(DireccionUnidad(cobjetivo,cinicio,unidad,false ) && ataque == false)
                    {


                        cobjetivo.SetUnidadEnCelda(cinicio.GetUnidadEnCelda()); // La celda donde muevo la unidad ahora tiene esa unidad
                        cinicio.SetUnidadEnCelda(null); // Esta celda ya no tiene unidad
                        movimientos.Add(new Movimiento(cinicio,cobjetivo));
                        //Primer movimiento realizado, se pasa el movimento en la direccion Norte de 2 a 1
                        NuevoMovimiento(unidad,Direccion.Norte,1);
                    

                    }
                    else
                    {

                        cinicio.SetUnidadEnCelda(cinicio.GetUnidadEnCelda()); 

                    }
                
                break;
            }
            case TipoUnidad.Caballo:
            {

                    

                    if(DireccionDirectaUnidad(cobjetivo,cinicio,unidad,false ))
                    {

                        Debug.Log(ataque);
                        if(ataque == true)
                        {
                        cobjetivo.GetUnidadEnCelda().gameObject.SetActive(false);cobjetivo.SetUnidadEnCelda(null);  // La unidad objetiva se esconde 
                        }                                                                                           // y se quita la ref en la celda objetivo

                                                                                                               
                            
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
            default:
            {

                    if(DireccionUnidad(cobjetivo,cinicio,unidad,false ) && ataque == true)
                    {

                        
                    
                        cobjetivo.GetUnidadEnCelda().gameObject.SetActive(false);cobjetivo.SetUnidadEnCelda(null);  // La unidad objetiva se esconde 
                                                                                                                    // y se quita la ref en la celda objetivo

                                                                                                           
                            
                        cobjetivo.SetUnidadEnCelda(cinicio.GetUnidadEnCelda()); // La celda donde muevo la unidad ahora tiene esa unidad
                        cinicio.SetUnidadEnCelda(null); // Esta celda ya no tiene unidad
                        movimientos.Add(new Movimiento(cinicio,cobjetivo));
                       
                    
                        
                    }
                    if(DireccionUnidad(cobjetivo,cinicio,unidad,false ) && ataque == false)
                    {


                        cobjetivo.SetUnidadEnCelda(cinicio.GetUnidadEnCelda()); // La celda donde muevo la unidad ahora tiene esa unidad
                        cinicio.SetUnidadEnCelda(null); // Esta celda ya no tiene unidad
                        movimientos.Add(new Movimiento(cinicio,cobjetivo));
                        //Primer movimiento realizado, se pasa el movimento en la direccion Norte de 2 a 1
                                           

                    }
                    else
                    {

                        cinicio.SetUnidadEnCelda(cinicio.GetUnidadEnCelda()); 

                    }

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


            if(unidad.MovimientoDirecto == false)
            {



                for (int z = 0; z < (cuadricula.filas*cuadricula.columnas); z++) // Bloquear movimiento por otra unidad en la direccion
                {
                   
                    if(celdas[z].GetUnidadEnCelda()==cinicio.GetUnidadEnCelda()){continue; } // Si es la misma unidad no cuenta para el calculo de bloquear
                    else if(celdas[z].GetUnidadEnCelda()!=null)
                    {

                        // Al mirar la cuadricula de forma ascendente hay que asegurarse de que las direccion Sur,SurEste,SurOeste y Oeste recojan la celda
                        // mas cercano a la celda de la unidad con la que se hace el calculo

                        if(celdas[z].fila - cinicio.fila > 0 && celdas[z].columna - cinicio.columna == 0)       // Norte
                        {if(celdabloqNorte==null){celdabloqNorte = celdas[z];}}

                        if(celdas[z].fila - cinicio.fila > 0  && celdas[z].columna - cinicio.columna > 0        // NorEste
                        && (celdas[z].fila - cinicio.fila)==(celdas[z].columna - cinicio.columna)) 
                        {if(celdabloqNorEste==null){celdabloqNorEste = celdas[z];}}

                        if(celdas[z].fila - cinicio.fila == 0  && (celdas[z].columna - cinicio.columna) > 0)    // Este
                        {if(celdabloqEste==null){celdabloqEste = celdas[z];}}

                        if(celdas[z].fila - cinicio.fila < 0  && (celdas[z].columna - cinicio.columna) > 0      // SurEste
                        && (celdas[z].fila - cinicio.fila)*-1==(celdas[z].columna - cinicio.columna))
                        {if(celdabloqSurEste==null){celdabloqSurEste = celdas[z];}
                        else if(celdabloqSurEste.fila<celdas[z].fila 
                        && celdabloqSurEste.columna>celdas[z].columna){celdabloqSurEste = celdas[z];}}

                        if(celdas[z].fila - cinicio.fila < 0 && celdas[z].columna - cinicio.columna == 0)       // Sur
                        {if(celdabloqSur==null){celdabloqSur = celdas[z];}
                        else if(celdabloqSur.fila<celdas[z].fila 
                        && celdabloqSur.columna==celdas[z].columna){celdabloqSur = celdas[z];}}

                        if(celdas[z].fila - cinicio.fila < 0  && (celdas[z].columna - cinicio.columna) < 0      // SurOeste
                        && (celdas[z].fila - cinicio.fila)==(celdas[z].columna - cinicio.columna))
                        {if(celdabloqSurOeste==null){celdabloqSurOeste = celdas[z];}
                        else if(celdabloqSurOeste.fila<celdas[z].fila 
                        && celdabloqSurOeste.columna<celdas[z].columna){celdabloqSurOeste = celdas[z];}}

                        if(celdas[z].fila - cinicio.fila == 0 && celdas[z].columna - cinicio.columna < 0)       // Oeste
                        {if(celdabloqOeste==null){celdabloqOeste = celdas[z];}
                        else if(celdabloqOeste.fila==celdas[z].fila 
                        && celdabloqOeste.columna<celdas[z].columna){celdabloqOeste = celdas[z];}}

                        if(celdas[z].fila - cinicio.fila > 0  && (celdas[z].columna - cinicio.columna) < 0      // NorOeste
                        && (celdas[z].fila - cinicio.fila)==(celdas[z].columna - cinicio.columna)*-1)
                        {if(celdabloqNorOeste==null){celdabloqNorOeste = celdas[z];}}  
                        
                        
  
                    }  
                    
                   
                            
                }

            }

                for (int i = 0; i < (cuadricula.filas*cuadricula.columnas); i++) 
		        {

                    if(unidad.MovimientoDirecto == false)
                    {

                        if(celdabloqNorte!=null && celdabloqNorte.fila<celdas[i].fila && celdabloqNorte.columna==celdas[i].columna)         {continue;}
                        if(celdabloqNorEste!=null && celdabloqNorEste.fila<celdas[i].fila && celdabloqNorEste.columna<celdas[i].columna)    {continue;}
                        if(celdabloqEste!=null  && celdabloqEste.fila==celdas[i].fila && celdabloqEste.columna<celdas[i].columna)           {continue;} 
                        if(celdabloqSurEste!=null && celdabloqSurEste.fila>celdas[i].fila && celdabloqSurEste.columna<celdas[i].columna)    {continue;}
                        if(celdabloqSur!=null && celdabloqSur.fila>celdas[i].fila && celdabloqSur.columna==celdas[i].columna)               {continue;}
                        if(celdabloqSurOeste!=null && celdabloqSurOeste.fila>celdas[i].fila && celdabloqSurOeste.columna>celdas[i].columna) {continue;}
                        if(celdabloqOeste!=null && celdabloqOeste.fila==celdas[i].fila && celdabloqOeste.columna>celdas[i].columna)         {continue;}
                        if(celdabloqNorOeste!=null && celdabloqNorOeste.fila<celdas[i].fila && celdabloqNorOeste.columna>celdas[i].columna) {continue;}
                        
                        DireccionUnidad(celdas[i],cinicio,unidad ,true);
                    
                    }
                    else
                    {

                        DireccionDirectaUnidad(celdas[i],cinicio,unidad ,true);

                    }

                }

    }


    bool DireccionDirectaUnidad(Celda celda, Celda celdainicio, Unidad unidad, bool calculo)
    {

        if(calculo==false)
        {

            
        }

        Celda[] celdas = cuadricula.GetCeldas();


        for (int i = 0; i < unidad.limiteDirDirec.Count; i++)
        {

            //N +
            //S -
            //E +
            //O -
            
            int tFila   = -1;
            int tColum  = -1;

            switch(unidad.limiteDirDirec[i].direccionUnidadFila)
            {

                case Direccion.Norte:   {tFila = celdainicio.fila + unidad.limiteDirDirec[i].limiteAtaqueFila;break;}
                case Direccion.Sur:     {tFila = celdainicio.fila - unidad.limiteDirDirec[i].limiteAtaqueFila;break;}
                case Direccion.Este:    {tFila = celdainicio.fila + unidad.limiteDirDirec[i].limiteAtaqueFila;break;}
                case Direccion.Oeste:   {tFila = celdainicio.fila - unidad.limiteDirDirec[i].limiteAtaqueFila;break;}
              
            }
            switch(unidad.limiteDirDirec[i].direccionUnidadColumna)
            {

                case Direccion.Norte:   {tColum = celdainicio.columna + unidad.limiteDirDirec[i].limiteAtaqueColumna;break;}
                case Direccion.Sur:     {tColum = celdainicio.columna - unidad.limiteDirDirec[i].limiteAtaqueColumna;break;}
                case Direccion.Este:    {tColum = celdainicio.columna + unidad.limiteDirDirec[i].limiteAtaqueColumna;break;}
                case Direccion.Oeste:   {tColum = celdainicio.columna - unidad.limiteDirDirec[i].limiteAtaqueColumna;break;}
              
            }

            if(tFila == celda.fila && tColum == celda.columna)                
            {if(calculo == true){if(celda.GetUnidadEnCelda()==null){celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;}
            else{celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[2].enabled = true;}}
            return true;}

        }


            // if(celda.fila - celdainicio.fila < 0  && (celda.columna - celdainicio.columna) == 0  
            //     && unidad.limiteDirMov[i].direccionUnidad == Direccion.Sur                                  // Sur
            //     && (celdainicio.fila-celda.fila)<=unidad.limiteDirMov[i].limiteMovimiento )        
            //         {if(calculo == true){celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;}return true;}
            // if(celda.fila - celdainicio.fila == 0  && (celda.columna - celdainicio.columna) > 0 
            //     && unidad.limiteDirMov[i].direccionUnidad == Direccion.Este                                 // Este
            //     && (celda.columna-celdainicio.columna)<=unidad.limiteDirMov[i].limiteMovimiento)       
            //         {if(calculo == true){celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;}return true;}
            // if(celda.fila - celdainicio.fila == 0  && (celda.columna - celdainicio.columna) < 0 
            //     && unidad.limiteDirMov[i].direccionUnidad == Direccion.Oeste                                // Oeste
            //     && (celdainicio.columna-celda.columna)<=unidad.limiteDirMov[i].limiteMovimiento)       
            //         {if(calculo == true){celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;}return true;}
         
            
    

        return false;

    }


    
    bool DireccionUnidad(Celda celda, Celda celdainicio, Unidad unidad, bool calculo)
    {


        for (int i = 0; i < unidad.limiteDirMov.Count; i++)
        {

            /*

            ██╗     ██╗███╗   ███╗██╗████████╗███████╗    ██████╗ ███████╗    ██████╗ ██╗      ██████╗  ██████╗ ██╗   ██╗███████╗ ██████╗ 
            ██║     ██║████╗ ████║██║╚══██╔══╝██╔════╝    ██╔══██╗██╔════╝    ██╔══██╗██║     ██╔═══██╗██╔═══██╗██║   ██║██╔════╝██╔═══██╗
            ██║     ██║██╔████╔██║██║   ██║   █████╗      ██║  ██║█████╗      ██████╔╝██║     ██║   ██║██║   ██║██║   ██║█████╗  ██║   ██║
            ██║     ██║██║╚██╔╝██║██║   ██║   ██╔══╝      ██║  ██║██╔══╝      ██╔══██╗██║     ██║   ██║██║▄▄ ██║██║   ██║██╔══╝  ██║   ██║
            ███████╗██║██║ ╚═╝ ██║██║   ██║   ███████╗    ██████╔╝███████╗    ██████╔╝███████╗╚██████╔╝╚██████╔╝╚██████╔╝███████╗╚██████╔╝
            ╚══════╝╚═╝╚═╝     ╚═╝╚═╝   ╚═╝   ╚══════╝    ╚═════╝ ╚══════╝    ╚═════╝ ╚══════╝ ╚═════╝  ╚══▀▀═╝  ╚═════╝ ╚══════╝ ╚═════╝ 
                  
            */

            if(calculo==false)
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

                    if(celdas[z].GetUnidadEnCelda()==celdainicio.GetUnidadEnCelda()){continue; } // Si es la misma unidad no cuenta para el calculo de bloquear
                    else if(celdas[z].GetUnidadEnCelda()!=null)
                    {

                        if(celdas[z].fila - celdainicio.fila > 0 && celdas[z].columna - celdainicio.columna == 0)       // Norte
                        {if(celdabloqNorte==null){celdabloqNorte = celdas[z];}}
                        if(celdas[z].fila - celdainicio.fila > 0  && celdas[z].columna - celdainicio.columna > 0        // NorEste
                        && (celdas[z].fila - celdainicio.fila)==(celdas[z].columna - celdainicio.columna)) 
                        {if(celdabloqNorEste==null){celdabloqNorEste = celdas[z];}}
                        if(celdas[z].fila - celdainicio.fila == 0  && (celdas[z].columna - celdainicio.columna) > 0)    // Este
                        {if(celdabloqEste==null){celdabloqEste = celdas[z];}} 
                        if(celdas[z].fila - celdainicio.fila < 0  && (celdas[z].columna - celdainicio.columna) > 0      // SurEste
                        && (celdas[z].fila - celdainicio.fila)*-1==(celdas[z].columna - celdainicio.columna))
                        {if(celdabloqSurEste==null){celdabloqSurEste = celdas[z];}
                        else if(celdabloqSurEste.fila<celdas[z].fila && celdabloqSurEste.columna>celdas[z].columna){celdabloqSurEste = celdas[z];}}  
                        if(celdas[z].fila - celdainicio.fila < 0 && celdas[z].columna - celdainicio.columna == 0)       // Sur
                        {if(celdabloqSur==null){celdabloqSur = celdas[z];}
                        else if(celdabloqSur.fila<celdas[z].fila && celdabloqSur.columna==celdas[z].columna){celdabloqSur = celdas[z];}}
                        if(celdas[z].fila - celdainicio.fila < 0  && (celdas[z].columna - celdainicio.columna) < 0      // SurOeste
                        && (celdas[z].fila - celdainicio.fila)==(celdas[z].columna - celdainicio.columna))
                        {if(celdabloqSurOeste==null){celdabloqSurOeste = celdas[z];}
                        else if(celdabloqSurOeste.fila<celdas[z].fila && celdabloqSurOeste.columna<celdas[z].columna){celdabloqSurOeste = celdas[z];}}    
                        if(celdas[z].fila - celdainicio.fila == 0 && celdas[z].columna - celdainicio.columna < 0)       // Oeste
                        {if(celdabloqOeste==null){celdabloqOeste = celdas[z];}
                        else if(celdabloqOeste.fila==celdas[z].fila && celdabloqOeste.columna<celdas[z].columna){celdabloqOeste = celdas[z];}}
                        if(celdas[z].fila - celdainicio.fila > 0  && (celdas[z].columna - celdainicio.columna) < 0      // NorOeste
                        && (celdas[z].fila - celdainicio.fila)==(celdas[z].columna - celdainicio.columna)*-1)
                        {if(celdabloqNorOeste==null){celdabloqNorOeste = celdas[z];}}  
                          
                    }  

                }
               
                    // Si hay una unidad bloqueando la direccion devuelve falso

                    if(celdabloqNorte!=null && unidad.limiteDirMov[i].direccionUnidad == Direccion.Norte && celdabloqNorte.fila<celda.fila && celdabloqNorte.columna==celda.columna){return false;}
                    else if(celdabloqNorEste!=null && unidad.limiteDirMov[i].direccionUnidad == Direccion.NorEste && celdabloqNorEste.fila<celda.fila && celdabloqNorEste.columna<celda.columna){return false;}
                    else if(celdabloqEste!=null  && unidad.limiteDirMov[i].direccionUnidad == Direccion.Este && celdabloqEste.fila==celda.fila && celdabloqEste.columna<celda.columna){return false;} 
                    else if(celdabloqSurEste!=null && unidad.limiteDirMov[i].direccionUnidad == Direccion.SurEste && celdabloqSurEste.fila>celda.fila && celdabloqSurEste.columna<celda.columna){return false;}
                    else if(celdabloqSur!=null && unidad.limiteDirMov[i].direccionUnidad == Direccion.Sur && celdabloqSur.fila>celda.fila && celdabloqSur.columna==celda.columna){return false;}
                    else if(celdabloqSurOeste!=null && unidad.limiteDirMov[i].direccionUnidad == Direccion.SurOeste && celdabloqSurOeste.fila>celda.fila && celdabloqSurOeste.columna>celda.columna){return false;}
                    else if(celdabloqOeste!=null && unidad.limiteDirMov[i].direccionUnidad == Direccion.Oeste && celdabloqOeste.fila==celda.fila && celdabloqOeste.columna>celda.columna){return false;}
                    else if(celdabloqNorOeste!=null && unidad.limiteDirMov[i].direccionUnidad == Direccion.NorOeste && celdabloqNorOeste.fila<celda.fila && celdabloqNorOeste.columna>celda.columna){return false;}
                    

            }

            // if(celda.fila == 2 && celda.columna==4)
            // {
            //      Debug.Log("-------------------------");
            //     Debug.Log("Celda inic : " + celdainicio.fila + " " + celdainicio.columna);Debug.Log("Celda obj : " + celda.fila + " " + celda.columna);
            //     Debug.Log( unidad.limiteDirAtaq[i].direccionUnidad );
            //     Debug.Log("-------------------------");
            // }

            /*
                        
            ██╗     ██╗███╗   ███╗██╗████████╗███████╗    ██████╗ ███████╗     █████╗ ████████╗ █████╗  ██████╗ ██╗   ██╗███████╗
            ██║     ██║████╗ ████║██║╚══██╔══╝██╔════╝    ██╔══██╗██╔════╝    ██╔══██╗╚══██╔══╝██╔══██╗██╔═══██╗██║   ██║██╔════╝
            ██║     ██║██╔████╔██║██║   ██║   █████╗      ██║  ██║█████╗      ███████║   ██║   ███████║██║   ██║██║   ██║█████╗  
            ██║     ██║██║╚██╔╝██║██║   ██║   ██╔══╝      ██║  ██║██╔══╝      ██╔══██║   ██║   ██╔══██║██║▄▄ ██║██║   ██║██╔══╝  
            ███████╗██║██║ ╚═╝ ██║██║   ██║   ███████╗    ██████╔╝███████╗    ██║  ██║   ██║   ██║  ██║╚██████╔╝╚██████╔╝███████╗
            ╚══════╝╚═╝╚═╝     ╚═╝╚═╝   ╚═╝   ╚══════╝    ╚═════╝ ╚══════╝    ╚═╝  ╚═╝   ╚═╝   ╚═╝  ╚═╝ ╚══▀▀═╝  ╚═════╝ ╚══════╝
                                                                                                                                
            */

            if(celda.fila - celdainicio.fila > 0  && (celda.columna - celdainicio.columna) == 0             // La celda objetivo es Norte respecto a la celda inicial
                && unidad.limiteDirAtaq[i].direccionUnidad == Direccion.Norte                               // Norte
                && (celda.fila-celdainicio.fila)<=unidad.limiteDirAtaq[i].limiteAtaque                      // Limite Movimiento   
                && celda.GetUnidadEnCelda()!=null)
                    {if(calculo == true){if(celda.GetUnidadEnCelda()==null){celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;}
                                        else{celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[2].enabled = true;}}return true;}
            if(celda.fila - celdainicio.fila < 0  && (celda.columna - celdainicio.columna) == 0  
                && unidad.limiteDirAtaq[i].direccionUnidad == Direccion.Sur                                  // Sur
                && (celdainicio.fila-celda.fila)<=unidad.limiteDirAtaq[i].limiteAtaque 
                && celda.GetUnidadEnCelda()!=null)
                    {if(calculo == true){if(celda.GetUnidadEnCelda()==null){celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;}
                                        else{celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[2].enabled = true;}}return true;}
            if(celda.fila - celdainicio.fila == 0  && (celda.columna - celdainicio.columna) > 0 
                && unidad.limiteDirAtaq[i].direccionUnidad == Direccion.Este                                 // Este
                && (celda.columna-celdainicio.columna)<=unidad.limiteDirAtaq[i].limiteAtaque
                && celda.GetUnidadEnCelda()!=null)
                    {if(calculo == true){if(celda.GetUnidadEnCelda()==null){celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;}
                                        else{celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[2].enabled = true;}}return true;}
            if(celda.fila - celdainicio.fila == 0  && (celda.columna - celdainicio.columna) < 0 
                && unidad.limiteDirAtaq[i].direccionUnidad == Direccion.Oeste                                // Oeste
                && (celdainicio.columna-celda.columna)<=unidad.limiteDirAtaq[i].limiteAtaque
                && celda.GetUnidadEnCelda()!=null)
                    {if(calculo == true){if(celda.GetUnidadEnCelda()==null){celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;}
                                        else{celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[2].enabled = true;}}return true;}
            if(celda.fila - celdainicio.fila > 0  && (celda.columna - celdainicio.columna) > 0 
                && (celda.fila - celdainicio.fila)==(celda.columna - celdainicio.columna) 
                && unidad.limiteDirAtaq[i].direccionUnidad == Direccion.NorEste                              // NorEste
                && (celda.fila-celdainicio.fila)<=unidad.limiteDirAtaq[i].limiteAtaque
                && (celda.columna-celdainicio.columna)<=unidad.limiteDirAtaq[i].limiteAtaque
                && celda.GetUnidadEnCelda()!=null)
                    {if(calculo == true){if(celda.GetUnidadEnCelda()==null){celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;}
                                        else{celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[2].enabled = true;}}return true;}
            if(celda.fila - celdainicio.fila > 0  && (celda.columna - celdainicio.columna) < 0 
                && (celda.fila - celdainicio.fila)==(celda.columna - celdainicio.columna)*-1  
                && unidad.limiteDirAtaq[i].direccionUnidad == Direccion.NorOeste                             // NorOeste
                && (celdainicio.fila-celda.fila)<=unidad.limiteDirAtaq[i].limiteAtaque
                && (celdainicio.columna-celda.columna)<=unidad.limiteDirAtaq[i].limiteAtaque
                && celda.GetUnidadEnCelda()!=null)
                    {if(calculo == true){if(celda.GetUnidadEnCelda()==null){celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;}
                                        else{celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[2].enabled = true;}}return true;}
            if(celda.fila - celdainicio.fila < 0  
                && (celda.columna - celdainicio.columna) < 0 
                && (celda.fila - celdainicio.fila)==(celda.columna - celdainicio.columna) 
                && unidad.limiteDirAtaq[i].direccionUnidad == Direccion.SurOeste                             // SurOeste
                && (celdainicio.fila-celda.fila)<=unidad.limiteDirAtaq[i].limiteAtaque
                && (celdainicio.columna-celda.columna)<=unidad.limiteDirAtaq[i].limiteAtaque
                && celda.GetUnidadEnCelda()!=null)
                    {if(calculo == true){if(celda.GetUnidadEnCelda()==null){celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;}
                                        else{celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[2].enabled = true;}}return true;}
            if(celda.fila - celdainicio.fila < 0  
                && (celda.columna - celdainicio.columna) > 0 
                && (celda.fila - celdainicio.fila)*-1==(celda.columna - celdainicio.columna) 
                && unidad.limiteDirAtaq[i].direccionUnidad == Direccion.SurEste                              // SurEste
                && (celda.fila-celdainicio.fila)<=unidad.limiteDirAtaq[i].limiteAtaque
                && (celda.columna-celdainicio.columna)<=unidad.limiteDirAtaq[i].limiteAtaque
                && celda.GetUnidadEnCelda()!=null)
                    {if(calculo == true){if(celda.GetUnidadEnCelda()==null){celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;}
                                        else{celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[2].enabled = true;}}return true;}


                
       

            /*

            ██╗     ██╗███╗   ███╗██╗████████╗███████╗    ██████╗ ███████╗    ███╗   ███╗ ██████╗ ██╗   ██╗██╗███╗   ███╗██╗███████╗███╗   ██╗████████╗ ██████╗ 
            ██║     ██║████╗ ████║██║╚══██╔══╝██╔════╝    ██╔══██╗██╔════╝    ████╗ ████║██╔═══██╗██║   ██║██║████╗ ████║██║██╔════╝████╗  ██║╚══██╔══╝██╔═══██╗
            ██║     ██║██╔████╔██║██║   ██║   █████╗      ██║  ██║█████╗      ██╔████╔██║██║   ██║██║   ██║██║██╔████╔██║██║█████╗  ██╔██╗ ██║   ██║   ██║   ██║
            ██║     ██║██║╚██╔╝██║██║   ██║   ██╔══╝      ██║  ██║██╔══╝      ██║╚██╔╝██║██║   ██║╚██╗ ██╔╝██║██║╚██╔╝██║██║██╔══╝  ██║╚██╗██║   ██║   ██║   ██║
            ███████╗██║██║ ╚═╝ ██║██║   ██║   ███████╗    ██████╔╝███████╗    ██║ ╚═╝ ██║╚██████╔╝ ╚████╔╝ ██║██║ ╚═╝ ██║██║███████╗██║ ╚████║   ██║   ╚██████╔╝
            ╚══════╝╚═╝╚═╝     ╚═╝╚═╝   ╚═╝   ╚══════╝    ╚═════╝ ╚══════╝    ╚═╝     ╚═╝ ╚═════╝   ╚═══╝  ╚═╝╚═╝     ╚═╝╚═╝╚══════╝╚═╝  ╚═══╝   ╚═╝    ╚═════╝ 
                
            */

            if(unidad.MovimientoConAtaque==false && celda.GetUnidadEnCelda()!=null){continue;} // Las unidades pueden no tener ataque con movimiento

            if(celda.fila - celdainicio.fila > 0  && (celda.columna - celdainicio.columna) == 0             // La celda objetivo es Norte respecto a la celda inicial
                && unidad.limiteDirMov[i].direccionUnidad == Direccion.Norte                                // Norte
                && (celda.fila-celdainicio.fila)<=unidad.limiteDirMov[i].limiteMovimiento )                 // Limite Movimiento       
                    {if(calculo == true){if(celda.GetUnidadEnCelda()==null){celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;}
                                        else{celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[2].enabled = true;}}return true;}
            if(celda.fila - celdainicio.fila < 0  && (celda.columna - celdainicio.columna) == 0  
                && unidad.limiteDirMov[i].direccionUnidad == Direccion.Sur                                  // Sur
                && (celdainicio.fila-celda.fila)<=unidad.limiteDirMov[i].limiteMovimiento )        
                    {if(calculo == true){if(celda.GetUnidadEnCelda()==null){celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;}
                                        else{celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[2].enabled = true;}}return true;}
            if(celda.fila - celdainicio.fila == 0  && (celda.columna - celdainicio.columna) > 0 
                && unidad.limiteDirMov[i].direccionUnidad == Direccion.Este                                 // Este
                && (celda.columna-celdainicio.columna)<=unidad.limiteDirMov[i].limiteMovimiento)       
                    {if(calculo == true){if(celda.GetUnidadEnCelda()==null){celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;}
                                        else{celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[2].enabled = true;}}return true;}
            if(celda.fila - celdainicio.fila == 0  && (celda.columna - celdainicio.columna) < 0 
                && unidad.limiteDirMov[i].direccionUnidad == Direccion.Oeste                                // Oeste
                && (celdainicio.columna-celda.columna)<=unidad.limiteDirMov[i].limiteMovimiento)       
                    {if(calculo == true){if(celda.GetUnidadEnCelda()==null){celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;}
                                        else{celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[2].enabled = true;}}return true;}
            if(celda.fila - celdainicio.fila > 0  && (celda.columna - celdainicio.columna) < 0 
                && (celda.fila - celdainicio.fila)==(celda.columna - celdainicio.columna)*-1  
                && unidad.limiteDirMov[i].direccionUnidad == Direccion.NorOeste                             // NorOeste
                && (celdainicio.fila-celda.fila)<=unidad.limiteDirMov[i].limiteMovimiento
                && (celdainicio.columna-celda.columna)<=unidad.limiteDirMov[i].limiteMovimiento)    
                    {if(calculo == true){if(celda.GetUnidadEnCelda()==null){celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;}
                                        else{celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[2].enabled = true;}}return true;}
            if(celda.fila - celdainicio.fila > 0  && (celda.columna - celdainicio.columna) > 0 
                && (celda.fila - celdainicio.fila)==(celda.columna - celdainicio.columna) 
                && unidad.limiteDirMov[i].direccionUnidad == Direccion.NorEste                              // NorEste
                && (celda.fila-celdainicio.fila)<=unidad.limiteDirMov[i].limiteMovimiento
                && (celda.columna-celdainicio.columna)<=unidad.limiteDirMov[i].limiteMovimiento)            
                    {if(calculo == true){if(celda.GetUnidadEnCelda()==null){celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;}
                                        else{celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[2].enabled = true;}}return true;}
            if(celda.fila - celdainicio.fila < 0  
                && (celda.columna - celdainicio.columna) < 0 
                && (celda.fila - celdainicio.fila)==(celda.columna - celdainicio.columna) 
                && unidad.limiteDirMov[i].direccionUnidad == Direccion.SurOeste                             // SurOeste
                && (celdainicio.fila-celda.fila)<=unidad.limiteDirMov[i].limiteMovimiento
                && (celdainicio.columna-celda.columna)<=unidad.limiteDirMov[i].limiteMovimiento)    
                    {if(calculo == true){if(celda.GetUnidadEnCelda()==null){celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;}
                                        else{celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[2].enabled = true;}}return true;}
            if(celda.fila - celdainicio.fila < 0  
                && (celda.columna - celdainicio.columna) > 0 
                && (celda.fila - celdainicio.fila)*-1==(celda.columna - celdainicio.columna) 
                && unidad.limiteDirMov[i].direccionUnidad == Direccion.SurEste                              // SurEste
                && (celda.fila-celdainicio.fila)<=unidad.limiteDirMov[i].limiteMovimiento
                && (celda.columna-celdainicio.columna)<=unidad.limiteDirMov[i].limiteMovimiento)             
                    {if(calculo == true){if(celda.GetUnidadEnCelda()==null){celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;}
                                        else{celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[2].enabled = true;}}return true;}


        }

        return false;

    }

    public void ReiniciarCalculo()
    {

        Celda[] celdas = cuadricula.GetCeldas();

        for (int i = 0; i < (cuadricula.filas*cuadricula.columnas); i++) 
		{
            
            celdas[i].gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = false;
            celdas[i].gameObject.GetComponentsInChildren<SpriteRenderer>()[2].enabled = false;

        }


    }

    public void NuevoMovimiento(Unidad unidad,Direccion direccion,int nuevoMovimiento)
    {

        List<Unidad.limiteDireccionMovimiento> limDirMov = unidad.limiteDirMov;

        for (int i = 0; i < limDirMov.Count; i++)
        {
           
            switch(direccion)
            {

                case Direccion.Norte:
                {if(limDirMov[i].direccionUnidad == Direccion.Norte)
                {
                    limDirMov[i].limiteMovimiento = nuevoMovimiento;}break;
                }
                case Direccion.NorEste:
                {if(limDirMov[i].direccionUnidad == Direccion.NorEste)
                {
                    limDirMov[i].limiteMovimiento = nuevoMovimiento;}break;
                }
                case Direccion.Este:
                {if(limDirMov[i].direccionUnidad == Direccion.Este)
                {
                    limDirMov[i].limiteMovimiento = nuevoMovimiento;}break;
                }
                case Direccion.SurEste:
                {if(limDirMov[i].direccionUnidad == Direccion.SurEste)
                {
                    limDirMov[i].limiteMovimiento = nuevoMovimiento;}break;
                }
                case Direccion.Sur:
                {if(limDirMov[i].direccionUnidad == Direccion.Sur)
                {
                    limDirMov[i].limiteMovimiento = nuevoMovimiento;}break;
                }
                case Direccion.SurOeste:
                {if(limDirMov[i].direccionUnidad == Direccion.SurOeste)
                {
                    limDirMov[i].limiteMovimiento = nuevoMovimiento;}break;
                }
                case Direccion.Oeste:
                {if(limDirMov[i].direccionUnidad == Direccion.Oeste)
                {
                    limDirMov[i].limiteMovimiento = nuevoMovimiento;}break;
                }
                case Direccion.NorOeste:
                {if(limDirMov[i].direccionUnidad == Direccion.NorOeste)
                {
                    limDirMov[i].limiteMovimiento = nuevoMovimiento;}break;
                }
                

            }
        }

    }

}
