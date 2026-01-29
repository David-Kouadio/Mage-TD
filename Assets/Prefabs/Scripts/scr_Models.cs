using System;
using System.Collections.Generic;
using UnityEngine;

public static class scr_Models
{

    #region - Player -

    [Serializable]
    public class PlayerSettingsModel
    {
        [Header("View Settings")]
        public float ViewXSensitivity;
        public float ViewYSensitivity;

        public bool ViewXInverted;
        public bool ViewYInverted;

        [Header("Movement")]
        public float WalkingFowardSpeed;
        public float WalkingBackwardSpeed;
        public float WalkingStrafeSpeed;
    }

    #endregion
}
