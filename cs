using System;
using cAlgo.API;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class ChartDisplaySettingsEnhanced : Indicator
    {
        private StackPanel _settingsPanel;
        private StackPanel _infoPanel;
        private TextBlock _spreadTextBlock;
        private TextBlock _closePriceTextBlock;
        private CheckBox _masterCheckBox;
        private StackPanel _optionsPanel;
        private TextBlock _tehranTimeTextBlock;

        // Define the color options for text
        public enum TextColorOptions
        {
            Red,
            Green,
            Blue,
            Yellow,
            Black
        }

        // Define the color options for the drop-down lists
        public enum ColorOptions
        {
            Transparent,
            Black,
            LightGreen,
            lightgray,
            Red,
            Green,
            Blue,
            Yellow,
            White
        }

        [Parameter("Text Color", DefaultValue = TextColorOptions.Red)]
        public TextColorOptions SelectedColor { get; set; }

        [Parameter("Settings Panel Text Size", DefaultValue = 12)]
        public int SettingsPanelTextSize { get; set; }

        [Parameter("Close Price Font Size", DefaultValue = 16)]
        public int ClosePriceFontSize { get; set; }

        [Parameter("Default Background Color", DefaultValue = ColorOptions.Transparent)]
        public ColorOptions DefaultBackgroundColor { get; set; }

        [Parameter("Default Text Color", DefaultValue = ColorOptions.White)]
        public ColorOptions DefaultTextColor { get; set; }

        [Parameter("Active Background Color", DefaultValue = ColorOptions.LightGreen)]
        public ColorOptions ActiveBackgroundColor { get; set; }

        [Parameter("Inactive Background Color", DefaultValue = ColorOptions.Red)]
        public ColorOptions InactiveBackgroundColor { get; set; }

        [Parameter("Active Text Color", DefaultValue = ColorOptions.Yellow)]
        public ColorOptions ActiveTextColor { get; set; }

        [Parameter("Inactive Text Color", DefaultValue = ColorOptions.White)]
        public ColorOptions InactiveTextColor { get; set; }

        protected override void Initialize()
        {
            // Create settings panel
            _settingsPanel = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                BackgroundColor = GetColor(DefaultBackgroundColor),
                Opacity = 0.8,
                Margin = 2,
                Height = 480, // Adjusted to fit all items
                Width = 105,
                Orientation = Orientation.Vertical
            };

            // Add master checkbox for controlling other options
            _masterCheckBox = new CheckBox
            {
                Text = "Show Options",
                Margin = 5,
                IsChecked = true,
                FontSize = SettingsPanelTextSize, // Set the font size for the settings panel
                BackgroundColor = GetColor(DefaultBackgroundColor),
                ForegroundColor = GetColor(DefaultTextColor)
            };
            _masterCheckBox.Click += MasterCheckBox_Click;
            _settingsPanel.AddChild(_masterCheckBox);

            // Create TextBlock for displaying Tehran local time
            _tehranTimeTextBlock = new TextBlock
            {
                Text = GetTehranTime(),
                ForegroundColor = GetColor(InactiveTextColor),
                Margin = 5,
                IsVisible = !_masterCheckBox.IsChecked.Value
            };
            _settingsPanel.AddChild(_tehranTimeTextBlock);

            // Create options panel that will contain other checkboxes
            _optionsPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                IsVisible = _masterCheckBox.IsChecked.Value
            };

            // Add checkboxes for all display settings
            AddCheckBox("Ask Line", Chart.DisplaySettings.AskPriceLine, args => Chart.DisplaySettings.AskPriceLine = args.CheckBox.IsChecked.Value);
            AddCheckBox("Bid Line", Chart.DisplaySettings.BidPriceLine, args => Chart.DisplaySettings.BidPriceLine = args.CheckBox.IsChecked.Value);
            AddCheckBox("Scale", Chart.DisplaySettings.ChartScale, args => Chart.DisplaySettings.ChartScale = args.CheckBox.IsChecked.Value);
            AddCheckBox("Deals", Chart.DisplaySettings.DealMap, args => Chart.DisplaySettings.DealMap = args.CheckBox.IsChecked.Value);
            AddCheckBox("Grid", Chart.DisplaySettings.Grid, args => Chart.DisplaySettings.Grid = args.CheckBox.IsChecked.Value);
            AddCheckBox("Indicators", Chart.DisplaySettings.IndicatorTitles, args => Chart.DisplaySettings.IndicatorTitles = args.CheckBox.IsChecked.Value);
            AddCheckBox("Sentiment", Chart.DisplaySettings.MarketSentiment, args => Chart.DisplaySettings.MarketSentiment = args.CheckBox.IsChecked.Value);
            AddCheckBox("Orders", Chart.DisplaySettings.Orders, args => Chart.DisplaySettings.Orders = args.CheckBox.IsChecked.Value);
            AddCheckBox("Separators", Chart.DisplaySettings.PeriodSeparators, args => Chart.DisplaySettings.PeriodSeparators = args.CheckBox.IsChecked.Value);
            AddCheckBox("Positions", Chart.DisplaySettings.Positions, args => Chart.DisplaySettings.Positions = args.CheckBox.IsChecked.Value);
            AddCheckBox("Alerts", Chart.DisplaySettings.PriceAlerts, args => Chart.DisplaySettings.PriceAlerts = args.CheckBox.IsChecked.Value);
            AddCheckBox("Axis Buttons", Chart.DisplaySettings.PriceAxisOverlayButtons, args => Chart.DisplaySettings.PriceAxisOverlayButtons = args.CheckBox.IsChecked.Value);
            AddCheckBox("Trade Buttons", Chart.DisplaySettings.QuickTradeButtons, args => Chart.DisplaySettings.QuickTradeButtons = args.CheckBox.IsChecked.Value);
            AddCheckBox("Targets", Chart.DisplaySettings.Targets, args => Chart.DisplaySettings.Targets = args.CheckBox.IsChecked.Value);
            AddCheckBox("Volume", Chart.DisplaySettings.TickVolume, args => Chart.DisplaySettings.TickVolume = args.CheckBox.IsChecked.Value);

            _settingsPanel.AddChild(_optionsPanel);

            Chart.AddControl(_settingsPanel);

            // Create info panel for spread and close price
            _infoPanel = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom, // Changed to bottom
                BackgroundColor = Color.FromArgb(204, 204, 204, 204), // Light gray with transparency
                Opacity = 0.8,
                Margin = 2,
                Width = 150,
                Orientation = Orientation.Vertical
            };

            _spreadTextBlock = new TextBlock
            {
                Text = "Spread: 0.000",
                ForegroundColor = Color.FromArgb(255, 255, 255, 255), // White text color
                BackgroundColor = Color.FromArgb(255, 0, 0, 0), // Black background color
                Margin = 5
            };

            _closePriceTextBlock = new TextBlock
            {
                Text = "Close: 0.000",
                ForegroundColor = GetSelectedColor(),
                Margin = 5,
                FontSize = ClosePriceFontSize // Set the font size for the close price
            };

            _infoPanel.AddChild(_spreadTextBlock);
            _infoPanel.AddChild(_closePriceTextBlock);

            Chart.AddControl(_infoPanel);
        }

        private void AddCheckBox(string text, bool isChecked, Action<CheckBoxEventArgs> onCheckedChanged)
        {
            var checkBox = new CheckBox
            {
                Text = text,
                Margin = 5,
                IsChecked = isChecked,
                FontSize = SettingsPanelTextSize, // Set the font size for the settings panel
                BackgroundColor = GetColor(DefaultBackgroundColor), // Ensure it is always visible
                ForegroundColor = GetColor(DefaultTextColor) // Ensure it is always visible
            };

            checkBox.Click += onCheckedChanged;
            _optionsPanel.AddChild(checkBox);
        }

        private void MasterCheckBox_Click(CheckBoxEventArgs args)
        {
            _optionsPanel.IsVisible = args.CheckBox.IsChecked.Value;
            _tehranTimeTextBlock.IsVisible = !args.CheckBox.IsChecked.Value;
            
            if (args.CheckBox.IsChecked.Value)
            {
                _masterCheckBox.BackgroundColor = GetColor(ActiveBackgroundColor);
                _masterCheckBox.ForegroundColor = GetColor(ActiveTextColor);
            }
            else
            {
                _masterCheckBox.BackgroundColor = GetColor(InactiveBackgroundColor);
                _masterCheckBox.ForegroundColor = GetColor(InactiveTextColor);
                _tehranTimeTextBlock.Text = GetTehranTime(); // Update the Tehran time
            }
        }

        private string GetTehranTime()
        {
            var tehranTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Iran Standard Time");
            var tehranTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, tehranTimeZone);
            return tehranTime.ToString("HH:mm:ss"); // Only show time
        }

        public override void Calculate(int index)
        {
            // Get current ask, bid, and close prices
            double ask = Symbol.Ask;
            double bid = Symbol.Bid;
            double close = Bars.LastBar.Open;

            // Calculate spread
            double spread = Math.Max(ask, bid) - Math.Min(ask, bid);

            // Update the text blocks with fixed digits
            _spreadTextBlock.Text = $"Spread: {spread:F3}";
            _closePriceTextBlock.Text = $"Close: {close:F3}";
        }

        private Color GetSelectedColor()
        {
            return SelectedColor switch
            {
                TextColorOptions.Red => Color.FromArgb(255, 255, 0, 0),
                TextColorOptions.Green => Color.FromArgb(255, 0, 255, 0),
                TextColorOptions.Blue => Color.FromArgb(255, 0, 0, 255),
                TextColorOptions.Yellow => Color.FromArgb(255, 255, 255, 0),
                TextColorOptions.Black => Color.FromArgb(255, 0, 0, 0),
                _ => Color.FromArgb(255, 0, 0, 0) // Default to black
            };
        }

        private Color GetColor(ColorOptions colorOption)
        {
            return colorOption switch
            {
                ColorOptions.Transparent => Color.FromArgb(0, 0, 0, 0),
                ColorOptions.Black => Color.FromArgb(255, 0, 0, 0),
                ColorOptions.LightGreen => Color.FromArgb(204, 144, 238, 144),
                ColorOptions.lightgray => Color.FromArgb(15, 128, 128, 128),
                ColorOptions.Red => Color.FromArgb(204, 255, 0, 0),
                ColorOptions.Green => Color.FromArgb(204, 0, 255, 0),
                ColorOptions.Blue => Color.FromArgb(204, 0, 0, 255),
                ColorOptions.Yellow => Color.FromArgb(204, 255, 255, 0),
                ColorOptions.White => Color.FromArgb(204, 255, 255, 255),
                _ => Color.FromArgb(0, 0, 0, 0) // Default to transparent
            };
        }
    }
}
