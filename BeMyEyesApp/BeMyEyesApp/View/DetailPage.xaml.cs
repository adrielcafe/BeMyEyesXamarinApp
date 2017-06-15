using BeMyEyesApp.Service;
using Plugin.Media.Abstractions;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BeMyEyesApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailPage : ContentPage
    {
        private string TranslatedDescription { get; set; }

        public DetailPage(string imagePath)
        {
            InitializeComponent();
            if (!String.IsNullOrEmpty(imagePath))
            {
                ImageView.Source = imagePath;
                AnalyzeImageAsync(imagePath);
            } else
            {
                Navigation.PopToRootAsync();
            }
        }

        private void SpeakAgainView_Clicked(object sender, EventArgs e)
        {
            SpeakDescriptionAsync();
        }

        private async void AnalyzeImageAsync(string imagePath)
        {
            CognitiveService.Instance.PlayAudio("Analizando foto");

            var imageDescription = await CognitiveService.Instance.AnalyzeImageAsync(imagePath);
            TranslatedDescription = await CognitiveService.Instance.TranslateTextAsync(imageDescription);

            FixTranslation();
            DescriptionView.Text = TranslatedDescription;

            await LoadingView.FadeTo(0, 500);
            await DescriptionView.FadeTo(1, 1000);
            await SpeakAgainView.FadeTo(1, 1000);

            SpeakDescriptionAsync();
        }

        private void FixTranslation()
        {
            TranslatedDescription = TranslatedDescription.Replace("tabela", "mesa");
        }

        private void SpeakDescriptionAsync()
        {
            if (!String.IsNullOrEmpty(TranslatedDescription))
            {
                CognitiveService.Instance.PlayAudio(TranslatedDescription);
            }
        }
    }
}