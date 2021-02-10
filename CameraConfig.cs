namespace GoogleARCore
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using UnityEngine;


    [Flags]
    [SuppressMessage("UnityRules.UnityStyleRules", "US1200:FlagsEnumsMustBePlural",
                     Justification = "Usage is plural.")]
    public enum CameraConfigDepthSensorUsage
    {

        RequireAndUse = 0x0001,
        DoNotUse = 0x0002,
    }

    [Flags]
    [SuppressMessage("UnityRules.UnityStyleRules", "US1200:FlagsEnumsMustBePlural",
                     Justification = "Usage is plural.")]
    public enum CameraConfigStereoCameraUsage
    {

        RequireAndUse = 0x0001,

        DoNotUse = 0x0002,
    }


    public struct CameraConfig
    {
        internal CameraConfig(Vector2 imageSize, Vector2 textureSize, int minFPS, int maxFPS,
            CameraConfigDepthSensorUsage depthSensor, CameraConfigStereoCameraUsage stereoCamera)
            : this()
        {
            ImageSize = imageSize;
            TextureSize = textureSize;
            MinFPS = minFPS;
            MaxFPS = maxFPS;
            DepthSensorUsage = depthSensor;
            StereoCameraUsage = stereoCamera;
        }


        public Vector2 ImageSize { get; private set; }
        public Vector2 TextureSize { get; private set; }
        public int MinFPS { get; private set; }
        public int MaxFPS { get; private set; }
        public CameraConfigDepthSensorUsage DepthSensorUsage { get; private set; }
        public CameraConfigStereoCameraUsage StereoCameraUsage { get; private set; }
    }
}
