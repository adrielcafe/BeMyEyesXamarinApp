using Plugin.Media;
using System.Diagnostics;
using Xamarin.Forms;
using System;
using Plugin.Media.Abstractions;
using BeMyEyesApp.Service;

namespace BeMyEyesApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            OpenCameraView.FadeTo(0, 0);
            OpenCameraView.FadeTo(1, 2000);
        }

        private void OpenCameraView_Clicked(object sender, System.EventArgs e)
        {
            TakePicture();
        }

        private async void TakePicture()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("Oops!", "Nenhuma câmera encontrada", "Fechar");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Name = "image.jpg",
                PhotoSize = PhotoSize.Medium,
                CompressionQuality = 90,
                AllowCropping = false,
                SaveToAlbum = false
            });

            if (file == null)
                return;

            CognitiveService.Instance.PlayAudioAsync("Analizando Imagem");

            var imageDescription = await CognitiveService.Instance.AnalyzeImageAsync(file.Path);

            file.Dispose();

            var translatedDescription = await CognitiveService.Instance.TranslateTextAsync(imageDescription);

            await CognitiveService.Instance.PlayAudioAsync(translatedDescription);
        }
    }
}
