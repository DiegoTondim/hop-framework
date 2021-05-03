using Hop.Framework.Domain.Commands;
using Hop.Framework.Domain.Services;
using Hop.Net5WebApi.Application.Filters;
using Hop.Net5WebApi.Application.ViewModels;
using Hop.Net5WebApi.Domain.Commands;
using Hop.Net5WebApi.Domain.Entities;
using System;

namespace Hop.Net5WebApi.Application.Services
{
    public interface IPersonService : ICrudServiceAsync<RegisterNewPersonCommand, UpdatePersonCommand, DeleteCommand<Guid>, Guid, PersonFilter, PersonReadViewModel>,
        ILookUpServiceAsync<PersonEntity, PersonFilter, PersonReadViewModel, Guid>
    {
    }
}
