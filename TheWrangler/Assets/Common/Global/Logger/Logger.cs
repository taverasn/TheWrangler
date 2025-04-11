using System;

public class Logger
{
    private string loggerName;
    private LogManager logManager;
    private LogLevel level;
    public Logger(string _name, LogLevel _level, LogManager _logManager)
    {
        this.loggerName = _name; 
        this.logManager = _logManager;
        this.level = _level;
    }

    private void Write(string msg, LogLevel _level)
    {
        if (_level > level)
            return;

        string formattedMsg = $"{FormattedDateTime()} - {_level.ToString()} - {this.loggerName} - {msg}";
        logManager.Write(formattedMsg, _level);
    }

    public void Error(string msg)
    {
        Write(msg, LogLevel.ERROR);
    }

    public void Warn(string msg)
    {
        Write(msg, LogLevel.WARN);
    }

    public void Debug(string msg)
    {
        Write(msg, LogLevel.DEBUG);
    }

    public void Info(string msg)
    {
        Write(msg, LogLevel.INFO);
    }

    public string FormattedDateTime()
    {
        return $"[{DateTime.Now.ToString()}]";
    }
}
