using System;
using System.Net;
using log4net;

namespace WebServer.Responses
{
    public class OptionsHeader : Header
    {
        private const string HeaderAllow = "Allow: Get, Head, Options\r\n";

        private readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public OptionsHeader(HttpStatusCode httpStatusCode)
        {
            if (httpStatusCode != HttpStatusCode.OK &&
                httpStatusCode != HttpStatusCode.MethodNotAllowed &&
                httpStatusCode != HttpStatusCode.NotImplemented)
            {
                var errorMessage = "Options header cannot be send for status " + (int)httpStatusCode;
                _log.Fatal(errorMessage);
                throw new ArgumentException(errorMessage);
            }

            Value = string.Format(HeaderGeneralFormat,
                    (int)httpStatusCode + " " + httpStatusCode.ToString(),
                    HeaderAllow);
        }

        public override string Value { get; }
    }
}
