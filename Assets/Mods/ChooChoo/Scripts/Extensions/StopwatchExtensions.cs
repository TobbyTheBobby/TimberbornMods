using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace ChooChoo.Extensions
{
    public static class StopwatchExtensions
    {
        public static void LogTime(this Stopwatch stopwatch, string context)
        {
            Debug.Log($"{context}: ElapsedTicks: {stopwatch.ElapsedTicks}, {stopwatch.ElapsedTicks/10000}ms");
        }
    }
}