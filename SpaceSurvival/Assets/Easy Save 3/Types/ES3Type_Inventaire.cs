using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("recoltables")]
	public class ES3Type_Inventaire : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3Type_Inventaire() : base(typeof(Inventaire))
		{
			Instance = this;
		}

		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (Inventaire)obj;
			
			writer.WriteProperty("recoltables", instance.recoltables);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (Inventaire)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "recoltables":
						instance.recoltables = reader.Read<System.Collections.Generic.List<Recoltable>>();
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}

	public class ES3Type_InventaireArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3Type_InventaireArray() : base(typeof(Inventaire[]), ES3Type_Inventaire.Instance)
		{
			Instance = this;
		}
	}
}