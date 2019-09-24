using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("velocity", "isKinematic", "constraints")]
	public class ES3Type_Rigidbody : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3Type_Rigidbody() : base(typeof(UnityEngine.Rigidbody))
		{
			Instance = this;
		}

		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (UnityEngine.Rigidbody)obj;
			
			writer.WriteProperty("velocity", instance.velocity, ES3Type_Vector3.Instance);
			writer.WriteProperty("isKinematic", instance.isKinematic, ES3Type_bool.Instance);
			writer.WriteProperty("constraints", instance.constraints);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (UnityEngine.Rigidbody)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "velocity":
						instance.velocity = reader.Read<UnityEngine.Vector3>(ES3Type_Vector3.Instance);
						break;
					case "isKinematic":
						instance.isKinematic = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "constraints":
						instance.constraints = reader.Read<UnityEngine.RigidbodyConstraints>();
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}

	public class ES3Type_RigidbodyArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3Type_RigidbodyArray() : base(typeof(UnityEngine.Rigidbody[]), ES3Type_Rigidbody.Instance)
		{
			Instance = this;
		}
	}
}