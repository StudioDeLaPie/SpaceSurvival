using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pression
{
    static public class PressionGestion
    {
        static public float constanteGazParfait = 8.314f;
        static public float temperatureEnKelvin = 298.15f; //25Degré Celsius
        /// <summary>
        /// en Pascal
        /// </summary>
        static public float goalPression = 101325f;

        static public float BarToPascal(float bar)
        {
            return bar * 100000;
        }

        static public float PascalToBar(float pascal)
        {
            return pascal/100000;
        }
    }
}
