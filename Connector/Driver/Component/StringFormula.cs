using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connector.Driver.Component
{
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
