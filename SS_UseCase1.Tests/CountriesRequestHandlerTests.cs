using Microsoft.VisualBasic;
using Moq;
using Moq.Protected;
using SS_UseCase1.Handlers;
using SS_UseCase1.Tests.TestData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SS_UseCase1.Tests
{
    public class CountriesRequestHandlerTests
    {
        private CountriesRequestHandler _sut;
        public CountriesRequestHandlerTests()
        {
            var mockHttpHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            // Set up the expected behavior for the SendAsync method
            mockHttpHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(Data.GetData()),
                })
                .Verifiable();

            // Create the HttpClient instance using the mocked HttpMessageHandler
            var httpClient = new HttpClient(mockHttpHandler.Object);

            _sut = new CountriesRequestHandler(httpClient);
        }

        [Fact]
        public async Task Handle_NoFilters_NoSorting_NoPaging_Returns_all_Data() {
            // Arrange
            CountryDataRequest dataRequest = new CountryDataRequest(null,null,null,null);

            // Act
            var actual = await _sut.Handle(dataRequest, new CancellationToken());

            // Assert
            Assert.True(actual.Count() == 9);
        }

        [Fact]
        public async Task Handle_FiltersPresent_Correct_Data_Returned()
        {
            // Arrange
            CountryDataRequest dataRequest = new CountryDataRequest("mariana", null, null, null);

            // Act
            var actual = await _sut.Handle(dataRequest, new CancellationToken());

            // Assert
            Assert.True(actual.Count() == 1);
            Assert.Equal("Northern Mariana Islands", actual.First().Name.Common);
        }

        [Fact]
        public async Task Handle_PopulationFilter_Correct_Data_Returned()
        {
            // Arrange
            CountryDataRequest dataRequest = new CountryDataRequest(null, 1, null, null);

            // Act
            var actual = await _sut.Handle(dataRequest, new CancellationToken());

            // Assert
            Assert.True(actual.Count() == 5);
            Assert.True(actual.All(c => c.Population < 1000000));
        }

        [Fact]
        public async Task Handle_SortedAccended()
        {
            // Arrange
            CountryDataRequest dataRequest = new CountryDataRequest(null, null, "asc", null);

            // Act
            var actual = await _sut.Handle(dataRequest, new CancellationToken());

            // Assert
            Assert.Equal("Andorra", actual.First().Name.Common);
            Assert.Equal("Turks and Caicos Islands", actual.Last().Name.Common);
        }

        [Fact]
        public async Task Handle_SortedDescended()
        {
            // Arrange
            CountryDataRequest dataRequest = new CountryDataRequest(null, null, "desc", null);

            // Act
            var actual = await _sut.Handle(dataRequest, new CancellationToken());

            // Assert
            Assert.Equal("Andorra", actual.Last().Name.Common);
            Assert.Equal("Turks and Caicos Islands", actual.First().Name.Common);
        }


        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(4)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(9)]
        public async Task Handle_Number_of_Countries_Limited(int limit)
        {
            // Arrange            
            CountryDataRequest dataRequest = new CountryDataRequest(null, null, null, limit);

            // Act
            var actual = await _sut.Handle(dataRequest, new CancellationToken());

            // Assert
            Assert.Equal(limit, actual.Count());
        }
    }
}
