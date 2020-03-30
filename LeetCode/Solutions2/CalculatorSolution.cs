namespace LeetCode.Csharp.Solutions2
{
    using System;
    using System.Collections.Generic;

    public class CalculatorSolution
    {
        // 224 - https://leetcode.com/problems/basic-calculator/
        public int Calculate(string s)
        {
            // TODO.
            return -1;
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