using System;

namespace Hop.Net5WebApi.Application.ViewModels
{
    public class PersonReadViewModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }

        public PersonReadViewModel(string name)
        {
            Name = name;
        }

        public PersonReadViewModel(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
