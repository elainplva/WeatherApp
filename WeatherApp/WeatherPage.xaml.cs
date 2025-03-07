using WeatherApp.Services;

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
		var result = await ApiService.GetWeather(latitude,longitude);
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

	public async Task GetLocation()
	{
		var location = await Geolocation.GetLocationAsync();
		latitude = location.Latitude;
		longitude = location.Longitude;
	}


}