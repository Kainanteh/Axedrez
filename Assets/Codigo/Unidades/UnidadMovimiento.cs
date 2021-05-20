using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnidadMovimiento : MonoBehaviour
{

    Cuadricula cuadricula;

    Turno Turnos;

    [System.Serializable]public struct Movimiento
    {
        public Celda cinicio;
        public Celda cobjetivo;

        public Unidad uinicio;

        public Unidad uobjetivo;

        public Movimiento(Celda cinicio, Celda cobjetivo, Unidad uinicio, Unidad uobjetivo)
        {
            this.cinicio = cinicio;
            this.cobjetivo = cobjetivo;
            this.uinicio = uinicio;
            this.uobjetivo = uobjetivo;
        }

    }

    public List<Movimiento> movimientos;

    AudioSource audioSource;
    AudioSistema audioSistema;

    public bool enPromocion = false;
    public Celda cEnPromocion = null;

   

    void Start()
    {

        movimientos = new List<Movimiento>();
        cuadricula = GameObject.Find("Cuadricula").GetComponent<Cuadricula>();

        Turnos = cuadricula.GetComponent<Turno>();

        audioSource = GameObject.Find("AudioSource").GetComponent<AudioSource>();
        audioSistema = GameObject.Find("AudioSource").GetComponent<AudioSistema>();

        JaqueCalculo(); //Si se empieza con un jaque

    }

    /*

    ██╗   ██╗███╗   ██╗██╗██████╗  █████╗ ██████╗ ███████╗███████╗
    ██║   ██║████╗  ██║██║██╔══██╗██╔══██╗██╔══██╗██╔════╝██╔════╝
    ██║   ██║██╔██╗ ██║██║██║  ██║███████║██║  ██║█████╗  ███████╗
    ██║   ██║██║╚██╗██║██║██║  ██║██╔══██║██║  ██║██╔══╝  ╚════██║
    ╚██████╔╝██║ ╚████║██║██████╔╝██║  ██║██████╔╝███████╗███████║
     ╚═════╝ ╚═╝  ╚═══╝╚═╝╚═════╝ ╚═╝  ╚═╝╚═════╝ ╚══════╝╚══════╝
         
    */

    public bool GenerarMovimiento(Unidad unidad, Celda cinicio, Celda cobjetivo, bool ataque)
    {

        if(enPromocion == true){return false;}

        // Una unidad no puede moverse u atacar poniendo en jaque a su rey
        if(unidad.GetTipoUnidad()!=TipoUnidad.Rey && JaqueCalculo(Turnos.GetJugadorActual().reyCelda,false,false,cinicio,cobjetivo) == true)
        {

            cinicio.SetUnidadEnCelda(cinicio.GetUnidadEnCelda());return false;
               
        }

        // Hay que impedir el movimento cuando hay un jaque, excepto...
        if(Turnos.GetJugadorActual().GetComponent<MateJaque>().Jaque == true)
        {

            // El jaque se puede resolver bloqueando a la unidad que esta amenazando al rey 
            if(JaqueBloqueoCalculo(Turnos.GetJugadorActual().GetComponent<MateJaque>().jaqueMovimiento.celdaJaque,
            Turnos.GetJugadorActual().reyCelda,cinicio,cobjetivo) ||

            // El jaque se puede resolver atacando a la unidad que esta amenazando al rey 
            DireccionUnidad(Turnos.GetJugadorActual().GetComponent<MateJaque>().jaqueMovimiento.celdaJaque,cinicio,unidad,false,false,false,false )
            && cobjetivo == Turnos.GetJugadorActual().GetComponent<MateJaque>().jaqueMovimiento.celdaJaque ||
            DireccionDirectaUnidad(Turnos.GetJugadorActual().GetComponent<MateJaque>().jaqueMovimiento.celdaJaque,cinicio,unidad,false,false,false)
            && cobjetivo == Turnos.GetJugadorActual().GetComponent<MateJaque>().jaqueMovimiento.celdaJaque ||

            // El jaque puede resolverlo el rey
            unidad.GetTipoUnidad()==TipoUnidad.Rey) 
            {}else{cinicio.SetUnidadEnCelda(cinicio.GetUnidadEnCelda());return false;} 

        }
     
     
        
        switch(unidad.GetTipoUnidad())
        {

            case TipoUnidad.Peon:
            {

                    if(DireccionUnidad(cobjetivo,cinicio,unidad,false,false,false,false ) && ataque == true)
                    {

                        movimientos.Add(new Movimiento(cinicio,cobjetivo,cinicio.GetUnidadEnCelda(),cobjetivo.GetUnidadEnCelda()));
                    
                        // La unidad objetiva se esconde y se quita la ref en la celda objetivo
                        cobjetivo.GetUnidadEnCelda().gameObject.SetActive(false);
                        //cobjetivo.SetUnidadEnCelda(null);  

                        // Peon tiene movimiento 2:1-N pero no puedo atacar
                        // si hay objetivo tiene movmiento de ataque 1-NO-NE

                        cobjetivo.SetUnidadEnCelda(cinicio.GetUnidadEnCelda()); // La celda donde muevo la unidad ahora tiene esa unidad
                        cinicio.SetUnidadEnCelda(null);                         // Esta celda ya no tiene unidad
                        
                        Turnos.CambiarTurno();
                        JaqueCalculo();

                        if(unidad.UnidadJugador.idJugador == 2)// !!! EL CAMBIO DEL MOVIMIENTO DEBERIA SER solo UNA VEZ
                        {

                        NuevoMovimiento(unidad,Direccion.Sur,1);

                        }
                        else
                        {

                        NuevoMovimiento(unidad,Direccion.Norte,1);

                        }

                        //Si la celda es el extremo de ese jugador, entonces el peon puede promocionar en caballo, alfil, torre o reina
                        if(cobjetivo.celdaPromocion == true)
                        {

                            unidad.gameObject.GetComponent<UnidadMovimientoEspecial>().Promocion(cobjetivo);

                        }
                        
                    }
                    else if(DireccionUnidad(cobjetivo,cinicio,unidad,false,false,false,false ) && ataque == false)
                    {

                        movimientos.Add(new Movimiento(cinicio,cobjetivo,cinicio.GetUnidadEnCelda(),cobjetivo.GetUnidadEnCelda()));

                        cobjetivo.SetUnidadEnCelda(cinicio.GetUnidadEnCelda()); // La celda donde muevo la unidad ahora tiene esa unidad
                        cinicio.SetUnidadEnCelda(null); // Esta celda ya no tiene unidad

                        Turnos.CambiarTurno();
                        JaqueCalculo();

                        //Primer movimiento realizado, se pasa el movimento en la direccion Norte o Sur de 2 a 1
                        if(unidad.UnidadJugador.idJugador == 2)
                        {
                        NuevoMovimiento(unidad,Direccion.Sur,1);    
                        }
                        else
                        {
                        NuevoMovimiento(unidad,Direccion.Norte,1);
                        }

                        //Si la celda es el extremo de ese jugador, entonces el peon puede promocionar en caballo, alfil, torre o reina
                        if(cobjetivo.celdaPromocion == true)
                        {

                            unidad.gameObject.GetComponent<UnidadMovimientoEspecial>().Promocion(cobjetivo);

                        }

                    }
                    else
                    {

                        cinicio.SetUnidadEnCelda(cinicio.GetUnidadEnCelda());
                        return false;

                    }
                
                break;

            }
            case TipoUnidad.Caballo:
            {
                    
                    if(DireccionDirectaUnidad(cobjetivo,cinicio,unidad,false,false,false ))
                    {

                        movimientos.Add(new Movimiento(cinicio,cobjetivo,cinicio.GetUnidadEnCelda(),cobjetivo.GetUnidadEnCelda()));

                        if(ataque == true)
                        {

                        cobjetivo.GetUnidadEnCelda().gameObject.SetActive(false);
                        cobjetivo.SetUnidadEnCelda(null);  // La unidad objetiva se esconde y se quita la ref en la celda objetivo

                        }                                                                                           

                        cobjetivo.SetUnidadEnCelda(cinicio.GetUnidadEnCelda()); // La celda donde muevo la unidad ahora tiene esa unidad
                        cinicio.SetUnidadEnCelda(null); // Esta celda ya no tiene unidad
                        
                        
                        Turnos.CambiarTurno();
                        JaqueCalculo();

                    }
                    else 
                    {

                        cinicio.SetUnidadEnCelda(cinicio.GetUnidadEnCelda()); 
                        return false;

                    }

                break;

            }
            case TipoUnidad.Rey:
            {

                    // ENROQUE
                    if(cobjetivo.GetUnidadEnCelda() != null)
                    {

                        if(cobjetivo.GetUnidadEnCelda().GetTipoUnidad()==TipoUnidad.Torre 
                        && cobjetivo.GetUnidadEnCelda().UnidadJugador == unidad.UnidadJugador)
                        {

                            if(unidad.gameObject.GetComponent<UnidadMovimientoEspecial>()
                            .Enroque(cinicio,cobjetivo,unidad))
                            {

                                movimientos.Add(new Movimiento(cinicio,cobjetivo,cinicio.GetUnidadEnCelda(),cobjetivo.GetUnidadEnCelda()));

                                audioSource.PlayOneShot(audioSistema.audioEnroque);

                                int ciniciocolum;       // rey
                                int cobjetivocolum;     // torre
                                if(Turnos.GetJugadorActual().miColor == ColorJugador.Blancas && Turnos.GetJugadorActual().idJugador == 1
                                || Turnos.GetJugadorActual().miColor == ColorJugador.Negras && Turnos.GetJugadorActual().idJugador == 2)
                                {

                                    if(cinicio.columna > cobjetivo.columna) // enroque largo
                                    {
                                    
                                        ciniciocolum = cinicio.columna-2;
                                        cobjetivocolum = cobjetivo.columna+3;
                                        
                                    }
                                    else                                    // enroque largo
                                    {

                                        ciniciocolum = cinicio.columna+2;
                                        cobjetivocolum = cobjetivo.columna-2;

                                    }

                                }
                                else
                                {

                                    if(cinicio.columna > cobjetivo.columna) // enroque corto
                                    {
                                    
                                        ciniciocolum = cinicio.columna-2;
                                        cobjetivocolum = cobjetivo.columna+2;
                                        
                                    }
                                    else                                    // enroque largo
                                    {

                                        ciniciocolum = cinicio.columna+2;
                                        cobjetivocolum = cobjetivo.columna-3;

                                    }

                                }

                                Celda cinicioenr = GameObject.Find("Celda " + cinicio.fila + " " + ciniciocolum).GetComponent<Celda>();
                                Celda cobjetivoenr = GameObject.Find("Celda " + cobjetivo.fila + " " + cobjetivocolum).GetComponent<Celda>();

                                cobjetivoenr.SetUnidadEnCelda(cobjetivo.GetUnidadEnCelda()); 
                                cinicioenr.SetUnidadEnCelda(cinicio.GetUnidadEnCelda()); 
                                cobjetivo.SetUnidadEnCelda(null); 
                                cinicio.SetUnidadEnCelda(null); 

                                unidad.UnidadJugador.reyCelda = cinicioenr;
                                Turnos.CambiarTurno();
                               
                                

                                return true;

                            }

                        }

                    }

                    // Un rey no puede moverse a una celda amenazada
                    if(JaqueCalculo(cobjetivo,false,false,null,null))
                    {
                        cinicio.SetUnidadEnCelda(cinicio.GetUnidadEnCelda()); 
                        return false;
                    }

                    if(DireccionUnidad(cobjetivo,cinicio,unidad,false,false,false,false ) && ataque == true)
                    {

                       

                        movimientos.Add(new Movimiento(cinicio,cobjetivo,cinicio.GetUnidadEnCelda(),cobjetivo.GetUnidadEnCelda()));

                        // La unidad objetiva se esconde y se quita la ref en la celda objetivo
                        cobjetivo.GetUnidadEnCelda().gameObject.SetActive(false);
                        cobjetivo.SetUnidadEnCelda(null);   

                        cobjetivo.SetUnidadEnCelda(cinicio.GetUnidadEnCelda()); // La celda donde muevo la unidad ahora tiene esa unidad
                        cinicio.SetUnidadEnCelda(null);                         // Esta celda ya no tiene unidad
                        
                        unidad.UnidadJugador.reyCelda = cobjetivo;
                         ReyAhogado();
                        Turnos.CambiarTurno();
                        JaqueCalculo();

                    }
                    else if(DireccionUnidad(cobjetivo,cinicio,unidad,false,false,false,false ) && ataque == false)
                    {


                        movimientos.Add(new Movimiento(cinicio,cobjetivo,cinicio.GetUnidadEnCelda(),cobjetivo.GetUnidadEnCelda()));

                        cobjetivo.SetUnidadEnCelda(cinicio.GetUnidadEnCelda()); // La celda donde muevo la unidad ahora tiene esa unidad
                        cinicio.SetUnidadEnCelda(null); // Esta celda ya no tiene unidad

                        unidad.UnidadJugador.reyCelda = cobjetivo;
                       
                        ReyAhogado(); 
                        Turnos.CambiarTurno();
                        JaqueCalculo();
                        

                    }
                    else
                    {

                        cinicio.SetUnidadEnCelda(cinicio.GetUnidadEnCelda()); 
                        return false;

                    }

                break;

            }
            default:
            {

                    if(DireccionUnidad(cobjetivo,cinicio,unidad,false,false,false,false ) && ataque == true)
                    {

                        movimientos.Add(new Movimiento(cinicio,cobjetivo,cinicio.GetUnidadEnCelda(),cobjetivo.GetUnidadEnCelda()));
                    
                        // La unidad objetiva se esconde y se quita la ref en la celda objetivo
                        cobjetivo.GetUnidadEnCelda().gameObject.SetActive(false);
                        cobjetivo.SetUnidadEnCelda(null);  

                        cobjetivo.SetUnidadEnCelda(cinicio.GetUnidadEnCelda()); // La celda donde muevo la unidad ahora tiene esa unidad
                        cinicio.SetUnidadEnCelda(null);                         // Esta celda ya no tiene unidad

                        
                        Turnos.CambiarTurno();
                        JaqueCalculo();

                    }
                    else if(DireccionUnidad(cobjetivo,cinicio,unidad,false,false,false,false ) && ataque == false)
                    {

                        movimientos.Add(new Movimiento(cinicio,cobjetivo,cinicio.GetUnidadEnCelda(),cobjetivo.GetUnidadEnCelda()));

                        cobjetivo.SetUnidadEnCelda(cinicio.GetUnidadEnCelda()); // La celda donde muevo la unidad ahora tiene esa unidad
                        cinicio.SetUnidadEnCelda(null);                         // Esta celda ya no tiene unidad

                        
                        Turnos.CambiarTurno();
                        JaqueCalculo();

                    }
                    else
                    {

                        cinicio.SetUnidadEnCelda(cinicio.GetUnidadEnCelda()); 
                        return false;
                        
                    }

                break;

            }

        }
        
        if(ataque == true)
        {

            audioSource.PlayOneShot(audioSistema.audioAtaque);

        }
        else
        {

            audioSource.PlayOneShot(audioSistema.audioMovimento);

        }

        return true;

    }

    /*

     ██████╗ █████╗ ██╗      ██████╗██╗   ██╗██╗      ██████╗ 
    ██╔════╝██╔══██╗██║     ██╔════╝██║   ██║██║     ██╔═══██╗
    ██║     ███████║██║     ██║     ██║   ██║██║     ██║   ██║
    ██║     ██╔══██║██║     ██║     ██║   ██║██║     ██║   ██║
    ╚██████╗██║  ██║███████╗╚██████╗╚██████╔╝███████╗╚██████╔╝
     ╚═════╝╚═╝  ╚═╝╚══════╝ ╚═════╝ ╚═════╝ ╚══════╝ ╚═════╝ 

    */

    public int CalculoMovimiento(Unidad unidad, Celda cinicio, bool jaquecalculo, bool ahogadocalculo)
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

        int movimientosNum = 0;

            if(unidad.MovimientoDirecto == false)
            {

                for (int z = 0; z < (cuadricula.filas*cuadricula.columnas); z++) // Bloquear movimiento por otra unidad en la direccion
                {
                     
                    if(celdas[z].GetUnidadEnCelda()==cinicio.GetUnidadEnCelda()){continue; } // Si es la misma unidad no cuenta para el calculo de bloquear
                    else if(celdas[z].GetUnidadEnCelda()!=null)
                    {
                        
                        // Al mirar la cuadricula de forma ascendente hay que asegurarse de que las direccion Sur, SurEste, SurOeste y Oeste 
                        //recojan la celda mas cercana a la celda de la unidad con la que se hace el calculo

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

                        if(celdabloqNorte!=null     && celdabloqNorte.fila<celdas[i].fila       && celdabloqNorte.columna==celdas[i].columna)   {continue;}
                        if(celdabloqNorEste!=null   && celdabloqNorEste.fila<celdas[i].fila     && celdabloqNorEste.columna<celdas[i].columna)  {continue;}
                        if(celdabloqEste!=null      && celdabloqEste.fila==celdas[i].fila       && celdabloqEste.columna<celdas[i].columna)     {continue;} 
                        if(celdabloqSurEste!=null   && celdabloqSurEste.fila>celdas[i].fila     && celdabloqSurEste.columna<celdas[i].columna)  {continue;}
                        if(celdabloqSur!=null       && celdabloqSur.fila>celdas[i].fila         && celdabloqSur.columna==celdas[i].columna)     {continue;}
                        if(celdabloqSurOeste!=null  && celdabloqSurOeste.fila>celdas[i].fila    && celdabloqSurOeste.columna>celdas[i].columna) {continue;}
                        if(celdabloqOeste!=null     && celdabloqOeste.fila==celdas[i].fila      && celdabloqOeste.columna>celdas[i].columna)    {continue;}
                        if(celdabloqNorOeste!=null  && celdabloqNorOeste.fila<celdas[i].fila    && celdabloqNorOeste.columna>celdas[i].columna) {continue;}

                        // Si la unidad es del mismo jugador
                        if(celdas[i].GetUnidadEnCelda()!=null 
                        && celdas[i].GetUnidadEnCelda().UnidadJugador == unidad.UnidadJugador){continue;}
                        
              
                      
                        // Debug.Log(" " + celdas[i] + " " + JaqueCalculo(celdas[i],false,ahogadocalculo,null,null));

                        // Si la celda esta amenazada para el rey
                        if(cinicio.GetUnidadEnCelda().GetTipoUnidad() == TipoUnidad.Rey 
                        && JaqueCalculo(celdas[i],false,ahogadocalculo,null,null))
                        {continue;} 
                        
                        
                        
                     

                        if(DireccionUnidad(celdas[i],cinicio,unidad ,true,false,jaquecalculo,ahogadocalculo))
                        {movimientosNum++;}

                        // if(ahogadocalculo == true)
                        // {
                        //     if(DireccionUnidad(celdas[i],cinicio,unidad ,true,false,jaquecalculo,ahogadocalculo) 
                        //     && cinicio.GetUnidadEnCelda().UnidadJugador.idJugador==2)
                        //     {Debug.Log("A : " + cinicio.gameObject.name + " " + celdas[i].gameObject.name);} 
                        // }

                    }
                    else
                    {

                        // Si la unidad es del mismo jugador
                        if(celdas[i].GetUnidadEnCelda()!=null 
                        && celdas[i].GetUnidadEnCelda().UnidadJugador == unidad.UnidadJugador)
                        {continue;} 

                        // Si la celda esta amenazada para el rey
                        if(cinicio.GetUnidadEnCelda().GetTipoUnidad() == TipoUnidad.Rey && JaqueCalculo(celdas[i],false,ahogadocalculo,null,null)){continue;}

                        if(DireccionDirectaUnidad(celdas[i],cinicio,unidad ,true,false,ahogadocalculo))
                        {movimientosNum++;}

                    }

                }

    return movimientosNum;

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

    /*

        ██████╗ ██╗██████╗ ███████╗ ██████╗████████╗ ██████╗ 
        ██╔══██╗██║██╔══██╗██╔════╝██╔════╝╚══██╔══╝██╔═══██╗
        ██║  ██║██║██████╔╝█████╗  ██║        ██║   ██║   ██║
        ██║  ██║██║██╔══██╗██╔══╝  ██║        ██║   ██║   ██║
        ██████╔╝██║██║  ██║███████╗╚██████╗   ██║   ╚██████╔╝
        ╚═════╝ ╚═╝╚═╝  ╚═╝╚══════╝ ╚═════╝   ╚═╝    ╚═════╝ 

    */

    bool DireccionDirectaUnidad(Celda celda, Celda celdainicio, Unidad unidad, bool calculo, bool calculojaque, bool calculoahogado)
    {

        if(calculo == false 
        && celda.GetUnidadEnCelda()!=null 
        && celda.GetUnidadEnCelda().UnidadJugador == unidad.UnidadJugador 
        && calculojaque == false
        && calculoahogado == false)
        {celdainicio.SetUnidadEnCelda(celdainicio.GetUnidadEnCelda()); return false;} //Si la unidad es del mismo jugador
        
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

        return false;

    }

    bool DireccionUnidad(Celda celda, Celda celdainicio, Unidad unidad, bool calculo, bool calculoataque, bool calculojaque, bool calculoahogado)
    {
       
        if(calculo == false 
        && celda.GetUnidadEnCelda()!=null 
        && celda.GetUnidadEnCelda().UnidadJugador == unidad.UnidadJugador 
        && calculojaque == false)
            {celdainicio.SetUnidadEnCelda(celdainicio.GetUnidadEnCelda()); return false;} //Si la unidad es del mismo jugador

        for (int i = 0; i < unidad.limiteDirMov.Count; i++)
        {

            /*

            ██████╗ ██╗      ██████╗  ██████╗ ██╗   ██╗███████╗ ██████╗ 
            ██╔══██╗██║     ██╔═══██╗██╔═══██╗██║   ██║██╔════╝██╔═══██╗
            ██████╔╝██║     ██║   ██║██║   ██║██║   ██║█████╗  ██║   ██║
            ██╔══██╗██║     ██║   ██║██║▄▄ ██║██║   ██║██╔══╝  ██║   ██║
            ██████╔╝███████╗╚██████╔╝╚██████╔╝╚██████╔╝███████╗╚██████╔╝
            ╚═════╝ ╚══════╝ ╚═════╝  ╚══▀▀═╝  ╚═════╝ ╚══════╝ ╚═════╝ 
   
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
                        if(celdas[z].GetUnidadEnCelda().GetTipoUnidad()==TipoUnidad.Rey && calculojaque == true){continue;}

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

                    // if(calculoahogado==true && celdainicio.GetUnidadEnCelda().GetTipoUnidad() == TipoUnidad.Rey
                    // && celdainicio.GetUnidadEnCelda().UnidadJugador != Turnos.GetJugadorActual())
                    // {Debug.Log(celdainicio.gameObject.name + " " + celda.gameObject.name);}
                    // Si hay una unidad bloqueando la direccion devuelve falso
                    
                    if(celdabloqNorte!=null         && unidad.limiteDirMov[i].direccionUnidad == Direccion.Norte    
                    && celdabloqNorte.fila<celda.fila       && celdabloqNorte.columna==celda.columna)   {return false;}
                    else if(celdabloqNorEste!=null  && unidad.limiteDirMov[i].direccionUnidad == Direccion.NorEste  
                    && celdabloqNorEste.fila<celda.fila     && celdabloqNorEste.columna<celda.columna)  {return false;}
                    else if(celdabloqEste!=null     && unidad.limiteDirMov[i].direccionUnidad == Direccion.Este     
                    && celdabloqEste.fila==celda.fila       && celdabloqEste.columna<celda.columna)     {return false;} 
                    else if(celdabloqSurEste!=null  && unidad.limiteDirMov[i].direccionUnidad == Direccion.SurEste  
                    && celdabloqSurEste.fila>celda.fila     && celdabloqSurEste.columna<celda.columna)  {return false;}
                    else if(celdabloqSur!=null      && unidad.limiteDirMov[i].direccionUnidad == Direccion.Sur      
                    && celdabloqSur.fila>celda.fila         && celdabloqSur.columna==celda.columna)     {return false;}
                    else if(celdabloqSurOeste!=null && unidad.limiteDirMov[i].direccionUnidad == Direccion.SurOeste 
                    && celdabloqSurOeste.fila>celda.fila    && celdabloqSurOeste.columna>celda.columna) {return false;}
                    else if(celdabloqOeste!=null    && unidad.limiteDirMov[i].direccionUnidad == Direccion.Oeste    
                    && celdabloqOeste.fila==celda.fila      && celdabloqOeste.columna>celda.columna)    {return false;}
                    else if(celdabloqNorOeste!=null && unidad.limiteDirMov[i].direccionUnidad == Direccion.NorOeste 
                    && celdabloqNorOeste.fila<celda.fila    && celdabloqNorOeste.columna>celda.columna) {return false;}

            }

            
       

            /*
                        

             █████╗ ████████╗ █████╗  ██████╗ ██╗   ██╗███████╗
            ██╔══██╗╚══██╔══╝██╔══██╗██╔═══██╗██║   ██║██╔════╝
            ███████║   ██║   ███████║██║   ██║██║   ██║█████╗  
            ██╔══██║   ██║   ██╔══██║██║▄▄ ██║██║   ██║██╔══╝  
            ██║  ██║   ██║   ██║  ██║╚██████╔╝╚██████╔╝███████╗
            ╚═╝  ╚═╝   ╚═╝   ╚═╝  ╚═╝ ╚══▀▀═╝  ╚═════╝ ╚══════╝
                                                                                                                       
            */

       

            if(unidad.MovimientoConAtaque==false)
            {
                if(celda.GetUnidadEnCelda()!=null || calculoataque == true)
                {
                if(celda.fila - celdainicio.fila > 0  && (celda.columna - celdainicio.columna) == 0             // La celda objetivo es Norte 
                                                                                                                // respecto a la celda inicial
                    && unidad.limiteDirAtaq[i].direccionUnidad == Direccion.Norte                               // Norte
                    && (celda.fila-celdainicio.fila)<=unidad.limiteDirAtaq[i].limiteAtaque                      // Limite Movimiento   
                    )
                    {if(calculo == true && calculojaque == false)
                        {if(celda.GetUnidadEnCelda()==null){celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;}
                        else{celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[2].enabled = true;}}return true;}
                if(celda.fila - celdainicio.fila < 0  && (celda.columna - celdainicio.columna) == 0  
                    && unidad.limiteDirAtaq[i].direccionUnidad == Direccion.Sur                                  // Sur
                    && (celdainicio.fila-celda.fila)<=unidad.limiteDirAtaq[i].limiteAtaque 
                    )
                    {if(calculo == true && calculojaque == false)
                        {if(celda.GetUnidadEnCelda()==null){celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;}
                        else{celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[2].enabled = true;}}return true;}
                if(celda.fila - celdainicio.fila == 0  && (celda.columna - celdainicio.columna) > 0 
                    && unidad.limiteDirAtaq[i].direccionUnidad == Direccion.Este                                 // Este
                    && (celda.columna-celdainicio.columna)<=unidad.limiteDirAtaq[i].limiteAtaque
                    )
                    {if(calculo == true && calculojaque == false)
                        {if(celda.GetUnidadEnCelda()==null){celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;}
                        else{celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[2].enabled = true;}}return true;}
                if(celda.fila - celdainicio.fila == 0  && (celda.columna - celdainicio.columna) < 0 
                    && unidad.limiteDirAtaq[i].direccionUnidad == Direccion.Oeste                                // Oeste
                    && (celdainicio.columna-celda.columna)<=unidad.limiteDirAtaq[i].limiteAtaque
                    )
                    {if(calculo == true && calculojaque == false)
                        {if(celda.GetUnidadEnCelda()==null){celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;}
                        else{celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[2].enabled = true;}}return true;}
                if(celda.fila - celdainicio.fila > 0  && (celda.columna - celdainicio.columna) > 0 
                    && (celda.fila - celdainicio.fila)==(celda.columna - celdainicio.columna) 
                    && unidad.limiteDirAtaq[i].direccionUnidad == Direccion.NorEste                              // NorEste
                    && (celda.fila-celdainicio.fila)<=unidad.limiteDirAtaq[i].limiteAtaque
                    && (celda.columna-celdainicio.columna)<=unidad.limiteDirAtaq[i].limiteAtaque
                    )
                    {if(calculo == true && calculojaque == false)
                        {if(celda.GetUnidadEnCelda()==null){celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;}
                        else{celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[2].enabled = true;}}return true;}
                if(celda.fila - celdainicio.fila > 0  && (celda.columna - celdainicio.columna) < 0 
                    && (celda.fila - celdainicio.fila)==(celda.columna - celdainicio.columna)*-1  
                    && unidad.limiteDirAtaq[i].direccionUnidad == Direccion.NorOeste                             // NorOeste
                    && (celdainicio.fila-celda.fila)<=unidad.limiteDirAtaq[i].limiteAtaque
                    && (celdainicio.columna-celda.columna)<=unidad.limiteDirAtaq[i].limiteAtaque
                    )
                    {if(calculo == true && calculojaque == false)
                        {if(celda.GetUnidadEnCelda()==null){celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;}
                        else{celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[2].enabled = true;}}return true;}
                if(celda.fila - celdainicio.fila < 0  
                    && (celda.columna - celdainicio.columna) < 0 
                    && (celda.fila - celdainicio.fila)==(celda.columna - celdainicio.columna) 
                    && unidad.limiteDirAtaq[i].direccionUnidad == Direccion.SurOeste                             // SurOeste
                    && (celdainicio.fila-celda.fila)<=unidad.limiteDirAtaq[i].limiteAtaque
                    && (celdainicio.columna-celda.columna)<=unidad.limiteDirAtaq[i].limiteAtaque
                    )
                    {if(calculo == true && calculojaque == false)
                        {if(celda.GetUnidadEnCelda()==null){celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;}
                        else{celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[2].enabled = true;}}return true;}
                if(celda.fila - celdainicio.fila < 0  
                    && (celda.columna - celdainicio.columna) > 0 
                    && (celda.fila - celdainicio.fila)*-1==(celda.columna - celdainicio.columna) 
                    && unidad.limiteDirAtaq[i].direccionUnidad == Direccion.SurEste                              // SurEste
                    && (celda.fila-celdainicio.fila)<=unidad.limiteDirAtaq[i].limiteAtaque
                    && (celda.columna-celdainicio.columna)<=unidad.limiteDirAtaq[i].limiteAtaque
                    )
                    {if(calculo == true && calculojaque == false)
                        {if(celda.GetUnidadEnCelda()==null){celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;}
                        else{celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[2].enabled = true;}}return true;}

                }

            }
            
            /*

            ███╗   ███╗ ██████╗ ██╗   ██╗██╗███╗   ███╗██╗███████╗███╗   ██╗████████╗ ██████╗ 
            ████╗ ████║██╔═══██╗██║   ██║██║████╗ ████║██║██╔════╝████╗  ██║╚══██╔══╝██╔═══██╗
            ██╔████╔██║██║   ██║██║   ██║██║██╔████╔██║██║█████╗  ██╔██╗ ██║   ██║   ██║   ██║
            ██║╚██╔╝██║██║   ██║╚██╗ ██╔╝██║██║╚██╔╝██║██║██╔══╝  ██║╚██╗██║   ██║   ██║   ██║
            ██║ ╚═╝ ██║╚██████╔╝ ╚████╔╝ ██║██║ ╚═╝ ██║██║███████╗██║ ╚████║   ██║   ╚██████╔╝
            ╚═╝     ╚═╝ ╚═════╝   ╚═══╝  ╚═╝╚═╝     ╚═╝╚═╝╚══════╝╚═╝  ╚═══╝   ╚═╝    ╚═════╝ 

            */

            if(unidad.MovimientoConAtaque==false && calculoataque == true){continue;}
            
            if(unidad.MovimientoConAtaque==false && celda.GetUnidadEnCelda()!=null){continue;} // Las unidades pueden no tener ataque con movimiento

            if(celda.fila - celdainicio.fila > 0  && (celda.columna - celdainicio.columna) == 0             // La celda objetivo es Norte respecto a la celda inicial
                && unidad.limiteDirMov[i].direccionUnidad == Direccion.Norte                                // Norte
                && (celda.fila-celdainicio.fila)<=unidad.limiteDirMov[i].limiteMovimiento )                 // Limite Movimiento       
                    {if(calculo == true && calculojaque == false)
                        {if(celda.GetUnidadEnCelda()==null){celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;}
                        else{celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[2].enabled = true;}}return true;}
            if(celda.fila - celdainicio.fila < 0  && (celda.columna - celdainicio.columna) == 0  
                && unidad.limiteDirMov[i].direccionUnidad == Direccion.Sur                                  // Sur
                && (celdainicio.fila-celda.fila)<=unidad.limiteDirMov[i].limiteMovimiento )        
                    {if(calculo == true && calculojaque == false)
                        {if(celda.GetUnidadEnCelda()==null){celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;}
                        else{celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[2].enabled = true;}}return true;}
            if(celda.fila - celdainicio.fila == 0  && (celda.columna - celdainicio.columna) > 0 
                && unidad.limiteDirMov[i].direccionUnidad == Direccion.Este                                 // Este
                && (celda.columna-celdainicio.columna)<=unidad.limiteDirMov[i].limiteMovimiento)       
                    {if(calculo == true)
                        {if(celda.GetUnidadEnCelda()==null){celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;}
                        else{celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[2].enabled = true;}}return true;}
            if(celda.fila - celdainicio.fila == 0  && (celda.columna - celdainicio.columna) < 0 
                && unidad.limiteDirMov[i].direccionUnidad == Direccion.Oeste                                // Oeste
                && (celdainicio.columna-celda.columna)<=unidad.limiteDirMov[i].limiteMovimiento)       
                    {if(calculo == true && calculojaque == false)
                        {if(celda.GetUnidadEnCelda()==null){celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;}
                        else{celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[2].enabled = true;}}return true;}
            if(celda.fila - celdainicio.fila > 0  && (celda.columna - celdainicio.columna) < 0 
                && (celda.fila - celdainicio.fila)==(celda.columna - celdainicio.columna)*-1  
                && unidad.limiteDirMov[i].direccionUnidad == Direccion.NorOeste                             // NorOeste
                && (celdainicio.fila-celda.fila)<=unidad.limiteDirMov[i].limiteMovimiento
                && (celdainicio.columna-celda.columna)<=unidad.limiteDirMov[i].limiteMovimiento)
                    {if(calculo == true && calculojaque == false)
                        {if(celda.GetUnidadEnCelda()==null){celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;}
                        else{celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[2].enabled = true;}}return true;}
            if(celda.fila - celdainicio.fila > 0  && (celda.columna - celdainicio.columna) > 0 
                && (celda.fila - celdainicio.fila)==(celda.columna - celdainicio.columna) 
                && unidad.limiteDirMov[i].direccionUnidad == Direccion.NorEste                              // NorEste
                && (celda.fila-celdainicio.fila)<=unidad.limiteDirMov[i].limiteMovimiento
                && (celda.columna-celdainicio.columna)<=unidad.limiteDirMov[i].limiteMovimiento)            
                    {if(calculo == true && calculojaque == false)
                        {if(celda.GetUnidadEnCelda()==null){celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;}
                        else{celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[2].enabled = true;}}return true;}
            if(celda.fila - celdainicio.fila < 0  
                && (celda.columna - celdainicio.columna) < 0 
                && (celda.fila - celdainicio.fila)==(celda.columna - celdainicio.columna) 
                && unidad.limiteDirMov[i].direccionUnidad == Direccion.SurOeste                             // SurOeste
                && (celdainicio.fila-celda.fila)<=unidad.limiteDirMov[i].limiteMovimiento
                && (celdainicio.columna-celda.columna)<=unidad.limiteDirMov[i].limiteMovimiento)    
                    {if(calculo == true && calculojaque == false)
                        {if(celda.GetUnidadEnCelda()==null){celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;}
                        else{celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[2].enabled = true;}}return true;}
            if(celda.fila - celdainicio.fila < 0  
                && (celda.columna - celdainicio.columna) > 0 
                && (celda.fila - celdainicio.fila)*-1==(celda.columna - celdainicio.columna) 
                && unidad.limiteDirMov[i].direccionUnidad == Direccion.SurEste                              // SurEste
                && (celda.fila-celdainicio.fila)<=unidad.limiteDirMov[i].limiteMovimiento
                && (celda.columna-celdainicio.columna)<=unidad.limiteDirMov[i].limiteMovimiento)             
                    {if(calculo == true && calculojaque == false)
                        {if(celda.GetUnidadEnCelda()==null){celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;}
                        else{celda.gameObject.GetComponentsInChildren<SpriteRenderer>()[2].enabled = true;}}return true;}

        }
        // if(calculoahogado == true && celdainicio.GetUnidadEnCelda() != null){
        //     if(celdainicio.GetUnidadEnCelda().UnidadJugador.idJugador==2){Debug.Log(celdainicio.gameObject.name + " " + celda.gameObject.name);}}
        return false;

    }



    /*

         ██╗ █████╗  ██████╗ ██╗   ██╗███████╗███████╗
         ██║██╔══██╗██╔═══██╗██║   ██║██╔════╝██╔════╝
         ██║███████║██║   ██║██║   ██║█████╗  ███████╗
    ██   ██║██╔══██║██║▄▄ ██║██║   ██║██╔══╝  ╚════██║
    ╚█████╔╝██║  ██║╚██████╔╝╚██████╔╝███████╗███████║
    ╚════╝ ╚═╝  ╚═╝ ╚══▀▀═╝  ╚═════╝ ╚══════╝╚══════╝
                                                    

    */

    public void JaqueCalculo() // Para saber si alguna unidad esta poniendo en jaque al rey de cada Jugador
    {

        Celda[] celdas = cuadricula.GetCeldas();

        Turno TurnoScript = cuadricula.GetComponent<Turno>();

            foreach(Jugador jugadores in TurnoScript.Jugadores)
            {
                jugadores.GetComponent<MateJaque>().Jaque = false;
                jugadores.GetComponent<MateJaque>().jaqueMovimiento = new MateJaque.MovimientoJaque();
            }

            for (int i = 0; i < (cuadricula.filas*cuadricula.columnas); i++) 
            {

                if(celdas[i].GetUnidadEnCelda()==null){continue;}

                foreach(Jugador jugadores in TurnoScript.Jugadores)
                {

                    // if(jugadores == TurnoScript.GetJugadorActual()){continue;}
                    if(celdas[i].GetUnidadEnCelda().UnidadJugador == TurnoScript.GetJugadorActual()){continue;}
                    if(celdas[i].GetUnidadEnCelda().MovimientoDirecto == false)
                    {
                         
                        if(DireccionUnidad(jugadores.reyCelda,celdas[i],celdas[i].GetUnidadEnCelda() ,false,true,false,false))
                        {

                            MateJaque mj = jugadores.GetComponent<MateJaque>();

                            mj.Jaque = true;
                            mj.jaqueMovimiento = new MateJaque.MovimientoJaque(celdas[i].GetUnidadEnCelda(),celdas[i],jugadores.reyCelda);

                        }

                    }
                    else
                    {
                       
                        // if(jugadores == TurnoScript.GetJugadorActual()){continue;}
                        if(DireccionDirectaUnidad(jugadores.reyCelda,celdas[i],celdas[i].GetUnidadEnCelda() ,false,false,false))
                        {

                            MateJaque mj = jugadores.GetComponent<MateJaque>();

                            mj.Jaque = true;
                            mj.jaqueMovimiento = new MateJaque.MovimientoJaque(celdas[i].GetUnidadEnCelda(),celdas[i],jugadores.reyCelda);

                        }

                    }

                }

            }

    }

    // Para saber si una celda en concreto esta siendo amenazada
    public bool JaqueCalculo(Celda cAmenazada,bool calculojaquemate, bool calculoahogado, Celda cInicioJaque, Celda cobjetivoJaque) 
    {

        Celda[] celdas = cuadricula.GetCeldas();

        Turno TurnoScript = cuadricula.GetComponent<Turno>();

        bool calculoataque = false;
        bool calculojaque = true;

        Unidad cobjetivotemp = null;
        if(cobjetivoJaque!=null)
        {
        if(cobjetivoJaque.GetUnidadEnCelda()!=null)
        {
            cobjetivotemp = cobjetivoJaque.GetUnidadEnCelda();
        }
        }

            for (int i = 0; i < (cuadricula.filas*cuadricula.columnas); i++) 
            {
                
                if(celdas[i].GetUnidadEnCelda()==null){continue;}
                if(celdas[i].GetUnidadEnCelda().GetTipoUnidad()==TipoUnidad.Rey && calculojaquemate==true){continue;}
                if(celdas[i].GetUnidadEnCelda().UnidadJugador == TurnoScript.GetJugadorActual() && calculojaquemate == false){continue;}


                if(celdas[i].GetUnidadEnCelda().MovimientoConAtaque==false){calculoataque=true;}

                if(calculojaquemate == true)
                {
                if(cAmenazada.GetUnidadEnCelda().UnidadJugador == TurnoScript.GetJugadorActual()){continue;}
                }
                


                    if(cInicioJaque != null && cobjetivoJaque !=null)
                    {
                      
                        cobjetivoJaque.SetUnidadEnCelda(cInicioJaque.GetUnidadEnCelda());
                        cInicioJaque.SetUnidadEnCelda(null); 
                        calculojaque = false;
                        
                    }
                    else
                    {calculojaque = true;}

                        if(celdas[i].GetUnidadEnCelda().MovimientoDirecto == false)
                        {

                             

                            if(DireccionUnidad(cAmenazada, celdas[i], celdas[i].GetUnidadEnCelda(), false, calculoataque, calculojaque, calculoahogado))
                            {

                                if(cInicioJaque != null && cobjetivoJaque !=null)
                                {

                                    cInicioJaque.SetUnidadEnCelda(cobjetivoJaque.GetUnidadEnCelda());
                                    cobjetivoJaque.SetUnidadEnCelda(cobjetivotemp); 


                                }

                                // if(calculoahogado == true){Debug.Log(celdas[i].gameObject.name + " " + cAmenazada.gameObject.name + " SI " );}

                                return true;

                            }
                            else
                            {

                                // if(calculoahogado == true){Debug.Log(celdas[i].gameObject.name + " " + cAmenazada.gameObject.name + " NO " );}

                            }

                        }
                        else
                        {


                            if(DireccionDirectaUnidad(cAmenazada, celdas[i], celdas[i].GetUnidadEnCelda(), false, calculojaque,calculoahogado))
                            {
                            
                                if(cInicioJaque != null && cobjetivoJaque !=null)
                                {

                                    cInicioJaque.SetUnidadEnCelda(cobjetivoJaque.GetUnidadEnCelda());
                                    cobjetivoJaque.SetUnidadEnCelda(cobjetivotemp); 

                                }

                                return true;

                            }

                        }

            if(cInicioJaque != null && cobjetivoJaque !=null)
            {

                cInicioJaque.SetUnidadEnCelda(cobjetivoJaque.GetUnidadEnCelda());
                cobjetivoJaque.SetUnidadEnCelda(cobjetivotemp); 

            }

            }

            return false;

    }

    public bool JaqueBloqueoCalculo(Celda celdaJaque, Celda celdaRey, Celda celdaObjetivo)
    {

        int filaCeldaJaque      = celdaJaque.fila;
        int columnaCeldaJaque   = celdaJaque.columna;
        int filaCeldaRey        = celdaRey.fila;
        int columnaCeldaRey     = celdaRey.columna;

        while(filaCeldaJaque != filaCeldaRey || columnaCeldaJaque != columnaCeldaRey)
        {


            if(filaCeldaJaque < filaCeldaRey)               {filaCeldaJaque++;}
            else if (filaCeldaJaque > filaCeldaRey)         {filaCeldaJaque--;}

            if(columnaCeldaJaque < columnaCeldaRey)         {columnaCeldaJaque++;}
            else if (columnaCeldaJaque > columnaCeldaRey)   {columnaCeldaJaque--;}


            Celda celdabloqueo = GameObject.Find("Celda " + filaCeldaJaque + " " + columnaCeldaJaque).GetComponent<Celda>();

            if(celdaObjetivo!=null){if(celdabloqueo != celdaObjetivo){continue;}}

            Celda[] celdas = cuadricula.GetCeldas();

            for (int i = 0; i < (cuadricula.filas*cuadricula.columnas); i++) 
            {

                if(celdas[i].GetUnidadEnCelda()==null){continue;}
                if(celdas[i].GetUnidadEnCelda().UnidadJugador != Turnos.GetJugadorActual()){continue;}

                if(celdas[i].GetUnidadEnCelda().MovimientoDirecto == false)
                {

                    if(DireccionUnidad(celdabloqueo,celdas[i],celdas[i].GetUnidadEnCelda(),false,false,false,false) == true)
                    {

                        return true;

                    }

                }
                else
                {

                    if(DireccionDirectaUnidad(celdabloqueo,celdas[i],celdas[i].GetUnidadEnCelda(),false,false,false) == true)
                    {

                        return true;

                    }

                }

            }

        }

    return false;

    }

    public bool JaqueBloqueoCalculo(Celda celdaJaque, Celda celdaRey, Celda celdaUnidad,Celda celdaObjetivo)
    {

        int filaCeldaJaque      = celdaJaque.fila;
        int columnaCeldaJaque   = celdaJaque.columna;
        int filaCeldaRey        = celdaRey.fila;
        int columnaCeldaRey     = celdaRey.columna;

        while(filaCeldaJaque != filaCeldaRey || columnaCeldaJaque != columnaCeldaRey)
        {

            if(filaCeldaJaque < filaCeldaRey)               {filaCeldaJaque++;}
            else if (filaCeldaJaque > filaCeldaRey)         {filaCeldaJaque--;}

            if(columnaCeldaJaque < columnaCeldaRey)         {columnaCeldaJaque++;}
            else if (columnaCeldaJaque > columnaCeldaRey)   {columnaCeldaJaque--;}

            Celda celdabloqueo = GameObject.Find("Celda " + filaCeldaJaque + " " + columnaCeldaJaque).GetComponent<Celda>();

            if(celdaObjetivo!=null){if(celdabloqueo != celdaObjetivo){continue;}}

            if(celdaUnidad.GetUnidadEnCelda().MovimientoDirecto == false)
            {

                if(DireccionUnidad(celdabloqueo,celdaUnidad,celdaUnidad.GetUnidadEnCelda(),false,false,false,false) == true)
                {

                    return true;

                }

            }
            else
            {

                if(DireccionDirectaUnidad(celdabloqueo,celdaUnidad,celdaUnidad.GetUnidadEnCelda(),false,false,false) == true)
                {

                    return true;

                }

            }

        }

    return false;

    }

    /*

         ██╗ █████╗  ██████╗ ██╗   ██╗███████╗███╗   ███╗ █████╗ ████████╗███████╗
         ██║██╔══██╗██╔═══██╗██║   ██║██╔════╝████╗ ████║██╔══██╗╚══██╔══╝██╔════╝
         ██║███████║██║   ██║██║   ██║█████╗  ██╔████╔██║███████║   ██║   █████╗  
    ██   ██║██╔══██║██║▄▄ ██║██║   ██║██╔══╝  ██║╚██╔╝██║██╔══██║   ██║   ██╔══╝  
    ╚█████╔╝██║  ██║╚██████╔╝╚██████╔╝███████╗██║ ╚═╝ ██║██║  ██║   ██║   ███████╗
     ╚════╝ ╚═╝  ╚═╝ ╚══▀▀═╝  ╚═════╝ ╚══════╝╚═╝     ╚═╝╚═╝  ╚═╝   ╚═╝   ╚══════╝

    */

    public void JaqueMateCalculo(Unidad unidadRey, Celda celdaRey)
    {

        // El rey no tiene movimientos disponibles
        if(CalculoMovimiento(Turnos.GetJugadorActual().reyCelda.GetUnidadEnCelda(),Turnos.GetJugadorActual().reyCelda,true,false) == 0)
        {

            foreach(Jugador jugadores in Turnos.Jugadores)
            {

                if(jugadores == Turnos.GetJugadorActual())
                {

                    // La unidad que amenaza al rey no esta amenazada
                    if(JaqueCalculo(jugadores.GetComponent<MateJaque>().jaqueMovimiento.celdaJaque,true,false,null,null) == true)
                    {

                            // No hay ninguna unidad que pueda bloquear el jaque
                            if(JaqueBloqueoCalculo(jugadores.GetComponent<MateJaque>().jaqueMovimiento.celdaJaque,Turnos.GetJugadorActual().reyCelda, 
                            null) == false)
                            {

                            Debug.Log("JAQUEMATE");

                            }

                    }

                }

            }

        }
  
       ReiniciarCalculo();

    }

    /*
        
    ██████╗ ███████╗██╗   ██╗     █████╗ ██╗  ██╗ ██████╗  ██████╗  █████╗ ██████╗  ██████╗ 
    ██╔══██╗██╔════╝╚██╗ ██╔╝    ██╔══██╗██║  ██║██╔═══██╗██╔════╝ ██╔══██╗██╔══██╗██╔═══██╗
    ██████╔╝█████╗   ╚████╔╝     ███████║███████║██║   ██║██║  ███╗███████║██║  ██║██║   ██║
    ██╔══██╗██╔══╝    ╚██╔╝      ██╔══██║██╔══██║██║   ██║██║   ██║██╔══██║██║  ██║██║   ██║
    ██║  ██║███████╗   ██║       ██║  ██║██║  ██║╚██████╔╝╚██████╔╝██║  ██║██████╔╝╚██████╔╝
    ╚═╝  ╚═╝╚══════╝   ╚═╝       ╚═╝  ╚═╝╚═╝  ╚═╝ ╚═════╝  ╚═════╝ ╚═╝  ╚═╝╚═════╝  ╚═════╝ 
                                                                                            
    */

    //No esta en jaque Y no hay ninguna unidad (rey incluido) que pueda hacer movimientos ==> TABLAS
    public void ReyAhogado()
    {

        // bool sinmovimientos = true;

        Celda[] celdas = cuadricula.GetCeldas();

        foreach(Jugador jugadores in Turnos.Jugadores)
        {
            int numMovimentos = 0;
            for (int i = 0; i < (cuadricula.filas*cuadricula.columnas); i++) 
            {

                if(celdas[i].GetUnidadEnCelda()==null){continue;}
                if(celdas[i].GetUnidadEnCelda().UnidadJugador != jugadores){continue;}
          

                // if(celdas[i].GetUnidadEnCelda().UnidadJugador == Turnos.GetJugadorActual()){

                    // Debug.Log(celdas[i].GetUnidadEnCelda() + " " + CalculoMovimiento(celdas[i].GetUnidadEnCelda(),celdas[i],false,true));
                    numMovimentos += CalculoMovimiento(celdas[i].GetUnidadEnCelda(),celdas[i],false,true);

                    // {

                    //     sinmovimientos = false;
                    //     break;

                    // }
                // }

            }
                // Debug.Log(jugadores.gameObject.name + " " + numMovimentos);
                if(numMovimentos==0)
                {
                    Debug.Log("Rey Ahogado TABLAS");
                    break;
                }
        }

     
        ReiniciarCalculo();

    }

    /*

     █████╗ ███████╗██╗ ██████╗ ███╗   ██╗ █████╗ ██████╗ 
    ██╔══██╗██╔════╝██║██╔════╝ ████╗  ██║██╔══██╗██╔══██╗
    ███████║███████╗██║██║  ███╗██╔██╗ ██║███████║██████╔╝
    ██╔══██║╚════██║██║██║   ██║██║╚██╗██║██╔══██║██╔══██╗
    ██║  ██║███████║██║╚██████╔╝██║ ╚████║██║  ██║██║  ██║
    ╚═╝  ╚═╝╚══════╝╚═╝ ╚═════╝ ╚═╝  ╚═══╝╚═╝  ╚═╝╚═╝  ╚═╝
            
    */

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

    public void NuevoAtaque(Unidad unidad,Direccion direccion,int nuevoAtaque)
    {

        List<Unidad.limiteDireccionAtaque> limDirAtaq = unidad.limiteDirAtaq;

        for (int i = 0; i < limDirAtaq.Count; i++)
        {
           
            switch(direccion)
            {

                case Direccion.Norte:
                {if(limDirAtaq[i].direccionUnidad == Direccion.Norte)
                {
                    limDirAtaq[i].limiteAtaque = nuevoAtaque;}break;
                }
                case Direccion.NorEste:
                {if(limDirAtaq[i].direccionUnidad == Direccion.NorEste)
                {
                    limDirAtaq[i].limiteAtaque = nuevoAtaque;}break;
                }
                case Direccion.Este:
                {if(limDirAtaq[i].direccionUnidad == Direccion.Este)
                {
                    limDirAtaq[i].limiteAtaque = nuevoAtaque;}break;
                }
                case Direccion.SurEste:
                {if(limDirAtaq[i].direccionUnidad == Direccion.SurEste)
                {
                    limDirAtaq[i].limiteAtaque = nuevoAtaque;}break;
                }
                case Direccion.Sur:
                {if(limDirAtaq[i].direccionUnidad == Direccion.Sur)
                {
                    limDirAtaq[i].limiteAtaque = nuevoAtaque;}break;
                }
                case Direccion.SurOeste:
                {if(limDirAtaq[i].direccionUnidad == Direccion.SurOeste)
                {
                    limDirAtaq[i].limiteAtaque = nuevoAtaque;}break;
                }
                case Direccion.Oeste:
                {if(limDirAtaq[i].direccionUnidad == Direccion.Oeste)
                {
                    limDirAtaq[i].limiteAtaque = nuevoAtaque;}break;
                }
                case Direccion.NorOeste:
                {if(limDirAtaq[i].direccionUnidad == Direccion.NorOeste)
                {
                    limDirAtaq[i].limiteAtaque = nuevoAtaque;}break;
                }

            }

        }

    }

    /*


    ██████╗ ██████╗  ██████╗ ███╗   ███╗ ██████╗  ██████╗██╗ ██████╗ ███╗   ██╗
    ██╔══██╗██╔══██╗██╔═══██╗████╗ ████║██╔═══██╗██╔════╝██║██╔═══██╗████╗  ██║
    ██████╔╝██████╔╝██║   ██║██╔████╔██║██║   ██║██║     ██║██║   ██║██╔██╗ ██║
    ██╔═══╝ ██╔══██╗██║   ██║██║╚██╔╝██║██║   ██║██║     ██║██║   ██║██║╚██╗██║
    ██║     ██║  ██║╚██████╔╝██║ ╚═╝ ██║╚██████╔╝╚██████╗██║╚██████╔╝██║ ╚████║
    ╚═╝     ╚═╝  ╚═╝ ╚═════╝ ╚═╝     ╚═╝ ╚═════╝  ╚═════╝╚═╝ ╚═════╝ ╚═╝  ╚═══╝
                                                                            


    */

    public void Promocionado(int unidaDePromo)
    {


                    
        GameObject UnidadObject = Instantiate(cuadricula.UnidadesPrefabs[unidaDePromo],Vector3.zero,Quaternion.identity);
        Unidad UnidadScript = UnidadObject.GetComponent<Unidad>();

        UnidadScript.UnidadJugador = cEnPromocion.GetUnidadEnCelda().UnidadJugador;
        UnidadScript.UnidadJugador.idJugador = cEnPromocion.GetUnidadEnCelda().UnidadJugador.idJugador;

        cEnPromocion.GetUnidadEnCelda().gameObject.SetActive(false);
		

        //if(UnidadScript.UnidadJugador.idJugador!=1) // Blancas
        if(UnidadScript.UnidadJugador.miColor == ColorJugador.Blancas) // Blancas
		{
            

			SpriteRenderer UnidadSpriteRend = UnidadObject.GetComponentsInChildren<SpriteRenderer>()[0];
			UnidadSpriteRend.sprite = cuadricula.UnidadesSprite[unidaDePromo]; // Cambio de negro a blanco

        }

        UnidadScript.SetCelda(cEnPromocion.gameObject.name);



        Transform InterfazPromocion = GameObject.Find("IntPromocion").GetComponent<Transform>();
        InterfazPromocion.gameObject.GetComponent<Renderer>().enabled = false;
            InterfazPromocion.gameObject.GetComponentsInChildren<Renderer>()[1].enabled = false;
            InterfazPromocion.gameObject.GetComponentsInChildren<Renderer>()[2].enabled = false;
            InterfazPromocion.gameObject.GetComponentsInChildren<Renderer>()[3].enabled = false;
            InterfazPromocion.gameObject.GetComponentsInChildren<Renderer>()[4].enabled = false;
            InterfazPromocion.gameObject.GetComponentsInChildren<Collider2D>()[0].enabled = false;
            InterfazPromocion.gameObject.GetComponentsInChildren<Collider2D>()[1].enabled = false;
            InterfazPromocion.gameObject.GetComponentsInChildren<Collider2D>()[2].enabled = false;
            InterfazPromocion.gameObject.GetComponentsInChildren<Collider2D>()[3].enabled = false;

        enPromocion = false;

        
        JaqueCalculo();

    }


    


}
