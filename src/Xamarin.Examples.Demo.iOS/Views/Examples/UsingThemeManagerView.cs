﻿using System;
using System.Linq;
using SciChart.Examples.Demo.Data;
using SciChart.Examples.Demo.Fragments.Base;
using SciChart.iOS.Charting;
using UIKit;
using Xamarin.Examples.Demo.iOS.Components;
using Xamarin.Examples.Demo.iOS.Resources.Layout;
using Xamarin.Examples.Demo.iOS.Views.Base;

namespace Xamarin.Examples.Demo.iOS.Views.Examples
{
    [ExampleDefinition("Using ThemeManager")]
    public class UsingThemeManagerView : ExampleBaseView
    {
        private static readonly string[] ThemeNames =
        {
            "Black Steel",
            "Bright Spark",
            "Chrome",
            "Chart V4 Dark",
            "Electric",
            "Expression Dark",
            "Expression Light",
            "Oscilloscope"
        };

        private readonly UsingThemeManagerLayout _exampleViewLayout = UsingThemeManagerLayout.Create();

        public SCIChartSurface Surface;

        public override UIView ExampleView => _exampleViewLayout;

        protected override void UpdateFrame()
        {
        }

        protected override void InitExampleInternal()
        {
            InitializeUIHandlers();

            Surface = new SCIChartSurface(_exampleViewLayout.SciChartSurfaceView);

            var xAxis = new SCINumericAxis {GrowBy = new SCIDoubleRange(0.1, 0.1), VisibleRange = new SCIDoubleRange(150, 180)};

            var yRightAxis = new SCINumericAxis
            {
                GrowBy = new SCIDoubleRange(0.1, 0.1),
                AxisAlignment = SCIAxisAlignment.Right,
                AutoRange = SCIAutoRange.Always,
                AxisId = "PrimaryAxisId",
                Style =
                {
                    DrawMajorTicks = false,
                    DrawMinorTicks = false,
                },
                LabelProvider = new ThousandsLabelProvider(),
            };

            var yLeftAxis = new SCINumericAxis
            {
                GrowBy = new SCIDoubleRange(0, 3d),
                AxisAlignment = SCIAxisAlignment.Left,
                AutoRange = SCIAutoRange.Always,
                AxisId = "SecondaryAxisId",
                Style =
                {
                    DrawMajorTicks = false,
                    DrawMinorTicks = false,
                },
                LabelProvider = new BillionsLabelProvider(),
            };

            var dataManager = DataManager.Instance;
            var priceBars = dataManager.GetPriceDataIndu();

            var mountainDataSeries = new XyDataSeries<double, double> {SeriesName = "Mountain Series"};
            var lineDataSeries = new XyDataSeries<double, double> {SeriesName = "Line Series"};
            var columnDataSeries = new XyDataSeries<double, long> {SeriesName = "Column Series"};
            var candlestickDataSeries = new OhlcDataSeries<double, double> {SeriesName = "Candlestick Series"};

            var xValues = Enumerable.Range(0, priceBars.Count).Select(x => (double)x).ToArray();

            mountainDataSeries.Append(xValues, priceBars.LowData.Select(x => x - 1000d));
            lineDataSeries.Append(xValues, dataManager.ComputeMovingAverage(priceBars.CloseData, 50));
            columnDataSeries.Append(xValues, priceBars.VolumeData);
            candlestickDataSeries.Append(xValues, priceBars.OpenData, priceBars.HighData, priceBars.LowData, priceBars.CloseData);

            var mountainRenderableSeries = new SCIFastMountainRenderableSeries {DataSeries = mountainDataSeries, YAxisId = "PrimaryAxisId"};
            var lineRenderableSeries = new SCIFastLineRenderableSeries {DataSeries = lineDataSeries, YAxisId = "PrimaryAxisId"};
            var columnRenderableSeries = new SCIFastColumnRenderableSeries {DataSeries = columnDataSeries, YAxisId = "SecondaryAxisId"};
            var candlestickRenderableSeries = new SCIFastCandlestickRenderableSeries {DataSeries = candlestickDataSeries, YAxisId = "PrimaryAxisId"};

            Surface.XAxes.Add(xAxis);
            Surface.YAxes.Add(yRightAxis);
            Surface.YAxes.Add(yLeftAxis);
            Surface.RenderableSeries.Add(mountainRenderableSeries);
            Surface.RenderableSeries.Add(lineRenderableSeries);
            Surface.RenderableSeries.Add(columnRenderableSeries);
            Surface.RenderableSeries.Add(candlestickRenderableSeries);

            Surface.ChartModifier = new SCIModifierGroup(new ISCIChartModifierProtocol[]
            {
                new SCILegendModifier {ShowCheckBoxes = false},
                new SCICursorModifier(),
                new SCIZoomExtentsModifier()
            });

            Surface.InvalidateElement();
            ApplyTheme(SCIThemeKey.SCIChartV4Dark);
        }

        private void InitializeUIHandlers()
        {
            _exampleViewLayout.SelectThemeButton.TouchUpInside += (sender, args) =>
            {
                var actionSheetAlert = UIAlertController.Create("Select Theme", null, UIAlertControllerStyle.ActionSheet);

                foreach (var themeName in ThemeNames)
                {
                    var themeAction = UIAlertAction.Create(themeName, UIAlertActionStyle.Default, action =>
                    {
                        ApplyTheme((SCIThemeKey) Array.IndexOf(ThemeNames, themeName));
                    });
                    actionSheetAlert.AddAction(themeAction);
                }

                actionSheetAlert.AddAction(UIAlertAction.Create("Cansel", UIAlertActionStyle.Cancel, null));

                if (actionSheetAlert.PopoverPresentationController != null)
                {
                    actionSheetAlert.PopoverPresentationController.SourceView = _exampleViewLayout;
                    actionSheetAlert.PopoverPresentationController.SourceRect = ((UIButton)sender).Frame;
                }

                _exampleViewLayout.Window.RootViewController.PresentViewController(actionSheetAlert, true, null);
            };
        }

        private void ApplyTheme(SCIThemeKey themeKey)
        {
            var themeProvider = new SCIThemeColorProvider(themeKey);
            Surface.ApplyThemeProvider(themeProvider);

            _exampleViewLayout.SelectThemeButton.SetTitle(ThemeNames[(int) themeKey], UIControlState.Normal);
        }
    }
}