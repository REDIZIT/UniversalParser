using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace IngameDebugConsole
{
	public class LogsCopier : MonoBehaviour
    {
        [SerializeField] private DebugLogManager manager;

        public void Copy()
        {
            List<DebugLogEntry> entries = manager.collapsedLogEntries;

            StringBuilder b = new StringBuilder();

            foreach (DebugLogEntry entry in entries)
            {
                b.AppendLine($"[ {entry.logType} ]");
                b.AppendLine(entry.logString);
                b.AppendLine("\n( Stack trace )");
                b.AppendLine(entry.stackTrace);
                b.AppendLine("\n\n\n\n");
            }

            GUIUtility.systemCopyBuffer = b.ToString();
        }
    }
}