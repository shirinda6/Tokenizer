using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SimpleCompiler
{
    class Compiler
    {


        public Compiler()
        {
        }

        //reads a file into a list of strings, each string represents one line of code
        public List<string> ReadFile(string sFileName)
        {
            StreamReader sr = new StreamReader(sFileName);
            List<string> lCodeLines = new List<string>();
            while (!sr.EndOfStream)
            {
                lCodeLines.Add(sr.ReadLine());
            }
            sr.Close();
            return lCodeLines;
        }



        //Computes the next token in the string s, from the begining of s until a delimiter has been reached. 
        //Returns the string without the token.
        private string Next(string s, char[] aDelimiters, out string sToken, out int cChars)
        {
            cChars = 1;
            sToken = s[0] + "";
            if (aDelimiters.Contains(s[0]))
                return s.Substring(1);
            int i = 0;
            for (i = 1; i < s.Length; i++)
            {
                if (aDelimiters.Contains(s[i]))
                    return s.Substring(i);
                else
                    sToken += s[i];
                cChars++;
            }
            return null;
        }

        //Splits a string into a list of tokens, separated by delimiters
        private List<string> Split(string s, char[] aDelimiters)
        {
            List<string> lTokens = new List<string>();
            while (s.Length > 0)
            {
                string sToken = "";
                int i = 0;
                for (i = 0; i < s.Length; i++)
                {
                    if (aDelimiters.Contains(s[i]))
                    {
                        if (sToken.Length > 0)
                            lTokens.Add(sToken);
                        lTokens.Add(s[i] + "");
                        break;
                    }
                    else
                        sToken += s[i];
                }
                if (i == s.Length)
                {
                    lTokens.Add(sToken);
                    s = "";
                }
                else
                    s = s.Substring(i + 1);
            }
            return lTokens;
        }

        //This is the main method for the Tokenizing assignment. 
        //Takes a list of code lines, and returns a list of tokens.
        //For each token you must identify its type, and instantiate the correct subclass accordingly.
        //You need to identify the token position in the file (line, index within the line).
        //You also need to identify errors, in this assignement - illegal identifier names.
        public List<Token> Tokenize(List<string> lCodeLines)
        {
            List<Token> lTokens = new List<Token>();
            int lineNumber = 0;
            int pos=0;       
            int index2 = 0;
            char[] Separators = new char[] { '*', '+', '-', '/', '<', '>', '&', '=', '|', '!', '(', ')', '[', ']', '{', '}', ',', ';', ' ','\t' };

            for (int i = 0; i < lCodeLines.Count; i++)
            {
                string sline = lCodeLines[i];
                pos = 0;
                
                if (sline.Length > 1)
                {
                    if (sline.Substring(0, 2) == "//")
                    {
                        lineNumber++;
                        continue;
                    }
                }
                
                if (sline.Contains("//"))
                {
                    index2 = sline.IndexOf("/");
                    

                }
                else
                {
                    index2 = sline.Length;
                }

                List<string> sp = Split(sline.Substring(0,index2), Separators);
                
                for (int j = 0; j < sp.Count; j++)
                {
                    if (sp[j] == " ")
                    {
                        pos += 1;
                        continue;
                    }
                    else if (sp[j] =="\t")
                    {
                        pos += 1;
                        continue;
                    }

                    else if (Token.Statements.Contains(sp[j]))
                    {
                        Statement state;

                        if (sp[j] == Token.Statements[0])
                        {
                            state = new Statement("function", lineNumber, pos);
                            lTokens.Add(state);
                            pos += 8;
                        }
                        else if (sp[j] == Token.Statements[1])
                        {
                            state = new Statement("var", lineNumber, pos);
                            lTokens.Add(state);
                            pos += 3;
                        }
                        else if (sp[j] == Token.Statements[2])
                        {
                            state = new Statement("let", lineNumber, pos);
                            lTokens.Add(state);
                            pos += 3;
                        }

                        else if (sp[j] == Token.Statements[3])
                        {
                            state = new Statement("while", lineNumber, pos);
                            lTokens.Add(state);
                            pos += 5;
                        }
                        else if (sp[j] == Token.Statements[4])
                        {
                            state = new Statement("if", lineNumber, pos);
                            lTokens.Add(state);
                            pos += 2;
                        }
                        else if (sp[j] == Token.Statements[5])
                        {
                            state = new Statement("else", lineNumber, pos);
                            lTokens.Add(state);
                            pos += 4;
                        }
                        else if (sp[j] == Token.Statements[6])
                        {
                            state = new Statement("return", lineNumber, pos);
                            lTokens.Add(state);
                            pos += 6;
                        }
                    }

                    else if (Token.VarTypes.Contains(sp[j]))
                    {
                        VarType var;

                        if (sp[j] == Token.VarTypes[0])
                        {
                            var = new VarType("int", lineNumber, pos);
                            lTokens.Add(var);
                            pos += 3;
                        }
                        else if (sp[j] == Token.VarTypes[1])
                        {
                            var = new VarType("char", lineNumber, pos);
                            lTokens.Add(var);
                            pos += 4;
                        }
                        else if (sp[j] == Token.VarTypes[2])
                        {
                            var = new VarType("boolean", lineNumber, pos);
                            lTokens.Add(var);
                            pos += 7;
                        }
                        else if (sp[j] == Token.VarTypes[3])
                        {
                            var = new VarType("array", lineNumber, pos);
                            lTokens.Add(var);
                            pos += 5;
                        }
                    }

                    else if (Token.Constants.Contains(sp[j]))
                    {
                        Constant cons;

                        if (sp[j] == Token.Constants[0])
                        {
                            cons = new Constant("true", lineNumber, pos);
                            lTokens.Add(cons);
                            pos += 4;
                        }
                        else if (sp[j] == Token.Constants[1])
                        {
                            cons = new Constant("false", lineNumber, pos);
                            lTokens.Add(cons);
                            pos += 5;
                        }
                        else if (sp[j] == Token.Constants[2])
                        {
                            cons = new Constant("null", lineNumber, pos);
                            lTokens.Add(cons);
                            pos += 4;
                        }

                    }

                    else if (Token.Operators.Contains(sp[j][0]))
                    {
                        Operator op;

                        if (sp[j][0] == Token.Operators[0])
                        {
                            op = new Operator('*', lineNumber, pos);
                            lTokens.Add(op);
                            pos += 1;
                        }
                        else if (sp[j][0] == Token.Operators[1])
                        {
                            op = new Operator('+', lineNumber, pos);
                            lTokens.Add(op);
                            pos += 1;
                        }
                        else if (sp[j][0] == Token.Operators[2])
                        {
                            op = new Operator('-', lineNumber, pos);
                            lTokens.Add(op);
                            pos += 1;
                        }
                        else if (sp[j][0] == Token.Operators[3])
                        {
                            op = new Operator('/', lineNumber, pos);
                            lTokens.Add(op);
                            pos += 1;
                        }
                        else if (sp[j][0] == Token.Operators[4])
                        {
                            op = new Operator('<', lineNumber, pos);
                            lTokens.Add(op);
                            pos += 1;
                        }
                        else if (sp[j][0] == Token.Operators[5])
                        {
                            op = new Operator('>', lineNumber, pos);
                            lTokens.Add(op);
                            pos += 1;
                        }
                        else if (sp[j][0] == Token.Operators[6])
                        {
                            op = new Operator('&', lineNumber, pos);
                            lTokens.Add(op);
                            pos += 1;
                        }
                        else if (sp[j][0] == Token.Operators[7])
                        {
                            op = new Operator('=', lineNumber, pos);
                            lTokens.Add(op);
                            pos += 1;
                        }
                        else if (sp[j][0] == Token.Operators[8])
                        {
                            op = new Operator('|', lineNumber, pos);
                            lTokens.Add(op);
                            pos += 1;
                        }
                        else if (sp[j][0] == Token.Operators[9])
                        {
                            op = new Operator('!', lineNumber, pos);
                            lTokens.Add(op);
                            pos += 1;
                        }
                    }

                    else if (Token.Parentheses.Contains(sp[j][0]))
                    {
                        Parentheses pare;

                        if (sp[j][0] == Token.Parentheses[0])
                        {
                            pare = new Parentheses('(', lineNumber, pos);
                            lTokens.Add(pare);
                            pos += 1;
                        }
                        else if (sp[j][0] == Token.Parentheses[1])
                        {
                            pare = new Parentheses(')', lineNumber, pos);
                            lTokens.Add(pare);
                            pos += 1;
                        }
                        else if (sp[j][0] == Token.Parentheses[2])
                        {
                            pare = new Parentheses('[', lineNumber, pos);
                            lTokens.Add(pare);
                            pos += 1;
                        }
                        else if (sp[j][0] == Token.Parentheses[3])
                        {
                            pare = new Parentheses(']', lineNumber, pos);
                            lTokens.Add(pare);
                            pos += 1;
                        }
                        else if (sp[j][0] == Token.Parentheses[4])
                        {
                            pare = new Parentheses('{', lineNumber, pos);
                            lTokens.Add(pare);
                            pos += 1;
                        }
                        else if (sp[j][0] == Token.Parentheses[5])
                        {
                            pare = new Parentheses('}', lineNumber, pos);
                            lTokens.Add(pare);
                            pos += 1;
                        }

                    }

                    else if (Token.Separators.Contains(sp[j][0]))
                    {
                        Separator sep;

                        if (sp[j][0] == Token.Separators[0])
                        {
                            sep = new Separator(',', lineNumber, pos);
                            lTokens.Add(sep);
                            pos += 1;
                        }
                        else if (sp[j][0] == Token.Separators[1])
                        {
                            sep = new Separator(';', lineNumber, pos);
                            lTokens.Add(sep);
                            pos += 1;
                        }
                    }

                    else if (Int32.TryParse(sp[j], out int num))
                    {
                        Number number;
                        number = new Number(sp[j], lineNumber, pos);
                        lTokens.Add(number);
                        pos += sp[j].Length;
                    }

                    else
                    {
                        if (!Int32.TryParse(sp[j].Substring(0, 1), out int n) && sp[j]!="#" && sp[j] != "@" && sp[j] != "$" && sp[j] != "%" && sp[j] != "^" && sp[j] != "~")
                        {
                            Identifier id;
                            id = new Identifier(sp[j], lineNumber, pos);
                            lTokens.Add(id);
                            pos += sp[j].Length;
                        }
                        else
                        {
                            throw new SyntaxErrorException("SyntaxErrorException", new Token());
                        }
                    }
                        
                }
                lineNumber++;
            }
            lineNumber = 0;
            return lTokens;
        }
    }
}

