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
        static string FX3CmdGen(int target, string args)
        {
            return $"A{target.ToHex(2)}{(args.Length / 8).ToHex(5)}{args}";
        }
    }
}
