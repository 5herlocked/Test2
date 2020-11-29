namespace Test2CSharp
{
    public enum TokenType
    {
        WHILE,
        IF,
        IDENTIFIER,
        WHITESPACE,
        STRING,
        INTEGER,
        CHARACTER,
        FLOAT,
        ADDITION,
        ASSIGNMENT,
        SUBTRACTION,
        DIVISION,
        MULTIPLICATION,
        INCREMENT,
        DECREMENT,
        MODULO,
        EQL,
        NEQ,
        LSS,
        LEQ,
        GTR,
        GEQ,
        AND,
        OR,
        NOT,
        OPENCODE,
        CLOSECODE,
        OPENPARAM,
        CLOSEPARAM,
        SEMICOLON,
        COMMA,
        ERROR,
    }

    class Token
    {

        private TokenType type;
        private string value;

        public Token(string value, TokenType type)
        {
            this.type = type;
            this.value = value;
        }

        public TokenType GetToken()
        {
            return type;
        }

        public string GetValue()
        {
            return value;
        }

        public override string ToString()
        {
            return "TOKEN-TYPE: " + type.ToString("g") + "VALUE: " + value;
        }
    }
}
