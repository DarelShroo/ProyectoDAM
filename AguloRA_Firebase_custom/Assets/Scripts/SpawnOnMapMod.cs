using System.IO;
using System.Linq;

namespace Mapbox.Examples
{
	using UnityEngine;
	using Mapbox.Utils;
	using Mapbox.Unity.Map;
	using Mapbox.Unity.Utilities;
	using System.Collections.Generic;

	public class SpawnOnMapMod : MonoBehaviour
	{
		[SerializeField]
		AbstractMap _map;

		//[Geocode]
		//string[] _locationStrings;
		[SerializeField]
		Vector2d[] _locations;

		//[SerializeField]
		//float _spawnScale = 100f;

		[SerializeField]
		GameObject _markerPrefab;

		[SerializeField]
		List<GameObject> _spawnedObjects;
		
		[SerializeField]
		public Material lugares;
		
		[SerializeField]
		public Material personajes;
		
		[SerializeField]
		public Material arquitectura;
		
		[SerializeField]
		public Material historiaaborigen;
		
		[SerializeField]
		public Material tradiciones;
		
		[SerializeField]
		public Material visitado;

		public GameObject imgCarga;
		void Start()
		{
			Parada p = new Parada();
			_locations = new Vector2d[Paradas.instance.listaParadas.Count];
			_spawnedObjects = new List<GameObject>();

			
			string path = Application.persistentDataPath+"/paradasVisitadas.txt";

			if (!File.Exists(path))
			{
				File.Create(path);
			}
			
			var lineas = File.ReadLines(path);
			var enumerable = lineas.ToList();
			
			for (int i = 0; i < Paradas.instance.listaParadas.Count; i++)
			{
				p = (Parada) Paradas.instance.listaParadas[i];
				var locationString = p.Coordenadas;
				_locations[i] = Conversions.StringToLatLon(locationString);
				if (p.Visible)
				{
					var instance = Instantiate(_markerPrefab);
					instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i], true);
					instance.GetComponentInChildren<TextMesh>().text = p.Nombre.Length > 14 ? addSaltoLinea(p.Nombre) : p.Nombre;
					var canvas = instance.transform.GetChild(1);
					var cube = canvas.GetChild(0);
					
					switch (p.Tipo)
						{
							case "lugares":
								comprueba(enumerable, cube, p.Nombre, lugares);
								break;
							case "personajes":
								comprueba(enumerable, cube, p.Nombre, personajes);
								break;
							case "arquitectura":
								comprueba(enumerable, cube, p.Nombre, arquitectura);
								break;
							case "historiaaborigen":
								comprueba(enumerable, cube, p.Nombre, historiaaborigen);
								break;
							case "tradiciones":
								comprueba(enumerable, cube, p.Nombre, tradiciones);
								break;
						}

					//instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
					_spawnedObjects.Add(instance);
				}
			}
		}

		private string addSaltoLinea(string nombre)
		{
			int tamanioCadena = nombre.Length;
			string temp = "";
			for (int i = 0; i < tamanioCadena; i++)
				{
					temp += nombre[i];
					if (i > 14 && nombre[i] == ' ')
					{
						temp += "\n";
					}
				}
			return temp;
		}

		private void comprueba(List<string> enumerable, Transform cube, string nombre, Material material)
		{
			
			if (enumerable.Contains(nombre))
			{
				cube.GetComponent<MeshRenderer>().material = visitado;
			}
			else
			{
				cube.GetComponent<MeshRenderer>().material = material;
			}
		}

		private void Update()
		{
			int count = _spawnedObjects.Count;
			for (int i = 0; i < count; i++)
			{
				var spawnedObject = _spawnedObjects[i];
				var location = _locations[i];
				spawnedObject.transform.localPosition = _map.GeoToWorldPosition(location, true);
				//spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
			}
			imgCarga.SetActive(false);
		}
	}
}