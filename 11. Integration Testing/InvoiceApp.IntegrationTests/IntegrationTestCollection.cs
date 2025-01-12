using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceApp.IntegrationTests
{
    [CollectionDefinition("IntegrationTests")]
    public class IntegrationTestCollection : ICollectionFixture<IntegrationTestingDatabaseFixture>
    {
    }
}
