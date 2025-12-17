using System;
using UnityEngine;

/// <summary>
/// Low-level helper to call JavaScript functions from Unity.
/// Wraps Application.ExternalCall with logging, Editor simulation, and typed overloads.
/// </summary>
public static class WebBridge
{
    public static void CallJS(string function, params object[] args)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        Application.ExternalCall(function, args);
#else
        Debug.Log($"[WebBridge] Simulated JS Call: {function}({string.Join(", ", args)})");
#endif
    }

    // Optional: callback simulation in Editor
    public static void CallJS(string function, Action callback, params object[] args)
    {
        CallJS(function, args);
        callback?.Invoke();
    }

    // Convenience typed overloads
    public static void CallJS(string function, int arg) => CallJS(function, new object[] { arg });
    public static void CallJS(string function, float arg) => CallJS(function, new object[] { arg });
    public static void CallJS(string function, bool arg) => CallJS(function, new object[] { arg });
    public static void CallJS(string function, string arg) => CallJS(function, new object[] { arg });

    // Optional safe wrapper
    public static void SafeCallJS(string function, params object[] args)
    {
        try { CallJS(function, args); }
        catch (Exception e) { Debug.LogError($"[WebBridge] Failed JS call: {function} - {e}"); }
    }
}