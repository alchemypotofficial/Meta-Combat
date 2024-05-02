using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log
{
    private static List<string> messages = new List<string>();

    public static void Normal(string label, string message)
    {
        string combinedMessage = label + ": " + message;
        messages.Add(combinedMessage);

        Debug.Log(combinedMessage);
    }

    public static void Stage(string label, string message)
    {
        string combinedMessage = "<color=yellow>" + label + "</color>: " + message;
        messages.Add(combinedMessage);

        Debug.Log(combinedMessage);
    }

    public static void Error(string message)
    {
        string combinedMessage = "<color=red>Error</color>: " + message;
        messages.Add(combinedMessage);

        Debug.LogError(combinedMessage);
    }

    public static void Warning(string message)
    {
        string combinedMessage = "<color=orange>Warning</color>: " + message;
        messages.Add(combinedMessage);

        Debug.LogWarning(combinedMessage);
    }

    public static void Alert(string message)
    {
        string combinedMessage = "<color=green>Alert</color>: " + message;
        messages.Add(combinedMessage);

        Debug.Log(combinedMessage);
    }
}
