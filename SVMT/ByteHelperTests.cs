using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SVM;
using System.Diagnostics;

namespace SVMT
{
    [TestClass]
    public class ByteHelperTests
    {
        [TestMethod]
        public void ToBytesTests()
        {
            for (int i = 0; i < 100000; i++)
            {
                byte[] bytes = ByteHelper.ToBytes(i, 4);

                int number = ByteHelper.ToInt(bytes);

                Assert.AreEqual(i, number);
            }
        }

        [TestMethod]
        public void AddInt32Tests()
        {
            byte[] lhs = null;
            byte[] rhs = null;

            for (int i = 0; i < 100000; i++)
            {
                lhs = ByteHelper.ToBytes(i * 2, 4);
                rhs = ByteHelper.ToBytes(i * 4, 4);

                int result = ByteHelper.AddInt(lhs, rhs);

                Assert.AreEqual((i * 2) + (i * 4), result, string.Format("i: {0} - res: {1}", i * 2, result));
            }
        }

        [TestMethod]
        public void SubtractInt32Tests()
        {
            byte[] lhs = null;
            byte[] rhs = null;

            for (int i = 0; i < 100000; i++)
            {
                lhs = ByteHelper.ToBytes(i * 2, 4);
                rhs = ByteHelper.ToBytes(i, 4);

                int result = ByteHelper.SubtractInt(lhs, rhs);

                Assert.AreEqual(i * 2 - i, result);
            }
        }

        [TestMethod]
        public void ToIntTests()
        {
            for (int i = 0; i < 100000; i++)
            {
                byte[] bytes = ByteHelper.ToBytes(i, 4);

                int value = ByteHelper.ToInt(bytes);

                Assert.AreEqual(i, value);
            }
        }

        [TestMethod]
        public void AddBytesTests()
        {
            byte[] lhs = null;
            byte[] rhs = null;
            byte[] real = null;

            for (int i = 0; i < 100000; i++)
            {
                lhs = ByteHelper.ToBytes(i, 4);
                rhs = ByteHelper.ToBytes(i, 4);
                real = ByteHelper.ToBytes(i * 2, 4);

                byte[] result = ByteHelper.AddBytes(lhs, rhs);

                for (int j = 0; j < result.Length; j++)
                {
                    Assert.AreEqual(result[j], real[j], string.Format("Real: {0} - Got: {1}", ByteHelper.ToBinaryString(real), ByteHelper.ToBinaryString(result)));
                }
            }
        }

        [TestMethod]
        public void SubtractBytesTests()
        {
            byte[] lhs = null;
            byte[] rhs = null;

            for (int i = 0; i < 100000; i++)
            {
                lhs = ByteHelper.ToBytes(i * 2, 4);
                rhs = ByteHelper.ToBytes(i, 4);

                byte[] bytes = ByteHelper.SubtractBytes(lhs, rhs);

                int result = ByteHelper.ToInt(bytes);

                Assert.AreEqual(i * 2 - i, result);
            }
        }
    }
}
