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
                byte[] bytes = new byte[4];
                ByteHelper.ToBytes(i, bytes);

                int number = ByteHelper.ToInt(bytes);

                Assert.AreEqual(i, number);
            }
        }

        [TestMethod]
        public void AddInt32Tests()
        {
            byte[] lhs = new byte[4];
            byte[] rhs = new byte[4];
            byte[] bytes = new byte[4];

            for (int i = 0; i < 100000; i++)
            {
                ByteHelper.ToBytes(i * 2, lhs);
                ByteHelper.ToBytes(i * 4, rhs);

                ByteHelper.AddIntBytes(lhs, rhs, bytes);

                int result = ByteHelper.ToInt(bytes);

                Assert.AreEqual((i * 2) + (i * 4), result, string.Format("i: {0} - res: {1}", i * 2, result));
            }
        }

        [TestMethod]
        public void SubtractInt32Tests()
        {
            byte[] lhs = new byte[4];
            byte[] rhs = new byte[4];
            byte[] bytes = new byte[4];

            for (int i = 0; i < 100000; i++)
            {
                ByteHelper.ToBytes(i * 2, lhs);
                ByteHelper.ToBytes(i, rhs);

                ByteHelper.SubtractIntBytes(lhs, rhs, bytes);

                int result = ByteHelper.ToInt(bytes);

                Assert.AreEqual(i * 2 - i, result);
            }
        }

        [TestMethod]
        public void ToIntTests()
        {
            byte[] bytes = new byte[4];

            for (int i = 0; i < 100000; i++)
            {
                ByteHelper.ToBytes(i, bytes);

                int value = ByteHelper.ToInt(bytes);

                Assert.AreEqual(i, value);
            }
        }

        [TestMethod]
        public void AddBytesTests()
        {
            byte[] lhs = new byte[4];
            byte[] rhs = new byte[4];
            byte[] real = new byte[4];
            byte[] result = new byte[4];

            for (int i = 0; i < 100000; i++)
            {
                ByteHelper.ToBytes(i, lhs);
                ByteHelper.ToBytes(i, rhs);
                ByteHelper.ToBytes(i * 2, real);

                ByteHelper.AddIntBytes(lhs, rhs, result);

                for (int j = 0; j < result.Length; j++)
                {
                    Assert.AreEqual(result[j], real[j], string.Format("Real: {0} - Got: {1}", ByteHelper.ToBinaryString(real), ByteHelper.ToBinaryString(result)));
                }
            }
        }

        [TestMethod]
        public void SubtractBytesTests()
        {
            byte[] lhs = new byte[4];
            byte[] rhs = new byte[4];
            byte[] bytes = new byte[4];

            for (int i = 0; i < 100000; i++)
            {
                ByteHelper.ToBytes(i * 2, lhs);
                ByteHelper.ToBytes(i, rhs);

                ByteHelper.SubtractIntBytes(lhs, rhs, bytes);

                int result = ByteHelper.ToInt(bytes);

                Assert.AreEqual(i * 2 - i, result);
            }
        }
    }
}
