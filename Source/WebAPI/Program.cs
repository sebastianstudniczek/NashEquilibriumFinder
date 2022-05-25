using Microsoft.AspNetCore.Mvc;
using NashEquilbriumFinder.WebAPI.Models;
using NashEquilibriumFinder.Core;
using NashEquilibriumFinder.Core.Contracts;
using NashEquilibriumFinder.Core.Domain;
using NashEquilibriumFinder.Core.Extensions;
using NashEquilibriumFinder.Core.Helpers;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication();
builder.Services.AddCors(options =>
{
    options.AddPolicy("Open", options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});
WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("Open");
app.UseHttpsRedirection();

app.MapPost($"/api/{nameof(NashFromLemkeHowson)}", NashFromLemkeHowson);
// TODO: Maybe refactor it later into separate endpoints
IResult NashFromLemkeHowson([FromServices] IGameTheoryService nashEquilibriumService, EquilibriumRequest request)
{
    // TODO: Do i need to pass this Game class instead of just request?
    Game gameData = new()
    {
        Title = request.Title,
        FirstPlayer = request.FirstPlayer,
        SecondPlayer = request.SecondPlayer
    };

    int[,] firstPlayerPayouts = request.FirstPlayer.Payouts.To2dArray<int>();
    int[,] secondPlayerPayouts = request.SecondPlayer.Payouts.To2dArray<int>();

    int[][] paretoFront = nashEquilibriumService.GetParetoFront(firstPlayerPayouts, secondPlayerPayouts);

    //TODO: Extract to separate request since there will be multiple algorithims
    List<PayoutCoordinate> sortedPayouts = ChartDataConverter
        .SortPointsCounterclockwise(firstPlayerPayouts, secondPlayerPayouts)
        .ToList();

    NashEquilibrium nashEquilibrium = nashEquilibriumService.GetNashEquilibriumFromLemkeHowson(gameData);

    // TODO: Think about dividing this into separate requests
    return Results.Ok(new AlgoritmResult()
    {
        NashEquilibrium = nashEquilibrium,
        SortedPayouts = sortedPayouts,
        ParetoFront = paretoFront
    });
}

app.MapPost($"/api/{nameof(CorrelatedFromSimplex)}", CorrelatedFromSimplex);

IResult CorrelatedFromSimplex([FromServices] IGameTheoryService gameTheoryService, EquilibriumRequest request)
{
    Game game = new()
    {
        FirstPlayer = request.FirstPlayer,
        SecondPlayer = request.SecondPlayer
    };

    CorrelatedEquilibrium ceEquilibrium = gameTheoryService.GetCorrelatedEquilibriumFromSimplex(game);

    return Results.Ok(ceEquilibrium);
}

app.MapGet("/api/HostInfo", GetHostInfo);

HostInfo GetHostInfo([FromServices] IDiagnosticService diagnosticService) =>
    diagnosticService.GetHostInfo();

app.Run();