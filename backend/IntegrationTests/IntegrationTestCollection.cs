using Xunit;

namespace IntegrationTests;

// This disables parallel test execution for all integration tests
// All tests in this collection will run sequentially
[CollectionDefinition("Integration Tests", DisableParallelization = true)]
public class IntegrationTestCollection
{
}
