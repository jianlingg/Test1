using CyUSB;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Test1.Serves
{
    public class FX3DataHandle
    {
        /// <summary>
        /// 入参.命令字符串只接收含十六进制字符串的字符串。（方法体内部会清除非十六进制字符串）
        /// </summary>
        /// <param name="usbDevice"></param>
        /// <param name="commandString"></param>
        /// <returns></returns>
        public bool FX3DataOut(CyUSBDevice usbDevice, string hexStr)
        {
            if (usbDevice == null && String.IsNullOrWhiteSpace(hexStr))
            {
                return false;
            }

            int hexStrLen = (hexStr.Length < 40) ? 40 :
             (hexStr.Length % 8 == 0) ? hexStr.Length : hexStr.Length - hexStr.Length % 8 + 8;

            //补足字符串
            for (int i = hexStr.Length; i < hexStrLen; i = i + 1)
            {
                hexStr = hexStr + "0";
            }

            var hexStrList = new List<string> { };

            for (int i = 0; i < hexStrLen; i = i + 2)
            {
                hexStrList.Add(hexStr.Substring(i, 2));
            }

            var bufOut = hexStrList.Select(x => x.HexToByte()).ToArray();

            var lenOut = bufOut.Length;

            return usbDevice!.BulkOutEndPt.XferData(ref bufOut, ref lenOut);
        }

       /// <summary>
       /// 该方法返回一个列表，列表元素均为长度为8的十六进制字符串。
       /// </summary>
       /// <param name="usbDevice"></param>
       /// <param name="readCount"></param>
       /// <returns></returns>
        public List<string>? FX3DataIn(CyUSBDevice usbDevice, int readCount)
        {
            if (usbDevice == null)
            {
                return null;
            }

            byte[] bufIn = new byte[readCount];

            usbDevice.BulkInEndPt.XferData(ref bufIn, ref readCount);
            var DataIn = bufIn
                .AsParallel()
                .AsOrdered()
                .Select((value, index) => new { value, index })
                .GroupBy(x => x.index / 4)
                .Select(g => BitConverter.ToInt32(g.Select(x => x.value).ToArray(), 0).ToHex(8))
                .ToList();

            return DataIn;

        }

        public List<double> Formula(string formula, List<int> dataList)
        {
            var parameter = Expression.Parameter(typeof(double), "x"); // 定义参数

            var parsedFormula = DynamicExpressionParser.ParseLambda(new[] { parameter }, null, formula); // 解析公式

            var compiledFormula = (Func<double, double>)parsedFormula.Compile();// 编译公式

            var NewDataList = dataList.Select(x => compiledFormula(x)).ToList();

            return NewDataList;
        }

        public CyUSBDevice FX3Device()
        {
            var usbDevices = new USBDeviceList(CyConst.DEVICES_CYUSB);

            var MyDevice = usbDevices[0x04B4, 0x00F1] as CyUSBDevice;

            if (MyDevice == null)
            {
                return null;
            }
            return MyDevice;

        }
    }
}
