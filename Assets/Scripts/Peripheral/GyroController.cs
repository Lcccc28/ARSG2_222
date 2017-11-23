using UnityEngine;
using System.Collections;

using DG.Tweening;
using Game.Manager;

// ***********************************************************
// Written by Heyworks Unity Studio http://unity.heyworks.com/
// ***********************************************************


/// <summary>
/// Gyroscope controller that works with any device orientation.
/// </summary>
public class GyroController : MonoBehaviour
    {
        // 游戏逻辑相关
        public new GameObject camera;
        private float cd;
        private int sceneId;
        public bool gyroEnabled = false;

        #region [Private fields]

        private const float lowPassFilterFactor = 0.2f;
        private readonly Quaternion baseIdentity = Quaternion.Euler(90, 0, 0);
        private readonly Quaternion landscapeRight = Quaternion.Euler(0, 0, 90);
        private readonly Quaternion landscapeLeft = Quaternion.Euler(0, 0, -90);
        private readonly Quaternion upsideDown = Quaternion.Euler(0, 0, 180);
        private Quaternion cameraBase = Quaternion.identity;
        private Quaternion calibration = Quaternion.identity;
        private Quaternion baseOrientation = Quaternion.Euler(90, 0, 0);
        private Quaternion baseOrientationRotationFix = Quaternion.identity;
        private Quaternion referanceRotation = Quaternion.identity;
        private bool debug = true;
        #endregion
        #region [Unity events]
        protected void Awake()
        {
            gyroEnabled = false;
            sceneId = GameSceneManager.Instance.GetSceneId();
            StartCoroutine(WaitForGyroInit());
        }

        protected IEnumerator WaitForGyroInit() {
            Input.gyro.enabled = true;
            yield return new WaitForSeconds(0.2f);
            AttachGyro();
        }

        protected void FixedUpdate()
        {
            if (!gyroEnabled)
                return;
            Quaternion baseRot = transform.rotation;
            transform.rotation = cameraBase * (ConvertRotation(referanceRotation * Input.gyro.attitude) * GetRotFix());

            // Quaternion.Slerp(transform.rotation, cameraBase * (ConvertRotation(referanceRotation * Input.gyro.attitude) * GetRotFix()), lowPassFilterFactor);

            // 游戏逻辑相关
            Vector3 lookTorward = transform.TransformPoint(Vector3.forward);
            if (sceneId == 1)
            {
                if (Vector3.Project(transform.forward, Vector3.forward).z < 0)
                {
                    lookTorward = new Vector3(lookTorward.x, lookTorward.y, transform.position.z);
                }
            }
            else if (sceneId == 3)
            {

            }

            Vector3 lookParallel = new Vector3(lookTorward.x, transform.position.y, lookTorward.z);

            
            transform.LookAt(lookParallel);
            camera.transform.LookAt(lookTorward);
            //transform.rotation = baseRot;
            //transform.DOLookAt(lookParallel, lowPassFilterFactor);
            //camera.transform.DOLookAt(lookTorward, lowPassFilterFactor);

            //Vector3 up = transform.up;
            //         // Quaternion baseRot = transform.rotation;
            //transform.LookAt(lookParallel, Vector3.Project(up, Vector3.up));
            //         // camera.transform.rotation = baseRot;
            //Quaternion q = Quaternion.LookRotation(lookTorward - transform.position, up);
            //camera.transform.rotation = q * landscapeRight;

            if (sceneId == 3)
            {
                if (transform.eulerAngles.y > 60 && transform.eulerAngles.y < 220)
                {
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, 60, transform.eulerAngles.z);
                }
                else if (transform.eulerAngles.y >= 220 && transform.eulerAngles.y < 320)
                {
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, 320, transform.eulerAngles.z);
                }

                if (camera.transform.localEulerAngles.x > 20 && camera.transform.localEulerAngles.x < 220)
                {
                    camera.transform.localEulerAngles = new Vector3(20, 0, 0);
                }
                else if (camera.transform.localEulerAngles.x >= 220 && camera.transform.localEulerAngles.x < 330)
                {
                    camera.transform.localEulerAngles = new Vector3(330, 0, 0);
                }
            }
            else
            {
                // 位移的
                Vector3 acceleration = Input.gyro.userAcceleration;
                Vector3 aX = Vector3.Project(transform.TransformPoint(new Vector3(-acceleration.x, -acceleration.y, -acceleration.z)) - transform.position, Vector3.right);
                if (acceleration.magnitude > 0.4f && cd <= 0)
                {
                    if (aX.x > 0 && transform.position.x <= 12)
                    {
                        cd = 3f;
                        transform.DOMove(transform.position + new Vector3(5, 0, 0), 1f);
                    }
                    else if (aX.x < 0 && transform.position.x >= -12)
                    {
                        cd = 3f;
                        transform.DOMove(transform.position + new Vector3(-5, 0, 0), 1f);
                    }
                }
                cd -= Time.fixedDeltaTime;
            }

        }
    //protected void OnGUI()
    //{
    //    if (!debug)
    //        return;
    //    GUILayout.Label("Orientation: " + Screen.orientation);
    //    GUILayout.Label("Calibration: " + calibration);
    //    GUILayout.Label("Camera base: " + cameraBase);
    //    GUILayout.Label("input.gyro.attitude: " + Input.gyro.attitude);
    //    GUILayout.Label("transform.rotation: " + transform.rotation);
    //    if (GUILayout.Button("On/off gyro: " + Input.gyro.enabled, GUILayout.Height(100)))
    //    {
    //        Input.gyro.enabled = !Input.gyro.enabled;
    //    }
    //    if (GUILayout.Button("On/off gyro controller: " + gyroEnabled, GUILayout.Height(100)))
    //    {
    //        if (gyroEnabled)
    //        {
    //            DetachGyro();
    //        }
    //        else
    //        {
    //            AttachGyro();
    //        }
    //    }
    //    if (GUILayout.Button("Update gyro calibration (Horizontal only)", GUILayout.Height(80)))
    //    {
    //        UpdateCalibration(true);
    //    }
    //    if (GUILayout.Button("Update camera base rotation (Horizontal only)", GUILayout.Height(80)))
    //    {
    //        UpdateCameraBaseRotation(true);
    //    }
    //    if (GUILayout.Button("Reset base orientation", GUILayout.Height(80)))
    //    {
    //        ResetBaseOrientation();
    //    }
    //    if (GUILayout.Button("Reset camera rotation", GUILayout.Height(80)))
    //    {
    //        transform.rotation = Quaternion.identity;
    //    }
    //}
    #endregion
    #region [Public methods]
    /// <summary>
    /// Attaches gyro controller to the transform.
    /// </summary>
    public void AttachGyro()
        {
            gyroEnabled = true;
            ResetBaseOrientation();
            UpdateCalibration(true);
            UpdateCameraBaseRotation(true);
            RecalculateReferenceRotation();
        }
        /// <summary>
        /// Detaches gyro controller from the transform
        /// </summary>
        public void DetachGyro()
        {
            gyroEnabled = false;
        }
        #endregion
        #region [Private methods]
        /// <summary>
        /// Update the gyro calibration.
        /// </summary>
        private void UpdateCalibration(bool onlyHorizontal)
        {
            if (onlyHorizontal)
            {
                var fw = (Input.gyro.attitude) * (-Vector3.forward);
                fw.z = 0;
                if (fw == Vector3.zero)
                {
                    calibration = Quaternion.identity;
                }
                else
                {
                    calibration = (Quaternion.FromToRotation(baseOrientationRotationFix * Vector3.up, fw));
                }
            }
            else
            {
                calibration = Input.gyro.attitude;
            }
        }
        /// <summary>
        /// Update the camera base rotation.
        /// </summary>
        /// <param name='onlyHorizontal'>
        /// Only y rotation.
        /// </param>
        private void UpdateCameraBaseRotation(bool onlyHorizontal)
        {
            if (onlyHorizontal)
            {
                var fw = transform.forward;
                fw.y = 0;
                if (fw == Vector3.zero)
                {
                    cameraBase = Quaternion.identity;
                }
                else
                {
                    cameraBase = Quaternion.FromToRotation(Vector3.forward, fw);
                }
            }
            else
            {
                cameraBase = transform.rotation;
            }
        }
        /// <summary>
        /// Converts the rotation from right handed to left handed.
        /// </summary>
        /// <returns>
        /// The result rotation.
        /// </returns>
        /// <param name='q'>
        /// The rotation to convert.
        /// </param>
        private static Quaternion ConvertRotation(Quaternion q)
        {
            return new Quaternion(q.x, q.y, -q.z, -q.w);
        }
        /// <summary>
        /// Gets the rot fix for different orientations.
        /// </summary>
        /// <returns>
        /// The rot fix.
        /// </returns>
        private Quaternion GetRotFix()
        {
         if (Screen.orientation == ScreenOrientation.Portrait)
             return Quaternion.identity;
         if (Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.Landscape)
             return landscapeLeft;     
         if (Screen.orientation == ScreenOrientation.LandscapeRight)
             return landscapeRight;
         if (Screen.orientation == ScreenOrientation.PortraitUpsideDown)
             return upsideDown;
         return Quaternion.identity;
        }
        /// <summary>
        /// Recalculates reference system.
        /// </summary>
        private void ResetBaseOrientation()
        {
            baseOrientationRotationFix = GetRotFix();
            baseOrientation = baseOrientationRotationFix * baseIdentity;
        }
        /// <summary>
        /// Recalculates reference rotation.
        /// </summary>
        private void RecalculateReferenceRotation()
        {
            referanceRotation = Quaternion.Inverse(baseOrientation) * Quaternion.Inverse(calibration);
        }
        #endregion
    }

