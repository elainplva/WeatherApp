namespace WeatherApp;

public partial class WelcomePage : ContentPage
{
	public WelcomePage()
	{
		InitializeComponent();
	}

    private void Window_Loaded(object sender, EventArgs e)
    {

    }
    private void BtnStarted_Clicked(object sender, EventArgs e)
    {
        Navigation.PushModalAsync(new WeatherPage());
    }

}