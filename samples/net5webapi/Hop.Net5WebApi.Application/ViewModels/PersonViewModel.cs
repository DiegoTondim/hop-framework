using System;

namespace Hop.Net5WebApi.Application.ViewModels
{
    public class PersonViewModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }

        public PersonViewModel(string name)
        {
            Name = name;
        }
        public PersonViewModel(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
