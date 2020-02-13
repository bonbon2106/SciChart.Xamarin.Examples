﻿namespace Xamarin.Examples.Demo.Showcase.ECG
{
    public class ECGData
    {
        public ECGData(double xValue, double ecgHeartRate, double bloodPressure, double bloodVolume, double bloodOxygenation, bool isATrace)
        {
            XValue = xValue;
            ECGHeartRate = ecgHeartRate;
            BloodPressure = bloodPressure;
            BloodVolume = bloodVolume;
            BloodOxygenation = bloodOxygenation;
            IsATrace = isATrace;
        }

        public double XValue { get; }
        public double ECGHeartRate { get; }
        public  double BloodPressure { get; }
        public double BloodVolume { get; }
        public double BloodOxygenation { get; }

        public bool IsATrace { get; }
    }
}