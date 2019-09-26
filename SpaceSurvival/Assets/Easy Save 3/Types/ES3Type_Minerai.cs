using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("data")]
	public class ES3Type_Minerai : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3Type_Minerai() : base(typeof(Minerai))
		{
			Instance = this;
		}

		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (Minerai)obj;
			
			writer.WritePropertyByRef("data", instance.data);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (Minerai)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "data":
						instance.data = reader.Read<Recoltable_SO>();
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}

	public class ES3Type_MineraiArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3Type_MineraiArray() : base(typeof(Minerai[]), ES3Type_Minerai.Instance)
		{
			Instance = this;
		}
	}
}