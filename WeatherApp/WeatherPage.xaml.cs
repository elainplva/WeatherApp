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
            foreach (var item in result.List)
            {
                WeatherList.Add(item);
            }
        }
        cvWeather.ItemsSource = WeatherList;

        if (result.City != null)
        {
            lbCity.Text = result.City.Name;
        }
        if (result.List != null && result.List.Count > 0)
        {
            lbWeatherDesc.Text = result.List[0].Weather?[0].Description;
            lbTemperature.Text = result.List[0].Main?.Temperature + "°C";
            lbHumidity.Text = result.List[0].Main?.Humidity + "%";
            lbWind.Text = result.List[0].Wind?.Speed + "km/h";
            imgWeatherIcon.Source = result.List[0].Weather?[0].CustomIcon;
        }
    }



}