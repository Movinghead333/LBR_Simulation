using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour
{

    private static float aSide = 0.39f;
    private static float bSide = 0.4f;

    // activate or deactivate manual control via sliders in inspector
    public bool manualControl = true;
    public Vector3 currentFlunchPosition;

    #region  Joint Sliders
    [Range(-170, 170)]
    public float j0Angle;

    [Range(-120, 120)]
    public float j1Angle;

    [Range(-170, 170)]
    public float j2Angle;

    [Range(-120, 120)]
    public float j3Angle;

    [Range(-170, 170)]
    public float j4Angle;

    [Range(-120, 120)]
    public float j5Angle;

    [Range(-170, 170)]
    public float j6Angle;
    #endregion

    // transforms
    public Transform[] segments;

    // joint angles
    //[HideInInspector]
    public float[] jointAngles;

    // matrices
    private Matrix4x4 basePosition;
    private Matrix4x4[] joints;

    // private offset vectors
    private Vector3[] jointOffsets;

    private float wristFlunchDist = 0.078f;

    // Start is called before the first frame update
    void Start()
    {
        joints = new Matrix4x4[7];
        jointOffsets = new Vector3[7];
        jointAngles = new float[7];
        jointAngles[3] = -90f;
        jointAngles[5] = -90f;

        jointOffsets[0] = new Vector3(0f, 0.12f, 0f);
        jointOffsets[1] = new Vector3(0f, 0.19f, 0f);
        jointOffsets[2] = new Vector3(0f, 0.2f, 0f);
        jointOffsets[3] = new Vector3(0f, 0.2f, 0f);
        jointOffsets[4] = new Vector3(0f, 0.195f, 0f);
        jointOffsets[5] = new Vector3(0f, 0.195f, 0f);
        jointOffsets[6] = new Vector3(0f, 0.06f, 0f);

    }

    // Update is called once per frame
    void Update()
    {
        if (manualControl)
        {
            jointAngles[0] = j0Angle;
            jointAngles[1] = j1Angle;
            jointAngles[2] = j2Angle;
            jointAngles[3] = j3Angle;
            jointAngles[4] = j4Angle;
            jointAngles[5] = j5Angle;
            jointAngles[6] = j6Angle;
        }
        else
        {
            j0Angle = jointAngles[0];
            j1Angle = jointAngles[1];
            j2Angle = jointAngles[2];
            j3Angle = jointAngles[3];
            j4Angle = jointAngles[4];
            j5Angle = jointAngles[5];
            j6Angle = jointAngles[6];
        }

        // base
        basePosition = Matrix4x4.Translate(transform.position);

        for (int i = 0; i < joints.Length; i++)
        {
            // calculate parent transform of current joint
            Matrix4x4 parentTransform = i == 0 ? basePosition : joints[i - 1];

            // calculate translation to previous joint
            Matrix4x4 translation = Matrix4x4.Translate(jointOffsets[i]);

            // calculate rotation of current joint
            Matrix4x4 rotation = i % 2 == 0 ?
                Matrix4x4.Rotate(Quaternion.Euler(0f, jointAngles[i], 0f)) :
                Matrix4x4.Rotate(Quaternion.Euler(0f, 0f, jointAngles[i]));

            // update joint transform
            joints[i] = parentTransform * translation * rotation;
        }

        Matrix4x4 flunchTransform = joints[6] * Matrix4x4.Translate(new Vector3(0f, 0.018f, 0f));
        currentFlunchPosition = getTranslation(flunchTransform);

        for (int i = 0; i < joints.Length; i++)
        {
            segments[i].position = getTranslation(joints[i]);
            segments[i].rotation = joints[i].rotation;
        }
    }

    public void LinearInterpolateConfigs(float[] startConfig, float[] endConfig, float progress)
    {
        float[] newConfig = new float[startConfig.Length];

        for (int i = 0; i < startConfig.Length; i++)
        {
            newConfig[i] = startConfig[i] + ((endConfig[i] - startConfig[i]) * progress);
            jointAngles[i] = newConfig[i];
        }
    }

    public static float[] GetLinearInterpolatedConfig(float[] startConfig, float[] endConfig, float progress)
    {
        float[] newConfig = new float[startConfig.Length];

        for (int i = 0; i < startConfig.Length; i++)
        {
            newConfig[i] = startConfig[i] + ((endConfig[i] - startConfig[i]) * progress);
        }

        return newConfig;
    }

    public static float[] CalculateSimpleIK(Vector3 robotBasePosition, Vector3 targetPosition)
    {
        float[] config = new float[7];

        Vector3 robotPos = robotBasePosition + new Vector3(0f, 0.31f, 0f);
        Vector3 targetObjectPos = targetPosition + new Vector3(0f, 0.078f, 0f);
        Vector3 objDirection = (targetObjectPos - robotPos);
        float robotTargetDist = objDirection.magnitude;

        if (robotTargetDist > (0.79f))
        {
            throw new System.InvalidOperationException("Distance of " + robotTargetDist + " is out of range! Point: " + targetPosition);
        }

        // joint 0
        Vector2 xzDirection = (new Vector2(objDirection.x, objDirection.z)).normalized;

        Vector2 rotatedDirection = new Vector2(-xzDirection.y, -xzDirection.x);

        float j0Angle = Mathf.Atan2(rotatedDirection.y, rotatedDirection.x) * Mathf.Rad2Deg;


        config[0] = j0Angle;


        // joint 1
        float acosArg = GetArcCosArgForAlphaAngle(aSide, bSide, robotTargetDist);
        float alphaAngle = Mathf.Acos(acosArg) * Mathf.Rad2Deg;
        float objDirUPDirAngle = Vector3.Angle(Vector3.up, objDirection);

        float j1Angle = objDirUPDirAngle - alphaAngle;
        config[1] = -j1Angle;


        // joint 3
        float acosArg2 = GetArcCosArgForAlphaAngle(robotTargetDist, bSide, aSide);
        float gammaAngle = Mathf.Acos(acosArg2) * Mathf.Rad2Deg;

        float j3Angle = 180f - gammaAngle;
        config[3] = -j3Angle;


        // joint 5
        float acosArg3 = GetArcCosArgForAlphaAngle(bSide, aSide, robotTargetDist);
        float betaAngle = Mathf.Acos(acosArg3) * Mathf.Rad2Deg;

        float objDirUpDirCounterAngle = 180f - objDirUPDirAngle;

        float j5Angle = objDirUpDirCounterAngle - betaAngle;
        config[5] = -j5Angle;

        return config;
    }

    public float[] CalculateAdvancedIK(
        Vector3 robotBasePosition,
        Vector3 targetPosition,
        Vector3 facingDirection,
        float facingAngle)
    {
        float[] config = new float[7];

        // calculate position of spherical should joint
        Vector3 shoulderPos = robotBasePosition + new Vector3(0f, 0.31f, 0f);

        // calculate position of spherical wrist joint
        float testDist = wristFlunchDist + 0.05f;
        Vector3 wristPos = targetPosition - (facingDirection.normalized * testDist);

        // calculate vector and distance between shoulder and wrist joint centers
        Vector3 shoulderWristDir = (wristPos - shoulderPos);
        float shoulderWristDistance = shoulderWristDir.magnitude;

        if (shoulderWristDistance > 0.79f)
        {
            throw new System.InvalidOperationException("Distance of " + shoulderWristDistance + " is out of range! Point: " + targetPosition);
        }

        // joint 0
        Vector2 xzDirection = (new Vector2(shoulderWristDir.x, shoulderWristDir.z)).normalized;

        Vector2 rotatedDirection = new Vector2(xzDirection.x, -xzDirection.y);

        float j0Angle = Mathf.Atan2(rotatedDirection.y, rotatedDirection.x) * Mathf.Rad2Deg;


        config[0] = j0Angle;

        // joint 1
        float acosArg = GetArcCosArgForAlphaAngle(aSide, bSide, shoulderWristDistance);
        float alphaAngle = Mathf.Acos(acosArg) * Mathf.Rad2Deg;
        float objDirUPDirAngle = Vector3.Angle(Vector3.up, shoulderWristDir);

        float j1Angle = objDirUPDirAngle - alphaAngle;
        config[1] = -j1Angle;

        // joint 2
        // this angle is fixed to default grapping plane
        config[2] = 0f;

        


        // joint 3
        float acosArg2 = GetArcCosArgForAlphaAngle(shoulderWristDistance, bSide, aSide);
        float gammaAngle = Mathf.Acos(acosArg2) * Mathf.Rad2Deg;

        float j3Angle = 180f - gammaAngle;
        config[3] = -j3Angle;

        // joint 4
        Vector3 theta0RotationAxis = Vector3.Cross(Vector3.up, shoulderWristDir);
        Matrix4x4 rotMatrix = Matrix4x4.Rotate(Quaternion.AngleAxis(j1Angle, theta0RotationAxis));
        Vector3 segment1Vec = rotMatrix.MultiplyVector(Vector3.up);
        Vector3 elbowPos = shoulderPos + segment1Vec.normalized * aSide;

        Vector3 j4RotationAxis = (wristPos - elbowPos).normalized;

        Vector3 wristTCPVec = targetPosition - wristPos;
        Vector3 wristTCPVecProjected = Vector3.ProjectOnPlane(wristTCPVec, j4RotationAxis).normalized;

        Vector3 wristShoulderVecProjected = Vector3.ProjectOnPlane(-shoulderWristDir, j4RotationAxis).normalized;

        float j4Angle = Vector3.Angle(wristShoulderVecProjected, wristTCPVecProjected);

        Vector3 grappingPlainNormal = Vector3.Cross(shoulderWristDir, Vector3.up).normalized;
        float tcpGrappingPlaneDist = Vector3.Dot(grappingPlainNormal, (targetPosition - wristPos));
        float tcpGrappingPlaneDistSign = Mathf.Sign(tcpGrappingPlaneDist);

        j4Angle = tcpGrappingPlaneDistSign == 1 ? j4Angle : -j4Angle;
        config[4] = -j4Angle;

        // joint 5
        float j5Angle = Vector3.Angle(j4RotationAxis, wristTCPVec);
        config[5] = -j5Angle;

        // joint 6
        config[6] = facingAngle;

        return config;
    }


    public static Vector3 getTranslation(Matrix4x4 mat)
    {
        return new Vector3(mat.m03, mat.m13, mat.m23);
    }

    private static float GetArcCosArgForAlphaAngle(float a, float b, float c)
    {
        return (b * b + c * c - a * a) / (2 * b * c);
    }
}
