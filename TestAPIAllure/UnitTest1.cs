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

            logFilePath = "test_log.txt"; // Replace this with your desired log file path
        }

        [Test]
        [AllureTag("API")]
        public void TestGetAllAddresses()
        {
            string url = "https://localhost:7097/GetAllAddresses";

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
            string url = "https://localhost:7097/CreateAddress";

            var address = new Address
            {
                Id = faker.Random.Number(),
                Street = faker.Address.StreetAddress(),
                City = faker.Address.City(),
                ZipCode = faker.Address.ZipCode()
            };

            string jsonData = JsonConvert.SerializeObject(address);

            HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url))
            {
                request.Content = content;

                using (HttpResponseMessage response = httpClient.SendAsync(request).Result)
                {
                    var responseContent = response.Content.ReadAsStringAsync().Result;
                    LogTestResult("TestCreateAddress", HttpMethod.Post, url, (int)response.StatusCode);
                    Console.WriteLine("POST Create Address Response content: " + responseContent);
                    Console.WriteLine("POST Create Address Status code: " + (int)response.StatusCode);
                    Assert.AreEqual(200, (int)response.StatusCode); // �������� ��������� �������� ������
                }
            }
        }


        private List<int> GetAvailableAddressIdsFromAPI()
        {
            List<int> availableIds = new List<int>();

            string url = "https://localhost:7097/GetAllAddresses";

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                using (HttpResponseMessage response = httpClient.SendAsync(request).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = response.Content.ReadAsStringAsync().Result;

                        // �������������� JSON � ���������� ��������� ID �������
                        var addresses = JsonConvert.DeserializeObject<List<Address>>(content);
                        availableIds = addresses.Select(a => a.Id).ToList();
                    }
                    else
                    {
                        // ��������� ������, ����� ������ ���������� ��������
                    }
                }
            }

            return availableIds;
        }


        [Test]
        [AllureTag("API")]
        public void TestUpdateAddress()
        {
            // ��������� ������ ��������� ID �������
            List<int> availableAddressIds = GetAvailableAddressIdsFromAPI();

            // ��������, ���� �� ��������� ������ ��� ����������
            if (!availableAddressIds.Any())
            {
                Assert.Fail("No available addresses to update.");
            }

            // ����� ������� ���������� ID ��� ����������
            int idToUpdate = availableAddressIds.First();
            string url = $"https://localhost:7097/UpdateAddress/{idToUpdate}";

            var updatedAddress = new Address
            {
                Id = idToUpdate,
                Street = faker.Address.StreetAddress(),
                City = faker.Address.City(),
                ZipCode = faker.Address.ZipCode()
            };

            // �������������� ������� updatedAddress � JSON � ��� ���������� � ������
            string jsonData = JsonConvert.SerializeObject(updatedAddress);
            HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // �������� PUT-������� � ������������ ������� ������
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, url))
            {
                request.Content = content; // ���������� ���� �������

                using (HttpResponseMessage response = httpClient.SendAsync(request).Result)
                {
                    var responseContent = response.Content.ReadAsStringAsync().Result;
                    LogTestResult("TestUpdateAddress", HttpMethod.Put, url, (int)response.StatusCode);
                    Console.WriteLine("PUT Update Address Response content: " + responseContent);
                    Console.WriteLine("PUT Update Address Status code: " + (int)response.StatusCode);
                    Assert.AreEqual(200, (int)response.StatusCode);
                }
            }
        }


        [Test]
        [AllureTag("API")]
        public void TestDeleteAddress()
        {
            List<int> availableAddressIds = GetAvailableAddressIdsFromAPI();
            if (!availableAddressIds.Any())
            {
                Assert.Fail("No available addresses to update.");
            }

            // ����� ������� ���������� ID ��� ����������
            int idToUpdate = availableAddressIds.First();
            int id = idToUpdate; // �������� �� ����������� ID ������, ������� �� ������ �������
            string url = $"https://localhost:7097/DeleteAddress/{id}";

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
            List<int> availableAddressIds = GetAvailableAddressIdsFromAPI();
            if (!availableAddressIds.Any())
            {
                Assert.Fail("No available addresses to update.");
            }

            // ����� ������� ���������� ID ��� ����������
            int idToUpdate = availableAddressIds.First();
            int id = idToUpdate;
            string url = $"https://localhost:7097/GetAddressById/{id}";

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
        private Bogus.Faker faker;
        private string logFilePath = "test_log.txt"; // ���� � ����� ����

        [OneTimeSetUp]
        public void Setup()
        {
            httpClient = new HttpClient();
            faker = new Bogus.Faker();
            //File.WriteAllText(logFilePath, string.Empty); // �������� ���������� ����� ����� ������� ������
        }

        private void LogTestResultForOrder(string methodName, HttpMethod method, string url, int statusCode)
        {
            //string logFilePath = "test_log.txt"; // ���� � ������ ���-�����
            string logMessage = $"Method: {method}, URL: {url}, Status Code: {statusCode}\n";

            try
            {
                // ������ � ���� ��� ������
                using (StreamWriter writer = File.AppendText(logFilePath))
                {
                    writer.WriteLine(logMessage);
                }
            }
            catch (Exception ex)
            {
                // ��������� ������ ������ � ���
                Console.WriteLine($"An error occurred while writing to the log file: {ex.Message}");
            }

            // ������������� ����� �������� ��������� � �������
            Console.WriteLine(logMessage);
        }


        [Test]
        [AllureTag("API")]
        public void TestGetAllOrders()
        {
            string url = "https://localhost:7097/GetAllOrders"; // ���������� URL ��� GetAllOrders

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                using (HttpResponseMessage response = httpClient.SendAsync(request).Result)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    LogTestResultForOrder("TestGetAllOrders", HttpMethod.Get, url, (int)response.StatusCode);
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
            string url = "https://localhost:7097/CreateOrder"; // ���������, ��� URL ������������� ������ API

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
                    Assert.AreEqual(200, (int)response.StatusCode); // �������� ��������� �������� ������
                }
            }
        }


        private void LogTestResult(string methodName, HttpMethod method, string url, int statusCode)
        {
            string logFilePath = "test_log.txt"; // ���� � ������ ���-�����
            string logMessage = $"Method: {method}, URL: {url}, Status Code: {statusCode}\n";

            try
            {
                // ������ � ���� ��� ������
                File.AppendAllText(logFilePath, logMessage);
            }
            catch (Exception ex)
            {
                // ��������� ������ ������ � ���
                Console.WriteLine($"An error occurred while writing to the log file: {ex.Message}");
            }

            // ������������� ����� �������� ��������� � �������
            Console.WriteLine(logMessage);
        }


        [Test]
        [AllureTag("API")]
        public void TestGetOrderById()
        {
            int orderId = 1; // �������� �� ����������� ID ������, ������� �� ������ ��������
            string url = $"https://localhost:7097/GetOrderById/{orderId}"; // ���������� URL ��� GetOrderById

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
            int orderId = 1; // �������� �� ����������� ID ������, ������� �� ������ ��������
            string url = $"https://localhost:7097/UpdateOrder/{orderId}";

            var updatedOrder = new Order
            {
                // ������� ����������� ������ ��� ������
                Id = orderId,
                UserId = faker.Random.Number(),
                OrderDate = DateTime.Now,
                Products = new List<Product>
        {
            new Product { Id = faker.Random.Number(), Name = faker.Commerce.ProductName(), Price = faker.Random.Decimal() },
            new Product { Id = faker.Random.Number(), Name = faker.Commerce.ProductName(), Price = faker.Random.Decimal() }
        }
            };

            string jsonData = JsonConvert.SerializeObject(updatedOrder);
            HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, url))
            {
                request.Content = content;

                using (HttpResponseMessage response = httpClient.SendAsync(request).Result)
                {
                    var responseContent = response.Content.ReadAsStringAsync().Result;
                    LogTestResult("TestUpdateOrder", HttpMethod.Put, url, (int)response.StatusCode);
                    Console.WriteLine($"PUT Update Order Response content for ID {orderId}: " + responseContent);
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
        private static string logFileName = "logfile.txt";
        private string logFilePath = Path.Combine(System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName), logFileName);

         private StreamWriter logStreamWriter;

        [OneTimeSetUp]
        public void Setup()
        {
            httpClient = new HttpClient();
            logFilePath = "test_log.txt"; // ������� ���� � ����� ����
            logStreamWriter = File.CreateText(logFilePath);
            // Setup any necessary configurations or authentication headers for your API requests
        }
        [OneTimeTearDown]
        public void TearDown()
        {
            logStreamWriter.Close();
        }

        private void LogResult(string testName, string requestType, string url, string content, int statusCode, TimeSpan duration)
        {
            logStreamWriter.WriteLine($"Test: {testName}");
            logStreamWriter.WriteLine($"Request Type: {requestType}");
            logStreamWriter.WriteLine($"URL: {url}");
            logStreamWriter.WriteLine($"Response Content: {content}");
            logStreamWriter.WriteLine($"Status Code: {statusCode}");
            logStreamWriter.WriteLine($"Duration: {duration.TotalMilliseconds} ms");
            logStreamWriter.WriteLine();
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

            Stopwatch stopwatch = Stopwatch.StartNew();

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                using (HttpResponseMessage response = httpClient.SendAsync(request).Result)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine($"GET All Products Response content: {content}");
                    Console.WriteLine($"GET All Products Status code: {(int)response.StatusCode}");
                    Assert.AreEqual(200, (int)response.StatusCode);

                    stopwatch.Stop();
                    LogResult(nameof(TestGetAllProducts), "GET", url, content, (int)response.StatusCode, stopwatch.Elapsed);
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

            Stopwatch stopwatch = Stopwatch.StartNew();

            var newProduct = new Product
            {
                // ����������� ������� ��� ������ ��������
                Id = Faker.Random.Number(),
                Name = Faker.Commerce.ProductName(),
                Price = Faker.Random.Decimal(),
                Category = Faker.Commerce.Categories(1)[0]
            };

            // �������������� ������� ������ �������� � JSON
            string jsonPayload = JsonConvert.SerializeObject(newProduct);

            using (var request = new HttpRequestMessage(HttpMethod.Post, url))
            {
                // ��������� �������� ������� � ������� ������ ��������
                request.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                using (HttpResponseMessage response = httpClient.SendAsync(request).Result)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine($"POST Create Product Response content: {content}");
                    Console.WriteLine($"POST Create Product Status code: {(int)response.StatusCode}");
                    Assert.AreEqual(200, (int)response.StatusCode); // �������� ��������� �������� ��������

                    stopwatch.Stop();
                    LogResult(nameof(TestCreateProduct), "POST", url, content, (int)response.StatusCode, stopwatch.Elapsed);
                }
            }
        }


        private string GenerateSafeFileName(string url)
        {
            char[] invalidPathChars = System.IO.Path.GetInvalidFileNameChars();
            string fileName = url.Replace("https://", "").Replace("http://", "").Replace("/", "_");
            foreach (char invalidChar in invalidPathChars)
            {
                fileName = fileName.Replace(invalidChar, '_');
            }
            return fileName;
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
            List<int> availableProductIds = GetAvailableProductIdsFromAPI();

            // ��������, ���� �� ��������� �������� ��� ����������
            if (!availableProductIds.Any())
            {
                Assert.Fail("No available products to update.");
            }

            // ����� ������� ���������� ID ��� ����������
            int productIdToUpdate = availableProductIds.First();
            int productId = productIdToUpdate; // Replace with a valid product ID

            string url = $"https://localhost:7097/praticka/GetProductById/{productId}";

            Stopwatch stopwatch = Stopwatch.StartNew();

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                using (HttpResponseMessage response = httpClient.SendAsync(request).Result)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine($"GET Product by ID Response content: {content}");
                    Console.WriteLine($"GET Product by ID Status code: {(int)response.StatusCode}");
                    Assert.AreEqual(200, (int)response.StatusCode);

                    stopwatch.Stop();
                    LogResult(nameof(TestGetProductById), "GET", url, content, (int)response.StatusCode, stopwatch.Elapsed);
                }
            }
        }


        private List<int> GetAvailableProductIdsFromAPI()
        {
            List<int> availableIds = new List<int>();

            string url = "https://localhost:7097/praticka/GetAllProducts";

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                using (HttpResponseMessage response = httpClient.SendAsync(request).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = response.Content.ReadAsStringAsync().Result;

                        // �������������� JSON � ���������� ��������� ID ���������
                        var products = JsonConvert.DeserializeObject<List<Product>>(content);
                        availableIds = products.Select(p => p.Id).ToList();
                    }
                    else
                    {
                        // ��������� ������, ����� ������ ���������� ��������
                    }
                }
            }

            return availableIds;
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
            // ��������� ������ ��������� ID ���������
            List<int> availableProductIds = GetAvailableProductIdsFromAPI();

            // ��������, ���� �� ��������� �������� ��� ����������
            if (!availableProductIds.Any())
            {
                Assert.Fail("No available products to update.");
            }

            // ����� ������� ���������� ID ��� ����������
            int productIdToUpdate = availableProductIds.First();
            string url = $"https://localhost:7097/praticka/UpdateProduct/{productIdToUpdate}";

            // �������� ������� ������������ ��������
            var updatedProduct = new Product
            {
                // ����������� ����������� ������� ��� �������� � ID 'productIdToUpdate'
                Name = Faker.Commerce.ProductName(),
                Price = Faker.Random.Decimal(),
                Category = Faker.Commerce.Categories(1)[0]
            };

            Stopwatch stopwatch = Stopwatch.StartNew();

            using (var request = new HttpRequestMessage(HttpMethod.Put, url))
            {
                string json = JsonConvert.SerializeObject(updatedProduct);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                using (HttpResponseMessage response = httpClient.SendAsync(request).Result)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine($"PUT Product Response content: {content}");
                    Console.WriteLine($"PUT Product Status code: {(int)response.StatusCode}");
                    Assert.AreEqual(200, (int)response.StatusCode);

                    stopwatch.Stop();
                    LogResult(nameof(TestUpdateProduct), "PUT", url, content, (int)response.StatusCode, stopwatch.Elapsed);
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
            List<int> availableProductIds = GetAvailableProductIdsFromAPI();

            // ��������, ���� �� ��������� �������� ��� ����������
            if (!availableProductIds.Any())
            {
                Assert.Fail("No available products to update.");
            }

            // ����� ������� ���������� ID ��� ����������
            int productIdToUpdate = availableProductIds.First();
            int productId = productIdToUpdate; // Replace with a valid product ID

            string url = $"https://localhost:7097/praticka/DeleteProduct/{productId}";

            Stopwatch stopwatch = Stopwatch.StartNew();

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, url))
            {
                using (HttpResponseMessage response = httpClient.SendAsync(request).Result)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine($"DELETE Product Response content: {content}");
                    Console.WriteLine($"DELETE Product Status code: {(int)response.StatusCode}");
                    Assert.AreEqual(200, (int)response.StatusCode);

                    stopwatch.Stop();
                    LogResult(nameof(TestDeleteProduct), "DELETE", url, content, (int)response.StatusCode, stopwatch.Elapsed);
                }
            }
        }

    }
}

