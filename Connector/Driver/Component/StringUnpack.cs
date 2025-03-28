using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connector.Driver.Component
{
    static class StringUnpack
    {
        // Получить параметры функции из строки ...Name(p1;p2;p3)...
        static public string[] Parse_ParamsString(string address, string nameFunc, char delim = ';')
        {
            string[] result = { };
            int index1 = address.IndexOf($"{nameFunc}("); // начало функции
            int index2 = 0;
            int index3 = 0;
            if (index1 >= 0)
            {
                index3 = index1;
                index1 += nameFunc.Length + 1;

                // конец функции
                int nx = 1;
                for (var i = index1 + 1; i < address.Length; i++)
                {
                    if (address[i] == '(')
                        nx++;

                    if (address[i] == ')')
                    {
                        nx--;
                        if (nx == 0)
                        {
                            index2 = i;
                            break;
                        }
                    }

                }
                //index2 = address.IndexOf(")", index1); // конец функции

                if (index2 > index1)
                {
                    Array.Resize(ref result, 1);
                    result[0] = address.Substring(index3, index2 - index3 + 1);
                    string[] arrayParams = address.Substring(index1, index2 - index1).Split(delim);
                    Array.Resize(ref result, 1 + arrayParams.Length);
                    arrayParams.CopyTo(result, 1);
                    return result;
                }
            }
            return result;
        }

        // Получить массив байт из параметров
        static public byte[] Parse_ParamsByte(string[] Params)
        {
            byte[] BT = new byte[Params.Length];
            for (int i = 1; i <= Params.Length; i++)
            {
                try
                {
                    BT[i - 1] = Convert.ToByte(Params[i - 1]);
                }
                catch
                {
                    BT[i - 1] = 0;
                }
            }
            return BT;
        }

        // ---------------------------------------------------------------------------------------------
    }
}
