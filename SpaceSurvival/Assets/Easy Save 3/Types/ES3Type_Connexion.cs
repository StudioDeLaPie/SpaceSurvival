using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("connexions_links")]
	public class ES3Type_Connexion : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3Type_Connexion() : base(typeof(Connexion))
		{
			Instance = this;
		}

		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (Connexion)obj;
			
			writer.WriteProperty("connexions_links", instance.connexions_links);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (Connexion)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "connexions_links":
						instance.connexions_links = reader.Read<System.Collections.Generic.Dictionary<Connexion, Link>>();
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}

	public class ES3Type_ConnexionArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3Type_ConnexionArray() : base(typeof(Connexion[]), ES3Type_Connexion.Instance)
		{
			Instance = this;
		}
	}
}