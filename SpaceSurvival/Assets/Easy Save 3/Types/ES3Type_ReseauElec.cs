using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("generateurElecs", "consoElecs", "batteries", "allEngins", "actif", "etatFonctionnementReseau")]
	public class ES3Type_ReseauElec : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3Type_ReseauElec() : base(typeof(ReseauElec))
		{
			Instance = this;
		}

		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (ReseauElec)obj;
			
			writer.WritePrivateField("generateurElecs", instance);
			writer.WritePrivateField("consoElecs", instance);
			writer.WritePrivateField("batteries", instance);
			writer.WritePrivateField("allEngins", instance);
			writer.WriteProperty("actif", instance.actif, ES3Type_bool.Instance);
			writer.WritePrivateField("etatFonctionnementReseau", instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (ReseauElec)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "generateurElecs":
					reader.SetPrivateField("generateurElecs", reader.Read<System.Collections.Generic.List<GenerateurElec>>(), instance);
					break;
					case "consoElecs":
					reader.SetPrivateField("consoElecs", reader.Read<System.Collections.Generic.List<ConsoElec>>(), instance);
					break;
					case "batteries":
					reader.SetPrivateField("batteries", reader.Read<System.Collections.Generic.List<BatterieElec>>(), instance);
					break;
					case "allEngins":
					reader.SetPrivateField("allEngins", reader.Read<System.Collections.Generic.List<EnginElec>>(), instance);
					break;
					case "actif":
						instance.actif = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "etatFonctionnementReseau":
					reader.SetPrivateField("etatFonctionnementReseau", reader.Read<System.Boolean>(), instance);
					break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}

	public class ES3Type_ReseauElecArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3Type_ReseauElecArray() : base(typeof(ReseauElec[]), ES3Type_ReseauElec.Instance)
		{
			Instance = this;
		}
	}
}