using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
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
                var Plot = myPlot.Plot.Add.SignalConst(dataY, 1);//实例信号图
                //Plot.LegendText = $"Max:{dataY.Max()}Min:{dataY.Min()},Average:{dataY.Average()}Counter:{dataY.Length}";//配置说明框内容
                
                myPlot.Plot.Add.Annotation($"Max:{dataY.Max()}  Min:{dataY.Min()}  P-P:{dataY.Max()- dataY.Min()}  Average:{dataY.Average()}  Count:{dataY.Length}");
                myPlot.Plot.HideGrid();//隐藏网格
                //myPlot.Plot.Axes.Rules.Clear();
                
                //myPlot.Plot.Axes.AutoScale();
                //myPlot.Plot.SavePng("demo.png", 600, 400);
                myPlot.Plot.Axes.SetLimits(0, dataY.Length, dataY.Min(), dataY.Max());
                ScottPlot.AxisRules.LockedVertical rule = new(myPlot.Plot.Axes.Left, dataY.Min(), dataY.Max());
                myPlot.Plot.Axes.Rules.Add(rule);

                myPlot.Plot.Axes.Bottom.Label.Text = $"x/ns";
                myPlot.Plot.Axes.Left.Label.Text = $"y/Point";
                myPlot.Plot.Axes.Bottom.Label.ForeColor = Plot.Color;
                myPlot.Plot.Axes.Left.Label.ForeColor = Plot.Color;
                //myPlot.Plot.XLabel("Plot X Title");
                //myPlot.Plot.YLabel("Plot Y Title");
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
