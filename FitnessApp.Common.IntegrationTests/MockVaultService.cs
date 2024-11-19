﻿using FitnessApp.Common.Vault;
using VaultSharp;

namespace FitnessApp.Common.IntegrationTests;

public class MockVaultService : IVaultService
{
    public MockVaultService(IVaultClient vaultClient)
    {
        ArgumentNullException.ThrowIfNull(vaultClient);
    }

    public Task<string> GetSecret(string secretKey)
    {
        return Task.FromResult(MockConstants.SvTest);
    }
}
