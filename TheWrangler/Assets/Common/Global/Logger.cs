using Unity.VisualScripting;
using UnityEngine;
using System;
using System.Diagnostics;

public class Logger
{
    private string loggerName;
    public Logger(string _name)
    {
        this.loggerName = _name; 
    }

    public void Warn(string msg)
    {
        UnityEngine.Debug.LogWarning($"{this.loggerName} - WARN - {FormattedDateTime()} - {msg}");
    }

    public void Debug(string msg)
    {
        UnityEngine.Debug.Log($"{this.loggerName} - DEBUG - {FormattedDateTime()} - {msg}");
    }

    public void Info(string msg)
    {
        UnityEngine.Debug.Log($"{this.loggerName} - INFO - {FormattedDateTime()} - {msg}");

    }

    public void Error(string msg)
    {
        UnityEngine.Debug.LogError($"{this.loggerName} - ERROR - {FormattedDateTime()} - {msg}");

    }

    public string FormattedDateTime()
    {
        return DateTime.Now.ToString();
    }
}
