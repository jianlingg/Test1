using OpenTK.Audio.OpenAL;
using System.Collections;
using System.Text.RegularExpressions;

namespace Test1.Serves
{
    public static class MyExtensions
    {
        // Write custom extension methods here. They will be available to all queries.
        //字符串扩展方法：正则.替换
        public static string RegReplace(this string input, string pattern, string replacement, RegexOptions options = RegexOptions.None)
        {
            return Regex.Replace(input, pattern, replacement, options);
        }

        //字符串扩展方法：正则.移除
        public static string Remove(this string input, string pattern, RegexOptions options = RegexOptions.None)
        {
            return Regex.Replace(input, pattern, string.Empty, options);
        }

        //字符串扩展方法：正则.分割
        public static string[] RegSplit(this string input, string pattern)
        {
            return Regex.Split(input, pattern);
        }

        //字符串扩展方法：正则.是否匹配
        public static bool IsMatch(this string input, string pattern, RegexOptions options = RegexOptions.None)
        {
            return Regex.IsMatch(input, pattern, options);
        }

        //字符串扩展方法：正则.匹配单个
        public static string Match(this string input, string pattern, RegexOptions options)
        {
            return Regex.Match(input, pattern, options).Value;
        }

        public static string Match(this string input, string pattern)
        {
            return Regex.Match(input, pattern).Value;
        }

        //字符串扩展方法：正则.匹配多个
        public static List<string> Matches(this string input, string pattern, RegexOptions options = RegexOptions.None)
        {
            return Regex.Matches(input, pattern, options).Select(x => x.Value).ToList();
        }

        //字符串扩展方法：进制转化.十进制到二进制。(入参：二进制字符长度)
        public static string DecToBin(this string input, int length = 1)
        {
            return Convert.ToString(int.Parse(input), 2).PadLeft(length, '0');
        }

        //字符串扩展方法：进制转化.十进制到十六进制。(入参：十六进制字符长度)
        public static string DecToHex(this string input, int length = 1)
        {
            return Convert.ToString(int.Parse(input), 16).PadLeft(length, '0');
        }

        //字符串扩展方法：进制转化.二进制到十进制。
        public static string BinToDec(this string input)
        {
            return Convert.ToInt64(input, 2).ToString();
        }

        //字符串扩展方法：进制转化.二进制到十六进制。(入参：十六进制字符长度)
        public static string BinToHex(this string input, int length = 1)
        {
            return Convert.ToString(Convert.ToInt64(input, 2), 16).PadLeft(length, '0');
        }

        //字符串扩展方法：进制转化.十六进制到二进制。(入参：二进制字符长度)
        public static string HexToBin(this string input, int length = 1)
        {
            return Convert.ToString(Convert.ToInt64(input, 16), 2).PadLeft(length, '0');
        }

        //字符串扩展方法：进制转化.十六进制到十进制。
        public static string HexToDec(this string input)
        {
            return Convert.ToInt64(input, 16).ToString();
        }

        //字符串扩展方法：类型转化.十进制字符串转int类型
        public static int ToInt(this string input)
        {
            return int.Parse(input);
        }

        //字符串扩展方法：十六进制数字字符串转byte类型
        public static byte HexToByte(this string input)
        {
            return Convert.ToByte(input, 16);
        }

        //字符串扩展方法：计算
        public static string Calculator(this string input)
        {
            return new NCalc.Expression(input).Evaluate().ToString();
        }

        //字符串扩展方法：判断是否为空或空白字符串
        public static bool IsNullOrEmpty(this string input)
        {
            return String.IsNullOrEmpty(input);
        }

        //字符串扩展方法：判断是否为空或空白字符串
        public static bool IsNullOrWhiteSpace(this string input)
        {
            return String.IsNullOrWhiteSpace(input);
        }

        //字符串扩展方法：功能.放入剪切板
        public static void Clipboard(this string input)
        {
            System.Windows.Clipboard.SetText(input);
        }

        //十六进制命令分化
        public static List<string> Div(this string input, int lenth)
        {
            var chunks = Enumerable.Range(0, input.Length / lenth)//生成列表
                                   .Select(i => input.Substring(i * lenth, lenth))
                                   .ToList();
            return chunks;
        }

        //int扩展方法：int转十六进制字符串。(入参：十六进制字符长度)
        public static string ToHex(this int input, int length = 1)
        {
            return Convert.ToString(input, 16).PadLeft(length, '0');
        }

        //int扩展方法：int16转十六进制字符串。(入参：十六进制字符长度)
        public static string ToHex(this Int16 input, int length = 4)
        {
            return Convert.ToString(input, 16).PadLeft(length, '0');
        }

        //int扩展方法：int16转十六进制字符串。(入参：十六进制字符长度)
        public static string ToHex(this ushort input, int length = 4)
        {
            return Convert.ToString(input, 16).PadLeft(length, '0');
        }

        //byte扩展方法：byte转十六进制字符串。(入参：十六进制字符长度)
        public static string ToHex(this byte input, int length = 2)
        {
            return Convert.ToString(input, 16).PadLeft(length, '0');
        }

        //int扩展方法：int转二进制字符串。(入参：十六进制字符长度)
        public static string ToBin(this int input, int length = 1)
        {
            return Convert.ToString(input, 2).PadLeft(length, '0');
        }

        //字符串列表扩展方法：将字符串列表所有元素以分隔符合并为一个字符串
        public static string Join(this List<string> input, string separator = "")
        {
            return string.Join(separator, input);
        }
    }
}
