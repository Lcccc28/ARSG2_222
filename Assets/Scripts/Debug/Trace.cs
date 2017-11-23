using System;
using UnityEngine;

/// <summary>
/// Author: NicolasTse
/// Email: xiehaojiejob@qq.com
/// </summary>
public class Trace {

#if UNITY_EDITOR
    public static Action<object> XHJ = Debug.Log;
    public static Action<object> Log = Debug.Log;
    public static Action<object> LogWarning = Debug.LogWarning;
    public static Action<object> LogError = Debug.LogError;
#else
    private static bool showLog = false;

	public static void XHJ(object message) {
        if (showLog) {
		    Debug.Log(message);
        }
	}

    public static void Log (object message, UnityEngine.Object context = null) {
        if (showLog) {
            Debug.Log(message, context);
        }
    }

    public static void LogWarning (object message, UnityEngine.Object context = null) {
        if (showLog) {
            Debug.LogWarning(message, context);
        }
    }

    public static void LogError(object message, UnityEngine.Object context = null) {
        if (showLog) {
            Debug.LogError(message, context);
        }
    }
#endif
}