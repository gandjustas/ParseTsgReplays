using System;

namespace ParseTsgReplays
{
    internal class ParseException : Exception
    {
        private ReplayParser parser;
        public ParseException(string message, ReplayParser parser) : base(message)
        {
            this.parser = parser;
        }

        public override string Message
        {
            get
            {
                return string.Format(base.Message + " at line {0} position {1}", parser.Line, parser.Postion);
            }
        }

    }
}