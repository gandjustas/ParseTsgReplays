using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseTsgReplays
{
    // Грамматика:
    // Стартовый элемент = E
    // M = [  ElList ]
    // ElList = E(,E)*
    // E = WS* E` WS* 
    // E` = S | N | M
    // S = "любые символы"
    // N = (+|-)?d+(.d+)(e(+|-)d+)?

    public class ReplayParser
    {

        TextReader text;
        char c;
        Stack<Tuple<int, int, bool>> stack = new Stack<Tuple<int, int, bool>>();

        int level = 0;
        int index = -1;

        int line = 1;
        int position = 1;
        bool isEof = false;

        bool hasMoreElements = true;

        StringBuilder buffer = new StringBuilder(32);
        StringBuilder tempBuffer = new StringBuilder(32);

        public int Level { get { return level; } }
        public int Index { get { return index; } }

        public int Line { get { return line; } }
        public int Postion { get { return position; } }

        public ReplayParser(TextReader text)
        {
            this.text = text;
            c = (char)text.Peek();
        }

        internal char Next
        {
            get
            {
                return c;
            }
        }

        internal void Consume(char c)
        {
            if (Next != c) throw new ParseException("Expected '" + c, this);
            Consume();
        }
        internal void Consume()
        {
            if (isEof)
            {
                throw new ParseException("Unexpected end of file", this);
            }

            var x = text.Read();
            position++;

            if (x == 10 || x == 13)
            {
                c = (char)text.Peek();
                if (Next == 10 || Next == 13)
                {
                    x = text.Read();
                }
                NewLine();
            }
            if (text.Peek() == -1)
            {
                isEof = true;
                hasMoreElements = false;
            }
            c = (char)text.Peek();
        }

        private void NewLine()
        {
            line++;
            position = 1;
        }

        private void Push()
        {
            stack.Push(Tuple.Create(level, index, hasMoreElements));
            index = -1;

        }
        private void Pop()
        {
            var t = stack.Pop();
            level = t.Item1;
            index = t.Item2;
            hasMoreElements = t.Item3;
        }

        public void Down()
        {
            Ws();
            Consume('[');
            Push();
            Ws();
            hasMoreElements = !isEof && Next != ']';
        }
        public void Up()
        {
            while (HasMoreElements) SkipElement();
            Consume(']');
            Pop();
            Advance();
        }

        private void Advance()
        {
            if (isEof) return;
            if (!HasMoreElements) throw new ParseException("End of array elements", this);

            Ws();
            if (Next == ']') hasMoreElements = false;
            if (Next == ',')
            {
                hasMoreElements = true;
                index++;
                Consume(',');
                Ws();
            }
        }

        public void SkipElement()
        {
            Ws();
            if (Next == '"') ReadString();
            else if (IsNumber()) ReadDouble();
            else if (Next == '[')
            {
                Down();
                while (HasMoreElements) SkipElement();
                Up();
            }

        }

        private bool IsNumber()
        {
            return char.IsDigit(Next) || Next == '-' || Next == '+';
        }

        private void Ws()
        {
            while (char.IsWhiteSpace(Next)) Consume();
        }

        public string ReadString()
        {
            Ws();
            buffer.Clear();
            if (Next != '"') throw new ParseException("Expected '\"'", this);
            Consume();


            while (true)
            {
                while (Next != '"')
                {
                    buffer.Append(Next);
                    Consume();
                }

                tempBuffer.Clear();
                tempBuffer.Append(Next);
                Consume();
                while (char.IsWhiteSpace(Next))
                {
                    tempBuffer.Append(Next);
                    Consume();
                }

                if (Next == ',' || Next == ']') break;
                buffer.Append(tempBuffer.ToString());
            }
            Advance();

            return buffer.ToString();
        }

        public int ReadInt()
        {
            Ws();
            buffer.Clear();
            Element();
            Advance();
            return int.Parse(buffer.ToString());
        }


        public double ReadDouble()
        {
            Ws();
            buffer.Clear();
            Element();
            Advance();
            double result = double.NaN;
            double.TryParse(buffer.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out result);
            return result;
        }

        private void Element()
        {
            while (Next != ',' && Next != ']')
            {
                buffer.Append(Next);
                Consume();
            }
        }

        public bool HasMoreElements
        {
            get
            {
                return hasMoreElements && !isEof;
            }
        }

    }
}
