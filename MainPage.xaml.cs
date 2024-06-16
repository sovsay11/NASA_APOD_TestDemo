using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;

namespace NASA_APOD_TestDemo
{
    public partial class MainPage : ContentPage
    {
        ApodData? apodData = null;

        public MainPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Obsolete method of sending web client requests
        /// </summary>
        public void SendWebClientRequest()
        {
            // Create a new web client
            using (WebClient wc = new WebClient())
            {
                try
                {
                    // For security certification
                    ////ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(delegate { return true; });

                    // Format date from date picker
                    string date = DatePickerLocalDate.Date.ToString("yyyy-MM-dd");

                    // Make call to API using the provided date
                    string response = wc.DownloadString($"https://api.nasa.gov/planetary/apod?date={date}&api_key=MvYzAHoYT4zKCkEoe3h514K3V49L7oauuvvV4ELS");

                    // Conver the JSON data into a class object
                    if (response != null)
                    {
                        apodData = JsonConvert.DeserializeObject<ApodData>(response);
                    }

                    // Display the image or the video depending on the media type of the JSON object
                    if (apodData != null)
                    {
                        if (apodData.Media_type == "image")
                        {
                            ImageNASAPicture.IsVisible = true;
                            MediaNASAVideo.IsVisible = false;
                            ImageNASAPicture.Source = apodData.Url;
                        }
                        else if (apodData.Media_type == "video")
                        {
                            MediaNASAVideo.IsVisible = true;
                            ImageNASAPicture.IsVisible = false;
                            MediaNASAVideo.Source = apodData.Url;
                        }
                    }
                }
                catch (Exception ex)
                {
                    DisplayAlert("Oh no!", ex.Message, "Close");
                }
            }
        }

        /// <summary>
        /// Send an HTTP client request asynchronously
        /// </summary>
        public async void SendHttpClientRequest()
        {
            // Create a new HTTP client object
            HttpClient httpClient = new HttpClient();

            // Format datepicker time and send out a GET request to the provided URI
            string date = DatePickerLocalDate.Date.ToString("yyyy-MM-dd");
            using HttpResponseMessage response = await httpClient.GetAsync($"https://api.nasa.gov/planetary/apod?date={date}&api_key=MvYzAHoYT4zKCkEoe3h514K3V49L7oauuvvV4ELS");

            // Ensure a response was received
            response.EnsureSuccessStatusCode();

            // Read and response and return it as a string
            string jsonResponse = await response.Content.ReadAsStringAsync();

            // Convert the JSON string into the APODData C# class
            apodData = JsonConvert.DeserializeObject<ApodData>(jsonResponse);

            // Display the image or the video depending on the media type of the JSON object
            if (apodData != null)
            {
                if (apodData.Media_type == "image")
                {
                    ImageNASAPicture.IsVisible = true;
                    MediaNASAVideo.IsVisible = false;
                    ImageNASAPicture.Source = apodData.Url;
                }
                else if (apodData.Media_type == "video")
                {
                    ImageNASAPicture.IsVisible = false;
                    MediaNASAVideo.IsVisible = true;
                    MediaNASAVideo.Source = apodData.Url;
                }
            }
        }

        /// <summary>
        /// Button event for search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonSearch_Clicked(object sender, EventArgs e)
        {
            // obsolete
            ////SendWebClientRequest();
            SendHttpClientRequest();
        }
    }

}
