using AMApp4.Helper;
using AMApp4.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AMApp4.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CurrentAMPage : ContentPage
    {
        public CurrentAMPage(string location = null)
        {
            InitializeComponent();
            CheckForCity(location);
        }
        public string Location { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        
        public void CheckForCity(string location = null)
        {
            if (location == null)
                GetCoordinates();
            else
            {
                navigationTitle.Text = "Searched Location";
                navigationTitle.Padding = new Thickness(0, 0, 75, 0);
                GetWeatherInfo(location);
            }
        }
        
        private async void GetCoordinates()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Best);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    Latitude = location.Latitude;
                    Longitude = location.Longitude;
                    Location = await GetCity(location);    
                }

                GetWeatherInfo();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        private async Task<string> GetCity(Location location)
        {
            var place = await Geocoding.GetPlacemarksAsync(location);
            var current = place?.FirstOrDefault();
            if (current != null)
            {
                return $"{current.Locality}, {current.CountryName}";
            }
            else
            {
                return null;
            }
        }
        private async void GetWeatherInfo(string location = null)
        {
            string url;
            if (location == null)
            {
                url = $"http://api.openweathermap.org/data/2.5/weather?q={Location}&appid=de5a8b4d5daa21aa6c98b9ac07f71601&units=metric";
            }
            else
            {
                url = $"http://api.openweathermap.org/data/2.5/weather?q={location}&appid=de5a8b4d5daa21aa6c98b9ac07f71601&units=metric";
            }

            var result = await ApiCaller.Get(url);

            if (result.Successful)
            {
                try
                {
                    var weatherInfo = JsonConvert.DeserializeObject<WeatherInfo>(result.Response);
                    descriptionTxt.Text = weatherInfo.weather[0].description.ToUpper();
                    iconImg.Source = $"w{weatherInfo.weather[0].icon}";
                    cityTxt.Text = weatherInfo.name.ToUpper();
                    temperatureTxt.Text = weatherInfo.main.temp.ToString("0");
                    humidityTxt.Text = $"{weatherInfo.main.humidity}%";
                    pressureTxt.Text = $"{weatherInfo.main.pressure} hpa";
                    windTxt.Text = $"{weatherInfo.wind.speed} m/s";
                    cloudinessTxt.Text = $"{weatherInfo.clouds.all}%";

                    var dt = DateTime.UtcNow;
                    dateTxt.Text = dt.ToString("dddd, MMM dd").ToUpper();

                    GetForecast(location);
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Weather Info", ex.Message, "OK");
                }
            }
            else
            {
                await DisplayAlert("Weather Info", "No weather information found", "OK");
            }
        }

        private async void GetForecast(string location = null)
        {
            string url;
            if (location == null)
            {
                url = $"http://api.openweathermap.org/data/2.5/forecast?q={Location}&appid=de5a8b4d5daa21aa6c98b9ac07f71601&units=metric";
            }
            else
            {
                url = $"http://api.openweathermap.org/data/2.5/forecast?q={location}&appid=de5a8b4d5daa21aa6c98b9ac07f71601&units=metric";
            }
            var result = await ApiCaller.Get(url);

            if (result.Successful)
            {
                try
                {
                    var forcastInfo = JsonConvert.DeserializeObject<ForecastInfo>(result.Response);

                    List<List> allList = new List<List>();

                    foreach (var list in forcastInfo.list)
                    {
                        var date = DateTime.Parse(list.dt_txt);

                        if (date > DateTime.Now && date.Hour == 0 && date.Minute == 0 && date.Second == 0)
                            allList.Add(list);
                    }

                    dayOneTxt.Text = DateTime.Parse(allList[0].dt_txt).ToString("dddd");
                    dateOneTxt.Text = DateTime.Parse(allList[0].dt_txt).ToString("dd MMM");
                    iconOneImg.Source = $"w{allList[0].weather[0].icon}";
                    tempOneTxt.Text = allList[0].main.temp.ToString("0");

                    dayTwoTxt.Text = DateTime.Parse(allList[1].dt_txt).ToString("dddd");
                    dateTwoTxt.Text = DateTime.Parse(allList[1].dt_txt).ToString("dd MMM");
                    iconTwoImg.Source = $"w{allList[1].weather[0].icon}";
                    tempTwoTxt.Text = allList[1].main.temp.ToString("0");

                    dayThreeTxt.Text = DateTime.Parse(allList[2].dt_txt).ToString("dddd");
                    dateThreeTxt.Text = DateTime.Parse(allList[2].dt_txt).ToString("dd MMM");
                    iconThreeImg.Source = $"w{allList[2].weather[0].icon}";
                    tempThreeTxt.Text = allList[2].main.temp.ToString("0");

                    dayFourTxt.Text = DateTime.Parse(allList[3].dt_txt).ToString("dddd");
                    dateFourTxt.Text = DateTime.Parse(allList[3].dt_txt).ToString("dd MMM");
                    iconFourImg.Source = $"w{allList[3].weather[0].icon}";
                    tempFourTxt.Text = allList[3].main.temp.ToString("0");

                }
                catch (Exception ex)
                {
                    await DisplayAlert("Weather Info", ex.Message, "OK");
                }
            }
            else
            {
                await DisplayAlert("Weather Info", "No forecast information found", "OK");
            }
        }

        [Obsolete]
        private async void SearchWeather(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SearchCityPage());
        }
    }
}