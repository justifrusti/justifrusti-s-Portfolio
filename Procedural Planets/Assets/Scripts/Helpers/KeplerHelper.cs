using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class KeplerHelper
{
    // Implementation reference: https://en.wikipedia.org/wiki/Kepler%27s_equation

    /// <summary>
    /// Samples a lower resolution version of an orbit path, useful for debug displays.
    /// </summary>
    public static Vector3[] SampleOrbitPath(float apoapsis, float periapsis, float argumentOfPeriapsis, float inclination, Vector3 centre, int resolution = 1000)
    {
        Vector3[] points = new Vector3[resolution + 1];

        for (int i = 0; i <= resolution; i++)
        {
            Vector3 orbitPoint = ComputePointOnOrbit(apoapsis, periapsis, argumentOfPeriapsis, inclination, (float)i / (float)resolution);
            points[i] = orbitPoint + centre;
        }

        return points;
    }

    /// <summary>
    /// Computes a position on the orbit for a given value of t ranging between 0 and 1
    /// </summary>
    public static Vector3 ComputePointOnOrbit(float apoapsis, float periapsis, float argumentOfPeriapsis, float inclination, float t)
    {
        float semiMajorAxis = (apoapsis + periapsis) / 2f;
        float semiMinorAxis = Mathf.Sqrt(apoapsis * periapsis);

        float meanAnomaly = t * Mathf.PI * 2f;
        float linearEccentricity = semiMajorAxis - periapsis;
        float eccentricity = linearEccentricity / semiMajorAxis;

        float eccentricAnomaly = SolveKepler(meanAnomaly, eccentricity);

        float x = semiMajorAxis * (Mathf.Cos(eccentricAnomaly) - eccentricity);
        float y = semiMinorAxis * Mathf.Sin(eccentricAnomaly);

        Quaternion inclinedPlane = Quaternion.AngleAxis(inclination, Vector3.forward);
        Quaternion parametricAngle = Quaternion.AngleAxis(argumentOfPeriapsis, Vector3.up);

        return parametricAngle * inclinedPlane * new Vector3(x, 0f, y);
    }

    /// <summary>
    /// Implementation of Kepler's equation: M = E - e * Sin(E) where M is the mean anomaly, E is the eccentric anomaly and e is the eccentricity
    /// </summary>
    public static float SolveKepler(float M, float e)
    {
        float accuracy = .000001f;

        int maxIterations = 100;

        float E = e > .8f ? Mathf.PI : M;

        for (int k = 0; k < maxIterations; k++)
        {
            float nextValue = E - KeplersEquation(M, E, e) / KeplersEquation_Differentiated(E, e);
            float difference = Mathf.Abs(E - nextValue);

            E = nextValue;

            if(difference < accuracy)
            {
                break;
            }
        }

        return E;
    }

    private static float KeplersEquation(float M, float E, float e)
    {
        return E - (e * Mathf.Sin(e)) - M;
    }

    private static float KeplersEquation_Differentiated(float E, float e)
    {
        return 1 - (e * Mathf.Cos(E));
    }
}
