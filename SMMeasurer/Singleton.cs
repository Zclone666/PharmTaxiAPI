using System;
using SM.Measurer;
using SM.Measurer.Services;

namespace SMMeasurer
{
    public static class Singleton
    {
        public static IMeasurerProvider MeasurerProvider { get; set; }

        public static void Initialize()
        {
            MeasurerProvider = new MeasurerProvider();
        }
    }
}
