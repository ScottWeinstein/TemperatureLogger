using System.Reactive.Concurrency;
namespace TLogger
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;

    public class ObservableUsbTemper : IDisposable
    {
        private readonly IDisposable _disp;
        public ObservableUsbTemper()
        {
            UsbTEMPer[] devices = UsbTEMPer.FindDevices()
                                            .Select((port, idx) => new UsbTEMPer(idx))
                                            .ToArray();
            var thermometer = devices.First();


            var txs = Observable.Generate(0, _ => true, t => t, t => thermometer.GetTemperature(), Scheduler.Default)
                .Skip(1)
                .Replay(1);
            TemperatureStreamCelcius = txs;
            TemperatureStreamFarenheight = txs.Select(UsbTEMPer.CelsiustoFarenheight);

            IDisposable txsConnect = txs.Connect();
            var disposables = (new[] { txsConnect }).Concat(devices);
            _disp = new CompositeDisposable(disposables);
        }

        public IObservable<double> TemperatureStreamCelcius { get; private set; }
        public IObservable<double> TemperatureStreamFarenheight { get; private set; }

        public void Dispose()
        {
            _disp.Dispose();
        }
    }
}
