using System.Collections.Generic;
using UnityEngine;
using System.IO;

public enum LogLevel
{
    ERROR = 0,
    WARN = 1,
    DEBUG = 2,
    INFO = 3,
}

public class LogManager : MonoBehaviour
{
    public static LogManager Instance;

    [SerializeField] private string logFile = "TheWrangler.log";

    private string path;

    private Dictionary<string, Logger> loggers = new Dictionary<string, Logger>();

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Found more than one Game Events Manager in the scene. Destroying the newest one");
            Destroy(this.gameObject);
            return;
        }
        Instance = this;

        path = Path.Combine(Application.persistentDataPath, logFile);

        File.Delete(path);
    }

    public Logger AddLogger(string name, LogLevel level)
    {
        if (!loggers.ContainsKey(name))
        {
            var logger = new Logger(name, level, this);
            loggers.Add(name, logger);
        }
        return loggers[name];
    }

    public void Write(string formattedLog, LogLevel level)
    {
        switch (level)
        {
            case LogLevel.INFO:
            case LogLevel.DEBUG:
                Debug.Log(formattedLog);
                break;
            case LogLevel.WARN:
                Debug.LogWarning(formattedLog);
                break;
            case LogLevel.ERROR:
                Debug.LogError(formattedLog);
                break;
        }

        File.AppendAllText(path, formattedLog + "\r\n");
    }

}