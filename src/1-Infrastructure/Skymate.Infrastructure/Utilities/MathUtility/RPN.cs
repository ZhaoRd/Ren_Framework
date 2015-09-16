// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RPN.cs" company="zsharp">
//   copyright (c) zsharp 2015
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.Utilities.MathUtility
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// 逆波兰表达式求值.
    /// </summary>
    public static class Rpn
    {
        
        /// <summary>
        /// 计算表达式.
        /// </summary>
        /// <param name="expression">
        /// The expression.
        /// 表达式
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ComputeRpn(string expression)
        {
            var s = BuildingRPN(expression);
            var empty = string.Empty;
            var sk = new Stack();
            var c = ' ';
            var operand = new StringBuilder();
            double x, y;
            for (int i = 0; i < s.Length; i++)
            {
                c = s[i];
                if (char.IsDigit(c) || c == '.')
                {// 数据值收集.
                    operand.Append(c);
                }
                else if (c == ' ' && operand.Length > 0)
                {
                    
                    try
                    {
                        empty = operand.ToString();
                        if (empty.StartsWith("-"))
                        {
                            // 现在我的算法里这个分支可能永远不会被执行.
                            // 负数的转换一定要小心...它不被直接支持.
                            sk.Push(-((double)Convert.ToDouble(empty.Substring(1, empty.Length - 1))));
                        }
                        else
                        {
                            sk.Push(Convert.ToDouble(empty));
                        }
                    }
                    catch
                    {
                        return "发现异常数据值.";
                    }

                    operand = new System.Text.StringBuilder();
                    
                }
                else if (c == '+'// 运算符处理.双目运算处理.
                    || c == '-'
                    || c == '*'
                    || c == '/'
                    || c == '%'
                    || c == '^')
                {
                    
                    if (sk.Count > 0)/*如果输入的表达式根本没有包含运算符.或是根本就是空串.这里的逻辑就有意义了.*/
                    {
                        y = (double)sk.Pop();
                    }
                    else
                    {
                        sk.Push(0);
                        break;
                    }

                    if (sk.Count > 0)
                        x = (double)sk.Pop();
                    else
                    {
                        sk.Push(y);
                        break;
                    }

                    switch (c)
                    {
                        case '+':
                            sk.Push(x + y);
                            break;
                        case '-':
                            sk.Push(x - y);
                            break;
                        case '*':
                            sk.Push(x * y);
                            break;
                        case '/':
                            sk.Push(x / y);
                            break;
                        case '%':
                            sk.Push(x % y);
                            break;
                        case '^':

                            sk.Push(System.Math.Pow(x, y));

                            break;
                    }
                    
                }
                else if (c == '!')
                {
                    // 单目取反.)
                    sk.Push(-((double)sk.Pop()));
                }
            }

            if (sk.Count > 1)
                return "运算没有完成.";
            if (sk.Count == 0)
                return "结果丢失..";
            return sk.Pop().ToString();
        }

        /// <summary>
        /// 优先级别测试函数.
        /// </summary>
        /// <param name="opr">
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        private static int Power(char opr)
        {
            switch (opr)
            {
                case '+':
                case '-':
                    return 1;
                case '*':
                case '/':
                    return 2;
                case '%':
                case '^':
                case '!':
                    return 3;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 规范化逆波兰表达式. 
        /// </summary>
        /// <param name="s">
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string FormatSpace(string s)
        {
            StringBuilder ret = new System.Text.StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                if (!(s.Length > i + 1 && s[i] == ' ' && s[i + 1] == ' '))
                    ret.Append(s[i]);
                else
                    ret.Append(s[i]);
            }

            return ret.ToString();// .Replace('!','-');
        }

        /****/

        /// <summary>
        /// 根据计算表达式求值
        /// 例子：
        /// 输入 expression="1+2"  输出：3
        /// 输入 expression="(1+2)*3-2"  输出：7
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="expression">
        /// </param>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public static T GetResultWithExpression<T>(string expression) 
        { 
            return default(T);
        }

        /// <summary>
        /// The building rpn.
        /// </summary>
        /// <param name="expression">
        /// The expression.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string BuildingRPN(string expression)
        {
            StringBuilder sb = new StringBuilder(expression);
            Stack<char> sk = new Stack<char>();
            StringBuilder re = new StringBuilder();
            char c = ' ';
            for (int i = 0; i < sb.Length; i++)
            {
                c = sb[i];
                if (char.IsDigit(c))
                {
                    re.Append(c);
                }

                switch (c)
                {
                    case '+':
                    case '-':
                    case '*':
                    case '/':
                    case '%':
                    case '^':
                    case '!':
                    case '(':
                    case ')':
                    case '.':
                        re.Append(c);
                        break;
                    default:
                        continue;
                }
            }

            sb = new StringBuilder(re.ToString());

            for (int i = 0; i < sb.Length - 1; i++)
            {
                if (sb[i] == '-' && (i == 0 || sb[i - 1] == '('))
                {
                    sb[i] = '!';// 字符转义.
                }
            }


            #region 将中缀表达式变为后缀表达式.

            re = new System.Text.StringBuilder();
            for (int i = 0; i < sb.Length; i++)
            {
                if (char.IsDigit(sb[i]) || sb[i] == '.')
                {
                    // 如果是数值.
                    re.Append(sb[i]);// 加入后缀式
                }
                else if (sb[i] == '+'
                    || sb[i] == '-'
                    || sb[i] == '*'
                    || sb[i] == '/'
                    || sb[i] == '%'
                    || sb[i] == '^'
                    || sb[i] == '!')
                {
                    // .
                    #region 运算符处理
                    while (sk.Count > 0)
                    {
                        // 栈不为空时 
                        c = (char)sk.Pop(); // 将栈中的操作符弹出.
                        if (c == '(')
                        {
                            // 如果发现左括号.停.
                            sk.Push(c); // 将弹 出的左括号压回.因为还有右括号要和它匹配.
                            break; // 中断.
                        }
                        else
                        {
                            if (Power(c) < Power(sb[i]))
                            {
                                // 如果优先级比上次的高,则压栈.
                                sk.Push(c);
                                break;
                            }
                            else
                            {
                                re.Append(' ');
                                re.Append(c);
                            }

                            // 如果不是左括号,那么将操作符加入后缀式中.
                        }
                    }

                    sk.Push(sb[i]); // 把新操作符入栈.
                    re.Append(' ');
                    #endregion
                }
                else if (sb[i] == '(')
                {
                    // 基本优先级提升
                    sk.Push('(');
                    re.Append(' ');
                }
                else if (sb[i] == ')')
                {
                    // 基本优先级下调
                    while (sk.Count > 0)
                    {
                        // 栈不为空时 
                        c = (char)sk.Pop(); // pop Operator 
                        if (c != '(')
                        {
                            re.Append(' ');
                            re.Append(c);// 加入空格主要是为了防止不相干的数据相临产生解析错误.
                            re.Append(' ');
                        }
                        else
                            break;
                    }
                }
                else
                    re.Append(sb[i]);
            }

            while (sk.Count > 0)
            {
                // 这是最后一个弹栈啦.
                re.Append(' ');
                re.Append(sk.Pop());
            }

            #endregion

            re.Append(' ');
            return FormatSpace(re.ToString());// 在这里进行一次表达式格式化.这里就是后缀式了.
        }


    }
}
