using System;
using System.Collections.Generic;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace ModBuilding.Editor
{
    public abstract class ModBuilderStopwatch
    {
        public static readonly Stopwatch Global = Stopwatch.StartNew();

        public static readonly Stopwatch Local = Stopwatch.StartNew();

        private static readonly List<LogMessage> Messages = new();

        public static void LogGlobal()
        {
            Global.Stop();
            Messages.Add(new LogMessage("Global", Global.Elapsed));
            Global.Start();
        } 
        
        public static void LogLocal(string message)
        {
            Global.Stop();
            Local.Stop();
            Messages.Add(new LogMessage(message, Global.Elapsed, Local.Elapsed));
            Global.Start();
            Local.Start();
        }

        public static void LogAll()
        {
            foreach (var logMessage in Messages)
            {
                var localTime = "";
                if (logMessage.LocalTime != null)
                    localTime = $@", Local time: {logMessage.LocalTime:mm\:ss\.ff}";
                Debug.Log($@"{logMessage.Message}: Global time: {logMessage.GlobalTime:mm\:ss\.ff}{localTime}");
            }
        }

        private class LogMessage
        {
            public readonly string Message;
            public readonly TimeSpan GlobalTime;
            public readonly TimeSpan? LocalTime;

            public LogMessage(string message, TimeSpan globalTime, TimeSpan? localTime = null)
            {
                Message = message;
                GlobalTime = globalTime;
                LocalTime = localTime;
            }
        }
    }
}