using HotChocolate.Data.Filters;
using SchoolManagement.Models;

namespace SchoolManagement.GraphQL.Filetrs;

public class CustomStudentFilterType : FilterInputType<Student>
{
    protected override void Configure(IFilterInputTypeDescriptor<Student> descriptor)
    {
        descriptor.BindFieldsExplicitly();
        descriptor.Name("CustomStudentFilterInput");
        descriptor.AllowAnd(false).AllowOr(false);
        descriptor.Field(t => t.GroupId).Type<CustomStudentGuidOperationFilterInputType>();
    }
}

public class CustomStudentGuidOperationFilterInputType : UuidOperationFilterInputType
{
    protected override void Configure(IFilterInputTypeDescriptor descriptor)
    {
        descriptor.Name("CustomStudentGuidOperationFilterInput");
        descriptor.Operation(DefaultFilterOperations.Equals).Type<IdType>();
        descriptor.Operation(DefaultFilterOperations.In).Type<ListType<IdType>>();
    }
}