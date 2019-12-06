using Microsoft.Azure;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Queue;
using System;

namespace AzureQueue.Server
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("SERVER");
            Console.ForegroundColor = ConsoleColor.White;

            ConsumeMessage();
        }

        private static void ConsumeMessage()
        {
            // Retrieve storage account from connection string
            var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create the queue client
            var queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a queue
            var queue = queueClient.GetQueueReference("myqueue");

            CloudQueueMessage retrievedMessage = null;

            do
            {
                // Get the next message
                retrievedMessage = queue.GetMessage();

                if (retrievedMessage != null)
                {
                    Console.WriteLine($" [x] Retrieve {retrievedMessage.AsString} on {DateTime.Now}");

                    //Process the message in less than 30 seconds, and then delete the message
                    queue.DeleteMessage(retrievedMessage);
                }
            } while (true);
        }
    }
}