﻿using SciChart.Examples.Demo.Data;
using SciChart.Examples.Demo.Fragments.Base;
using SciChart.iOS.Charting;
using Xamarin.Examples.Demo.iOS.Helpers;
using Xamarin.Examples.Demo.iOS.Views.Base;

namespace Xamarin.Examples.Demo.iOS.Views.Examples
{
    [ExampleDefinition("Line Chart")]
    public class LineChartView : ExampleBaseView
    {
        public SCIChartSurface Surface;

        protected override void InitExample()
        {
            var fourierSeries = DataManager.Instance.GetFourierSeries(1.0, 0.1);

            var dataSeries = new SCIXyDataSeries<double, double>();
            dataSeries.AppendRange(fourierSeries.XData, fourierSeries.YData, fourierSeries.Count);

            //TODO Remove AxisId, should be default (DefaultAxisId)
            var axisStyle = StyleHelper.GetDefaultAxisStyle();
            //TODO Add GrowBy = new DoubleRange(0.1, 0.1)
            var xAxis = new SCINumericAxis {IsXAxis = true, AxisId = "xAxis", Style = axisStyle};
            //TODO Add GrowBy = new DoubleRange(0.1, 0.1)
            var yAxis = new SCINumericAxis {AxisId = "yAxis", Style = axisStyle};

            var renderSeries = new SCIFastLineRenderableSeries
            {
                DataSeries = dataSeries,
                XAxisId = "xAxis",
                YAxisId = "yAxis",
                Style = {LinePen = new SCIPenSolid(0xFF99EE99, 0.7f)}
            };

            Surface = new SCIChartSurface(this);
            StyleHelper.SetSurfaceDefaultStyle(Surface);

            Surface.AttachAxis(xAxis, true);
            Surface.AttachAxis(yAxis, false);
            Surface.AttachRenderableSeries(renderSeries);

            Surface.ChartModifier = new SCIModifierGroup(new ISCIChartModifierProtocol[]
            {
                new SCIZoomPanModifier(),
                new SCIPinchZoomModifier(),
                new SCIZoomExtentsModifier()
            });

            Surface.InvalidateElement();
        }
    }
}