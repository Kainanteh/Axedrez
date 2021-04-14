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

	public TextMeshProUGUI celdaEtiquetaPrefab;

	[SerializeField] private Canvas cuadriculaCanvas;

	public Transform Limite;

	public Color32 ColorNegras;
	public Color32 ColorBlancas;


	public GameObject[] UnidadesPrefabs;
	public Sprite[] UnidadesSprite;

	//	| 0 (fila) * 0 (columna) * 0 (id unidad) * 0 (jugador blanco o negro) |
	private string semilla = 
	"/0*0*2*0/0*1*1*0/0*2*3*0/0*3*5*0/0*4*4*0/0*5*3*0/0*6*1*0/0*7*2*0/1*0*0*0/1*1*0*0/1*2*0*0/1*3*0*0/1*4*0*0/1*5*0*0/1*6*0*0/1*7*0*0/7*0*2*1/7*1*1*1/7*2*3*1/7*3*5*1/7*4*4*1/7*5*3*1/7*6*1*1/7*7*2*1/6*0*0*1/6*1*0*1/6*2*0*1/6*3*0*1/6*4*0*1/6*5*0*1/6*6*0*1/6*7*0*1";

	

	public Celda[] GetCeldas()
	{

		return celdas;

	}

	void Awake () 
	{

		
		

		cuadriculaCanvas.transform.position = new Vector2(((float)(columnas-1)/2)*-1,((float)(filas-1)/2)*-1); // Para centrar la cuadricula
		Limite.localScale = new Vector3((float)columnas+0.5f,(float)filas+0.5f,0f); 

		celdas = new Celda[filas * columnas];

		for (int y = 0, i = 0; y < filas; y++) 
		{
			for (int x = 0; x < columnas; x++) 
			{
				CrearCelda(x, y, i++);
			}
		}
		
		

	}
	void Start () 
	{

		Semillero();

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
		//etiqueta.rectTransform.SetParent(cuadriculaCanvas.transform, false); // Padre la Cuadricula
		etiqueta.rectTransform.SetParent(celda.transform, false); // Padre la Celda
		//etiqueta.rectTransform.anchoredPosition =
		//	new Vector2(cuadriculaCanvas.transform.position.x, cuadriculaCanvas.transform.position.y);
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

			if(unidad.Split('*')[3]=="1")
			{

				SpriteRenderer UnidadSpriteRend = UnidadObject.GetComponentsInChildren<SpriteRenderer>()[0];
				UnidadSpriteRend.sprite = UnidadesSprite[int.Parse(unidad.Split('*')[2])]; // Cambio de negro a blanco

			}

		 }

		

	}

}
