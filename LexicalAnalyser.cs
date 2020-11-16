using System;
using System.IO;
using System.Collections.Generic;

namespace Test2CSharp
{
    class LexicalAnalyser
    {
        static void Main(string[] args)
        {
            List<Token> tokens = new List<Token>();

            switch (args.Length) {
                case 1:
                    Console.WriteLine("Arguments are required. Please type in a file path: ");
                    Environment.Exit(-1);
                    break;
                case 2:
                    Console.WriteLine("Reading from file {0}", args[1]);
                    Tokenise(args[1], ref tokens);
                    break;
                default:
                    Console.WriteLine("Too many arguments");
                    Environment.Exit(-1);
                    break;
            }
        }

        private static void Tokenise(String filePath, ref List<Token> tokens)
        {
            try
            {
                using StreamReader sr = new StreamReader(filePath);
                string line = null;

                while ((line = sr.ReadLine()) != null)
                {
                    tokens.AddRange(TokeniseLine(line));
                }
            } 
            catch (Exception e)
            {
                Console.WriteLine("File could not be read");
                Console.WriteLine(e.Message);
            }
        }

        private static List<Token> TokeniseLine (String line)
        {
            List<Token> tokensInLine = new List<Token>();
            Token errorToken = new Token("", TokenType.ERROR);
            Char[] lineArray = line.ToCharArray();

            for(int i = 0; i < line.Length; i++)
            {
                if (lineArray[i] == '$')
                {
                    // Perl Identifier
                    tokensInLine.Add(GetIdentifier(lineArray, i));
                    goto EndOfLoop;
                }

                if (lineArray[i] == '"')
                {
                    // String Literal
                    tokensInLine.Add(GetString(lineArray, i + 1));
                    goto EndOfLoop;
                }

                if (lineArray[i] == '\'')
                {
                    // Character Literal
                    tokensInLine.Add(GetString(lineArray, i + 1));
                    goto EndOfLoop;
                }

                if (IsDigit(lineArray[i]))
                {
                    // Integer OR Float Literal
                    tokensInLine.Add(GetNum(lineArray, i));
                    goto EndOfLoop;
                }
                
                if (IsWhiteSpace(lineArray[i]))
                {
                    // Ignore
                    goto EndOfLoop;
                }

                Char nextChar = GetNext(lineArray, i);
                if (nextChar == Char.MinValue)
                {
                    tokensInLine.Add(errorToken);
                    goto EndOfLoop;
                }

                switch (lineArray[i])
                {
                    // Operators
                    case '+':
                        // Addition OR Increment
                        tokensInLine.Add(GetAdditionOrIncrement(new Char[] { lineArray[i], nextChar }));
                        break;
                    case ':':
                        // Assignment
                        if (nextChar == '=')
                        {
                            tokensInLine.Add(new Token(":=", TokenType.ASSIGNMENT));
                        }
                        else
                        {
                            goto default;
                        }
                        break;
                    case '-':
                        // Subtraction OR Decerement
                        tokensInLine.Add(GetSubtractionOrDecrement(new char[] { lineArray[i], nextChar }));
                        break;
                    case '/':
                        // Division
                        tokensInLine.Add(new Token("/", TokenType.DIVISION));
                        break;
                    case '*':
                        // Multiplication
                        tokensInLine.Add(new Token("*", TokenType.MULTIPLICATION));
                        break;
                    case '%':
                        // Modulus
                        tokensInLine.Add(new Token("%", TokenType.MODULO));
                        break;
                    case '&':
                        // Logical AND
                        if (nextChar == '&')
                        {
                            tokensInLine.Add(new Token("&&", TokenType.AND));
                        }
                        else
                        {
                            goto default;
                        }
                        break;
                    case '|':
                        // Logical OR
                        if (nextChar == '|')
                        {
                            tokensInLine.Add(new Token("||", TokenType.OR));
                        }
                        else
                        {
                            goto default;
                        }
                        break;
                    case '`':
                        // Logical NOT
                        tokensInLine.Add(new Token("`", TokenType.NOT));
                        break;
                    case '{':
                        // Open Code
                        tokensInLine.Add(new Token("{", TokenType.OPENCODE));
                        break;
                    case '}':
                        // Close Code
                        tokensInLine.Add(new Token("}", TokenType.CLOSECODE));
                        break;
                    case '(':
                        // Open Parameter
                        tokensInLine.Add(new Token("(", TokenType.OPENPARAM));
                        break;
                    case ')':
                        // Close Parameter
                        tokensInLine.Add(new Token(")", TokenType.CLOSEPARAM));
                        break;
                    default:
                        // Error
                        tokensInLine.Add(errorToken);
                        break;
                }

            EndOfLoop:;
            }
            return tokensInLine;
        }

        private static Token GetIdentifier (Char[] line, int index)
        {
            String newIdentifier = line[index].ToString();
            int currentIndex = index;
            TokenType tokenType = TokenType.IDENTIFIER;

            while (!IsWhiteSpace(line[index]))
            {
                newIdentifier += line[currentIndex];
                currentIndex += 1;
            }

            return new Token(newIdentifier, tokenType);
        }

        private static Char GetNext(Char[] line, int index)
        {
            if (index + 1 < line.Length)
            {
                return line[index + 1];
            }
            else return Char.MinValue;
        }

        private static Token GetNum(Char[] line, int index)
        {
            String newNumber = line[index].ToString();
            int currentIndex = index;
            TokenType nextToken = TokenType.INTEGER;

            while(IsDigit(line[currentIndex]) || line[currentIndex] == '.')
            {
                if (line[currentIndex] == '.')
                {
                    nextToken = TokenType.FLOAT;
                }
                newNumber += line[currentIndex];
                currentIndex += 1;
            }

            return new Token(newNumber, nextToken);
        }

        private static Token GetString(Char[] line, int index)
        {
            int currentIndex = index;
            String newString = line[currentIndex].ToString();

            while (GetNext(line, currentIndex) != '"')
            {
                currentIndex += 1;
                newString += line[currentIndex];
            }
            return new Token(newString, TokenType.STRING);
        }

        private static Token GetCharacter(Char[] line, int index)
        {
            int currentIndex = index;
            String newCharacter = line[index].ToString();

            while (GetNext(line, currentIndex) != '\'')
            {
                currentIndex += 1;
                newCharacter += line[currentIndex];
            }

            return new Token(newCharacter, TokenType.CHARACTER);
        }

        private static Token GetAdditionOrIncrement(Char[] toCheck)
        {
            if (toCheck[0] == '+' && toCheck[0] == toCheck[1])
            {
                return new Token("++", TokenType.INCREMENT);
            }
            else if (toCheck[0] == '+' && IsWhiteSpace(toCheck[1]))
            {
                return new Token("+", TokenType.ADDITION);
            }
            else return new Token("", TokenType.ERROR);
        }

        private static Token GetSubtractionOrDecrement(Char[] toCheck)
        {
            if (toCheck[0] == '-' && toCheck[0] == toCheck[1])
            {
                return new Token("--", TokenType.DECREMENT);
            }
            else if (toCheck[0] == '-' && IsWhiteSpace(toCheck[1]))
            {
                return new Token("-", TokenType.SUBTRACTION);
            }
            else return new Token("", TokenType.ERROR);
        }

        private static bool IsWhiteSpace(Char v)
        {
            return Char.IsWhiteSpace(v);
        }

        private static bool IsDigit(char v)
        {
            return Char.IsDigit(v);
        }
    }
}
