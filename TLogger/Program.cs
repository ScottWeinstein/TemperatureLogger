using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.ServiceProcess;
using ServiceStack.Redis;
using ServiceStack.Text;

namespace TLogger
{
    static class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, e) => Console.Error.WriteLine(e.ExceptionObject);

            if (args.Length >= 0)
            {
                JsConfig.DateHandler = JsonDateHandler.ISO8601;

                string host = args.Length == 0 ? "localhost" : args.First();

                var redisClient = new RedisClient(host);
                var temprc = redisClient.As<TemperatureItem>();
                var rtempList = temprc.Lists["tempatureReadings"];

                //Console.WriteLine(temprc.GetAllItemsFromList(rtempList).First().Timestamp);
                //Console.WriteLine(temprc.GetAllItemsFromList(rtempList).Last().Temperature);

                Console.WriteLine(string.Join("\n", UsbTEMPer.FindDevices().ToArray()));
                var xs = new ObservableUsbTemper();
                var temps = xs.TemperatureStreamFarenheight
                              .Timestamp()
                              .Do(ts => Console.WriteLine(ts));

                var sub = temps.Select(TemperatureItem.FromTDS)
                               .Subscribe(tst => temprc.AddItemToList(rtempList, tst), Console.Error.WriteLine, () => { });

                var disp = new CompositeDisposable(xs, redisClient, sub);
                Console.ReadKey();
                disp.Dispose();
            }
            else
            {
                ServiceBase.Run(new ServiceBase[] 
                { 
                    new TempLoggerService() 
                });
            }
        }
    }
}
