using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using CyUSB;
using System.Collections.ObjectModel;
using System.Linq.Dynamic.Core;
using Test1.Models;
using Test1.Serves;

namespace Test1.ViewModels
{
    public partial class DataPageViewModel : ObservableRecipient
    {
        USBDeviceList? usbDevices;
        CyUSBDevice? myDevice;

        private string? title;
        public string? Title
        {
            get => title;
            set
            {
                SetProperty(ref title, value);
                WeakReferenceMessenger.Default.Send(new ValueChangedMessage<string>(Title));
            }
        }

        [ObservableProperty]
        private ObservableCollection<FX3Data>? _DataGrid;

        [ObservableProperty]
        private string? _InString = "00000064000000c80000012c00000064000000c80000012c";



        [RelayCommand]
        private void OutData()
        {
            if (InString != String.Empty && myDevice != null)
            {
                var e = new FX3DataHandle();

                var bufOut = e.FX3DataOut(InString!);

                e.FX3DataOut(myDevice, bufOut);

                var bufInList = e.FX3DataIn(myDevice, bufOut.Length);

                var InDataString = new List<string>();

                for (int i = 0; i < bufInList!.Count; i = i + 4)
                {
                    InDataString.Add(bufInList[i] + bufInList[i + 1] + bufInList[i + 2] + bufInList[i + 3]);
                }

                var DataInt = InDataString
                .AsParallel()
                .AsOrdered()
                .Select(Item => Convert.ToInt32(Item, 16))
                .AsSequential()
                .ToList();

                string formula = "x * 2";

                var DateComputes = new FX3DataHandle().Formula(formula, DataInt);
            }
        }



        public DataPageViewModel()
        {
            FX3ConnectState();
        }


        public void FX3ConnectState()
        {
            usbDevices = new USBDeviceList(CyConst.DEVICES_CYUSB);

            myDevice = usbDevices[0x04B4, 0x00F1] as CyUSBDevice;

            if (myDevice != null)
            {
                Title = myDevice.FriendlyName + " 已连接";
            }

            usbDevices.DeviceAttached += new EventHandler(usbDevices_DeviceAttached);

            usbDevices.DeviceRemoved += new EventHandler(usbDevices_DeviceRemoved);
        }


        private void usbDevices_DeviceRemoved(object? sender, EventArgs e)
        {
            var usbEvent = e as USBEventArgs;

            Title = usbEvent!.FriendlyName + " 已移除";
        }

        private void usbDevices_DeviceAttached(object? sender, EventArgs e)
        {
            var usbEvent = e as USBEventArgs;

            Title = usbEvent!.FriendlyName + " 已连接";
        }

    }

}
