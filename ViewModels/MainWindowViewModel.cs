using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using CyUSB;
using Test1.Serves;

namespace Test1.ViewModels
{
    public partial class MainWindowViewModel : ObservableRecipient
    {

        [ObservableProperty]
        private string _title = "FX3设备未连接";

        public MainWindowViewModel()
        {
            var usbDevices = new USBDeviceList(CyConst.DEVICES_CYUSB);

            var myDevice = new FX3DataHandle().FX3Device();

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
