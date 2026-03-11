using dvd.bca.Samples;
using Xunit;

namespace dvd.bca.EntityFrameworkCore.Domains;

[Collection(bcaTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<bcaEntityFrameworkCoreTestModule>
{

}
