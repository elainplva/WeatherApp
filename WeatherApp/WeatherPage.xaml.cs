using WeatherApp.Services;
using WeatherApp.Models;

namespace WeatherApp;

public partial class WeatherPage : ContentPage
{
    public List<Models.List> WeatherList;
    public double latitude;
    public double longitude;
    public WeatherPage()
	{
		InitializeComponent();
        WeatherList = new ();

    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await GetLocation();
            await GetWeatherDataByLocation(latitude, longitude);
        });
    }

    public async Task GetLocation()
    {
        var location = await Geolocation.GetLocationAsync();
        if (location != null)
        {
            latitude = location.Latitude;
            longitude = location.Longitude;
        }
        else
        {
            Console.WriteLine("Location not available.");
        }
    }


    private async void TapLocation_Tapped(object sender, EventArgs e)
    {
        await GetLocation();
        await GetWeatherDataByLocation(latitude, longitude);

    }

    public async Task GetWeatherDataByLocation(double latitude, double longitude)
    {
        var result = await ApiService.GetWeather(latitude, longitude);
        if (result != null)
        {
            UpdateUI(result);
        }
        else
        {
            Console.WriteLine("Weather data not available.");
        }
    }

    private async void ImageButton_Clicked(object sender, EventArgs e)    
    {
        var response = await DisplayPromptAsync(title: "", message: "", placeholder: "Search weather by city", accept: "Search", cancel: "Cancel");
        if (response != null)
        {
            await GetWeatherDataByCity(response);
        }
    }

    public async Task GetWeatherDataByCity(string city)
    {
        var result = await ApiService.GetWeatherByCity(city);
        if (result != null)
        {
            UpdateUI(result);
        }
        else
        {
            Console.WriteLine("Weather data not available.");
        }
    }


    public void UpdateUI(Root result)
    {
        WeatherList.Clear();
        if (result.List != null)
        {
            WeatherList.AddRange(result.List);
        }
        cvWeather.ItemsSource = WeatherList;

        var city = result.City?.Name;
        var firstItem = result.List?.FirstOrDefault();

        if (city != null || firstItem != null)
        {
            lbCity.Text = city ?? string.Empty;
            lbWeatherDesc.Text = firstItem?.Weather?[0].Description ?? string.Empty;
            lbTemperature.Text = firstItem?.Main?.Temperature + "°C" ?? string.Empty;
            lbHumidity.Text = firstItem?.Main?.Humidity + "%" ?? string.Empty;
            lbWind.Text = firstItem?.Wind?.Speed + "km/h" ?? string.Empty;
            imgWeatherIcon.Source = firstItem?.Weather?[0].CustomIcon ?? string.Empty;
        }
    }



}