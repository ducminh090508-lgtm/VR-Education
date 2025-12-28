using OpenAI;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Samples.Whisper
{
    public class Whisper : MonoBehaviour
    {
        [SerializeField] private Button recordButton;
        [SerializeField] private Image progressBar;
        [SerializeField] private Text message;
        [SerializeField] private Dropdown dropdown;
        
        private readonly string fileName = "output.wav";
        private readonly int duration = 5;

        private AudioClip clip;
        private bool isRecording;
        private float time;
        private OpenAIApi openai = new OpenAIApi();
        private List<ChatMessage> output = new List<ChatMessage>();


        //chatgpt part
        private string instruction = "Act as a vietnamese historian and reply to the questions. Don't break character. Say only 65 words maxed. Don't ever mention that you are an AI model.\nQ: ";
        private string soundInput; 


        private void Start()
        {
            #if UNITY_WEBGL && !UNITY_EDITOR
            dropdown.options.Add(new Dropdown.OptionData("Microphone not supported on WebGL"));
            #else
            foreach (var device in Microphone.devices) //gets the microphone + device name 
            {
                dropdown.options.Add(new Dropdown.OptionData(device));
                Debug.Log("Name: " + device);
            }
            recordButton.onClick.AddListener(StartRecording);
            dropdown.onValueChanged.AddListener(ChangeMicrophone);
            
            var index = PlayerPrefs.GetInt("user-mic-device-index");
            dropdown.SetValueWithoutNotify(index);
            #endif
        }

        private void ChangeMicrophone(int index)
        {
            PlayerPrefs.SetInt("user-mic-device-index", index);
        }
        
        private void StartRecording()
        {
            isRecording = true;
            recordButton.enabled = false;

            var index = PlayerPrefs.GetInt("user-mic-device-index");
            
            #if !UNITY_WEBGL
            clip = Microphone.Start(dropdown.options[index].text, false, duration, 44100);
            #endif
        }
        
        private async void EndRecording()
        {
            message.text = "Transcripting...";
            
            #if !UNITY_WEBGL
            Microphone.End(null);
            #endif
            
            byte[] data = SaveWav.Save(fileName, clip);  
            
            var req = new CreateAudioTranscriptionsRequest
            {
                FileData = new FileData() {Data = data, Name = "audio.wav"},
                // File = Application.persistentDataPath + "/" + fileName,
                Model = "whisper-1",
                Language = "en"
            };
            var res = await openai.CreateAudioTranscription(req);

            progressBar.fillAmount = 0;
            soundInput = res.Text;
            Debug.Log(soundInput);

            //store the test message into a string --> now shove string into chatgpt api :))
 
            recordButton.enabled = true;
            
            ChatMessage newMessage = new ChatMessage(); 
            newMessage.Content = instruction + soundInput; 
            newMessage.Role = "user"; 

            output.Add(newMessage); 
            

            CreateChatCompletionRequest request = new CreateChatCompletionRequest();
            request.Messages = output; 
            request.Model = "gpt-4o-mini";

            var response = await openai.CreateChatCompletion(request);

            if(response.Choices != null && response.Choices.Count > 0){
                var chatResponse = response.Choices[0].Message;
                output.Add(chatResponse); 

                Debug.Log(chatResponse.Content);
                message.text = chatResponse.Content; 
            }
            else
            {
                Debug.LogWarning("No text was generated from this prompt.");
            }
        }

        private void Update()
        {
            if (isRecording)
            {
                time += Time.deltaTime;
                progressBar.fillAmount = time / duration;
                
                if (time >= duration)
                {
                    time = 0;
                    isRecording = false;
                    EndRecording();
                }
            }
        }
    }
}

