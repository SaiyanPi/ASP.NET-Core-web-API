using Grpc.Core;

namespace GrpcDemo.Services;

public class RandomNumbersService(ILogger<RandomNumbersService>logger) : RandomNumbers.RandomNumbersBase
{
    public override async Task<SendRandomNumbersResponse> 
    SendRandomNumbers(IAsyncStreamReader<SendRandomNumbersRequest> requestStream, ServerCallContext context)
    {
        var count = 0;
        var sum = 0;
        await foreach (var request in requestStream.ReadAllAsync())
        {
            logger.LogInformation($"Received: {request.Number}");
            count++;
            sum += request.Number;
        }
        return new SendRandomNumbersResponse
        {
            Count = count,
            Sum = sum
        };
    }  
}