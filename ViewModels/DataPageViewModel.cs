using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System.DirectoryServices.ActiveDirectory;
using System.Linq.Dynamic.Core;
using Test1.Serves;

namespace Test1.ViewModels
{
    public partial class DataPageViewModel : ObservableRecipient
    {

        [ObservableProperty]
        public string? _CMD = "1";//a0100003000001000000010000000004


        [ObservableProperty]
        private string? _InCount = "65535";

        [RelayCommand]
        private void OutData()
        {
            var myDevice = new FX3DataHandle().FX3Device();

            //全空返
            if (myDevice == null)
            {
                return;
            }


            myDevice.BulkOutEndPt.TimeOut = 1000;

            myDevice.BulkInEndPt.TimeOut = 1000;

            //myDevice.BulkInEndPt.Reset();

            var fx3 = new FX3DataHandle();

            //命令空，则发长度
            if (String.IsNullOrWhiteSpace(CMD) && !String.IsNullOrWhiteSpace(InCount))
            {
                var strxx = $"d8600002003800010018{InCount.DecToHex(4)}";
                var s = fx3.FX3DataOut(myDevice, strxx);
                return;
            }

            if (String.IsNullOrWhiteSpace(CMD) || String.IsNullOrWhiteSpace(InCount))
            {
                return;
            }

            var commandString = $"d850000400000032000000320000001400000014d8100001027FD801d830000180000000d8680001{CMD}0000000d890000100000001";

            if (fx3.FX3DataOut(myDevice, commandString!))
            {

                //var cmdReturnLen = commandString.Length / 2;

                //var cmdReturn = fx3.FX3DataIn(myDevice, cmdReturnLen);

                //await Task.Delay(10);

                //var testReturnLen = InCount.ToInt();
                var testReturnLen = 65536;


                var testReturn = fx3.FX3DataIn(myDevice, testReturnLen);

                if (testReturn != null && testReturn.Length > 0)
                {
                    List<ushort> AData = new List<ushort>();

                    for (int i = 0; i < testReturn.Length; i += 2)
                    {
                        ushort combined = (ushort)((testReturn[i] << 8) | testReturn[i + 1]);
                        AData.Add(combined);
                    }
                    var s = AData.Select(x => ((int)(x & 0x0FFF))).ToArray();

                    WeakReferenceMessenger.Default.Send(new ValueChangedMessage<int[]>(s));
                }
                return;
            }
            return;
        }

    }

}
