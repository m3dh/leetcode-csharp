namespace LeetCode.Csharp.Solutions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CalculatorSolution
    {
        // 224 - https://leetcode.com/problems/basic-calculator/
        public int Calculate(string s)
        {
            var ops = Tokenize('|' + s);
            var operatorStack = new Stack<Op>();
            var operandStack = new Stack<Op>();

            foreach (Op op in ops.ToArray() .Reverse())
            {
                Console.WriteLine(string.Join(", ", operatorStack));
                Console.WriteLine(string.Join(", ", operandStack));

                if (op.IsOperator)
                {
                    if (op.Operator == ')')
                    {
                        operatorStack.Push(op);
                    }
                    else if (op.Operator == '(' || op.Operator == '|')
                    {
                        while (operatorStack.Count > 0)
                        {
                            var topOper = operatorStack.Pop();
                            if (topOper.Operator == ')')
                            {
                                break;
                            }
                            else
                            {
                                int val = 0;
                                var val1 = operandStack.Pop().Operand;
                                var val2 = operandStack.Pop().Operand;

                                switch (topOper.Operator)
                                {
                                    case '+':
                                        val = val1 + val2;
                                        break;
                                    case '-':
                                        val = val1 - val2;
                                        break;
                                    case '*':
                                        val = val1 * val2;
                                        break;
                                    case '/':
                                        val = val1 / val2;
                                        break;
                                    default: throw new Exception();
                                }

                                operandStack.Push(new Op { Operand = val });
                            }
                        }
                    }
                    else if (op.Operator == '+' || op.Operator == '-')
                    {
                        while (operatorStack.Count > 0 && (operatorStack.Peek().Operator == '*' || operatorStack.Peek().Operator == '/'))
                        {
                                int val = 0;
                                var val1 = operandStack.Pop().Operand;
                                var val2 = operandStack.Pop().Operand;

                                var topOper = operatorStack.Pop();
                                switch (topOper.Operator)
                                {
                                    case '+':
                                        val = val1 + val2;
                                        break;
                                    case '-':
                                        val = val1 - val2;
                                        break;
                                    case '*':
                                        val = val1 * val2;
                                        break;
                                    case '/':
                                        val = val1 / val2;
                                        break;
                                    default: throw new Exception();
                                }

                                operandStack.Push(new Op { Operand = val });
                        }

                        operatorStack.Push(op);
                    }
                    else
                    {
                        operatorStack.Push(op);
                    }
                }
                else
                {
                    operandStack.Push(op);
                }
            }

            return operandStack.Single().Operand;
        }

        private List<Op> Tokenize(string s)
        {
            char[] operators = new[] { '(', ')', '+', '-', '/', '*', '|' };

            List<Op> ret = new List<Op>();
            int? val = null;
            foreach (char c in s)
            {
                if (operators.Any(o => o == c))
                {
                    if (val != null)
                    {
                        ret.Add(new Op
                        {
                            Operand = val.Value,
                        });

                        val = null;
                    }

                    ret.Add(new Op
                    {
                        IsOperator = true,
                        Operator = c,
                    });
                }
                else if (char.IsWhiteSpace(c))
                {
                    if (val != null)
                    {
                        ret.Add(new Op
                        {
                            Operand = val.Value,
                        });

                        val = null;
                    }
                }
                else
                {
                    if (val == null)
                    {
                        val = 0;
                    }

                    val *= 10;
                    val += (c - '0');
                }
            }

            if (val != null)
            {
                ret.Add(new Op
                {
                    Operand = val.Value,
                });
            }

            return ret;
        }

        private class Op
        {
            public bool IsOperator { get; set; }
            public char Operator { get; set; }
            public int Operand { get; set; }

            public override string ToString()
            {
                return IsOperator ? Operator.ToString() : Operand.ToString();
            }
        }

        // 227 - https://leetcode.com/problems/basic-calculator-ii/
        public int Calculate2(string s)
        {
            int index = 0;

            Stack<int> operands = new Stack<int>();
            Stack<char> operators = new Stack<char>();

            while (index < s.Length)
            {
                Tuple<int, int> operandTuple = this.GetNextOperand(s, index);
                index = operandTuple.Item2;
                operands.Push(operandTuple.Item1);

                Tuple<char, int> operatorTuple = this.GetNextOperator(s, index);
                index = operatorTuple.Item2;
                if (operatorTuple.Item1 == '+' || operatorTuple.Item1 == '-' || operatorTuple.Item1 == '|')
                {
                    // Pop and calc everything.
                    while (operands.Count > 1)
                    {
                        var val2 = operands.Pop();
                        var val1 = operands.Pop();
                        var oper = operators.Pop();
                        var newVal = this.Calc(val1, val2, oper);
                        operands.Push(newVal);
                    }

                    operators.Push(operatorTuple.Item1);
                }
                else
                {
                    while (operators.Count > 0)
                    {
                        var oper = operators.Peek();
                        if (oper == '*' || oper == '/')
                        {
                            operators.Pop();
                            var val2 = operands.Pop();
                            var val1 = operands.Pop();
                            var newVal = this.Calc(val1, val2, oper);
                            operands.Push(newVal);
                        }
                        else
                        {
                            break;
                        }
                    }

                    operators.Push(operatorTuple.Item1);
                }

                Console.WriteLine($"OPERATOR: {string.Join(",", operators)}, OPERAND: {string.Join(",", operands)}");
            }

            return operands.Pop();
        }

        public int Calc(int val1, int val2, char oper)
        {
            switch (oper)
            {
                case '+': return val1 + val2;
                case '-': return val1 - val2;
                case '*': return val1 * val2;
                case '/': return val1 / val2;
                default: throw new Exception();
            }
        }

        public Tuple<int, int> GetNextOperand(string s, int idx)
        {
            while (s[idx] == ' ') idx++;

            int val = 0;
            while (idx < s.Length && char.IsDigit(s[idx]))
            {
                val *= 10;
                val += s[idx] - '0';
                idx++;
            }

            return Tuple.Create(val, idx);
        }

        public Tuple<char, int> GetNextOperator(string s, int idx)
        {
            while (idx < s.Length && s[idx] == ' ') idx++;
            return Tuple.Create(idx >= s.Length ? '|' : s[idx], idx + 1);
        }
    }
}