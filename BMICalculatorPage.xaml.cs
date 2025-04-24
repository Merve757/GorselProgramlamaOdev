using Microsoft.Maui.Controls;
using System;
namespace MauiApp4
{
    public partial class BMICalculatorPage : ContentPage
    {
        private bool isUpdatingControls = false;

        public BMICalculatorPage()
        {
            InitializeComponent();
            // Sayfa y�klendi�inde ilk hesaplama
            CalculateBMI();
        }

        private void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (isUpdatingControls)
                return;

            isUpdatingControls = true;

            try
            {
                if (sender == WeightSlider)
                    WeightEntry.Text = $"{Math.Round(WeightSlider.Value, 1):F1}";
                else if (sender == HeightSlider)
                    HeightEntry.Text = $"{Math.Round(HeightSlider.Value, 0):F0}";

                CalculateBMI();
            }
            finally
            {
                isUpdatingControls = false;
            }
        }

        private void OnWeightEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            if (isUpdatingControls)
                return;

            isUpdatingControls = true;

            try
            {
                if (double.TryParse(WeightEntry.Text, out double newWeight))
                {
                    // S�n�r kontrol�
                    newWeight = Math.Clamp(newWeight, WeightSlider.Minimum, WeightSlider.Maximum);

                    if (Math.Abs(WeightSlider.Value - newWeight) > 0.01) // K���k farklar� yok sayma
                    {
                        WeightSlider.Value = newWeight;
                        // Giri�teki de�eri d�zeltme (s�n�r d��� de�er girildiyse)
                        if (Math.Abs(newWeight - double.Parse(WeightEntry.Text)) > 0.01)
                            WeightEntry.Text = $"{newWeight:F1}";
                    }
                }
                CalculateBMI();
            }
            catch (Exception ex)
            {
                // Hata olu�ursa varsay�lan de�ere d�n
                WeightEntry.Text = "70";
                WeightSlider.Value = 70;
            }
            finally
            {
                isUpdatingControls = false;
            }
        }

        private void OnHeightEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            if (isUpdatingControls)
                return;

            isUpdatingControls = true;

            try
            {
                if (double.TryParse(HeightEntry.Text, out double newHeight))
                {
                    // S�n�r kontrol�
                    newHeight = Math.Clamp(newHeight, HeightSlider.Minimum, HeightSlider.Maximum);

                    if (Math.Abs(HeightSlider.Value - newHeight) > 0.01) // K���k farklar� yok sayma
                    {
                        HeightSlider.Value = newHeight;
                        // Giri�teki de�eri d�zeltme (s�n�r d��� de�er girildiyse)
                        if (Math.Abs(newHeight - double.Parse(HeightEntry.Text)) > 0.01)
                            HeightEntry.Text = $"{newHeight:F0}";
                    }
                }
                CalculateBMI();
            }
            catch (Exception ex)
            {
                // Hata olu�ursa varsay�lan de�ere d�n
                HeightEntry.Text = "170";
                HeightSlider.Value = 170;
            }
            finally
            {
                isUpdatingControls = false;
            }
        }

        private void CalculateBMI()
        {
            try
            {
                if (!double.TryParse(WeightEntry.Text, out double weight) ||
                    !double.TryParse(HeightEntry.Text, out double heightInCm) ||
                    heightInCm <= 0 || weight <= 0)
                {
                    BMIValueLabel.Text = "Ge�erli bir de�er girin.";
                    BMICategoryLabel.Text = "";
                    return;
                }

                double heightInMeter = heightInCm / 100;
                double bmi = weight / (heightInMeter * heightInMeter);

                // Mant�ks�z de�erleri kontrol etme
                if (double.IsInfinity(bmi) || double.IsNaN(bmi) || bmi > 100)
                {
                    BMIValueLabel.Text = "Ge�erli bir de�er girin.";
                    BMICategoryLabel.Text = "";
                    return;
                }

                BMIValueLabel.Text = $"VK�: {bmi:F2}";

                string category;
                if (bmi < 16)
                    category = "�leri D�zeyde Zay�f";
                else if (bmi < 17)
                    category = "Orta D�zeyde Zay�f";
                else if (bmi < 18.5)
                    category = "Hafif D�zeyde Zay�f";
                else if (bmi < 25)
                    category = "Normal";
                else if (bmi < 30)
                    category = "Hafif �i�man / Fazla Kilolu";
                else if (bmi < 35)
                    category = "1. Derecede Obez";
                else if (bmi < 40)
                    category = "2. Derecede Obez";
                else
                    category = "3. Derecede Obez / Morbid Obez";

                BMICategoryLabel.Text = $"Durum: {category}";
            }
            catch (Exception ex)
            {
                BMIValueLabel.Text = "Hesaplama hatas� olu�tu.";
                BMICategoryLabel.Text = "";
            }
        }
    }
}