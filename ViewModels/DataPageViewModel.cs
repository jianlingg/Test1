using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System.IO;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Windows;
using Test1.Serves;

namespace Test1.ViewModels
{
    public partial class DataPageViewModel : ObservableRecipient
    {

        [ObservableProperty]
        public string? _CMD = "test:data.16384,gap.1,count.1";//a0100003000001000000010000000004


        [RelayCommand]
        private void OutData()
        {
            var myDevice = new FX3DataHandle().FX3Device();

            if (myDevice == null || String.IsNullOrWhiteSpace(CMD))
            {
                return;
            }

            myDevice.BulkInEndPt.TimeOut = 100000;

            //myDevice.BulkInEndPt.Reset();

            var fx3 = new FX3DataHandle();

            var commandString = CommandGenerate.CmdParse(CMD).Item1;

            if (fx3.FX3DataOut(myDevice, commandString!))
            {

                var bufInLen = CommandGenerate.CmdParse(CMD).Item2;

                if (bufInLen > 0)
                {
                    var bufIn = fx3.FX3DataIn(myDevice, bufInLen);

                    List<int> DataIn = new List<int>();

                    for (int i = 0; i < bufIn!.Length; i += 2)
                    {
                        int combined = (int)((bufIn[i + 1] << 8) | (bufIn[i]));
                        DataIn.Add(combined);
                    }

                    bool isTrue = true;

                    for (int i = 0; i < DataIn!.Count(); i += 1)
                    {
                        if (DataIn[i] != i % 16384)
                        {
                            isTrue = false; break;
                        }
                    }

                    //Task writeTask = WriteTextAsync(@$"E:\xxx.txt", DataIn);

                    //var stringsToWrite = s.Select(x => $"{x.ToHex(4)}  {x.ToString()}").ToList().Join("\n");

                    

                    //File.WriteAllTextAsync(@$"E:\xxx.txt", stringsToWrite);

                    var s = DataIn.ToArray();

                    WeakReferenceMessenger.Default.Send(new ValueChangedMessage<int[]>(s));

                    MessageBox.Show(isTrue.ToString());
                }

                return;
            }
            return;
        }

        static async Task WriteTextAsync(string filePath, List<int> input)
        {
            var content = input.Select(x => $"{x.ToHex(4)}  {x.ToString()}").ToList().Join("\n");

            byte[] encodedText = Encoding.Unicode.GetBytes(content);

            using (FileStream sourceStream = new FileStream(filePath,
                FileMode.Create, FileAccess.Write, FileShare.None,
                bufferSize: 4096, useAsync: true))
            {
                await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
            }
        }

    }

}
