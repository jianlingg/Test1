using CyUSB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Test1.Serves
{
    public class FX3DataHandle
    {
        public byte[] FX3DataOut(string CommandString)
        {
            Regex reg = new Regex(@"[^\d|a-f|A-F]");

            CommandString = reg.Replace(CommandString, "");

            int OutLen = (CommandString.Length < 40) ? 40 :
             (CommandString.Length % 8 == 0) ? CommandString.Length : CommandString.Length - CommandString.Length % 8 + 8;

            //补足字符串
            for (int i = CommandString.Length; i < OutLen; i = i + 1)
            {
                CommandString = CommandString + "0";
            }

            var CommandStringList = new List<string> { };

            for (int i = 0; i < OutLen; i = i + 2)
            {
                CommandStringList.Add(CommandString.Substring(i, 2));
            }

            var CommandBytes = CommandStringList.Select(ByteString => Convert.ToByte(ByteString, 16)).ToArray();

            return CommandBytes;
        }

        public List<double> Formula(string formula, List<int> DataList)
        {
            var parameter = Expression.Parameter(typeof(double), "x"); // 定义参数

            var parsedFormula = DynamicExpressionParser.ParseLambda(new[] { parameter }, null, formula); // 解析公式

            var compiledFormula = (Func<double, double>)parsedFormula.Compile();// 编译公式

            var NewDataList = DataList.Select(x => compiledFormula(x)).ToList();

            return NewDataList;
        }

        public void FX3DataOut(CyUSBDevice USBDevice, byte[] bufOut)
        {
            if (USBDevice != null)
            {
                var lenOut = bufOut.Length;
                USBDevice.BulkOutEndPt.XferData(ref bufOut, ref lenOut);
            }
        }

        public List<string>? FX3DataIn(CyUSBDevice USBDevice, int ReadCount)
        {
            if (USBDevice != null)
            {
                byte[] bufIn = new byte[ReadCount];

                USBDevice.BulkInEndPt.XferData(ref bufIn, ref ReadCount);

                var DataIn = bufIn
                    .AsParallel()
                    .AsOrdered()
                    .Select(x => x.ToString("X2"))
                    .AsSequential()
                    .ToList();

                return DataIn;
            }
            return null;
        }
    }
}
