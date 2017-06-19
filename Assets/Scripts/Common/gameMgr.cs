using UnityEngine;
using System.Collections;

/// <summary>
/// static game manager class.  Controls the highest level games states.
/// </summary>
public static class gameMgr 
{
    static bool pauseBool = false;
    public static bool Pause
    {
        get { return pauseBool; }
        set { Pause = value; }
    }

    public static float GetSeconds()
    {
        if (pauseBool)
            return 0.0f;
        return Time.smoothDeltaTime;
    }

	public static float GetFixedSeconds()
	{
		if (pauseBool)
			return 0.0f;
		return Time.fixedDeltaTime;
	}

    public static float GetNonStopSeconds()
    {
        return Time.smoothDeltaTime;
    }
}
