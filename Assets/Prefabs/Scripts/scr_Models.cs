using System;
using System.Collections.Generic;
using UnityEngine;

public static class scr_Models
{

    #region - Player -

    public enum PlayerStance
    {
        Stand,
        Crouch,
        Prone
    }

    [Serializable]
    public class PlayerSettingsModel
    {
        [Header("View Settings")]
        public float ViewXSensitivity;
        public float ViewYSensitivity;

        public bool ViewXInverted;
        public bool ViewYInverted;

        [Header("Movement Settings")]
        public bool SprintingHold;
        public float MovementSmoothing;

        [Header("Movement - Running")]
        public float RunningFowardSpeed;
        public float RunningStrafeSpeed;

        [Header("Movement - Walking")]
        public float WalkingFowardSpeed;
        public float WalkingBackwardSpeed;
        public float WalkingStrafeSpeed;

        [Header ("Jumping")]
        public float JumpingHeight;
        public float JumpingFalloff;
        public float FallingSmoothing;

        [Header("Speed Effectors")]
        public float SpeedEffector = 1;
        public float CrouchSpeedEffector;
        public float ProneSpeedEffector;
        public float FallingSpeedEffector;
    }

    [Serializable]
    public class CharacterStance
    {
        public float CameraHeight;
        public CapsuleCollider StanceCollider;
    }

    #endregion

    #region - Wepons -

    [Serializable]
    public class WeaponSettingsModel
    {
        [Header("Sway")]
        public float swayAmount;
        public bool SwayYInverted;
        public bool SwayXInverted;
        public float swaySmoothning;

    }

    #endregion
}
