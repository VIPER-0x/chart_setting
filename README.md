# chart_setting
Below is a detailed explanation of the `ChartDisplaySettingsEnhanced` code, which can be used for a GitHub README file:

---

# Chart Display Settings Enhanced

This cTrader custom indicator, `ChartDisplaySettingsEnhanced`, provides a user-friendly interface to configure and monitor various chart display settings. It combines flexibility and enhanced visualization to give traders more control over their chart preferences. Below is a breakdown of the features and functionality of the indicator.

---

## Features

1. **Interactive Settings Panel**  
   A collapsible settings panel located at the top-right corner of the chart allows users to enable or disable various display settings dynamically.

2. **Customizable Appearance**  
   - **Text Colors:** Choose from predefined text color options such as red, green, blue, yellow, or black.
   - **Background Colors:** Configure default, active, and inactive background colors.
   - **Font Sizes:** Set font sizes for both the settings panel and the price display.

3. **Tehran Time Display**  
   - A dedicated TextBlock dynamically displays the current time in Tehran.  
   - This display can toggle visibility depending on the "Show Options" checkbox.

4. **Live Spread and Close Price Information**  
   - At the bottom-right corner of the chart, the indicator displays the live spread and last close price.  
   - The values update dynamically as the market changes.

5. **Toggleable Chart Display Options**  
   Users can individually enable or disable various chart elements, such as:
   - Ask Line  
   - Bid Line  
   - Grid  
   - Indicators  
   - Orders, Positions, and Alerts  
   - And more...

6. **Dynamic UI Updates**  
   - The colors and visibility of components are updated in real time based on user interactions.
   - Active and inactive states of components are visually distinct.

---

## Parameters

The indicator includes several configurable parameters to personalize the display:

| Parameter                     | Description                                              | Default Value            |
|-------------------------------|----------------------------------------------------------|--------------------------|
| Text Color                    | Text color for close price and other elements.           | Red                      |
| Settings Panel Text Size      | Font size for the settings panel text.                   | 12                       |
| Close Price Font Size         | Font size for the close price display.                   | 16                       |
| Default Background Color      | Background color of the settings panel.                  | Transparent              |
| Default Text Color            | Default text color of the settings panel.                | White                    |
| Active Background Color       | Background color for active components.                  | LightGreen               |
| Inactive Background Color     | Background color for inactive components.                | Red                      |
| Active Text Color             | Text color for active components.                        | Yellow                   |
| Inactive Text Color           | Text color for inactive components.                      | White                    |

---

## How It Works

### Settings Panel
The settings panel contains:
- A master checkbox ("Show Options") to toggle the visibility of all other options.
- A set of individual checkboxes for enabling/disabling specific chart settings like Ask Line, Bid Line, Grid, etc.

When the "Show Options" checkbox is unchecked:
- The settings panel collapses, and the Tehran time display becomes visible.

### Info Panel
The info panel displays:
- **Spread**: The difference between the ask and bid prices.
- **Close Price**: The last closing price of the current bar.

### Customization
- The visual appearance of the settings panel and the info panel can be customized using parameters for font size, text color, and background color.

---

## Code Highlights

### Adding Checkboxes
The `AddCheckBox` method dynamically creates checkboxes for each chart display option:
```csharp
private void AddCheckBox(string text, bool isChecked, Action<CheckBoxEventArgs> onCheckedChanged)
{
    var checkBox = new CheckBox
    {
        Text = text,
        Margin = 5,
        IsChecked = isChecked,
        FontSize = SettingsPanelTextSize,
        BackgroundColor = GetColor(DefaultBackgroundColor),
        ForegroundColor = GetColor(DefaultTextColor)
    };

    checkBox.Click += onCheckedChanged;
    _optionsPanel.AddChild(checkBox);
}
```

### Updating Tehran Time
The Tehran time is calculated and displayed using the `GetTehranTime` method:
```csharp
private string GetTehranTime()
{
    var tehranTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Iran Standard Time");
    var tehranTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, tehranTimeZone);
    return tehranTime.ToString("HH:mm:ss");
}
```

### Real-Time Data Updates
The `Calculate` method updates the spread and close price dynamically:
```csharp
public override void Calculate(int index)
{
    double ask = Symbol.Ask;
    double bid = Symbol.Bid;
    double close = Bars.LastBar.Open;

    double spread = Math.Max(ask, bid) - Math.Min(ask, bid);

    _spreadTextBlock.Text = $"Spread: {spread:F3}";
    _closePriceTextBlock.Text = $"Close: {close:F3}";
}
```

### Color Management
The `GetColor` method maps the `ColorOptions` to actual colors:
```csharp
private Color GetColor(ColorOptions colorOption)
{
    return colorOption switch
    {
        ColorOptions.Transparent => Color.FromArgb(0, 0, 0, 0),
        ColorOptions.Black => Color.FromArgb(255, 0, 0, 0),
        ColorOptions.LightGreen => Color.FromArgb(204, 144, 238, 144),
        ColorOptions.lightgray => Color.FromArgb(15, 128, 128, 128),
        ColorOptions.Red => Color.FromArgb(204, 255, 0, 0),
        _ => Color.FromArgb(0, 0, 0, 0) // Default to transparent
    };
}
```

---

## Usage

1. Compile and add the indicator to your cTrader platform.
2. Configure the parameters to suit your preferences.
3. Use the settings panel to toggle chart display settings in real time.

This indicator enhances your trading experience by allowing you to personalize and simplify chart displays.

--- 

