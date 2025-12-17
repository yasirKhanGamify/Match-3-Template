using Unity.VisualScripting;
using UnityEngine;

public class DebugLogToggle : MonoBehaviour
{
    [Header("Master Switch")]
    [SerializeField] private bool enableAllLog = true;

    [Header("Log Types")]
    [SerializeField] private bool enableNormal = true;
    [SerializeField] private bool enableWarning = true;
    [SerializeField] private bool enableError = true;

    private ILogHandler _defaultHandler;

    void Awake()
    {
        _defaultHandler = Debug.unityLogger.logHandler;

        Debug.Log("DebugLog Awake");
        if (!enableAllLog)
        {
            Debug.unityLogger.logEnabled = false;
            Debug.LogWarning("All Logs are Disabled");
        }
        else
        {
            Debug.unityLogger.logEnabled = true;
            Debug.unityLogger.logHandler = new CustomLogHandler(
                _defaultHandler,
                enableNormal,
                enableWarning,
                enableError
            );
        }

        // Test log
        Debug.Log("DebugLogToggle initialized", this);
    }

    /// <summary>
    /// Apply changes at runtime (toggle logs)
    /// </summary>
    public void Apply()
    {
        Debug.unityLogger.logEnabled = enableAllLog;

        if (enableAllLog)
        {
            Debug.unityLogger.logHandler = new CustomLogHandler(
                _defaultHandler,
                enableNormal,
                enableWarning,
                enableError
            );
        }
    }

    private class CustomLogHandler : ILogHandler
    {
        private readonly ILogHandler _defaultHandler;
        private readonly bool _enableNormal;
        private readonly bool _enableWarning;
        private readonly bool _enableError;

        // Colors
        private const string INFO_COLOR = "#4A90E2";     // Blue
        private const string WARNING_COLOR = "#F5A623";  // Orange
        private const string ERROR_COLOR = "#D0021B";    // Red

        public CustomLogHandler(ILogHandler defaultHandler, bool normal, bool warning, bool error)
        {
            _defaultHandler = defaultHandler;
            _enableNormal = normal;
            _enableWarning = warning;
            _enableError = error;
        }

        public void LogFormat(LogType logType, Object context, string format, params object[] args)
        {
            // Check if this log type is enabled
            if ((logType == LogType.Log && !_enableNormal) ||
                (logType == LogType.Warning && !_enableWarning) ||
                (logType == LogType.Error && !_enableError))
            {
                return;
            }

            string className = "";
            string color = INFO_COLOR;

            if (context != null)
            {
                className = $"[{context.GetType().Name}]";
            }
            else if (args != null && args.Length > 0)
            {
               // className = $"[{args[0].GetType().Name}]";
            }

            switch (logType)
            {
                case LogType.Warning:
                    color = WARNING_COLOR;
                    break;
                case LogType.Error:
                case LogType.Exception:
                    color = ERROR_COLOR;
                    break;
            }

            string message = $"<color={color}>{className}</color> {string.Format(format, args)}";

            _defaultHandler.LogFormat(logType, context, message);
        }

        public void LogException(System.Exception exception, Object context)
        {
            if (!_enableError) return;
            _defaultHandler.LogException(exception, context);
        }
    }
}
