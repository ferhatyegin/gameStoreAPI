using System.IO.Compression;
using GameStore.Api.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

const string GetGameEndpointName = "GetGame";

List<GameDto> games = [
    new(
        1,
        "Street Fighter",
        "Fighting",
        19.99M,
        new DateOnly(1992, 7, 15)
    ),
    new(
        2,
        "Final Fantasy",
        "Role-playing",
        59.99M,
        new DateOnly(1999, 9, 27)
    ),
    new(
        3,
        "GTA 3",
        "Action",
        99.99M,
        new DateOnly(2003, 9, 12)
    )
];

// GET /games
app.MapGet("/games", () => games);

// GET /games/<id>
app.MapGet("/games/{id}", (int id) =>
{
    GameDto? game = games.Find(game => game.Id == id);

    return game is null ? Results.NotFound() : Results.Ok(game);
})
.WithName(GetGameEndpointName);

// POST /games
app.MapPost("/games", (CreateGameDto newGame) =>
{
    GameDto game = new(
        games.Count + 1,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.ReleaseDate);
    games.Add(game);

    return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
});

// PUT games/<id>
app.MapPut("/games/{id}", (int id, UpdateGameDto updatedGame) =>
{
    var index = games.FindIndex(game => game.Id == id);

    if (index == -1)
    {
        return Results.NotFound();
    }

    games[index] = new GameDto(
        id,
        updatedGame.Name,
        updatedGame.Genre,
        updatedGame.Price,
        updatedGame.ReleaseDate
    );

    return Results.NoContent();
});

// DELETE games/<id>
app.MapDelete("games/{id}", (int id) =>
{
    games.RemoveAll(game => game.Id == id);

    return Results.NoContent();
});


app.Run();
