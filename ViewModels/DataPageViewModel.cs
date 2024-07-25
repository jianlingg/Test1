using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System.Linq.Dynamic.Core;
using Test1.Serves;

namespace Test1.ViewModels
{
    public partial class DataPageViewModel : ObservableRecipient
    {

        [ObservableProperty]
        public string? _CMD = "a050000100001400a0800001000000000";


        [ObservableProperty]
        private string? _InCount = "1024";

        [RelayCommand]
        private void OutData()
        {
            var myDevice = new FX3DataHandle().FX3Device();
            if (myDevice == null || String.IsNullOrWhiteSpace(CMD) || String.IsNullOrWhiteSpace(InCount))
            {
                return;
            }

            myDevice.BulkOutEndPt.TimeOut = 1000;

            myDevice.BulkInEndPt.TimeOut = 1000;

            //myDevice.BulkInEndPt.Reset();

            var fx3 = new FX3DataHandle();

            var commandString = CMD;

            if (fx3.FX3DataOut(myDevice, commandString!))
            {

                //var cmdReturnLen = commandString.Length / 2;

                //var cmdReturn = fx3.FX3DataIn(myDevice, cmdReturnLen);

                //await Task.Delay(10);

                var testReturnLen = InCount.ToInt() * 2;

                var testReturn = fx3.FX3DataIn(myDevice, testReturnLen);

                if (testReturn != null && testReturn.Length > 0)
                {
                    List<ushort> AData = new List<ushort>();

                    for (int i = 0; i < testReturn.Length; i += 2)
                    {
                        ushort combined = (ushort)((testReturn[i] << 8) | testReturn[i + 1]);
                        AData.Add(combined);
                    }

                    var s = AData.Select(x => (int)x).ToArray();

                    WeakReferenceMessenger.Default.Send(new ValueChangedMessage<int[]>(s));
                }
                return;
            }
            return;
        }

    }

}
