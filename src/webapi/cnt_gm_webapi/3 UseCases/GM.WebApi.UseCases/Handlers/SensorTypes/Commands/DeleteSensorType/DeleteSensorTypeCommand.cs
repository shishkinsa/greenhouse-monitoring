using Requestum.Contract;

namespace GM.WebApi.UseCases.Handlers.SensorTypes.Commands.DeleteSensorType;

/// <summary>
/// Деактивация типа датчика (soft delete: <c>is_active = false</c>).
/// </summary>
public sealed class DeleteSensorTypeCommand : ICommand
{
    public Guid SensorTypeId { get; set; }
}
