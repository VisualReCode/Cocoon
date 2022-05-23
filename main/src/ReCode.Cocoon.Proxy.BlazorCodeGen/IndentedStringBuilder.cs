using System;
using System.Text;

namespace ReCode.Cocoon.Proxy.BlazorCodeGen
{
    public class IndentedStringBuilder
    {
        private readonly int _indentSpaces;
        private readonly StringBuilder _builder = new();
        private bool _newLine = true;

        public IndentedStringBuilder(int indentSpaces = 4)
        {
            _indentSpaces = indentSpaces;
        }

        public int Indent { get; set; }

        public IndentedStringBuilder Append(string value)
        {
            if (_newLine)
            {
                if (Indent > 0)
                {
                    _builder.Append(new string(' ', _indentSpaces * Indent));
                    _newLine = false;
                }
            }

            _builder.Append(value);

            return this;
        }

        public IndentedStringBuilder AppendLine(string value)
        {
            Append(value);
            _builder.AppendLine();
            _newLine = true;
            return this;
        }
        
        public IndentedStringBuilder AppendLine()
        {
            _builder.AppendLine();
            _newLine = true;
            return this;
        }

        public IDisposable OpenBrace() => new Block(this);

        private class Block : IDisposable
        {
            private readonly IndentedStringBuilder _builder;

            public Block(IndentedStringBuilder builder)
            {
                _builder = builder;
                _builder.AppendLine("{");
                _builder.Indent++;
            }

            public void Dispose()
            {
                _builder.Indent--;
                _builder.AppendLine("}");
            }
        }

        public override string ToString() => _builder.ToString();
    }
}