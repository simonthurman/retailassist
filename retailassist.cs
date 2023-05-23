using System;
using System.Threading.Tasks;
using Azure;
using Azure.AI.OpenAI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

public static class OpenAIChatFunction
{
    private static string endPoint = "https://openaisimont.openai.azure.com/";
    private static string key = "";
    private static string engine = "hackchat";
    private static OpenAIClient myAI = new OpenAIClient(new Uri(endPoint), new AzureKeyCredential(key));

    [FunctionName("OpenAIChatFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
    {
        string requesttext = req.Query["requesttext"];

        log.LogInformation($"Request body: {requesttext}");

        var chatCompletionsOptions = new ChatCompletionsOptions
        {
            Messages =
            {
                new ChatMessage(ChatRole.System, "You are a retail assistant, called Fred."),
                new ChatMessage(ChatRole.User, requesttext),
            }
        };

        var chatCompletionsResponse = await myAI.GetChatCompletionsAsync(
            engine,
            chatCompletionsOptions
        );

        var chatMessage = chatCompletionsResponse.Value.Choices[0].Message;

        return new OkObjectResult(chatMessage.Content);
    }
}

