using System.Threading;

namespace Mapbox.Examples
{
	using UnityEngine;
	using Mapbox.Utils;
	using Mapbox.Unity.Map;
	using Mapbox.Unity.MeshGeneration.Factories;
	using Mapbox.Unity.Utilities;
	using System.Collections.Generic;

	public class SpawnOnMap : MonoBehaviour
	{
		[SerializeField]
		AbstractMap _map;

		[SerializeField]
		//[Geocode]
		//string[] _locationStrings;
		Vector2d[] _locations;

		//[SerializeField]
		//float _spawnScale = 100f;

		[SerializeField]
		GameObject _markerPrefab;

		List<GameObject> _spawnedObjects;

		void Start()
		{
			Parada p = new Parada();
			_locations = new Vector2d[Paradas.instance.listaParadas.Count];
			_spawnedObjects = new List<GameObject>();
			for (int i = 0; i < Paradas.instance.listaParadas.Count; i++)
			{
				p = (Parada) Paradas.instance.listaParadas[i];
				var locationString = p.Coordenadas;
				_locations[i] = Conversions.StringToLatLon(locationString);
				if (p.Visible)
				{
					var instance = Instantiate(_markerPrefab);
					instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i], true);
					var texto = instance.GetComponentInChildren<TextMesh>();
					var nombre = addSaltoLinea(p.Nombre);
					
					texto.text = nombre;
					Thread.Sleep(1);
					//instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
					_spawnedObjects.Add(instance);
				}
			}
		}
		
		
		private string addSaltoLinea(string nombre)
		{
			int tamanioCadena = nombre.Length;
			string temp = "";
			 
			if (tamanioCadena > 10)
			{
				
				for (int i = 0; i < tamanioCadena; i++)
				{
					temp += nombre[i];
					if (i > 10 && nombre[i] == ' ')
					{
						temp += "\n";
					}
				}
			}

			return temp;
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
		}
	}
}