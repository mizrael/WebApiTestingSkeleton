using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using WebApiTestingSkeleton.Tests.Fixtures;
using FluentAssertions;
using WebApiTestingSkeleton.Core.Web;

namespace WebApiTestingSkeleton.Tests.E2E.Routes
{
    public class UsersTests: IClassFixture<WebApiFixture<API.Startup>>
    {
        private readonly HttpClient _client;

        public UsersTests(WebApiFixture<API.Startup> fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task GET_should_return_valid_response()
        {
            var response = await _client.GetAsync("/api/users");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Should().NotBeNull();

            var responseString = await response.Content.ReadAsStringAsync();
            responseString.Should().NotBeNullOrWhiteSpace();

            var results = await response.Content.ReadAsJsonAsync<IEnumerable<Core.Models.User>>();
            results.Should().NotBeNull();
        }
        
        [Fact]
        public async Task POST_should_fail_with_invalid_data()
        {
            var dto = new
            {
            };
            var response = await _client.PostAsJsonAsync("/api/users", dto);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Content.Should().NotBeNull();

            var errorResponse = await response.Content.ReadAsJsonAsync<ApiErrorResponse>();
            errorResponse.Should().NotBeNull();
        }

        [Fact]
        public async Task POST_should_create_new_user()
        {
            var createUserDto = new 
            {
                fullname = Guid.NewGuid().ToString(),
                email = $"{Guid.NewGuid()}@test.com"
            };
            var createResponse = await _client.PostAsJsonAsync("/api/users", createUserDto);
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            createResponse.Content.Should().NotBeNull();

            var createdUserId = await createResponse.Content.ReadAsJsonAsync<Guid>();

            createResponse.Headers.Contains("Location").Should().BeTrue();

            // from details
            var getResponse = await _client.GetAsync(createResponse.Headers.Location.AbsolutePath);
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var createdUserDto = await getResponse.Content.ReadAsJsonAsync<Core.Models.User>();
            createdUserDto.Should().NotBeNull();
            createdUserDto.Id.Should().Be(createdUserId);
            createdUserDto.Fullname.Should().Be(createUserDto.fullname);
            createdUserDto.Email.Should().Be(createUserDto.email);

            //from archive
            var archiveResponse = await _client.GetAsync("/api/users");
            archiveResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var results = await archiveResponse.Content.ReadAsJsonAsync<IEnumerable<Core.Models.User>>();
            results.Should().NotBeNull();
            results.Should().Contain(u => u.Id == createdUserId);
        }

        [Fact]
        public async Task PUT_should_update_user()
        {
            var createUserDto = new 
            {
                fullname = Guid.NewGuid().ToString(),
                email = $"{Guid.NewGuid()}@test.com"
            };
            var createResponse = await _client.PostAsJsonAsync("/api/users", createUserDto);

            var getResponse = await _client.GetAsync(createResponse.Headers.Location.AbsolutePath);
            var createdUserDto = await getResponse.Content.ReadAsJsonAsync<Core.Models.User>();

            var updateUserDto = new
            {
                fullname = Guid.NewGuid().ToString(),
                email = $"{Guid.NewGuid()}@test.com"
            };
            var updateResponse = await _client.PutAsJsonAsync($"/api/users/{createdUserDto.Id}", updateUserDto);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            updateResponse.Content.Should().NotBeNull();
            var updatedUserId = await updateResponse.Content.ReadAsJsonAsync<Guid>();
            updatedUserId.Should().Be(createdUserDto.Id);
        }

        [Fact]
        public async Task DELETE_should_return_valid_response()
        {
            var createUserDto = new 
            {
                fullname = Guid.NewGuid().ToString(),
                email = $"{Guid.NewGuid()}@test.com"
            };
            var createResponse = await _client.PostAsJsonAsync("/api/users", createUserDto);

            createResponse.Should().NotBeNull();
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            createResponse.Content.Should().NotBeNull();

            var getResponse = await _client.GetAsync(createResponse.Headers.Location.AbsolutePath);
            getResponse.Should().NotBeNull();
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var deleteResponse = await _client.DeleteAsync(createResponse.Headers.Location.AbsolutePath);
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            getResponse = await _client.GetAsync(createResponse.Headers.Location.AbsolutePath);
            getResponse.Should().NotBeNull();
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
