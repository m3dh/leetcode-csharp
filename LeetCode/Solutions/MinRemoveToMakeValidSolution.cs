namespace LeetCode.Csharp.Solutions
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class MinRemoveToMakeValidSolution
    {
        // 1249 - https://leetcode.com/problems/minimum-remove-to-make-valid-parentheses/
        public string MinRemoveToMakeValid(string s)
        {
            List<int> toRemove = new List<int>();
            Stack<int>lpIndexes= new Stack<int>();
            bool foundChar = true; // (A)
            for (int i = 0; i < s.Length; i++)
            {
                if (char.IsLetter(s[i]))
                {
                    foundChar = true;
                }
                else if (s[i] == '(')
                {
                    if (!foundChar)
                    {
                        // A((
                        toRemove.Add(i);
                    }
                    else
                    {
                        // foundChar = false; // THIS IS ALLOWED!
                        lpIndexes.Push(i);
                    }
                }
                else if (s[i] == ')')
                {
                    if (!foundChar)
                    {
                        // A()
                        toRemove.Add(i);
                    }
                    else
                    {
                        if (lpIndexes.Count == 0)
                        {
                            toRemove.Add(i);
                        }
                        else
                        {
                            lpIndexes.Pop();
                        }
                    }
                }
                else
                {
                    throw new Exception();
                }
            }
            
            if(lpIndexes.Count > 0) toRemove.AddRange(lpIndexes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                if (!toRemove.Contains(i)) sb.Append(s[i]);
            }

            return sb.ToString();
        }
    }
}