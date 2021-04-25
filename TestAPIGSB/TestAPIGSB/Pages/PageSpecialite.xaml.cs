using Android.Widget;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using TestAPIGSB.ClassesMetier;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static System.Net.Mime.MediaTypeNames;
using Specialite = TestAPIGSB.ClassesMetier.Specialite;

namespace TestAPIGSB.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageSpecialite : ContentPage
    {
        public PageSpecialite()
        {
            InitializeComponent();
        }
        HttpClient ws;
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            List<Specialite> lesSpecialites = new List<Specialite>();

            ws = new HttpClient();
            var reponse = await ws.GetAsync("http://10.0.2.2/SIO2ALT/APIGSB/specialites/");
            var content = await reponse.Content.ReadAsStringAsync();
            lesSpecialites = JsonConvert.DeserializeObject<List<Specialite>>(content);
            lvSpecialites.ItemsSource = lesSpecialites;
        }

        private void lvSpecialites_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (lvSpecialites.SelectedItem != null)
            {
                txtNomSpecialite.Text = (lvSpecialites.SelectedItem as Specialite).Nom;
                
            }
        }

        private async void btnModifier_Clicked(object sender, EventArgs e)
        {
            if (txtNomSpecialite.Text == null)
            {
                Toast.MakeText(Android.App.Application.Context, "Sélectionner une spécialité", ToastLength.Short).Show();
            }
            else
            {
                ws = new HttpClient();
                Specialite spe = (lvSpecialites.SelectedItem as Specialite);
                spe.Nom = txtNomSpecialite.Text;
               
                JObject jspe = new JObject
                {
                    {"Id",spe.Id},
                    {"Nom",spe.Nom},
                
                };
                string json = JsonConvert.SerializeObject(jspe);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                var reponse = await ws.PutAsync("http://10.0.2.2/SIO2ALT/APIGSB/specialites/", content);
                List<Specialite> lesSpecialites = new List<Specialite>();

                ws = new HttpClient();
                reponse = await ws.GetAsync("http://10.0.2.2/SIO2ALT/APIGSB/specialites/");
                var flux = await reponse.Content.ReadAsStringAsync();
                lesSpecialites = JsonConvert.DeserializeObject<List<Specialite>>(flux);
                lvSpecialites.ItemsSource = lesSpecialites;
            }
        }

        private async void btnAjouter_Clicked(object sender, EventArgs e)
        {
            if (txtNomSpecialite.Text == null)
            {
                Toast.MakeText(Android.App.Application.Context, "Saisir un nom de specialite", ToastLength.Short).Show();
            }
            else
            {
                ws = new HttpClient();
                //Region newRegion = new Region();
                //newRegion.Nom = txtNomRegion.Text;
                JObject spe = new JObject
                {
                    { "Reg" , txtNomSpecialite.Text},
                   
                };
                string json = JsonConvert.SerializeObject(spe);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                var reponse = await ws.PostAsync("http://10.0.2.2/SIO2ALT/APIGSB/specialites/", content);

                List<Specialite> lesSpecialites = new List<Specialite>();

                ws = new HttpClient();
                reponse = await ws.GetAsync("http://10.0.2.2/SIO2ALT/APIGSB/specialites/");
                var flux = await reponse.Content.ReadAsStringAsync();
                lesSpecialites = JsonConvert.DeserializeObject<List<Specialite>>(flux);
                lvSpecialites.ItemsSource = lesSpecialites;
            }
        }
    }
}