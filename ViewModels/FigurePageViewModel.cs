using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Collections.ObjectModel;

namespace Test1.ViewModels
{
    public partial class FigurePageViewModel : ObservableRecipient, IRecipient<ValueChangedMessage<List<string>>>
    {
        [ObservableProperty]
        private ObservableCollection<ISeries>? series;

        [ObservableProperty]
        private ObservableCollection<Axis>? xAxes;

        [ObservableProperty]
        private ObservableCollection<Axis>? yAxes;

        private LineSeries<double>? _lineSeries;

        public void Receive(ValueChangedMessage<List<string>> message)
        {
            if (message != null)
            {
                var DataInt = message.Value.Select(DataString => Convert.ToInt32(DataString, 16)).ToList();

                Series = new ObservableCollection<ISeries>();
                XAxes = new ObservableCollection<Axis> { new Axis { Labeler = value => value.ToString(), Name = "DataPointer" } };
                YAxes = new ObservableCollection<Axis> { new Axis { Labeler = value => value.ToString(), Name = "DataValue" } };
                _lineSeries = new LineSeries<double>
                {
                    Values = new ObservableCollection<double>(DataInt.ConvertAll(x => (double)x)),
                    Fill = null
                };

                Series.Add(_lineSeries);
            }
        }

    }


}
