using Xunit;

namespace dvd.bca.EntityFrameworkCore;

[CollectionDefinition(bcaTestConsts.CollectionDefinitionName)]
public class bcaEntityFrameworkCoreCollection : ICollectionFixture<bcaEntityFrameworkCoreFixture>
{

}
