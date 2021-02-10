namespace GoogleARCore
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using GoogleARCoreInternal;
    using UnityEngine;


    public class DetectedPlane : Trackable
    {

        /// <param name="nativeHandle"> </param>
        /// <param name="nativeApi"> </param>
        internal DetectedPlane(IntPtr nativeHandle, NativeSession nativeApi)
            : base(nativeHandle, nativeApi)
        {
            _trackableNativeHandle = nativeHandle;
            _nativeSession = nativeApi;
        }


        public DetectedPlane SubsumedBy
        {
            get
            {
                if (IsSessionDestroyed())
                {
                    Debug.LogError(
                        "SubsumedBy:: Trying to access a session that has already been destroyed.");
                    return null;
                }

                return _nativeSession.PlaneApi.GetSubsumedBy(_trackableNativeHandle);
            }
        }
        public Pose CenterPose
        {
            get
            {
                if (IsSessionDestroyed())
                {
                    Debug.LogError(
                        "CenterPose:: Trying to access a session that has already been destroyed.");
                    return new Pose();
                }

                return _nativeSession.PlaneApi.GetCenterPose(_trackableNativeHandle);
            }
        }

        public float ExtentX
        {
            get
            {
                if (IsSessionDestroyed())
                {
                    Debug.LogError(
                        "ExtentX:: Trying to access a session that has already been destroyed.");
                    return 0f;
                }

                return _nativeSession.PlaneApi.GetExtentX(_trackableNativeHandle);
            }
        }

        public float ExtentZ
        {
            get
            {
                if (IsSessionDestroyed())
                {
                    Debug.LogError(
                        "ExtentZ:: Trying to access a session that has already been destroyed.");
                    return 0f;
                }

                return _nativeSession.PlaneApi.GetExtentZ(_trackableNativeHandle);
            }
        }

        public DetectedPlaneType PlaneType
        {
            get
            {
                if (IsSessionDestroyed())
                {
                    Debug.LogError(
                        "PlaneType:: Trying to access a session that has already been destroyed.");
                    return DetectedPlaneType.HorizontalUpwardFacing;
                }

                return _nativeSession.PlaneApi.GetPlaneType(_trackableNativeHandle);
            }
        }


        /// <param name="boundaryPolygonPoints">A list of <b>Vector3</b> to be filled by the method
        /// call.</param>
        [SuppressMemoryAllocationError(Reason = "List could be resized.")]
        public void GetBoundaryPolygon(List<Vector3> boundaryPolygonPoints)
        {
            if (IsSessionDestroyed())
            {
                Debug.LogError(
                    "GetBoundaryPolygon:: Trying to access a session that has already been " +
                    "destroyed.");
                return;
            }

            _nativeSession.PlaneApi.GetPolygon(_trackableNativeHandle, boundaryPolygonPoints);
        }
    }
}
