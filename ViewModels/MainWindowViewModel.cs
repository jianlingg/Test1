using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using CyUSB;

namespace Test1.ViewModels
{
    public partial class MainWindowViewModel : ObservableRecipient
    {
        private USBDeviceList? usbDevices;
        private CyUSBDevice? _myDevice;

        // 使用完整属性定义
        public CyUSBDevice? MyDevice
        {
            get => _myDevice;
            private set
            {
                SetProperty(ref _myDevice, value);
                if (value != null)
                {
                    WeakReferenceMessenger.Default.Send(new ValueChangedMessage<CyUSBDevice>(value));
                }
                
            }
        }

        [ObservableProperty]
        private string _title = "FX3设备未连接";

        public MainWindowViewModel()
        {
            FX3ConnectState();
        }

        public void FX3ConnectState()
        {
            usbDevices = new USBDeviceList(CyConst.DEVICES_CYUSB);

            MyDevice = usbDevices[0x04B4, 0x00F1] as CyUSBDevice;

            if (MyDevice != null)
            {
                Title = MyDevice.FriendlyName + " 已连接";
                
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
