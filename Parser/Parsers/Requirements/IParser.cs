using Storage.Requirements.Model.Entity;

namespace Parser.Parsers.Requirements
{
    public interface IParser
    {
        void AddRequirementToStorage(Requirement requirement);
    }
}
