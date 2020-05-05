using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotPath
{
    public int currentPathSegment = 0;
    private float[][] configurations;
    private float[] thresholds;

    public RobotPath(Vector3 robotPosition, Vector3[] pointsOnPath)
    {
        float[] distances = new float[pointsOnPath.Length - 1];
        thresholds = new float[pointsOnPath.Length - 1];
        configurations = new float[pointsOnPath.Length][];

        float pathLength = 0f;

        for (int i = 0; i < distances.Length; i++)
        {
            distances[i] = (pointsOnPath[i + 1] - pointsOnPath[i]).magnitude;
            pathLength += distances[i];
        }


        float currentProgress = 0f;
        for (int i = 0; i < thresholds.Length; i++)
        {
            currentProgress += distances[i];
            thresholds[i] = currentProgress / pathLength;
        }

        for (int i = 0; i < pointsOnPath.Length; i++)
        {
            try
            {
                configurations[i] = RobotController.CalculateSimpleIK(robotPosition, pointsOnPath[i]);
            }
            catch (System.InvalidOperationException e)
            {
                Debug.LogWarning(e.Message);
            }
        }
    }

    public float[] GetConfiguration(float progress)
    {
        progress = progress > 1f ? 1f : progress;

        //if (progress == 1f)
        //{
        //    return configurations[configurations.Length - 1];
        //}

        float[] currentConfig = new float[7];


        int currentIndex = 0;
        for (int i = 0; i < thresholds.Length; i++)
        {
            if (progress <= thresholds[i])
            {
                currentIndex = i;
                break;
            }
        }

        currentPathSegment = currentIndex + 1;

        float interval = currentIndex == 0 ? thresholds[currentIndex] :
            thresholds[currentIndex] - thresholds[currentIndex - 1];

        float normedProgress = currentIndex == 0 ? progress : progress - thresholds[currentIndex - 1];

        float interpolationProgress = normedProgress / interval;

        currentConfig = RobotController.GetLinearInterpolatedConfig(
            configurations[currentIndex],
            configurations[currentIndex + 1],
            interpolationProgress);

        return currentConfig;
    }

}
