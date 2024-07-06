using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using CyUSB;
using System.Linq.Dynamic.Core;
using Test1.Serves;

namespace Test1.ViewModels
{
    public partial class DataPageViewModel : ObservableRecipient
    {

        [ObservableProperty]
        public string? riseTime = "上升时间:256";

        [ObservableProperty]
        private string? gapTime = "间隔时间:1ns";

        [ObservableProperty]
        private string? cycleNumber = "周期个数:1";

        [ObservableProperty]
        private string? fx3Return;

        [RelayCommand]
        private async void OutData()
        {
            var myDevice = new FX3DataHandle().FX3Device();
            if (myDevice == null || String.IsNullOrWhiteSpace(RiseTime) || String.IsNullOrWhiteSpace(GapTime) || String.IsNullOrWhiteSpace(RiseTime))
            {
                return;
            }

            myDevice.BulkOutEndPt.TimeOut = 1000;

            myDevice.BulkInEndPt.TimeOut = 1000;

            //myDevice.BulkInEndPt.Reset();

            var fx3 = new FX3DataHandle();

            var riseTime = RiseTime!.Match(@"\d+");

            var gapTime = GapTime!.Match(@"\d+");

            var cycleNumber = CycleNumber!.Match(@"\d+");

            var commandString = new CommandGenerate().TestParse(riseTime, gapTime, cycleNumber);

            if (fx3.FX3DataOut(myDevice, commandString!))
            {

                //var cmdReturnLen = commandString.Length / 2;

                //var cmdReturn = fx3.FX3DataIn(myDevice, cmdReturnLen);

                //await Task.Delay(10);

                var testReturnLen = riseTime.ToInt() * cycleNumber.ToInt() * 4;

                var testReturn = fx3.FX3DataIn(myDevice, testReturnLen);

                if (testReturn != null)
                {
                    var dataInt = testReturn
                        .AsParallel()
                        .AsOrdered()
                        .Select(x => x.HexToDec().ToInt())
                        .ToArray();
                    WeakReferenceMessenger.Default.Send(new ValueChangedMessage<int[]>(dataInt));
                }
            }

        }

    }
}
