using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class InputCustomMethods : MonoBehaviour
{
    public static float timeToHold = 1;
    public List<string> holdableAxis;

    private static Dictionary<string, float> holdTime;

    // Start is called before the first frame update
    void Start()
    {
        holdTime = new Dictionary<string, float>();
        foreach (string str in holdableAxis)
        {
            holdTime.Add(str, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Dictionary<string, float> keys = new Dictionary<string, float>(holdTime);
        foreach (KeyValuePair<string, float> btn in keys)
        {
            if ((btn.Value == 0 && Input.GetButtonDown(btn.Key)) || Input.GetButton(btn.Key))
            {
                holdTime[btn.Key] += Time.deltaTime;
            }

            if (Input.GetButtonUp(btn.Key))
                holdTime[btn.Key] = 0;
        }
    }

    public static bool GetButtonHold(string btn)
    {
        return holdTime[btn] >= timeToHold;
    }
}
