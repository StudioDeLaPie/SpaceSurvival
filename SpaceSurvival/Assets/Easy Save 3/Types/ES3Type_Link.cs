using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("firstConnexion", "secondConnexion", "anchor1", "anchor2", "typelink", "enabled")]
	public class ES3Type_Link : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3Type_Link() : base(typeof(Link))
		{
			Instance = this;
		}

		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (Link)obj;
			
			writer.WritePrivateFieldByRef("firstConnexion", instance);
			writer.WritePrivateFieldByRef("secondConnexion", instance);
			writer.WritePrivateFieldByRef("anchor1", instance);
			writer.WritePrivateFieldByRef("anchor2", instance);
			writer.WritePrivateField("typelink", instance);
			writer.WriteProperty("enabled", instance.enabled, ES3Type_bool.Instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (Link)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "firstConnexion":
					reader.SetPrivateField("firstConnexion", reader.Read<Connexion>(), instance);
					break;
					case "secondConnexion":
					reader.SetPrivateField("secondConnexion", reader.Read<Connexion>(), instance);
					break;
					case "anchor1":
					reader.SetPrivateField("anchor1", reader.Read<UnityEngine.Transform>(), instance);
					break;
					case "anchor2":
					reader.SetPrivateField("anchor2", reader.Read<UnityEngine.Transform>(), instance);
					break;
					case "typelink":
					reader.SetPrivateField("typelink", reader.Read<TypeLink>(), instance);
					break;
					case "enabled":
						instance.enabled = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
            instance.Init();
		}
	}

	public class ES3Type_LinkArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3Type_LinkArray() : base(typeof(Link[]), ES3Type_Link.Instance)
		{
			Instance = this;
		}
	}
}