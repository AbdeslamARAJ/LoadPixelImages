using MvvmHelpers;
using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net;
using Newtonsoft.Json;

namespace LoadPixelImages.ViewModel
{
    public class ImgesViewModel : BaseViewModel
    {
        private const string imageURI = "https://api.pexels.com/v1/search?query=nature";
        private const string apiKey = "563492ad6f91700001000001ee5795aaa78e4e75b35c79e481469109";
        private const int pages = 15;
        private int currentPage = 1;
        private ObservableRangeCollection<Photo> photosToDisplay;
        private Root root;

        public ObservableRangeCollection<Photo> PhotosToDisplay
        {
            get => photosToDisplay;
            set => SetProperty(ref photosToDisplay, value);
        }

        public Command LoadMorePhotosCommand { get; set; }

        public ImgesViewModel()
        {
            LoadMorePhotosCommand = new Command(LoadMoreAsync);
            Task.Run(async () =>
            {
                var photos = await LoadData();
                PhotosToDisplay = new ObservableRangeCollection<Photo>(photos);
            });
        }

        private async void LoadMoreAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            var photos = await LoadData();
            
            if (photos.Any())
                PhotosToDisplay.AddRange(photos);
            IsBusy = false;
        }

        private async Task<List<Photo>> LoadData()
        {
            var httpClient = new HttpClient();
            List<Photo> returnedList = new List<Photo>();
            httpClient.DefaultRequestHeaders.Add("Authorization", apiKey);
            var response = await httpClient.GetAsync(imageURI);
            if ((response.StatusCode == HttpStatusCode.OK) || (response.StatusCode == HttpStatusCode.Created))
            {
                root = JsonConvert.DeserializeObject<Root>(await response.Content.ReadAsStringAsync());
                if (root != null)
                    returnedList = root.photos;
            }
            return returnedList;
        }
    }

    public class Src
    {
        public string original { get; set; }
        public string large2x { get; set; }
        public string large { get; set; }
        public string medium { get; set; }
        public string small { get; set; }
        public string portrait { get; set; }
        public string landscape { get; set; }
        public string tiny { get; set; }
    }

    public class Photo
    {
        public int id { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string url { get; set; }
        public string photographer { get; set; }
        public string photographer_url { get; set; }
        public int photographer_id { get; set; }
        public string avg_color { get; set; }
        public Src src { get; set; }
        public bool liked { get; set; }
    }

    public class Root
    {
        public int page { get; set; }
        public int per_page { get; set; }
        public List<Photo> photos { get; set; }
        public int total_results { get; set; }
        public string next_page { get; set; }
    }


}

