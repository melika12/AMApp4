using AMApp4.Helper;
using AMApp4.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AMApp4.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchCityPage : ContentPage
    {
        [Obsolete]
        public SearchCityPage()
        {
            InitializeComponent();
        }

        internal List<string> cities;
        private CityInfo[] Cities { get; set; }

        [Obsolete]
        private async void SearchCity(object sender, EventArgs e)
        {
            var children = gridLayout.Children.ToList();
            if (children != null)
            {
                gridLayout.Children.Clear();
                gridLayout.RowDefinitions.Clear();
                
                await GetCities(city.Text);
            }
        }

        [Obsolete]
        private async Task GetCities(string city)
        {
            var url = $"https://wft-geo-db.p.rapidapi.com/v1/geo/cities?namePrefix={city}&limit=10&types=city";

            var result = await ApiCaller.Get(url, apikey: "0ee773c837mshf583cf8cf656b69p18a0f6jsn41cac5cc7e15", host: "wft-geo-db.p.rapidapi.com");

            if (result.Successful)
            {
                try
                {
                    var jsonCities = JsonConvert.DeserializeObject<Data>(result.Response);
                    Cities = jsonCities.data;
                    cities = new List<string>();
                    var rowIndex = 0;
                    foreach (var item in Cities)
                    {
                        cities.Add(item.city);
                        var output = string.Format("{0}, {1}", item.city, item.country);
                        gridLayout.RowDefinitions.Add(new RowDefinition());
                        var label = new Label
                        {
                            Text = output,
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            TextColor = Color.White,
                            FontSize = 20
                        };
                        var click = new TapGestureRecognizer();
                        click.Tapped += async (sender, eventArgs) =>
                        {
                            var clickedLabel = (Label)sender;
                            var text = clickedLabel.Text;
                            await Navigation.PushAsync(new CurrentAMPage(text));
                        };
                        label.GestureRecognizers.Add(click);
                        gridLayout.Children.Add(label, 0, rowIndex);
                        rowIndex++;
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("City Info", ex.Message, "OK");
                }
            }
            else
            {
                await DisplayAlert("City Info", "No cities found", "OK");
            }
        }
    }
}