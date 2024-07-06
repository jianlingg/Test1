using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using ScottPlot;
using System.Windows.Controls;

namespace Test1.Views.Pages
{
    /// <summary>
    /// DataPage.xaml 的交互逻辑
    /// </summary>
    public partial class DataPage : Page, IRecipient<ValueChangedMessage<int[]>>
    {
        int[] dataY = Array.Empty<int>();


        public DataPage()
        {
            InitializeComponent();
            // 注册消息接收器
            WeakReferenceMessenger.Default.Register<ValueChangedMessage<int[]>>(this);
        }

        public void Receive(ValueChangedMessage<int[]> message)
        {
            dataY = message.Value;

            // 刷新绘图
            if (dataY.Length > 0)
            {
                myPlot.Plot.Clear(); // 清除之前的绘图
                var s = myPlot.Plot.Add.Signal(dataY, 1);
                s.LegendText = $"Max:{dataY.Max()}Min:{dataY.Min()},Average:{dataY.Average()}Counter:{dataY.Length}";
                myPlot.Plot.Axes.AutoScale();
                myPlot.Plot.SavePng("demo.png", 600, 400);
                //myPlot.Plot.Axes.SetLimits(0, dataY.Length, dataY.Min(), dataY.Max());
                //myPlot.Plot.XLabel("Plot Title");
                //myPlot.Plot.YLabel("Plot Title");
                //myPlot.Plot.Title("Plot Title");

                //var myScatter = WpfPlot1.Plot.Add.Scatter(dataX, dataY);
                //myScatter.Color = Colors.Green.WithOpacity(.2);
                //myScatter.LineWidth = 5;
                //myScatter.MarkerSize = 15;
                myPlot.Refresh();
            }
        }
    }
}
