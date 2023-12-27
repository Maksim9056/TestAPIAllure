using NUnit.Framework;

namespace TestAPIAllure
{
    using Allure.Commons;
    using Microsoft.VisualBasic;
    using NUnit.Allure.Attributes;
    using NUnit.Allure.Core;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using TechTalk.SpecFlow;

    //using ;
    //using Bogus.DataSets;
    using Newtonsoft.Json;
    using System.IO;
    using System.Text;
    using System.Text.Json.Serialization;
    using static TestAPIAllure.ProductsApiTests;
    using System.Security.Cryptography;

    [TestFixture]
    [AllureNUnit]
    public class AddressControllerTests
    {
        private HttpClient httpClient;
        private Bogus.Faker faker;
        private string logFilePath;

        [OneTimeSetUp]
        public void Setup()
        {
            httpClient = new HttpClient();
            faker = new Bogus.Faker();
            logFilePath = "logfile.txt"; // Replace this with your desired log file path
        }

        [Test]
        [AllureTag("API")]
        public void TestGetAllAddresses()
        {
            string url = "https://localhost:7097/praticka/GetAllAddresses";

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                using (HttpResponseMessage response = httpClient.SendAsync(request).Result)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    LogTestResult("TestGetAllAddresses", HttpMethod.Get, url, (int)response.StatusCode);
                    Console.WriteLine("GET All Addresses Response content: " + content);
                    Console.WriteLine("GET All Addresses Status code: " + (int)response.StatusCode);
                    Assert.AreEqual(200, (int)response.StatusCode);
                }
            }
        }
        private void LogTestResult(string methodName, HttpMethod method, string url, int statusCode)
        {
            string logMessage = $"{DateTime.Now} - Method: {method}, URL: {url}, Status Code: {statusCode}";
            File.AppendAllText(logFilePath, logMessage + Environment.NewLine);
        }

        [Test]
        [AllureTag("API")]
        public void TestCreateAddress()
        {
            string url = "https://localhost:7097/praticka/CreateAddress";

            var address = new Address
            {
                Id = faker.Random.Number(),
                Street = faker.Address.StreetAddress(),
                City = faker.Address.City(),
                ZipCode = faker.Address.ZipCode()
            };

        }

        [Test]
        [AllureTag("API")]
        public void TestUpdateAddress()
        {
            int id = faker.Random.Number();
            string url = $"https://localhost:7097/praticka/UpdateProduct/{id}";

            var updatedAddress = new Address
            {
                Id = id,
                Street = faker.Address.StreetAddress(),
                City = faker.Address.City(),
                ZipCode = faker.Address.ZipCode()
            };

            // Implement the code to send PUT request with updatedAddress payload

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, url))
            {
                // Add necessary request headers or content (e.g., request.Content = ...)

                using (HttpResponseMessage response = httpClient.SendAsync(request).Result)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    LogTestResult("TestUpdateAddress", HttpMethod.Put, url, (int)response.StatusCode);
                    Console.WriteLine("PUT Update Address Response content: " + content);
                    Console.WriteLine("PUT Update Address Status code: " + (int)response.StatusCode);
                    Assert.AreEqual(200, (int)response.StatusCode);
                }
            }
        }

        [Test]
        [AllureTag("API")]
        public void TestDeleteAddress()
        {
            int id = faker.Random.Number();
            string url = $"https://localhost:7097/praticka/DeleteAddress/{id}";

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, url))
            {
                using (HttpResponseMessage response = httpClient.SendAsync(request).Result)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    LogTestResult("TestDeleteAddress", HttpMethod.Delete, url, (int)response.StatusCode);
                    Console.WriteLine($"DELETE Address for ID {id} Response content: " + content);
                    Console.WriteLine($"DELETE Address for ID {id} Status code: " + (int)response.StatusCode);
                    Assert.AreEqual(200, (int)response.StatusCode);
                }
            }
        }
        [Test]
        [AllureTag("API")]
        public void TestGetAddressById()
        {
            int id = faker.Random.Number();
            string url = $"https://localhost:7097/praticka/GetAddressById/{id}";

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                using (HttpResponseMessage response = httpClient.SendAsync(request).Result)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    LogTestResult("TestGetAddressById", HttpMethod.Get, url, (int)response.StatusCode);
                    Console.WriteLine($"GET Address by ID Response for ID {id}: " + content);
                    Console.WriteLine($"GET Address by ID Status code for ID {id}: " + (int)response.StatusCode);
                    Assert.AreEqual(200, (int)response.StatusCode);
                }
            }
        }
        public class Address
        {
            public int Id { get; set; }
            public string Street { get; set; }
            public string City { get; set; }
            public string ZipCode { get; set; }
        }
    }
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public List<Product> Products { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    [TestFixture]
    [AllureNUnit]
    public class OrderApiTests
    {
        private HttpClient httpClient;
        private Bogus. Faker faker;

        [OneTimeSetUp]
        public void Setup()
        {
            httpClient = new HttpClient();
            faker = new Bogus.Faker();
        }

        [Test]
        [AllureTag("API")]
        public void TestGetAllOrders()
        {
            string url = "https://localhost:7097/praticka/GetAllOrders";

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                using (HttpResponseMessage response = httpClient.SendAsync(request).Result)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    LogTestResult("TestGetAllOrders", HttpMethod.Get, url, (int)response.StatusCode);
                    Console.WriteLine("GET All Orders Response content: " + content);
                    Console.WriteLine("GET All Orders Status code: " + (int)response.StatusCode);
                    Assert.AreEqual(200, (int)response.StatusCode);
                }
            }
        }

        [Test]
        [AllureTag("API")]
        public void TestCreateOrder()
        {
            string url = "https://localhost:7097/CreateOrder"; // Проверьте, что URL соответствует вашему API

            var newOrder = new Order
            {
                Id = faker.Random.Number(),
                UserId = faker.Random.Number(),
                OrderDate = DateTime.Now,
                Products = new List<Product>
        {
            new Product { Id = faker.Random.Number(), Name = faker.Commerce.ProductName(), Price = faker.Random.Decimal() },
            new Product { Id = faker.Random.Number(), Name = faker.Commerce.ProductName(), Price = faker.Random.Decimal() }
        }
            };

            string jsonData = JsonConvert.SerializeObject(newOrder);
            HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url))
            {
                request.Content = content;

                using (HttpResponseMessage response = httpClient.SendAsync(request).Result)
                {
                    var responseContent = response.Content.ReadAsStringAsync().Result;
                    LogTestResult("TestCreateOrder", HttpMethod.Post, url, (int)response.StatusCode);
                    Console.WriteLine("POST Create Order Response content: " + responseContent);
                    Console.WriteLine("POST Create Order Status code: " + (int)response.StatusCode);
                    Assert.AreEqual(200, (int)response.StatusCode); // Проверка успешного создания заказа
                }
            }
        }


        private void LogTestResult(string methodName, HttpMethod method, string url, int statusCode)
        {
            Console.WriteLine($"Method: {method}, URL: {url}, Status Code: {statusCode}");
            // Implement code to log test results to a file or a logger
        }
        [Test]
        [AllureTag("API")]
        public void TestGetOrderById()
        {
            int orderId = 1; // Replace with the actual order ID you want to retrieve
            string url = $"https://localhost:7097/praticka/GetOrderById/{orderId}";

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                using (HttpResponseMessage response = httpClient.SendAsync(request).Result)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    LogTestResult("TestGetOrderById", HttpMethod.Get, url, (int)response.StatusCode);
                    Console.WriteLine($"GET Order by ID Response content for ID {orderId}: " + content);
                    Console.WriteLine($"GET Order by ID Status code for ID {orderId}: " + (int)response.StatusCode);
                    Assert.AreEqual(200, (int)response.StatusCode);
                }
            }
        }

        [Test]
        [AllureTag("API")]
        public void TestUpdateOrder()
        {
            int orderId = 1; // Replace with the actual order ID you want to update
            string url = $"https://localhost:7097/praticka/UpdateOrder/{orderId}";

            var updatedOrder = new Order
            {
                // Provide updated data for the order
                Id = orderId,
                UserId = faker.Random.Number(),
                OrderDate = DateTime.Now,
                Products = new List<Product>
        {
            new Product { Id = faker.Random.Number(), Name = faker.Commerce.ProductName(), Price = faker.Random.Decimal() },
            new Product { Id = faker.Random.Number(), Name = faker.Commerce.ProductName(), Price = faker.Random.Decimal() }
        }
            };

            // Implement the code to send PUT request with updatedOrder payload to the specified URL

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, url))
            {
                // Add necessary request headers or content (e.g., request.Content = ...)

                using (HttpResponseMessage response = httpClient.SendAsync(request).Result)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    LogTestResult("TestUpdateOrder", HttpMethod.Put, url, (int)response.StatusCode);
                    Console.WriteLine($"PUT Update Order Response content for ID {orderId}: " + content);
                    Console.WriteLine($"PUT Update Order Status code for ID {orderId}: " + (int)response.StatusCode);
                    Assert.AreEqual(200, (int)response.StatusCode);
                }
            }
        }

        [Test]
        [AllureTag("API")]
        public void TestDeleteOrder()
        {
            int orderId = 1; // Replace with the actual order ID you want to delete
            string url = $"https://localhost:7097/praticka/DeleteOrder/{orderId}";

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, url))
            {
                using (HttpResponseMessage response = httpClient.SendAsync(request).Result)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    LogTestResult("TestDeleteOrder", HttpMethod.Delete, url, (int)response.StatusCode);
                    Console.WriteLine($"DELETE Order Response content for ID {orderId}: " + content);
                    Console.WriteLine($"DELETE Order Status code for ID {orderId}: " + (int)response.StatusCode);

                    // Expect either 200 (if successful delete) or 404 (if the order was not found)
                    Assert.IsTrue((int)response.StatusCode == 200 || (int)response.StatusCode == 404);
                }
            }
        }


    }
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("Products API Tests")]
    [AllureDisplayIgnored]
    public class ProductsApiTests
    {
        private HttpClient httpClient;
        Bogus. Faker Faker = new Bogus.Faker();
        [OneTimeSetUp]
        public void Setup()
        {
            httpClient = new HttpClient();
            // Setup any necessary configurations or authentication headers for your API requests
        }

        [Test]
        [AllureTag("API")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureIssue("YourIssueTracker#1234")]
        [AllureOwner("YourName")]
        [AllureFeature("Products")]
        [AllureSubSuite("GetAllProducts")]
        public void TestGetAllProducts()
        {
            string url = "https://localhost:7097/praticka/GetAllProducts";

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                using (HttpResponseMessage response = httpClient.SendAsync(request).Result)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    LogTestResult("TestGetAllProducts", HttpMethod.Get, url, (int)response.StatusCode);
                    Console.WriteLine($"GET All Products Response content: {content}");
                    Console.WriteLine($"GET All Products Status code: {(int)response.StatusCode}");
                    Assert.AreEqual(200, (int)response.StatusCode);
                }
            }
        }
        [Test]
        [AllureTag("API")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureIssue("YourIssueTracker#1234")]
        [AllureOwner("YourName")]
        [AllureFeature("Products")]
        [AllureSubSuite("CreateProduct")]
        public void TestCreateProduct()
        {
            string url = "https://localhost:7097/praticka/CreateProduct";

            var newProduct = new Product
            {
                // Define the properties for the new product
                Id = Faker.Random.Number(),
                Name = Faker.Commerce.ProductName(),
                Price = Faker.Random.Decimal(),
                Category = Faker.Commerce.Categories(1)[0]
            };

            // Implement the code to send a POST request with newProduct payload to the specified URL
            // Validate the response and log the test result
        }
        private void LogTestResult(string methodName, HttpMethod method, string url, int statusCode)
        {
            string safeFileName = GenerateSafeFileName(methodName, method, url);

            AllureLifecycle.Instance.WrapInStep(() =>
            {
                AllureLifecycle.Instance.AddAttachment($"{methodName}_Request", "application/json", $"{method} {url}");
                AllureLifecycle.Instance.AddAttachment($"{methodName}_Response", "application/json", $"Status code: {statusCode}");
            }, safeFileName + " Details");
        }


     private string GenerateSafeFileName(string methodName, HttpMethod method, string url)
    {
        string combinedString = $"{methodName}_{method}_{url}";
        byte[] bytes = Encoding.UTF8.GetBytes(combinedString);

        using (var algorithm = SHA1.Create())
        {
            byte[] hashBytes = algorithm.ComputeHash(bytes);
            return BitConverter.ToString(hashBytes).Replace("-", "") + ".json";
        }
    }

    private void AddAttachment(string attachmentName, string contentType, string content)
        {
            byte[] contentBytes = System.Text.Encoding.UTF8.GetBytes(content);

            using (MemoryStream stream = new MemoryStream(contentBytes))
            using (StreamReader reader = new StreamReader(stream))
            {
                string contentString = reader.ReadToEnd();
                attachmentName =SanitizeFileName(attachmentName);
                AllureLifecycle.Instance.AddAttachment(attachmentName, contentType, contentString);
            }
        }
        private string SanitizeFileName(string fileName)
        {
            // Заменить запрещенные символы на подчеркивание
            string invalidChars = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            foreach (char c in invalidChars)
            {
                fileName = fileName.Replace(c, '_');
            }
            return fileName;
        }

        public class Product
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public string Category { get; set; }

        }

        [Test]
        [AllureTag("API")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureIssue("YourIssueTracker#1234")]
        [AllureOwner("YourName")]
        [AllureFeature("Products")]
        [AllureSubSuite("GetProductById")]
        public void TestGetProductById()
        {
            int productId = Faker.Random.Number(1, 100); // Replace with a valid product ID

            string url = $"https://localhost:7097/praticka/GetProductById/{productId}";

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                using (HttpResponseMessage response = httpClient.SendAsync(request).Result)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    LogTestResult("TestGetProductById", HttpMethod.Get, url, (int)response.StatusCode);
                    Console.WriteLine($"GET Product by ID Response content: {content}");
                    Console.WriteLine($"GET Product by ID Status code: {(int)response.StatusCode}");
                    Assert.AreEqual(200, (int)response.StatusCode);
                }
            }
        }


        [Test]
        [AllureTag("API")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureIssue("YourIssueTracker#1234")]
        [AllureOwner("YourName")]
        [AllureFeature("Products")]
        [AllureSubSuite("UpdateProduct")]
        public void TestUpdateProduct()
        {
            int productId = Faker.Random.Number(1, 100); // Replace with a valid product ID

            string url = $"https://localhost:7097/praticka//UpdateProduct/{productId}";

            var updatedProduct = new Product
            {
                // Define the updated properties for the product with ID 'productId'
                Name = Faker.Commerce.ProductName(),
                Price = Faker.Random.Decimal(),
                Category = Faker.Commerce.Categories(1)[0]
            };

            using (var request = new HttpRequestMessage(HttpMethod.Put, url))
            {
                string json = JsonConvert.SerializeObject(updatedProduct);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                using (HttpResponseMessage response = httpClient.SendAsync(request).Result)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    LogTestResult("TestUpdateProduct", HttpMethod.Put, url, (int)response.StatusCode);
                    Console.WriteLine($"PUT Product Response content: {content}");
                    Console.WriteLine($"PUT Product Status code: {(int)response.StatusCode}");
                    Assert.AreEqual(200, (int)response.StatusCode);
                }
            }
        }

        [Test]
        [AllureTag("API")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureIssue("YourIssueTracker#1234")]
        [AllureOwner("YourName")]
        [AllureFeature("Products")]
        [AllureSubSuite("DeleteProduct")]
        public void TestDeleteProduct()
        {
            int productId = Faker.Random.Number(1, 100); // Replace with a valid product ID

            string url = $"https://localhost:7097/praticka/DeleteProduct/{productId}";

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, url))
            {
                using (HttpResponseMessage response = httpClient.SendAsync(request).Result)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    LogTestResult("TestDeleteProduct", HttpMethod.Delete, url, (int)response.StatusCode);
                    Console.WriteLine($"DELETE Product Response content: {content}");
                    Console.WriteLine($"DELETE Product Status code: {(int)response.StatusCode}");
                    Assert.AreEqual(200, (int)response.StatusCode);
                }
            }



        }
    }
}

