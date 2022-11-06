using UnityEngine;

[System.Serializable]
public struct Duration
{
    public float Min;
    public float Max;

    [Space]

    public bool UseScaledTime;

    private float nextDelay;
    private float timestamp;

    private bool once;

    public bool Once
    {
        get
        {
            if (IsUp && once)
            {
                once = false;
                return true;
            }

            return false;
        }
    }

    private float time
    {
        get
        {
            return UseScaledTime ? Time.time : Time.unscaledTime;
        }
    }

    public Duration(float delay, bool useScaledTime = true)
    {
        Min = delay;
        Max = delay;

        timestamp = -delay;
        nextDelay = 0.0f;
        UseScaledTime = useScaledTime;
        once = false;
    }

    public Duration(float min, float max, bool useScaledTime = true)
    {
        Min = min;
        Max = max;

        timestamp = -max;
        nextDelay = 0.0f;
        UseScaledTime = useScaledTime;
        once = false;
    }

    public bool IsUp
    {
        get
        {
            return time >= timestamp + nextDelay;
        }
    }

    public float Remaining
    {
        get
        {
            return Mathf.Max(nextDelay - Elapsed, 0f);
        }
    }

    public float Elapsed
    {
        get
        {
            return time - timestamp;
        }
    }

    public float Elapsed01
    {
        get
        {
            if (nextDelay == 0)
                return 1f;

            return Mathf.Clamp01(Elapsed / nextDelay);
        }
    }

    public float SmoothStep01
    {
        get
        {
            if (nextDelay == 0)
                return 1f;

            return Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(Elapsed / nextDelay));
        }
    }

    public float ElapsedQuad
    {
        get
        {
            if (nextDelay == 0)
                return 1f;

            var norm = Mathf.Clamp01(Elapsed / nextDelay);
            return norm * norm;
        }
    }

    public Duration Next()
    {
        nextDelay = Random.Range(Min, Max);
        timestamp = time;
        once = true;

        return this;
    }

    public Duration Next(float min, float max)
    {
        Min = min;
        Max = max;

        nextDelay = Random.Range(Min, Max);
        timestamp = time;
        once = true;

        return this;
    }

    public Duration Next(float duration)
    {
        nextDelay = duration;
        timestamp = time;
        once = true;

        return this;
    }

    public Duration Extend(float duration)
    {
        nextDelay += duration;
        return this;
    }

    public void Stop()
    {
        timestamp = -nextDelay;
        once = false;
    }

    public override string ToString()
    {
        return $"Delay: IsUp({IsUp}), Min({Min}), Max({Max}), Next({nextDelay}), Elapsed({Elapsed})";
    }
}
