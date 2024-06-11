using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using CyUSB;
using Test1.Serves;

namespace Test1.ViewModels
{
    public partial class MainWindowViewModel : ObservableRecipient,IRecipient<ValueChangedMessage<string>>
    {
        
        [ObservableProperty]
        private string _Title = "设备连接的状态展示";

        public void Receive(ValueChangedMessage<string> message)
        {
            Title = message.Value;
        }
    }
}
