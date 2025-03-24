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
        WeatherList = new List<Models.List>();

    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await GetLocation();
        await GetWeatherDataByLocation(latitude, longitude);

    }

    public async Task GetLocation()
    {
        var location = await Geolocation.GetLocationAsync();
        latitude = location.Latitude;
        longitude = location.Longitude;
    }

    private async Task TapLocation_Tapped(object sender, EventArgs e)
    {
        await GetLocation();
        await GetWeatherDataByLocation(latitude, longitude);

    }

    public async Task GetWeatherDataByLocation(double latiude, double longitude)
    {
        var result = await ApiService.GetWeather(latitude, longitude);
        UpdateUI(result);
    }

    private async void ImgButton_Clicked(object sender, EventArgs e)
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
        UpdateUI(result);
    }

    public void UpdateUI(dynamic result)
    {
        foreach (var item in result.list)
        {
            WeatherList.Add(item);
        }
        cvWeather.ItemsSource = WeatherList;

        lbCity.Text = result.city.name;
        lbWeatherDesc.Text = result.list[0].weather[0].description;
        lbTemperature.Text = result.list[0].main.temperature + "°C";
        lbHumidity.Text = result.list[0].main.humidity + "%";
        lbWind.Text = result.list[0].wind.speed + "km/h";
        imgWeatherIcon.Source = result.list[0].weather[0].customIcon;

    }


}