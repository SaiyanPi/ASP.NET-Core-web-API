using HotChocolate.Data.Filters;

namespace SchoolManagement.GraphQL.Filetrs;
public class StudentStringOperationFilterInputType : StringOperationFilterInputType
{
    protected override void Configure(IFilterInputTypeDescriptor descriptor)
    {
        descriptor.Operation(DefaultFilterOperations.Equals).Type<StringType>();
        descriptor.Operation(DefaultFilterOperations.Contains).Type<StringType>();
    }
}