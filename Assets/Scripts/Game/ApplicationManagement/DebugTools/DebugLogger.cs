using System;
using System.IO;
using UnityEngine;

namespace ApplicationManagement.DebugTools {
    public class DebugLogger : IDebugLogger {

        #region Variables

        public static DebugLogger Instance { get; set; }

        private readonly string logPath = Application.dataPath + "/DebugLogs";
        private string curLogFile;
        private readonly bool writeToFile;

        #endregion

        public DebugLogger(bool useLogFile) {
#if UNITY_EDITOR
            writeToFile = useLogFile;
#else
        writeToFile = true;
#endif
        }

        public void LogInfo(params string[] infoLog) {
            string infoLine = string.Format("[I] {0:G}: ", DateTime.Now);

            foreach(string s in infoLog)
                infoLine += s;

            if(writeToFile) {
                StreamWriter infoWriter = GetLogFile();

                infoWriter.WriteLine(infoLine);
                infoWriter.Close();
            } else
                Debug.Log(infoLine);
        }

        public void LogWarning(params string[] warningLog) {
            string warningLine = string.Format("[W] {0:G}: ", DateTime.Now);

            foreach(string s in warningLog)
                warningLine += s;

            if(writeToFile) {
                StreamWriter warningWriter = GetLogFile();

                warningWriter.WriteLine(warningLine);
                warningWriter.Close();
            } else
                Debug.LogWarning(warningLine);
        }

        public void LogError(int severity = 0, params string[] errorLog) {
            string errorLine = string.Format("[E{0}] {1:G}: ", severity, DateTime.Now);

            foreach(string s in errorLog)
                errorLine += s;

            if(writeToFile) {
                StreamWriter errorWriter = GetLogFile();

                errorWriter.WriteLine(errorLine);
                errorWriter.Close();
            } else
                Debug.LogError(errorLine);

            if(severity > 0) {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
//Add ErrorQuit Dialogue
            Application.Quit();
#endif
            }
        }

        private StreamWriter GetLogFile() {
            if(!Directory.Exists(logPath))
                Directory.CreateDirectory(logPath);

            curLogFile = CreateFileName();

            return File.AppendText(logPath + "/" + curLogFile);
        }

        private static string CreateFileName() {
            DateTime curDateTime = DateTime.Now;

            return string.Format("{0}_{1}_{2}_Log.txt",
                curDateTime.Year, curDateTime.Month, curDateTime.Day);
        }
    }
}