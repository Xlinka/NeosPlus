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

    [Category("LogiX/NeosPlus/Utility")]
    [NodeName("Write to Log")]
    public class WriteToLog : LogixNode
    {
        public readonly Input<string> Value;
        public readonly Input<LogSeverity> Severity;
        public readonly Input<string> Tag;
        public readonly Input<User> HandlingUser;

        [ImpulseTarget]
        public void Write()
        {
            User user = HandlingUser.Evaluate(base.LocalUser);
            if (user != null)
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
}
