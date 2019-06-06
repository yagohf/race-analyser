using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Yagohf.Gympass.RaceAnalyser.UnitTests
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ExpectedExceptionWithMessageAttribute : ExpectedExceptionBaseAttribute
    {
        /// <summary>
        /// Tipo da exceção.
        /// </summary>
        public Type ExceptionType { get; set; }
        /// <summary>
        /// Mensagem esperada da exceção.
        /// </summary>
        public string ExpectedMessage { get; set; }
        /// <summary>
        /// Mensagens esperadas na exceção (separadas na exception por ";").
        /// </summary>
        public string[] ExpectedMessages { get; set; }

        public ExpectedExceptionWithMessageAttribute(Type exceptionType)
        {
            this.ExceptionType = exceptionType;
        }

        public ExpectedExceptionWithMessageAttribute(Type exceptionType, string expectedMessage)
        {
            this.ExceptionType = exceptionType;
            this.ExpectedMessage = expectedMessage;
            this.ExpectedMessages = null;
        }

        public ExpectedExceptionWithMessageAttribute(Type exceptionType, string[] expectedMessages)
        {
            this.ExceptionType = exceptionType;
            this.ExpectedMessage = null;
            this.ExpectedMessages = expectedMessages;
        }

        protected override void Verify(Exception e)
        {
            if (e.GetType() != this.ExceptionType)
            {
                Assert.Fail(String.Format(
                                "ExpectedExceptionWithMessageAttribute failed. Expected exception type: {0}. Actual exception type: {1}. Exception message: {2}",
                                this.ExceptionType.FullName,
                                e.GetType().FullName,
                                e.Message
                                )
                            );
            }

            var actualMessage = e.Message.Trim();

            if (this.ExpectedMessage != null)
            {
                Assert.AreEqual(this.ExpectedMessage, actualMessage);
            }
            else if (this.ExpectedMessages != null)
            {
                string[] actualMessages = actualMessage.Split(';');
                var notFoundMessages = from em in ExpectedMessages
                                       where !actualMessages.Contains(em)
                                       select em;

                if (notFoundMessages.Any())
                {
                    Assert.Fail(string.Format(
                        "Some expected messages were not returned by method execution. Expected: {0}. Actual: {1}. Missing: {2}",
                        string.Join(", ", this.ExpectedMessages),
                        string.Join(", ", actualMessages),
                        string.Join(", ", notFoundMessages)));
                }
            }

            Console.Write("ExpectedExceptionWithMessageAttribute:" + e.Message);
        }
    }
}
