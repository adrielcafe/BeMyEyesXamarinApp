using Plugin.Media;
using Xamarin.Forms;
using System.Threading.Tasks;
using Plugin.Media.Abstractions;
using System;

namespace BeMyEyesApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            TakePictureView.FadeTo(1, 1000);
        }

        private async void TakePictureView_Clicked(object sender, EventArgs e)
        {
            using (var imageFile = await TakePicture())
            {
                if (imageFile != null)
                {
                    await Navigation.PushModalAsync(new DetailPage(imageFile.Path));
                }
            }
        }

        private async Task<MediaFile> TakePicture()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("Oops!", "Nenhuma câmera encontrada", "Fechar");
                return null;
            }

            var imageFile = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Name = "image.jpg",
                PhotoSize = PhotoSize.Medium,
                CompressionQuality = 90,
                AllowCropping = false,
                SaveToAlbum = false
            });

            return imageFile;
        }
    }
}
