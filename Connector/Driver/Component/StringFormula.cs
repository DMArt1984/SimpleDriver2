using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsIDevice.Connector.Driver.Component
{
    class StringFormula
    {
        static public string[] operators = { "-", "+", "/", "*", "^", "&", "|", "!", ">", "<", "=", "%" };
        private Func<double, double, double>[] _operations = {
            (a1, a2) => NormValue(a1) - NormValue(a2),
            (a1, a2) => NormValue(a1) + NormValue(a2),
            (a1, a2) => NormValue(a1) / NormValue(a2),
            (a1, a2) => NormValue(a1) * NormValue(a2),
            (a1, a2) => Math.Pow(NormValue(a1), NormValue(a2)),
            (a1, a2) => NormValue(NormBool(a1) && NormBool(a2)),
            (a1, a2) => NormValue(NormBool(a1) || NormBool(a2)),
            (a1, a2) => NormValue(!NormBool(a2)),
            (a1, a2) => NormValue(NormValue(a1) > NormValue(a2)),
            (a1, a2) => NormValue(NormValue(a1) < NormValue(a2)),
            (a1, a2) => NormValue(NormValue(a1) == NormValue(a2)),
            (a1, a2) => NormValue(a1) % NormValue(a2)
        };

        static bool ValueToBOOL(dynamic Val) => (Val > 0) ? true : false;
        static int BoolToVALUE(bool Val) => (Val == true) ? 1 : 0;
        static dynamic NormValue(dynamic Val) => (Val.GetType() == typeof(System.Boolean)) ? BoolToVALUE(Val) : Val;
        static dynamic NormBool(dynamic Val) => (Val.GetType() != typeof(System.Boolean)) ? ValueToBOOL(Val) : Val;


        public double Eval(string expression)
        {
            expression = getBracketsForFirstMinus(expression); // исправление ошибки работы с отрицательными числами

            List<string> tokens = getTokens(expression);
            Stack<double> operandStack = new Stack<double>();
            Stack<string> operatorStack = new Stack<string>();
            int tokenIndex = 0;

            while (tokenIndex < tokens.Count)
            {
                string token = tokens[tokenIndex];
                if (token == "(")
                {
                    string subExpr = getSubExpression(tokens, ref tokenIndex);
                    operandStack.Push(Eval(subExpr));
                    continue;
                }
                if (token == ")")
                {
                    throw new ArgumentException("Mis-matched parentheses in expression");
                }
                //If this is an operator  
                if (Array.IndexOf(operators, token) >= 0)
                {
                    while (operatorStack.Count > 0 && Array.IndexOf(operators, token) < Array.IndexOf(operators, operatorStack.Peek()))
                    {
                        string op = operatorStack.Pop();
                        double arg2 = operandStack.Pop();
                        if (operandStack.Count > 0)
                        {
                            double arg1 = operandStack.Pop();
                            operandStack.Push(_operations[Array.IndexOf(operators, op)](arg1, arg2));
                        }
                        else
                        {
                            operandStack.Push(_operations[Array.IndexOf(operators, op)](0, arg2));
                        }
                    }
                    operatorStack.Push(token);
                }
                else
                {
                    operandStack.Push(double.Parse(token));
                }
                tokenIndex += 1;
            }

            while (operatorStack.Count > 0)
            {
                string op = operatorStack.Pop();
                double arg2 = operandStack.Pop();
                if (operandStack.Count > 0)
                {
                    double arg1 = operandStack.Pop();
                    operandStack.Push(_operations[Array.IndexOf(operators, op)](arg1, arg2));
                }
                else
                {
                    operandStack.Push(_operations[Array.IndexOf(operators, op)](0, arg2));
                }
            }
            return operandStack.Pop();
        }

        private string getSubExpression(List<string> tokens, ref int index)
        {
            StringBuilder subExpr = new StringBuilder();
            int parenlevels = 1;
            index += 1;
            while (index < tokens.Count && parenlevels > 0)
            {
                string token = tokens[index];
                if (tokens[index] == "(")
                {
                    parenlevels += 1;
                }

                if (tokens[index] == ")")
                {
                    parenlevels -= 1;
                }

                if (parenlevels > 0)
                {
                    subExpr.Append(token);
                }

                index += 1;
            }

            if ((parenlevels > 0))
            {
                throw new ArgumentException("Mis-matched parentheses in expression");
            }
            return subExpr.ToString();
        }

        private List<string> getTokens(string expression)
        {
            string operators = "()!^*/%+-&|><="; // & - AND, | - OR, ! - NOT (0!x)
            List<string> tokens = new List<string>();
            StringBuilder sb = new StringBuilder();

            foreach (char c in expression.Replace(" ", string.Empty))
            {
                if (operators.IndexOf(c) >= 0)
                {
                    if ((sb.Length > 0))
                    {
                        tokens.Add(sb.ToString());
                        sb.Length = 0;
                    }
                    tokens.Add(Convert.ToString(c));
                }
                else
                {
                    sb.Append(c);
                }
            }

            if ((sb.Length > 0))
            {
                tokens.Add(sb.ToString());
            }
            return tokens;
        }

        // Закрыть в скобки первое число, если оно отрицательное
        private string getBracketsForFirstMinus(string value)
        {
            string value2 = value.Replace(" ", "");
            value2 = value2.Replace("+-", "-").Replace("-+", "-").Replace("--", "+");

            if (value2[0] == '0')
                value2 = value2.Substring(1);

            if (value2[0] == '-')
            {
                char[] nums = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.', ',' };
                var second = value2.Length;
                for (var i = 1; i < value2.Length; i++)
                {
                    var one = value2[i];
                    if (nums.Contains(one) == false)
                    {
                        second = i;
                        break;
                    }
                }
                string value3 = "(" + value2.Substring(0, second) + ")";
                if (second < value2.Length - 1)
                {
                    value3 += value2.Substring(second);
                    return value3;
                }

            }

            return value;
        }
    }

    class StringFormula2
    {
        static public string[] operators = { "-", "+", "/", "*", "^", "&", "|", "!", ">", "<", "=", "%" };

        bool ValueToBOOL(dynamic Val) => (Val > 0) ? true : false;
        double BoolToVALUE(bool Val) => (Val == true) ? 1 : 0;
        dynamic NormValue(dynamic Val) => (Val.GetType() == typeof(System.Boolean)) ? BoolToVALUE(Val) : Val;
        dynamic NormBool(dynamic Val) => (Val.GetType() != typeof(System.Boolean)) ? ValueToBOOL(Val) : Val;

        public double Calc(string expression)
        {
            // Преобразование команд логики
            //expression = expression.Replace(" AND ", " & ").Replace(" OR ", " | ");
            //expression = " " + expression;
            //expression = expression.Replace(" NOT ", " 0! ");

            // Вычисления
            return Eval(expression);
        }

        double Eval(string expression)
        {
            // Нормализация
            expression = expression.Replace(" ", "").Replace("+-", "-").Replace("-+", "-").Replace("--", "+").Replace("++", "+");

            // Скобки
            bool OC;
            do
            {
                int Open = -1;
                int Close = -1;
                OC = false;
                for (var i = 0; i < expression.Length; i++)
                {
                    if (expression[i] == '(' && Close == -1)
                        Open = i;

                    if (expression[i] == ')' && Open >= 0)
                    {
                        Close = i;
                        OC = true;
                        if (Open + 1 >= 0 && Close - Open - 1 >= 1)
                        {
                            var InSTR = expression.Substring(Open + 1, Close - Open - 1);

                            var value = Eval(InSTR);

                            var newExp = "";
                            if (Open > 0)
                            {
                                newExp = expression.Substring(0, Open);
                            }

                            newExp += value.ToString();

                            if (Close < expression.Length - 1)
                            {
                                newExp += expression.Substring(Close + 1);
                            }

                            expression = newExp;
                        }

                        break;
                    }
                }
            } while (OC);
            
            // Разделение на элементы
            List<string> op = new List<string>();
            string tempOp = "";
            for (var i=0; i<expression.Length;i++)
            {
                if (operators.Contains(expression[i].ToString()) && (i>0 || (i==0 && expression[i] != '-')))
                {
                    if (tempOp !="")
                    {
                        op.Add(tempOp);
                        string cmd = expression[i].ToString();
                        if ((cmd == "<" || cmd == ">") && (i+1 < expression.Length-1) && (expression[i+1].ToString() == "="))
                        {
                            cmd += "=";
                            i++;
                        }
                        op.Add(cmd);
                        tempOp = "";
                    }
                } else
                {
                    tempOp += expression[i].ToString();
                }
            }
            if (tempOp != "")
            {
                op.Add(tempOp);
            }

            // Операции
            Combine(ref op, "^");
            Combine(ref op, "%");
            Combine(ref op, "!");
            Combine(ref op, "*", "/");
            Combine(ref op, "+", "-");
            Combine(ref op, "=");
            Combine(ref op, ">=", "<=");
            Combine(ref op, ">", "<");
            Combine(ref op, "&", "|");

            expression = op[0];

            // Результат
            return double.Parse(expression);
        }

        void Combine(ref List<string> op, string use1, string use2 = "")
        {
            bool calc;
            do
            {
                calc = false;
                for (var i = 0; i < op.Count; i++)
                {
                    if (op[i] == use1 || (use2 != "" && op[i] == use2))
                    {
                        calc = true;
                        double value = 0;
                        if (i > 0 && i < op.Count - 1)
                        {
                            value = Operate(op[i - 1], op[i], op[i + 1]);

                            op[i] = value.ToString();
                            op.RemoveAt(i - 1);
                            op.RemoveAt(i);
                        }
                        else
                        {
                            op.RemoveAt(i);
                        }
                        break;
                    }
                }
            } while (calc);
        }

        double Operate(string value1, string use, string value2)
        {
            switch (use)
            {
                case "^":
                    return Math.Pow(double.Parse(value1), double.Parse(value2));

                case "%":
                    return double.Parse(value1) % double.Parse(value2);

                case "!":
                    return Convert.ToDouble(!(NormBool(double.Parse(value2))));

                case "*":
                    if (value1.ToLower() == "false")
                        value1 = "0";
                    if (value1.ToLower() == "true")
                        value1 = "1";
                    if (value2.ToLower() == "false")
                        value2 = "0";
                    if (value2.ToLower() == "true")
                        value2 = "1";

                    return double.Parse(value1) * double.Parse(value2);

                case "/":
                    if (value1.ToLower() == "false")
                        value1 = "0";
                    if (value1.ToLower() == "true")
                        value1 = "1";
                    if (value2.ToLower() == "false")
                        value2 = "0";
                    if (value2.ToLower() == "true")
                        value2 = "1";

                    return double.Parse(value1) / double.Parse(value2);

                case "+":
                    if (value1.ToLower() == "false")
                        value1 = "0";
                    if (value1.ToLower() == "true")
                        value1 = "1";
                    if (value2.ToLower() == "false")
                        value2 = "0";
                    if (value2.ToLower() == "true")
                        value2 = "1";

                    return double.Parse(value1) + double.Parse(value2);

                case "-":
                    if (value1.ToLower() == "false")
                        value1 = "0";
                    if (value1.ToLower() == "true")
                        value1 = "1";
                    if (value2.ToLower() == "false")
                        value2 = "0";
                    if (value2.ToLower() == "true")
                        value2 = "1";

                    return double.Parse(value1) - double.Parse(value2);

                case "=":
                    return BoolToVALUE(double.Parse(value1) == double.Parse(value2));

                case ">=":
                    return BoolToVALUE(double.Parse(value1) >= double.Parse(value2));

                case ">":
                    return BoolToVALUE(double.Parse(value1) > double.Parse(value2));

                case "<=":
                    return BoolToVALUE(double.Parse(value1) <= double.Parse(value2));

                case "<":
                    return BoolToVALUE(double.Parse(value1) < double.Parse(value2));

                case "&":
                    return BoolToVALUE(NormBool(double.Parse(value1)) && NormBool(double.Parse(value2)));

                case "|":
                    return BoolToVALUE(NormBool(double.Parse(value1)) || NormBool(double.Parse(value2)));

                default:
                    return 0;

            }
        }

    }
}
