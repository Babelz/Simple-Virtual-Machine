using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVMAssembler
{
    public enum LogLevel : byte
    {
        Error,
        Warning,
        Message
    }

    public sealed class Logger
    {
        #region Fields
        private readonly List<string>[] buffers;
        #endregion

        #region Properties
        public bool HasErrors
        {
            get
            {
                return buffers[(uint)LogLevel.Error].Count > 0;
            }
        }
        #endregion

        #region Singleton
        private readonly static Logger instance;

        public static Logger Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion

        static Logger()
        {
            instance = new Logger();
        }

        private Logger()
        {
            buffers = new List<string>[]
            {
                new List<string>(),
                new List<string>(),
                new List<string>()
            };
        }

        public void Log(string message, LogLevel logLevel)
        {
            buffers[(uint)logLevel].Add(message);
        }
        public IEnumerable<string> GetMessages(LogLevel logLevel)
        {
            return buffers[(uint)logLevel];
        }
        public void ClearMessages(LogLevel logLevel)
        {
            buffers[(uint)logLevel].Clear();
        }
    }
}
