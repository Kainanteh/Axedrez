using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Cuadricula : MonoBehaviour
{

	public int filas = 6;
    public int columnas = 6;
	
	


	public Celda celdaPrefab;
    public Celda[] celdas;
	public List<Unidad> unidades;

	public TextMeshProUGUI celdaEtiquetaPrefab;

	[SerializeField] private Canvas cuadriculaCanvas;

	public Transform Limite;
	// public RectTransform Interfaz;

	public Color32 ColorNegras;
	public Color32 ColorBlancas;


	public GameObject[] UnidadesPrefabs;
	public Sprite[] UnidadesSprite;

	//	| 0 (fila) * 0 (columna) * 0 (id unidad) * 0 (jugador blanco o negro) |
	private string semilla = 
	// "/0*0*2*1/0*1*1*1/0*2*3*1/0*3*5*1/0*4*4*1/0*5*3*1/0*6*1*1/0*7*2*1/1*0*0*1/1*1*0*1/1*2*0*1/1*3*0*1/1*4*0*1/1*5*0*1/1*6*0*1/1*7*0*1/7*0*2*2/7*1*1*2/7*2*3*2/7*3*5*2/7*4*4*2/7*5*3*2/7*6*1*2/7*7*2*2/6*0*0*2/6*1*0*2/6*2*0*2/6*3*0*2/6*4*0*2/6*5*0*2/6*6*0*2/6*7*0*2";
	// "/1*1*5*1/2*1*0*1/4*1*0*2/5*1*5*2"; // 2 peones 2 reyes
	//"/2*0*5*1/5*1*2*2/3*1*2*1/7*7*5*2/"; // Posicion bloqueo jaque 1 rey 2 torres
	"/2*0*5*1/5*1*0*1/3*1*0*2/7*7*5*2/";


	/*

		Rey 	-> 5	
		Reina 	-> 4
		Alfil 	-> 3
		Torre 	-> 2
		Caballo -> 1
		Peon 	-> 0

	*/

	public Celda[] GetCeldas()
	{

		return celdas;

	}

	void Awake () 
	{

		
		

		cuadriculaCanvas.transform.position = new Vector2(((float)(columnas-1)/2)*-1,((float)(filas-1)/2)*-1); // Para centrar la cuadricula
		
		// Los limites se calculan segun el tablero
		Limite.localScale = new Vector3((float)columnas+0.5f,(float)filas+0.5f,0f);
		// Interfaz.localScale = new Vector3((float)columnas+5f,(float)filas+0.5f,0f);
		
		BoxCollider2D PantallaLimiteOeste 	= GameObject.Find("PantallaLimite").GetComponents<BoxCollider2D>()[0];
		BoxCollider2D PantallaLimiteNorte 	= GameObject.Find("PantallaLimite").GetComponents<BoxCollider2D>()[1];
		BoxCollider2D PantallaLimiteEste 	= GameObject.Find("PantallaLimite").GetComponents<BoxCollider2D>()[2];
		BoxCollider2D PantallaLimiteSur 	= GameObject.Find("PantallaLimite").GetComponents<BoxCollider2D>()[3];

		PantallaLimiteOeste.offset 	= new Vector2((float)columnas*-1,PantallaLimiteOeste.offset.y);
		PantallaLimiteOeste.size	= new Vector2((float)columnas,(float)filas);
		PantallaLimiteNorte.offset 	= new Vector2(PantallaLimiteNorte.offset.x,(float)filas);
		PantallaLimiteNorte.size	= new Vector2((float)columnas+16f,(float)filas);
		PantallaLimiteEste.offset 	= new Vector2((float)columnas,PantallaLimiteEste.offset.y);
		PantallaLimiteEste.size		= new Vector2((float)columnas,(float)filas);
		PantallaLimiteSur.offset 	= new Vector2(PantallaLimiteSur.offset.x,(float)filas*-1);
		PantallaLimiteSur.size		= new Vector2((float)columnas+16f,(float)filas);
	

		celdas = new Celda[filas * columnas];

		for (int y = 0, i = 0; y < filas; y++) 
		{
			for (int x = 0; x < columnas; x++) 
			{
				CrearCelda(x, y, i++);
			}
		}
		
		Semillero();

	}
	void Start () 
	{

		

	}
	
	void CrearCelda (int x, int y, int i) 
	{
		Vector2 position;
		position.y = y * 1f;
		position.x = x * 1f;
		

		Celda celda = celdas[i] = Instantiate<Celda>(celdaPrefab);
		celda.transform.SetParent(cuadriculaCanvas.transform, false);
		celda.transform.localPosition = position;
		celda.gameObject.name = "Celda " + y.ToString() + " " + x.ToString(); 
		celda.fila = y;
		celda.columna = x;
		
		if(y == 0 || y == filas-1){celda.celdaPromocion = true;}
	
		if((x + y)%2 == 0 ) // Si es par color "negro" si es impar color "blanco"
		{
			//celda.gameObject.GetComponent<SpriteRenderer>().color = new Color32(144, 104, 62,255);//90683E
			celda.gameObject.GetComponent<SpriteRenderer>().color = ColorNegras;
		}
		else
		{
			//celda.gameObject.GetComponent<SpriteRenderer>().color = new Color32(222, 161, 98,255);//DEA162
			celda.gameObject.GetComponent<SpriteRenderer>().color = ColorBlancas;
		}

		
		
		
		

		TextMeshProUGUI etiqueta = Instantiate<TextMeshProUGUI>(celdaEtiquetaPrefab);
		etiqueta.rectTransform.SetParent(celda.transform, false); // Padre la Celda
		etiqueta.text = y.ToString() + " " + x.ToString();

	}

	/*

	███████╗███████╗███╗   ███╗██╗██╗     ██╗     ███████╗██████╗  ██████╗ 
	██╔════╝██╔════╝████╗ ████║██║██║     ██║     ██╔════╝██╔══██╗██╔═══██╗
	███████╗█████╗  ██╔████╔██║██║██║     ██║     █████╗  ██████╔╝██║   ██║
	╚════██║██╔══╝  ██║╚██╔╝██║██║██║     ██║     ██╔══╝  ██╔══██╗██║   ██║
	███████║███████╗██║ ╚═╝ ██║██║███████╗███████╗███████╗██║  ██║╚██████╔╝
	╚══════╝╚══════╝╚═╝     ╚═╝╚═╝╚══════╝╚══════╝╚══════╝╚═╝  ╚═╝ ╚═════╝ 
																		

	*/

	void Semillero()
	{

		string[] unidadsemilla = semilla.Split('/');
		

		 foreach (string unidad in unidadsemilla)
		 {
			if(unidad==""){continue;}

			// Debug.Log("Fila: " +unidad.Split('*')[0] + " Columna: " + unidad.Split('*')[1] + " Id unidad " 
			// + UnidadesPrefabs[int.Parse(unidad.Split('*')[2])].name + " Jugador " + unidad.Split('*')[3] );

			GameObject UnidadObject = Instantiate(UnidadesPrefabs[int.Parse(unidad.Split('*')[2])],Vector3.zero,Quaternion.identity);

			Unidad UnidadScript = UnidadObject.GetComponent<Unidad>();

			UnidadScript.SetCelda("Celda " + unidad.Split('*')[0] + " " + unidad.Split('*')[1]);

			UnidadScript.UnidadJugador = GameObject.Find("Jugador "+int.Parse(unidad.Split('*')[3])).GetComponent<Jugador>();
			UnidadScript.UnidadJugador.idJugador = int.Parse(unidad.Split('*')[3]);

			if(unidad.Split('*')[3]=="2")
			{

				SpriteRenderer UnidadSpriteRend = UnidadObject.GetComponentsInChildren<SpriteRenderer>()[0];
				UnidadSpriteRend.sprite = UnidadesSprite[int.Parse(unidad.Split('*')[2])]; // Cambio de negro a blanco

				if(UnidadScript.GetTipoUnidad()==TipoUnidad.Peon) // Los peones blancos tienen que moverse S y atacar SE SO
				{

				this.GetComponent<UnidadMovimiento>().NuevoMovimiento(UnidadScript,Direccion.Norte,0); 
				this.GetComponent<UnidadMovimiento>().NuevoMovimiento(UnidadScript,Direccion.Sur,2);
				this.GetComponent<UnidadMovimiento>().NuevoAtaque(UnidadScript,Direccion.NorEste,0);
				this.GetComponent<UnidadMovimiento>().NuevoAtaque(UnidadScript,Direccion.NorOeste,0);
				this.GetComponent<UnidadMovimiento>().NuevoAtaque(UnidadScript,Direccion.SurEste,1);
				this.GetComponent<UnidadMovimiento>().NuevoAtaque(UnidadScript,Direccion.SurOeste,1);

				}

			}

			if(UnidadScript.GetTipoUnidad()==TipoUnidad.Rey && unidad.Split('*')[3]=="2")
			{UnidadScript.UnidadJugador.reyCelda = GameObject.Find("Celda " + unidad.Split('*')[0] + " " + unidad.Split('*')[1])
			.GetComponent<Celda>();}
			else if(UnidadScript.GetTipoUnidad()==TipoUnidad.Rey && unidad.Split('*')[3]=="1")
			{UnidadScript.UnidadJugador.reyCelda = GameObject.Find("Celda " + unidad.Split('*')[0] + " " + unidad.Split('*')[1])
			.GetComponent<Celda>();}

			unidades.Add(UnidadScript);

		 }

	}

}
