using Microsoft.Maui.Controls;
using System;

namespace MauiApp4
{
    public partial class ColorPickerPage : ContentPage
    {
        private readonly Random random = new Random();

        public ColorPickerPage()
        {
            InitializeComponent();
            UpdateColor();
        }

        private void OnColorSliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            RedLabel.Text = ((int)RedSlider.Value).ToString();
            GreenLabel.Text = ((int)GreenSlider.Value).ToString();
            BlueLabel.Text = ((int)BlueSlider.Value).ToString();

            UpdateColor();
        }

        private void UpdateColor()
        {
            int r = (int)RedSlider.Value;
            int g = (int)GreenSlider.Value;
            int b = (int)BlueSlider.Value;

            var color = Color.FromRgb(r, g, b);
            ColorPreview.Fill = color;

            string hex = $"#{r:X2}{g:X2}{b:X2}";
            HexColorLabel.Text = $"Renk Kodu: {hex}";

            // Sadece RGB label'larýnýn rengini deðiþtiriyoruz
            Color dynamicTextColor = (r + g + b) / 3 < 128 ? Colors.White : Colors.Black;
            RedLabel.TextColor = dynamicTextColor;
            GreenLabel.TextColor = dynamicTextColor;
            BlueLabel.TextColor = dynamicTextColor;

            // Bunlar sabit (manuel set ile force override yapýyoruz)
            HexColorLabel.TextColor = Colors.Black;

            // Android'de TextColor override edilmediðinden emin olmak için stil uygula
            CopyButton.TextColor = Colors.White;
            CopyButton.BackgroundColor = Color.FromArgb("#007AFF");
            CopyButton.FontAttributes = FontAttributes.Bold;

            RandomButton.TextColor = Colors.White;
            RandomButton.BackgroundColor = Color.FromArgb("#34C759");
            RandomButton.FontAttributes = FontAttributes.Bold;
        }

        private async void OnCopyClicked(object sender, EventArgs e)
        {
            string hex = HexColorLabel.Text.Replace("Renk Kodu: ", "");
            await Clipboard.SetTextAsync(hex);
            await DisplayAlert("Kopyalandý", hex, "Tamam");
        }

        private void OnRandomClicked(object sender, EventArgs e)
        {
            int r = random.Next(256);
            int g = random.Next(256);
            int b = random.Next(256);

            RedSlider.Value = r;
            GreenSlider.Value = g;
            BlueSlider.Value = b;

            RedLabel.Text = r.ToString();
            GreenLabel.Text = g.ToString();
            BlueLabel.Text = b.ToString();

            UpdateColor();
        }
    }
}
