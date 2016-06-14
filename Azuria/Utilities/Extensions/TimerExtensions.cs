﻿using System.Timers;

namespace Azuria.Utilities.Extensions
{
    internal static class TimerExtensions
    {
        #region

        internal static void Reset(this Timer timer)
        {
            timer.Stop();
            timer.Start();
        }

        #endregion
    }
}