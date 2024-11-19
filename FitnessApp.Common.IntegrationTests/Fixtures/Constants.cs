namespace FitnessApp.Common.IntegrationTests.Fixtures;

public static class MockConstants
{
    public const string Scheme = "TestAuth";
    public const string SvTest = "svTest";
}

public static class IdsConstants
{
    public const string IdToGet = nameof(IdToGet);
    public const string IdToCreate = nameof(IdToCreate);
    public const string IdToUpdate = nameof(IdToUpdate);
    public const string IdToDelete = nameof(IdToDelete);

    public static string[] IdsToSeed { get; } = [
        IdToGet,
        IdToUpdate,
        IdToDelete
    ];
}