using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;

namespace FrooxEngine.LogiX.Utility
{
    public enum LogSeverity
    {
        Log,
        Warning,
        Error
    }

    [Category("LogiX/Utility")]
    [NodeName("Write to Log")]
    public class WriteToLog<T> : LogixNode
    {
        public readonly Input<T> Value;
        public readonly Input<LogSeverity> Severity;
        public readonly Input<string> Tag;

        [ImpulseTarget]
        public void Write()
        {
            switch (Severity.Evaluate())
            {
                case LogSeverity.Log:
                    UniLog.Log(Tag.EvaluateRaw() + Value.EvaluateRaw()?.ToString());
                    break;
                case LogSeverity.Warning:
                    UniLog.Warning(Tag.EvaluateRaw() + Value.EvaluateRaw()?.ToString());
                    break;
                case LogSeverity.Error:
                    UniLog.Error(Tag.EvaluateRaw() + Value.EvaluateRaw()?.ToString());
                    break;
            }
        }
    }
}
