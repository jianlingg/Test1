namespace Test1.Serves
{
    class CommandGenerate
    {

        static public (string, int) CmdParse(string input)
        {
            var inputs = input.Remove(@"\s").ToLower();

            var Mark = inputs.Match(@"\w+:");

            int ReturnLen = 0;

            if (Mark == "test:")
            {
                var MyCmd = inputs.Remove(@"\w+:")
                .Split(",")
                .Select(x => x.Split(".").ToArray())
                .ToDictionary(x => x[0], x => x[1]);

                ReturnLen = MyCmd["data"].ToInt() * MyCmd["count"].ToInt() * 2;

                return (TestParse(MyCmd["data"], MyCmd["gap"], MyCmd["count"]), ReturnLen);
            }
            return (string.Empty, 0);
        }

        //测试回传指令
        static string TestParse(string data, string gap, string count)
        {
            var outCommand = $"{data.DecToHex(8)}{gap.DecToHex(8)}{count.DecToHex(8)}";

            return FX3CmdGen(1, outCommand);
        }


        //FX3控制指令字符串生成
        static public string FX3CmdGen(int target, string args)
        {
            return $"A{target.ToHex(2)}{(args.Length / 8).ToHex(5)}{args}";
        }

        //DAC波形配置指令生成
        static public string WaveConfig(string Data, string Range, string Count = "1")
        {
            var range = Range.Matches(@"\d+");
            var output = string.Empty;
            for (var i = 0; i < range.Count(); i = i + 2)
            {
                output = output + $"{"2".DecToBin(4)}{range[i].DecToBin(14)}{range[i + 1].DecToBin(14)}".BinToHex(8);
            }

            var count = $"{"3".DecToBin(4)}{Count.DecToBin(28)}".BinToHex(8);

            var start = $"{"4".DecToBin(4)}{"0".DecToBin(28)}".BinToHex(8);

            return Data + output + count + start;
        }

        //DAC波形数据指令生成
        static public string WaveGenerate(List<int> input)
        {
            var output = string.Empty;
            for (var i = 0; i < input.Count(); i = i + 2)
            {
                output = output + $"{"1".DecToBin(4)}{input[i].ToBin(14)}{input[i + 1].ToBin(14)}".BinToHex(8);
            }
            return output;
        }

        //DAC三角波数据指令生成
        static public List<int> Sawtooth(int t = 10, double width = 0.5, double Amplitude = 1.0)
        {
            // 检查参数有效性
            if (t <= 0)
                throw new ArgumentException("Number of points must be positive.");
            if (width <= 0 || width >= 1)
                throw new ArgumentException("Width must be between 0 and 1.");

            var x = Enumerable.Range(0, t).Select(i => (double)i / t).ToList(); // 生成归一化的时间向量
            var waveform = x.Select(xi =>
            {
                double sawtoothValue = (xi % 1.0) / width;
                if (sawtoothValue >= 1.0)
                    sawtoothValue = 2.0 - sawtoothValue;
                return (int)Math.Round(Amplitude * 16383 * sawtoothValue);
            }).ToList();

            return waveform;
        }

        //DAC正弦波数据指令生成
        static public List<int> Sinwave(int t = 10, double Amplitude = 1.0)
        {
            // 检查参数有效性
            if (t <= 0)
                throw new ArgumentException("Number of points must be positive.");

            var x = Enumerable.Range(0, t);// 生成归一化的时间向量

            double step = 2 * Math.PI / (t - 1); // 归一化(归2Pi化)

            var waveform = x.Select(i =>
            {
                double x = i * step;
                double y = Amplitude * 16383 * (Math.Sin(x) + 1) / 2;
                return (int)Math.Round(y);
            }).ToList();

            return waveform;
        }

        //波形编辑器数据指令生成
        static public List<int> Formula(string input)
        {
            var input2 = input.Remove(@"\s")
                          .Split(",")
                          .Select(x => x.Split(":"))
                          .ToArray();
            var Lists = new List<int>();
            foreach (var item in input2)
            {
                var tmp = item[0].Split("-").Select(x => x.ToInt()).ToArray();
                var dif = tmp[1] - tmp[0] + 1;
                var e = new NCalc.Expression(item[1]);

                var x = Enumerable.Range(0, dif)
                                  .Select(x =>
                                  {
                                      e.Parameters["x"] = x;
                                      return e.Evaluate().ToString().ToInt();
                                  }).ToList();
                Lists.AddRange(x);
            }
            return Lists;
        }
    }
}
