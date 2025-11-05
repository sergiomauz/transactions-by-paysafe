using AutoMapper;
using Bogus;

namespace ApplicationTests.Application
{
    public class BaseFormatValidationTest
    {
        public Faker Faker;
        public IMapper Mapper;

        public BaseFormatValidationTest()
        {
            Faker = new Faker();
        }
    }
}
