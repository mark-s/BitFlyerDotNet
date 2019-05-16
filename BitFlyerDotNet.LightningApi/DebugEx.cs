using System.Diagnostics;
using System.IO;
using System.Threading;

namespace BitFlyerDotNet.LightningApi
{
    public static class DebugEx
    {
        [Conditional("DEBUG")]
        public static void Trace()
        {
            Debug.WriteLine(CreatePrifix(new StackFrame(1, true)));
        }

        [Conditional("DEBUG")]
        public static void Trace(string format, params object[] args)
        {
            Debug.WriteLine(CreatePrifix(new StackFrame(1, true)) + format, args);
        }

        [Conditional("DEBUG")]
        public static void EnterMethod()
        {
            Debug.WriteLine(CreatePrifix(new StackFrame(1, true)) + "EnterMethod");
        }

        [Conditional("DEBUG")]
        public static void ExitMethod()
        {
            Debug.WriteLine(CreatePrifix(new StackFrame(1, true)) + "ExitMethod");
        }

        private static string CreatePrifix(StackFrame sf)
        {
            var method = sf.GetMethod();
            var methodname = method.DeclaringType + "." + method.Name;
            var fileName = Path.GetFileName(sf.GetFileName());
            var lineNum = sf.GetFileLineNumber();
            var threadId = Thread.CurrentThread.ManagedThreadId;
            return $"{fileName}({lineNum}):{methodname}({threadId}) ";
        }
    }
}