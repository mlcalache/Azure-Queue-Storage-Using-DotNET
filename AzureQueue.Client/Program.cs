using Microsoft.Azure;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Queue;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AzureQueue.Client
{
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public static List<Person> CreatePeople(int quantity)
        {
            var people = new List<Person>();

            for (int i = 0; i < quantity; i++)
            {
                people.Add(new Person { Name = $"n{i}", Age = i });
            }

            return people;
        }

        public static Person CreatePerson(string name, int age)
        {
            return new Person { Name = name, Age = age };
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            //var quantity = 1;

            var quantity = Convert.ToInt32(args[0]);

            AddMessage(quantity);
        }

        private static void AddMessage(int quantity)
        {
            // Retrieve storage account from connection string.
            var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create the queue client.
            var queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a container.
            var queue = queueClient.GetQueueReference("myqueue");

            // Create the queue if it doesn't already exist
            queue.CreateIfNotExists();

            for (int i = 1; i <= quantity; i++)
            {
                var person = Person.CreatePerson($"name{i}", i);

                var message = JsonConvert.SerializeObject(person);

                //var message = $"message {i}...";

                // Create a message and add it to the queue.
                var queueMessage = new CloudQueueMessage(message);
                queue.AddMessage(queueMessage);

                Console.WriteLine($" [x] Sent {message}");
            }

            //// Peek at the next message
            //var peekedMessage = queue.PeekMessage();

            //// Display message.
            //Console.WriteLine(peekedMessage.AsString);

            Console.ReadKey();
        }
    }
}