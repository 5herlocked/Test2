using System;
using System.Collections.Generic;

namespace Test2CSharp
{
    // while (conditional) { <code_block> }
    // if (conditional) { <code_block> }
    // <var> <ident> = <number>;
    // <const> <ident> = <number>;

    class RecursiveDescent
    {
        private Token Symbol;
        private List<Token> SymbolList;
        private int currentIndex = 0;
        private int Accept(TokenType type)
        {
            if (type.Equals(Symbol))
            {
                NextSym();
                return 1;
            }
            return 0;
        }

        private int Expect(TokenType type)
        {
            if (Accept(type) == 1)
            {
                return 1;
            }
            Error("Expect: Unexpected Symbol");
            return 0;
        }

        private void Factor()
        {
            if (Accept(TokenType.IDENTIFIER) == 1) // Identifier
            {
                
            }
            else if (Accept(TokenType.INTEGER) == 1 || Accept(TokenType.FLOAT) == 1) // Number
            {

            }
            else if (Accept(TokenType.OPENPARAM) == 1) // Left Parenthesis
            {

            }
            else
            {
                // Error
                Error("Factor: Syntax Error");
                NextSym();
            }
        }

        private void Term()
        {
            Factor();
            while (Accept(TokenType.MULTIPLICATION) == 1 || Accept(TokenType.DIVISION) == 1) // Multiplication OR Division
            {
                NextSym();
                Factor();
            }
        }

        private void Expression()
        {
            if (Symbol.GetToken() == TokenType.ADDITION || Symbol.GetToken() == TokenType.SUBTRACTION) // Plus Or Minus
            {
                Expression();
            }
            Term();
            while (Symbol.GetToken() == TokenType.ADDITION || Symbol.GetToken() == TokenType.SUBTRACTION)
            {
                NextSym();
                Term();
            }
        }

        private void Conditional ()
        {
            Expression();
            if (Symbol.GetToken() == TokenType.EQL || Symbol.GetToken() == TokenType.NEQ ||
                Symbol.GetToken() == TokenType.LSS || Symbol.GetToken() == TokenType.LEQ ||
                Symbol.GetToken() == TokenType.GTR || Symbol.GetToken() == TokenType.GEQ) // Valid Conditionals
            {
                NextSym(); // Get Next Symbol
                Expression(); // Expect Expression
            }
            else
            {
                // Error
                Error("Condition: Invalid Operator");
                NextSym();
            }
        }

        private void Statement()
        {
            if (Accept(TokenType.IDENTIFIER) == 1) // Identifier
            {
                Expect(TokenType.ASSIGNMENT);
                Expression();
            }
            else if (Accept(TokenType.IF) == 1) // If Statement
            {
                Conditional();
                Expect(TokenType.OPENCODE); // if () {}
                Statement();

            }
            else if (Accept(TokenType.WHILE) == 1) // While Statement
            {
                Conditional();
                Expect(TokenType.OPENCODE); // if () {}
                Statement();
            }
            else
            {
                // Error
                Error("Statement: Syntax Error");
                NextSym();
            }
        }

        private void Block()
        {
            if (Accept(TokenType.IDENTIFIER) == 1) // Variable Declaration
            {
                do
                {
                    Expect(TokenType.IDENTIFIER); // Identifier
                } while (Accept(TokenType.COMMA) == 1); // Commas (Multiple Declarations)
                Expect(TokenType.SEMICOLON); // Semicolon
            }
            if (Accept(TokenType.OPENCODE) == 1)
            {
                while (Accept(TokenType.CLOSECODE) == 0)
                {
                    Statement();
                    Block();
                    Expect(TokenType.SEMICOLON);
                }
                Statement(); // Statement
            }
        }

        private void NextSym()
        {
            currentIndex += 1;
            Symbol = SymbolList[currentIndex];
        }

        private void Error(String msg)
        {
            Console.WriteLine(msg);
        }

        public void Program()
        {
            NextSym(); // Expect While
            Block(); // Expect Expressions
            Expect(TokenType.CLOSECODE); // Expect Close Block
        }
    }
}
