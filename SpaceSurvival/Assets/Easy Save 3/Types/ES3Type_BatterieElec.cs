using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("quantiteElectricite", "canRecharge", "canConsume", "reseauMaitre", "ON_OffElec", "Fonctionnel")]
	public class ES3Type_BatterieElec : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3Type_BatterieElec() : base(typeof(BatterieElec))
		{
			Instance = this;
		}

		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (BatterieElec)obj;
			
			writer.WritePrivateField("quantiteElectricite", instance);
			writer.WritePrivateField("canRecharge", instance);
			writer.WritePrivateField("canConsume", instance);
			writer.WritePropertyByRef("reseauMaitre", instance.reseauMaitre);
			writer.WriteProperty("ON_OffElec", instance.ON_OffElec, ES3Type_bool.Instance);
			writer.WriteProperty("Fonctionnel", instance.Fonctionnel, ES3Type_bool.Instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (BatterieElec)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "quantiteElectricite":
					reader.SetPrivateField("quantiteElectricite", reader.Read<System.Single>(), instance);
					break;
					case "canRecharge":
					reader.SetPrivateField("canRecharge", reader.Read<System.Boolean>(), instance);
					break;
					case "canConsume":
					reader.SetPrivateField("canConsume", reader.Read<System.Boolean>(), instance);
					break;
					case "reseauMaitre":
						instance.reseauMaitre = reader.Read<ReseauElec>(ES3Type_ReseauElec.Instance);
						break;
					case "ON_OffElec":
						instance.ON_OffElec = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "Fonctionnel":
						instance.Fonctionnel = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}

	public class ES3Type_BatterieElecArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3Type_BatterieElecArray() : base(typeof(BatterieElec[]), ES3Type_BatterieElec.Instance)
		{
			Instance = this;
		}
	}
}