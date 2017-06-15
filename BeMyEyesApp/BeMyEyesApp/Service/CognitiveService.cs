using System.Threading.Tasks;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using System.IO;
using PCLStorage;
using TranslatorService;
using Plugin.SimpleAudioPlayer.Abstractions;
using Plugin.SimpleAudioPlayer;
using Plugin.TextToSpeech;

namespace BeMyEyesApp.Service
{
    public sealed class CognitiveService
    {
        private static string VISION_API_URL = "https://westcentralus.api.cognitive.microsoft.com/vision/v1.0";
        
        private static readonly CognitiveService instance = new CognitiveService();
        public static CognitiveService Instance
        {
            get
            {
                return instance;
            }
        }

        private VisionServiceClient VisionService { get; set; }
        private TranslatorServiceClient TranslatorService { get; set; }
        private ISimpleAudioPlayer AudioPlayerService { get; set; }

        private CognitiveService() { }

        public async Task<string> AnalyzeImageAsync(string imageFilePath)
        {
            if (VisionService == null)
            {
                VisionService = new VisionServiceClient(Keys.VISION_API_KEY, VISION_API_URL);
            }
            
            IFile imageFile = await FileSystem.Current.GetFileFromPathAsync(imageFilePath);
            using (Stream imageFileStream = await imageFile.OpenAsync(FileAccess.Read))
            {
                AnalysisResult result = await VisionService.DescribeAsync(imageFileStream, 1);
                return result.Description.Captions[0].Text;
            }
        }

        public async Task<string> TranslateTextAsync(string text)
        {
            if (TranslatorService == null)
            {
                TranslatorService = new TranslatorServiceClient(Keys.TRANSLATOR_API_KEY);
            }

            return await TranslatorService.TranslateAsync(text, "pt");
        }

        public void PlayAudio(string text)
        {
            if (AudioPlayerService == null)
            {
                AudioPlayerService = CrossSimpleAudioPlayer.Current;
            }

            CrossTextToSpeech.Current.Speak(text);
        }
    }
}