namespace GoogleARCore.Examples.HelloAR
{
    using System.Collections.Generic;
    using GoogleARCore;
    using GoogleARCore.Examples.Common;
    using UnityEngine;
    using UnityEngine.EventSystems;

#if UNITY_EDITOR

    using Input = InstantPreviewInput;
#endif


    public class HelloARController : MonoBehaviour
    {

        public DepthMenu DepthMenu;
        public InstantPlacementMenu InstantPlacementMenu;
        public GameObject InstantPlacementPrefab;
        public Camera FirstPersonCamera;
        public GameObject GameObjectVerticalPlanePrefab;
        public GameObject GameObjectHorizontalPlanePrefab;
        public GameObject GameObjectPointPrefab;

        private const float _prefabRotation = 180.0f;

        private bool _isQuitting = false;


        public void Awake()
        {

            Application.targetFrameRate = 60;
        }


        public void Update()
        {

            UpdateApplicationLifecycle();
            Touch touch;
            if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
            {
                return;
            }

            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                return;
            }

            TrackableHit hit;
            bool foundHit = false;
            if (InstantPlacementMenu.IsInstantPlacementEnabled())
            {
                foundHit = Frame.RaycastInstantPlacement(
                    touch.position.x, touch.position.y, 1.0f, out hit);
            }
            else
            {
                TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
                    TrackableHitFlags.FeaturePointWithSurfaceNormal;
                foundHit = Frame.Raycast(
                    touch.position.x, touch.position.y, raycastFilter, out hit);
            }

            if (foundHit)
            {

                if ((hit.Trackable is DetectedPlane) &&
                    Vector3.Dot(FirstPersonCamera.transform.position - hit.Pose.position,
                        hit.Pose.rotation * Vector3.up) < 0)
                {
                    Debug.Log("Hit at back of the current DetectedPlane");
                }
                else
                {
                    if (DepthMenu != null)
                    {
                        DepthMenu.ConfigureDepthBeforePlacingFirstAsset();
                    }

                    GameObject prefab;
                    if (hit.Trackable is InstantPlacementPoint)
                    {
                        prefab = InstantPlacementPrefab;
                    }
                    else if (hit.Trackable is FeaturePoint)
                    {
                        prefab = GameObjectPointPrefab;
                    }
                    else if (hit.Trackable is DetectedPlane)
                    {
                        DetectedPlane detectedPlane = hit.Trackable as DetectedPlane;
                        if (detectedPlane.PlaneType == DetectedPlaneType.Vertical)
                        {
                            prefab = GameObjectVerticalPlanePrefab;
                        }
                        else
                        {
                            prefab = GameObjectHorizontalPlanePrefab;
                        }
                    }
                    else
                    {
                        prefab = GameObjectHorizontalPlanePrefab;
                    }

                    var gameObject = Instantiate(prefab, hit.Pose.position, hit.Pose.rotation);


                    gameObject.transform.Rotate(0, _prefabRotation, 0, Space.Self);


                    var anchor = hit.Trackable.CreateAnchor(hit.Pose);

                    gameObject.transform.parent = anchor.transform;

                    if (hit.Trackable is InstantPlacementPoint)
                    {
                        gameObject.GetComponentInChildren<InstantPlacementEffect>()
                            .InitializeWithTrackable(hit.Trackable);
                    }
                }
            }
        }


        private void UpdateApplicationLifecycle()
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }

            if (Session.Status != SessionStatus.Tracking)
            {
                Screen.sleepTimeout = SleepTimeout.SystemSetting;
            }
            else
            {
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
            }

            if (_isQuitting)
            {
                return;
            }

            if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
            {
                ShowAndroidToastMessage("Camera permission is needed to run this application.");
                _isQuitting = true;
                Invoke("DoQuit", 0.5f);
            }
            else if (Session.Status.IsError())
            {
                ShowAndroidToastMessage(
                    "ARCore encountered a problem connecting.  Please start the app again.");
                _isQuitting = true;
                Invoke("DoQuit", 0.5f);
            }
        }


        private void DoQuit()
        {
            Application.Quit();
        }

      
        /// <param name="message">Message string to show in the toast.</param>
        private void ShowAndroidToastMessage(string message)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity =
                unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            if (unityActivity != null)
            {
                AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
                unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    AndroidJavaObject toastObject =
                        toastClass.CallStatic<AndroidJavaObject>(
                            "makeText", unityActivity, message, 0);
                    toastObject.Call("show");
                }));
            }
        }
    }
}
