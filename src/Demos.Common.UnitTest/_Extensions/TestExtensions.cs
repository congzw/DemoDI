using System;
using System.Collections;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace Demos.Common
{
    public static class TestExtensions
    {
        public static void ShouldThrows<T>(this Action action) where T : Exception
        {
            AssertHelper.ShouldThrows<T>(action);
        }

        public static void ShouldEqual(this object value, object expectedValue)
        {
            string message = string.Format("Should {0} equals {1}", value, expectedValue);
            Assert.AreEqual(expectedValue, value, message.WithKoPrefix());
            AssertHelper.WriteLineOk(message);
        }

        public static void ShouldNotEqual(this object value, object expectedValue)
        {
            string message = string.Format("Should {0} not equals {1}", value, expectedValue);
            Assert.AreNotEqual(expectedValue, value, message.WithKoPrefix());
            AssertHelper.WriteLineOk(message);
        }


        public static void ShouldSame(this object value, object expectedValue)
        {
            if (value == null || expectedValue == null)
            {
                Assert.AreNotSame(expectedValue, value);
                return;
            }
            string message = string.Format("Should Same [{0}] => <{1}> : <{2}>", value.GetType().Name, value.GetHashCode(), expectedValue.GetHashCode());
            Assert.AreSame(expectedValue, value, message.WithKoPrefix());
            AssertHelper.WriteLine(message.WithOkPrefix());
        }

        public static void ShouldNotSame(this object value, object expectedValue)
        {
            if (value == null || expectedValue == null)
            {
                Assert.AreNotSame(expectedValue, value);
                return;
            }
            string message = string.Format("Should Not Same [{0}] => <{1}> : <{2}>", value.GetType().Name, value.GetHashCode(), expectedValue.GetHashCode());
            Assert.AreNotSame(expectedValue, value, message.WithKoPrefix());
            AssertHelper.WriteLine(message.WithOkPrefix());
        }
        
        public static object ShouldNull(this object value, string appendMessage = null)
        {
            AssertHelper.WriteLineForShouldBeNull(value, appendMessage);
            Assert.IsNull(value);
            return value;
        }

        public static object ShouldNotNull(this object value, string appendMessage = null)
        {
            AssertHelper.WriteLineForShouldBeNotNull(value, appendMessage);
            Assert.IsNotNull(value);
            return value;
        }
        
        public static void ShouldTrue(this bool result, string appendMessage = null)
        {
            AssertHelper.WriteLineForShouldBeTrue(result, appendMessage);
            Assert.IsTrue(result);
        }

        public static void ShouldFalse(this bool result, string appendMessage = null)
        {
            AssertHelper.WriteLineForShouldBeFalse(result, appendMessage);
            Assert.IsFalse(result);
        }

        public static void LogHashCode(this object value)
        {
            string message = string.Format("{0} <{1}>", value.GetType().Name, value.GetHashCode());
            AssertHelper.WriteLine(message);
        }
        public static void LogHashCodeWiths(this object value, object value2)
        {
            string message = string.Format("{0} <{1}> {2} {3}<{4}>", value.GetHashCode(), value.GetType().Name, value == value2 ? "==" : "!=", value2.GetHashCode(), value2.GetType().Name);
            AssertHelper.WriteLine(message);
        }

        public static void Log(this object value)
        {
            if (value == null)
            {
                Debug.WriteLine("null");
            }

            if (value is string)
            {
                Debug.WriteLine(value);
                return;
            }

            var items = value as IEnumerable;
            if (items != null)
            {
                foreach (var item in items)
                {
                    Debug.WriteLine(item);
                }
                return;
            }
            Debug.WriteLine(value);
        }

        public static string WithOkPrefix(this string value)
        {
            return AssertHelper.PrefixOk(value);
        }
        public static string WithKoPrefix(this string value)
        {
            return AssertHelper.PrefixKo(value);
        }
        public static string WithPrefix(this string value, bool isOk = true)
        {
            return AssertHelper.PrefixKo(value);
        }

        public static void LogJson(this object value)
        {
            if (value == null)
            {
                Debug.WriteLine("null");
            }
            Debug.WriteLine(JsonConvert.SerializeObject(value));
        }

        public static string ObjectInfo(this object obj)
        {
            return string.Format("<{0},{1}>", obj.GetType().Name, obj.GetHashCode());
        }

        public static MessageResult ShouldSuccess(this MessageResult messageResult, string addtionMessage = null)
        {
            return Should(messageResult, true, addtionMessage);
        }

        public static MessageResult ShouldFail(this MessageResult messageResult, string addtionMessage = null)
        {
            return Should(messageResult, false, addtionMessage);
        }

        public static MessageResult Should(this MessageResult messageResult, bool expectSuccess, string addtionMessage = null)
        {
            var message = messageResult.Message + " => ";
            if (!string.IsNullOrWhiteSpace(addtionMessage))
            {
                message += addtionMessage;
            }
            if (expectSuccess)
            {
                messageResult.Success.ShouldTrue(message);
            }
            else
            {
                messageResult.Success.ShouldFalse(message);
            }
            return messageResult;
        }
    }
}