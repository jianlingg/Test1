namespace Test1.Serves
{
    class CommandGenerate
    {
        
        public string TestParse(string riseTime, string gapTime, string cycleNumber)
        {
            var outCommand = $"{riseTime.DecToHex(8)}{gapTime.DecToHex(8)}{cycleNumber.DecToHex(8)}";

            return FX3CommandFrame(1, outCommand);
        }

        //采样通道字符串解析
        string ChannelParse(string AcqChannel, string AcqRate, string AcqCounter)
        {
            var ch1 = AcqChannel.IsMatch(@"1") ? "1" : "0";
            var ch2 = AcqChannel.IsMatch(@"2") ? "1" : "0";
            var ch3 = AcqChannel.IsMatch(@"3") ? "1" : "0";
            var ch4 = AcqChannel.IsMatch(@"4") ? "1" : "0";
            var Channels = ch1 + ch2 + ch3 + ch4;
            return $"d8100002{Convert.ToString(Convert.ToInt32(Channels, 2), 16)}{AcqRate.Match(@"\d+").DecToHex(7)}{AcqCounter.Match(@"\d+").DecToHex(8)}";
        }

        //FX3控制指令字符串生成
        public static string FX3CommandFrame(int target, List<string> args)
        {
            return $"A{target.ToHex(2)}{args.Count.ToHex(5)}{args.Aggregate((c, n) => c + n)}";
        }

        public static string FX3CommandFrame(int target, string args)
        {
            return $"A{target.ToHex(2)}{(args.Length / 8).ToHex(5)}{args}";
        }
    }
}
