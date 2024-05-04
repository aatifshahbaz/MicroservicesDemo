using Catalog.Service.Test.Fixtures;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Catalog.Service.Test.Integration
{
    public class ItemsControllerTest : IClassFixture<SharedContextFixture>, IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly SharedContextFixture _fixture;

        public ItemsControllerTest(SharedContextFixture fixture, WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
            _fixture = fixture;

            //Initiating with 2 valid Ids to avoid Stack Underflow, as test methods run in isolation (parallel) in random sequence and
            //If POP operation happened before PUSH (i.e. PUT or DELETE Endpoints) it will fail that particular test
            _fixture.Ids.Enqueue(new Guid("BA8E6E05-4D00-4BDF-8036-039BF277D0E8"));
            _fixture.Ids.Enqueue(new Guid("32E77B5B-F271-4F8F-B7AD-98626C5B1C9F"));
        }


        //[MethodWeTest_StateUnderTest_ExpectedBehavior]

        [Fact]
        public async Task Get_Endpoint_ReturnsSuccessAndCorrectContentType()
        {
            // Arrange

            // Act
            var response = await _client.GetAsync("/api/items");

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
        }

        [Fact]
        public async Task GetById_Endpoint_ReturnsSuccessAndCorrectContentType()
        {
            // Arrange
            var validRecordId = "BC4B3902-56A6-4184-B657-40EF5222641A";

            // Act
            var response = await _client.GetAsync($"/api/items/{validRecordId}");
            response.EnsureSuccessStatusCode(); // Status Code 200-299 Throws exception if IsSuccessStatusCode is FALSE

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
        }


        [Fact]
        public async Task GetById_Endpoint_ReturnsNotFoundAndProblemContentType()
        {
            // Arrange
            var inValidRecordId = "BC4B3902-56A6-4184-B657-40EF5222641B";
            // Act
            var response = await _client.GetAsync($"/api/items/{inValidRecordId}");

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.NotFound);
            Assert.NotEqual("application/json", response.Content.Headers.ContentType.MediaType);
        }

        [Theory]
        [InlineData("923BBF43-C138-4D1A-8735-012CA58E5842")]
        [InlineData("E3214904-0BE8-465A-8DF0-C36472757D43")]
        [InlineData("BC4B3902-56A6-4184-B657-40EF5222641A")]
        public async Task GetById_Endpoint_ReturnsValidItem(Guid validRecordId)
        {
            // Arrange
            var items = new List<string> { "Antidote", "Shield", "Sword" };

            // Act
            var response = await _client.GetAsync($"/api/items/{validRecordId}");
            var item = await response.Content.ReadFromJsonAsync<ItemDto>();

            // Assert
            Assert.NotNull(item);
            Assert.Equal(validRecordId, item.Id);
            Assert.True(items.Contains(item.Name));
        }


        [Theory]
        [InlineData("Spring", "Let you high jump", 9)]
        public async Task Post_Endpoint_ReturnsSuccessAndCorrectContentType(string name, string description, decimal price)
        {
            // Arrange
            var item = new CreateItemDto(name, description, price);
            var content = new StringContent(JsonSerializer.Serialize(item), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/items", content);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }


        [Theory]
        [InlineData("HI Potion", "Increase HP", 10)]
        [InlineData("Guns", "Increase Firepower", 25)]
        public async Task Post_Endpoint_ReturnsCreatedItem(string name, string description, decimal price)
        {
            // Arrange
            var item = new CreateItemDto(name, description, price);
            var content = new StringContent(JsonSerializer.Serialize(item), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/items", content);
            var createdItem = await response.Content.ReadFromJsonAsync<ItemDto>();
            _fixture.Ids.Enqueue(createdItem.Id);

            // Assert
            Assert.NotNull(createdItem);
            Assert.Equal(name, createdItem.Name);
            Assert.Equal(description, createdItem.Description);
            Assert.Equal(price, createdItem.Price);
        }


        [Theory]
        [InlineData("Gears", "Speedup", 3)]
        public async Task Put_Endpoint_ReturnsSuccessAndCorrectContentType(string name, string description, decimal price)
        {
            // Arrange
            var item = new UpdateItemDto(name, description, price);
            var content = new StringContent(JsonSerializer.Serialize(item), Encoding.UTF8, "application/json");
            var validRecordId = _fixture.Ids.Dequeue();

            // Act
            var response = await _client.PutAsync($"/api/items/{validRecordId}", content);
            _fixture.Ids.Enqueue(validRecordId);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }

        [Theory]
        [InlineData("Spring", "Let you high jump", 9)]
        [InlineData("Wings", "Gives you Flight", 32)]
        public async Task Put_Endpoint_ReturnsUpdatedItem(string name, string description, decimal price)
        {
            // Arrange
            var item = new UpdateItemDto(name, description, price);
            var content = new StringContent(JsonSerializer.Serialize(item), Encoding.UTF8, "application/json");
            var validRecordId = _fixture.Ids.Dequeue();

            // Act
            var response = await _client.PutAsync($"/api/items/{validRecordId}", content);
            var updatedItem = await response.Content.ReadFromJsonAsync<ItemDto>();
            _fixture.Ids.Enqueue(validRecordId);

            // Assert
            Assert.NotNull(updatedItem);
            Assert.Equal(name, updatedItem.Name);
            Assert.Equal(description, updatedItem.Description);
            Assert.Equal(price, updatedItem.Price);
        }



        [Fact]
        public async Task Delete_Endpoint_ReturnsNoContentAndCorrectContentType()
        {
            // Arrange
            var validRecordId = _fixture.Ids.Dequeue();

            // Act
            var response = await _client.DeleteAsync($"/api/items/{validRecordId}");
            //_fixture.Ids.Push(validRecordId); //Dont Push it back since this item has been deleted and this id no longer exists in database

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.NoContent);
            Assert.Null(response.Content.Headers.ContentType); //No MediaType if there are NoContent (204)
        }


        [Fact]
        public async Task Delete_Endpoint_ReturnsNotFoundAndProblemContentType()
        {
            // Arrange
            var inValidRecordId = "533F03A9-3B30-4975-866E-4ED147638359";
            // Act
            var response = await _client.DeleteAsync($"/api/items/{inValidRecordId}");

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.NotFound);
            Assert.Equal("application/problem+json", response.Content.Headers.ContentType.MediaType);
        }



    }
}
