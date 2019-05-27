using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CQC
{
    /// <summary>
    /// This class is unused but kept for future potential updates.
    /// It extracts pitch/yaw/roll from a rotation matrix.
    /// </summary>
    static class MatrixHelper
    {
        //public static void ExtractYawPitchRoll(Matrix matrix, out float yaw, out float pitch, out float roll)
        //{
        //    yaw = (float)System.Math.Atan2(matrix.M13, matrix.M33);
        //    pitch = (float)System.Math.Asin(-matrix.M23);
        //    roll = (float)System.Math.Atan2(matrix.M21, matrix.M22);
        //}


        // Takes a rotation matrix and returns the yaw pitch roll as a vector3
        public static Vector3 ExtractYawPitchRoll(Matrix matrix)
        {
            float yaw = (float)System.Math.Atan2(-matrix.M13, matrix.M33);
            float pitch = (float)System.Math.Asin(matrix.M23);
            float roll = (float)System.Math.Atan2(matrix.M21, matrix.M22);

            return new Vector3(pitch, yaw, roll);
        }
    }
}
