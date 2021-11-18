using System;
using System.Collections;
using System.Text;
using UnityEngine;

public class GameState : MonoBehaviour
{
    private System.IO.TextWriter stateStream;

    #region Important
    //Player's velocity in vector format
    private Vector3 playerVelocityVector;
    // If game is paused
    public bool movementFrozen = false;
    public bool menuFrozen = false;
    private bool menuKeyDown = false;
    private bool spaceKeyDown = false;

    // Gamma (Update each Frame)
    [HideInInspector] public double sqrtOneMinusVSquaredCWDividedByCSquared;
    #endregion

    #region Member Variables
    private Quaternion orientation = Quaternion.identity;
    private Matrix4x4 worldRotation;
   
    public Transform playerTransform;
    public double playerVelocity;
    private double deltaTimeWorld;
    private double deltaTimePlayer;
    private double totalTimePlayer;
    private double totalTimeWorld;

    private double c = 200;
    public double totalC = 200;
    public double maxPlayerSpeed;
    private double maxSpeed;
    //speed of light squared
    [HideInInspector] public double cSqrd;

    //Player rotation and change in rotation since last frame
    public Vector3 playerRotation = new Vector3(0, 0, 0);
    public Vector3 deltaRotation = new Vector3(0, 0, 0);
    public double pctOfSpdUsing = 0; // Percent of velocity you are using
    #endregion

    #region Properties
    public bool keyHit = false;
    public float finalMaxSpeed = .99f;

    public double SpeedOfLight { get { return c; } set { c = value; cSqrd = value * value; } }
    public double MaxSpeed { get { return maxSpeed; } set { maxSpeed = value; } }
    public bool MovementFrozen { get { return movementFrozen; } set { movementFrozen = value; } }
    public bool MenuFrozen { get { return menuFrozen; } set { menuFrozen = value; } }
    public Vector3 PlayerVelocityVector { get { return playerVelocityVector; } set { playerVelocityVector = value; } }
    #endregion

    #region consts
    private const float ORB_SPEED_INC = 0.05f;
    private const float ORB_DECEL_RATE = 0.6f;
    private const float ORB_SPEED_DUR = 2f;
    private const float MAX_SPEED = 20.00f;
    public const float NORM_PERCENT_SPEED = .99f;
    public const int splitDistance = 21000;
    #endregion

    #region Menu
    public GameObject pauseMenuUI;
    public GameObject infoPanel;
    #endregion


    public void Awake()
    {
        //Initialize the player's speed to zero
        playerVelocityVector = Vector3.zero;
        playerVelocity = 0;

        MaxSpeed = MAX_SPEED;
        pctOfSpdUsing = NORM_PERCENT_SPEED;

        c = totalC;
        cSqrd = c * c;
        movementFrozen = false;
    }

    public void reset()
    {
        //Reset everything not level-based
        playerRotation.x = 0;
        playerRotation.y = 0;
        playerRotation.z = 0;
        pctOfSpdUsing = 0;
    }

