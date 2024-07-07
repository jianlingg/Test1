using System.Collections;
using System.Text.RegularExpressions;

namespace Test1.Serves
{
    public static class MyExtensions
    {
        // Write custom extension methods here. They will be available to all queries.
        //字符串扩展方法：正则.替换
        public static string regReplace(this string input, string pattern, string replacement, RegexOptions options)
        {
            return Regex.Replace(input, pattern, replacement, options);
        }

        public static string regReplace(this string input, string pattern, string replacement)
        {
            return Regex.Replace(input, pattern, replacement);
        }

        //字符串扩展方法：正则.分割
        public static string[] regSplit(this string input, string pattern)
        {
            return Regex.Split(input, pattern);
        }

        //字符串扩展方法：正则.是否匹配
        public static bool IsMatch(this string input, string pattern, RegexOptions options)
        {
            return Regex.IsMatch(input, pattern, options);
        }

        public static bool IsMatch(this string input, string pattern)
        {
            return Regex.IsMatch(input, pattern);
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
        public static List<string> Matches(this string input, string pattern)
        {
            return Regex.Matches(input, pattern).Select(x => x.Value).ToList();
        }

        //字符串扩展方法：数字字符串间转化.十进制到二进制。(入参：二进制字符长度)
        public static string DecToBin(this string input, int length)
        {
            return Convert.ToString(int.Parse(input), 2).PadLeft(length, '0');
        }

        public static string DecToBin(this string input)
        {
            return Convert.ToString(int.Parse(input), 2);
        }

        //字符串扩展方法：数字字符串间转化.十进制到十六进制。(入参：十六进制字符长度)
        public static string DecToHex(this string input, int length)
        {
            return Convert.ToString(int.Parse(input), 16).PadLeft(length, '0');
        }

        public static string DecToHex(this string input)
        {
            return Convert.ToString(int.Parse(input), 16);
        }

        //字符串扩展方法：数字字符串间转化.二进制到十进制。
        public static string BinToDec(this string input)
        {
            return Convert.ToInt64(input, 2).ToString();
        }

        //字符串扩展方法：数字字符串间转化.二进制到十六进制。(入参：十六进制字符长度)
        public static string BinToHex(this string input, int length)
        {
            return Convert.ToString(Convert.ToInt64(input, 2), 16).PadLeft(length, '0');
        }

        public static string BinToHex(this string input)
        {
            return Convert.ToString(Convert.ToInt64(input, 2), 16);
        }

        //字符串扩展方法：数字字符串间转化.十六进制到二进制。(入参：二进制字符长度)
        public static string HexToBin(this string input, int length)
        {
            return Convert.ToString(Convert.ToInt64(input, 16), 2).PadLeft(length, '0');
        }

        public static string HexToBin(this string input)
        {
            return Convert.ToString(Convert.ToInt64(input, 16), 2);
        }

        //字符串扩展方法：数字字符串间转化.十六进制到十进制。
        public static string HexToDec(this string input)
        {
            return Convert.ToInt64(input, 16).ToString();
        }

        //字符串扩展方法：十进制数字字符串转int类型
        public static int ToInt(this string input)
        {
            return int.Parse(input);
        }

        //字符串扩展方法：十六进制数字字符串转byte类型
        public static byte HexToByte(this string input)
        {
            return Convert.ToByte(input, 16);
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

        //int扩展方法：int转十六进制字符串。(入参：十六进制字符长度)
        public static string ToHex(this int input, int length)
        {
            return Convert.ToString(input, 16).PadLeft(length, '0');
        }

        //字符串列表扩展方法：将字符串列表所有元素以分隔符合并为一个字符串
        public static string Join(this List<string> input, string separator)
        {
            return string.Join(separator, input);
        }

    }
}
