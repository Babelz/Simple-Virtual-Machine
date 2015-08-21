using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVMAssembler
{
    public sealed class Logger
    {
        #region Fields
        private readonly List<string> errors;
        private readonly List<string> warnings;
        private readonly List<string> messages;
        #endregion

        #region Properties
        public bool HasErrors
        {
            get
            {
                return errors.Count > 0;
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
            errors = new List<string>();
            warnings = new List<string>();
            messages = new List<string>();
        }

        public void LogError(string error)
        {
            errors.Add(error);
        }
        public void LogWarning(string warning)
        {
            warnings.Add(warning);
        }
        public void LogMessage(string message)
        {
            messages.Add(message);
        }

        public void ClearErrors()
        {
            errors.Clear();
        }
        public void ClearWarnings()
        {
            warnings.Clear();
        }
        public void ClearMessages()
        {
            messages.Clear();
        }

        public IEnumerable<string> GetErrors()
        {
            return errors;
        }
        public IEnumerable<string> GetWarnings()
        {
            return warnings;
        }
        public IEnumerable<string> GetMessages()
        {
            return messages;
        }
    }
}