    // Pause/Unpause game
    public void ChangeMenuState()
    {
        if (menuFrozen)
        {
            menuFrozen = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            pauseMenuUI.SetActive(false);
            infoPanel.SetActive(true);
        }
        else
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().velocity = Vector3.zero;
            menuFrozen = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            pauseMenuUI.SetActive(true);
            infoPanel.SetActive(false);
        }
    }
    public void ChangeState()
    {
        if (movementFrozen)
            movementFrozen = false;
        else
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().velocity = Vector3.zero;
            movementFrozen = true;
        }
    }

    public void LateUpdate()
    {
        if (Input.GetAxis("Menu Key") > 0 && !menuKeyDown)
        {
            menuKeyDown = true;
            ChangeMenuState();
        }
        else if (!(Input.GetAxis("Menu Key") > 0))
        {
            menuKeyDown = false;
        }
        if (Input.GetAxis("Space") > 0 && !spaceKeyDown)
        {
            spaceKeyDown = true;
            ChangeState();
        }
        else if (!(Input.GetAxis("Space") > 0))
        {
            spaceKeyDown = false;
        }

        // Update Everything
        if (!menuFrozen)
        {
            // keep at max speed
            if (playerVelocityVector.magnitude >= (float)MaxSpeed - .01f)
                playerVelocityVector = playerVelocityVector.normalized * ((float)MaxSpeed - .01f);

            playerVelocity = playerVelocityVector.magnitude;

            // calculate gamma
            sqrtOneMinusVSquaredCWDividedByCSquared = (double)Math.Sqrt(1 - (playerVelocity * playerVelocity) / cSqrd);

            deltaTimePlayer = (double)Time.deltaTime;

            // Display purposes
            if (keyHit)
            {
                totalTimePlayer += deltaTimePlayer;
                if (!double.IsNaN(sqrtOneMinusVSquaredCWDividedByCSquared))
                {
                    deltaTimeWorld = deltaTimePlayer / sqrtOneMinusVSquaredCWDividedByCSquared;
                    totalTimeWorld += deltaTimeWorld;
                }
            }

            // Set our rigidbody's velocity
            if (!double.IsNaN(deltaTimePlayer) && !double.IsNaN(sqrtOneMinusVSquaredCWDividedByCSquared) && !movementFrozen)
                GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().velocity = -1 * playerVelocityVector;
            // GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().velocity = -1 * (playerVelocityVector / (float)sqrtOneMinusVSquaredCWDividedByCSquared);
            else
                GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().velocity = Vector3.zero;

            orientation = Quaternion.AngleAxis(playerRotation.y, Vector3.up) * Quaternion.AngleAxis(playerRotation.x, Vector3.right);
            Quaternion WorldOrientation = Quaternion.Inverse(orientation);
            Normalize(orientation);
            worldRotation = CreateFromQuaternion(WorldOrientation);
            playerRotation += deltaRotation;
        }
    }

    // Thanks Google
    #region Matrix/Quat math
    // This function takes in a quaternion and creates a rotation matrix from it
    public Matrix4x4 CreateFromQuaternion(Quaternion q)
    {
        double w = q.w;
        double x = q.x;
        double y = q.y;
        double z = q.z;

        double wSqrd = w * w;
        double xSqrd = x * x;
        double ySqrd = y * y;
        double zSqrd = z * z;

        Matrix4x4 matrix;
        matrix.m00 = (float)(wSqrd + xSqrd - ySqrd - zSqrd);
        matrix.m01 = (float)(2 * x * y - 2 * w * z);
        matrix.m02 = (float)(2 * x * z + 2 * w * y);
        matrix.m03 = (float)0;
        matrix.m10 = (float)(2 * x * y + 2 * w * z);
        matrix.m11 = (float)(wSqrd - xSqrd + ySqrd - zSqrd);
        matrix.m12 = (float)(2 * y * z + 2 * w * x);
        matrix.m13 = (float)0;
        matrix.m20 = (float)(2 * x * z - 2 * w * y);
        matrix.m21 = (float)(2 * y * z - 2 * w * x);
        matrix.m22 = (float)(wSqrd - xSqrd - ySqrd + zSqrd);
        matrix.m23 = 0;
        matrix.m30 = 0;
        matrix.m31 = 0;
        matrix.m32 = 0;
        matrix.m33 = 1;
        return matrix;
    }

    public Quaternion Normalize(Quaternion quat)
    {
        Quaternion q = quat;

        double magnitudeSqr = q.w * q.w + q.x * q.x + q.y * q.y + q.z * q.z;
        if (magnitudeSqr != 1)
        {
            double magnitude = (double)Math.Sqrt(magnitudeSqr);
            q.w = (float)((double)q.w / magnitude);
            q.x = (float)((double)q.x / magnitude);
            q.y = (float)((double)q.y / magnitude);
            q.z = (float)((double)q.z / magnitude);
        }
        return q;
    }
    public Vector3 TransformNormal(Vector3 normal, Matrix4x4 matrix)
    {
        Vector3 final;
        final.x = matrix.m00 * normal.x + matrix.m10 * normal.y + matrix.m20 * normal.z;

        final.y = matrix.m02 * normal.x + matrix.m11 * normal.y + matrix.m21 * normal.z;

        final.z = matrix.m03 * normal.x + matrix.m12 * normal.y + matrix.m22 * normal.z;
        return final;
    }
    #endregion
}

