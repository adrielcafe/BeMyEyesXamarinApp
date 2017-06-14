using Plugin.Media;
using System.Diagnostics;
using Xamarin.Forms;
using System;
using System.Threading.Tasks;
using Plugin.Media.Abstractions;
using BeMyEyesApp.Service;
using BeMyEyesApp.View;

namespace BeMyEyesApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            OpenCameraView.FadeTo(0, 0);
            OpenCameraView.FadeTo(1, 2000);
            Navigation.PushModalAsync(new DetailPage());
        }

        private async void OpenCameraView_Clicked(object sender, System.EventArgs e)
        {
            using (var imageFile = await TakePicture())
            {
                if (imageFile != null)
                {
                    CognitiveService.Instance.PlayAudio("Analizando Imagem");

                    var imageDescription = await CognitiveService.Instance.AnalyzeImageAsync(imageFile.Path);
                    var translatedDescription = await CognitiveService.Instance.TranslateTextAsync(imageDescription);

                    CognitiveService.Instance.PlayAudio(translatedDescription);
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
