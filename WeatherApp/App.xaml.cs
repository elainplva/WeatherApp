namespace WeatherApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            VersionTracking.Track();

        }

        protected override Window CreateWindow(IActivationState? activationState) // Fix: Make activationState nullable
        {
            return new Window(DetermineStartupPage());
        }

        private Page DetermineStartupPage()
        {
            if (VersionTracking.IsFirstLaunchEver)
            {
                return new WelcomePage();
            }
            else
            {
                return new WeatherPage();
            }

        }


    }
}
