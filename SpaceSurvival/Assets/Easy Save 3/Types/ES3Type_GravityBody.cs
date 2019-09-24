using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("weight", "standUp", "freezeRbRotations", "enabled")]
	public class ES3Type_GravityBody : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3Type_GravityBody() : base(typeof(GravityBody))
		{
			Instance = this;
		}

		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (GravityBody)obj;
			
			writer.WriteProperty("weight", instance.weight, ES3Type_float.Instance);
			writer.WriteProperty("standUp", instance.standUp, ES3Type_bool.Instance);
			writer.WriteProperty("freezeRbRotations", instance.freezeRbRotations, ES3Type_bool.Instance);
			writer.WriteProperty("enabled", instance.enabled, ES3Type_bool.Instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (GravityBody)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "weight":
						instance.weight = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "standUp":
						instance.standUp = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "freezeRbRotations":
						instance.freezeRbRotations = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "enabled":
						instance.enabled = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}

	public class ES3Type_GravityBodyArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3Type_GravityBodyArray() : base(typeof(GravityBody[]), ES3Type_GravityBody.Instance)
		{
			Instance = this;
		}
	}
}