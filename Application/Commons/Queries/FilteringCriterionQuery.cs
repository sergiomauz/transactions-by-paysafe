using System.Text.Json;
using AutoMapper;
using Application.Commons.Mapping;
using Application.Commons.RequestParams;


namespace Application.Commons.Queries
{
    public class FilteringCriterionQuery :
        IMapFrom<FilteringCriterionRequestParams>
    {
        public string Operator { get; set; }
        public JsonElement? Operand { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<FilteringCriterionRequestParams, FilteringCriterionQuery>()
                .ForMember(d => d.Operator, m => m.MapFrom(o => o.Operator))
                .ForMember(d => d.Operand, m => m.MapFrom(o => o.Operand));
        }
    }
}
