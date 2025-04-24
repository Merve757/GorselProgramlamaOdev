using Microsoft.Maui.Controls;
using System;

namespace MauiApp4
{
    public partial class CreditCalculatorPage : ContentPage
    {
        public CreditCalculatorPage()
        {
            InitializeComponent();
            CreditTypePicker.SelectedIndex = 0;
        }

        private void OnTermSliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            int months = (int)e.NewValue;
            TermLabel.Text = $"{months} Ay";
        }

        private async void OnCalculateClicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(CreditAmountEntry.Text) ||
                    string.IsNullOrWhiteSpace(InterestRateEntry.Text) ||
                    CreditTypePicker.SelectedIndex == -1)
                {
                    await DisplayAlert("Hata", "L�tfen t�m alanlar� doldurunuz.", "Tamam");
                    return;
                }

                if (!double.TryParse(CreditAmountEntry.Text, out double principal))
                {
                    await DisplayAlert("Hata", "Ge�erli bir kredi tutar� giriniz.", "Tamam");
                    return;
                }

                if (!double.TryParse(InterestRateEntry.Text, out double interestRate))
                {
                    await DisplayAlert("Hata", "Ge�erli bir faiz oran� giriniz.", "Tamam");
                    return;
                }

                int term = (int)TermSlider.Value;

                // KKDF ve BSMV de�erleri
                double kkdf = 0;
                double bsmv = 0;
                string creditType = CreditTypePicker.SelectedItem.ToString();

                switch (creditType)
                {
                    case "�htiya� Kredisi":
                        kkdf = 15; // %15
                        bsmv = 10; // %10
                        break;
                    case "Ta��t Kredisi":
                        kkdf = 15;
                        bsmv = 5;
                        break;
                    case "Konut Kredisi":
                        kkdf = 0;
                        bsmv = 0;
                        break;
                    case "Ticari Kredisi":
                        kkdf = 0;
                        bsmv = 5;
                        break;
                    default:
                        kkdf = 15;
                        bsmv = 10;
                        break;
                }

                double brutFaiz = ((interestRate + (interestRate * bsmv / 100) + (interestRate * kkdf / 100)) / 100);

                double taksit = ((Math.Pow(1 + brutFaiz, term) * brutFaiz) / (Math.Pow(1 + brutFaiz, term) - 1)) * principal;

                double toplam = taksit * term;
                double totalInterest = toplam - principal;

                MontlyPaymentLabel.Text = $"Ayl�k �deme: {taksit:N2} TL";
                TotalPaymentLabel.Text = $"Toplam �deme: {toplam:N2} TL";
                TotalInterestLabel.Text = $"Toplam Faiz: {totalInterest:N2} TL";
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hesaplama Hatas�", $"Hesaplama s�ras�nda bir hata olu�tu: {ex.Message}", "Tamam");
            }
        }
    }
}
