using SVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SVMAssembler
{
    public static class StringHelper
    {
        #region Fields
        private static readonly Dictionary<string, byte> registerMappings;
        #endregion

        static StringHelper()
        {
            registerMappings = new Dictionary<string, byte>();

            IEnumerable<FieldInfo> fieldInfos = typeof(Registers).GetFields()
                                                                 .Where(f => f.Name.StartsWith("R"));

            foreach (FieldInfo fieldInfo in fieldInfos) registerMappings.Add(fieldInfo.Name.ToLower(), (byte)fieldInfo.GetValue(null));
        }

        public static byte WordToByte(string size)
        {
            size = size.ToLower();

            if (size == "hword") return Sizes.HWORD;
            if (size == "word")  return Sizes.WORD;
            if (size == "lword") return Sizes.LWORD;
            if (size == "dword") return Sizes.DWORD;

            return 0;
        }
        public static byte RegisterToByte(string register)
        {
            return registerMappings[register];
        }

        /// <summary>
        /// Converts given string to int or float and returns
        /// that values bytes.
        /// </summary>
        /// <param name="value">string to convert into int or float</param>
        public static byte[] ToBytes(string value, int bytesCount)
        {
            byte[] results = new byte[bytesCount];
            float f = 0.0f;
            int i = 0;

            if      (float.TryParse(value, out f))  ByteHelper.ToBytes(f, results);
            else if (int.TryParse(value, out i))    ByteHelper.ToBytes(i, results);

            return results;
        }
    }
}
