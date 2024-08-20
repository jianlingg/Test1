using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using Test1.Serves;

namespace Test1.ViewModels
{
    public partial class WavePageViewModel : ObservableRecipient
    {

        [ObservableProperty]
        public string? _waveRange = "0-99";

        [ObservableProperty]
        public string? _waveCount = "1";

        [ObservableProperty]
        public int? _Saw_t = 100;//点的数量

        [ObservableProperty]
        public double? _Saw_width = 0.5;

        [ObservableProperty]
        public double? _Saw_Amplitude = 1.0;

        [ObservableProperty]
        public int? _Sin_t = 10;

        [ObservableProperty]
        public double? _Sin_Amplitude = 1.0;

        [ObservableProperty]
        public string? _waveFormula = "0-99:16200, 100-299:0, 300-499:10*x";

        

        [RelayCommand]
        private void SinGen()
        {
            var myDevice = new FX3DataHandle().FX3Device();

            if (myDevice == null || Sin_t == null || Sin_Amplitude == null)
                return;

            var sinWave = CommandGenerate.Sinwave(t: (int)Sin_t!, Amplitude: (double)Sin_Amplitude!);
            var sinData = CommandGenerate.WaveGenerate(sinWave);
            var wave = CommandGenerate.WaveConfig(sinData, WaveRange!, WaveCount!);
            var cmd = CommandGenerate.FX3CmdGen(2, wave);

            new FX3DataHandle().FX3DataOut(myDevice, cmd!);

        }

        [RelayCommand]
        private void SawGen()
        {
            var myDevice = new FX3DataHandle().FX3Device();

            if (myDevice == null || Saw_t == null || Saw_width == null || Saw_Amplitude == null)
                return;

            var sawWave = CommandGenerate.Sawtooth(t: (int)Saw_t!, width: (double)Saw_width!, Amplitude: (double)Saw_Amplitude!);
            var sawData = CommandGenerate.WaveGenerate(sawWave);
            var wave = CommandGenerate.WaveConfig(sawData, WaveRange!, WaveCount!);
            var cmd = CommandGenerate.FX3CmdGen(2, wave);

            new FX3DataHandle().FX3DataOut(myDevice, cmd!);

        }

        [RelayCommand]
        private void WaveGen()
        {
            var myDevice = new FX3DataHandle().FX3Device();

            if (myDevice == null || WaveFormula == null)
                return;

            var FormulaWave = CommandGenerate.Formula(WaveFormula!);
            var FormulaData = CommandGenerate.WaveGenerate(FormulaWave);
            var wave = CommandGenerate.WaveConfig(FormulaData, WaveRange!, WaveCount!);
            var cmd = CommandGenerate.FX3CmdGen(2, wave);

            new FX3DataHandle().FX3DataOut(myDevice, cmd!);

        }

    }
}
